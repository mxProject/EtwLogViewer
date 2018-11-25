using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Diagnostics.Tracing;

namespace mxProject.Tools.EtwLogViewer
{

    /// <summary>
    /// 
    /// </summary>
    internal sealed class EtwContext : IObserver<TraceEvent>, IDisposable
    {

        /// <summary>
        /// 
        /// </summary>
        public static void Initialize()
        {

            EtwStream.ObservableEventListener.ClearAllActiveObservableEventListenerSession();

        }

        #region ctor/dtor

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="maxTraceEventCount"></param>
        /// <param name="filter"></param>
        public EtwContext(int maxTraceEventCount, Predicate<TraceEvent> filter) : this(maxTraceEventCount, filter, null)
        {
        }

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="maxTraceEventCount"></param>
        /// <param name="filter"></param>
        /// <param name="onReceivedLog">The method to be executed when TraceEvent is received.</param>
        public EtwContext(int maxTraceEventCount, Predicate<TraceEvent> filter, Action<TraceEvent> onReceivedLog)
        {
            m_Filter = filter;
            SetMaxTraceEventCount(maxTraceEventCount);
            OnReceivedLog = onReceivedLog;
        }

        /// <summary>
        /// destructor
        /// </summary>
        ~EtwContext()
        {
            Dispose(false);
        }

        #endregion

        #region dispose

        /// <summary>
        /// Releases all resources used by this instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases all resources used by this instance.
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            UnregistProviders();
        }

        #endregion

        #region subscribe

        private readonly Dictionary<string, SubscriberState> m_Subscribers = new Dictionary<string, SubscriberState>();

        /// <summary>
        /// Adds the specified provider.
        /// </summary>
        /// <param name="providerNameOrGuid">The provider name or Guid.</param>
        /// <exception cref="ArgumentException">
        /// The specified provider has already been registered.
        /// </exception>
        public void RegistProvider(string providerNameOrGuid)
        {

            lock (m_Subscribers)
            {

                if (m_Subscribers.ContainsKey(providerNameOrGuid))
                {
                    throw new ArgumentException("The specified provider has already been registered.");
                }

                IObservable<TraceEvent> target = EtwStream.ObservableEventListener.FromTraceEvent(providerNameOrGuid);

                IDisposable subscriber = target.Subscribe(this);

                SubscriberState state = new SubscriberState(providerNameOrGuid, subscriber);

                m_Subscribers.Add(providerNameOrGuid, state);

            }

        }

        /// <summary>
        /// Removes the specified provider.
        /// </summary>
        /// <param name="providerNameOrGuid">The provider name or Guid.</param>
        public void UnregistProvider(string providerNameOrGuid)
        {

            lock (m_Subscribers)
            {

                if (m_Subscribers.TryGetValue(providerNameOrGuid, out SubscriberState state))
                {
                    state.Dispose();
                    m_Subscribers.Remove(providerNameOrGuid);
                }

            }

        }

        /// <summary>
        /// Removes all providers.
        /// </summary>
        public void UnregistProviders()
        {

            lock (m_Subscribers)
            {

                foreach (SubscriberState state in m_Subscribers.Values)
                {
                    state.Dispose();
                }

                m_Subscribers.Clear();

            }

        }

        /// <summary>
        /// State object of subscriber.
        /// </summary>
        private sealed class SubscriberState : IDisposable
        {

            /// <summary>
            /// 
            /// </summary>
            /// <param name="providerNameOrGuid"></param>
            /// <param name="subscriber"></param>
            internal SubscriberState(string providerNameOrGuid, IDisposable subscriber)
            {
                ProviderNameOrGuid = providerNameOrGuid;
                Subscriber = subscriber;
            }

            /// <summary>
            /// 
            /// </summary>
            internal void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="disposing"></param>
            private void Dispose(bool disposing)
            {
                Subscriber?.Dispose();
            }

            /// <summary>
            /// 
            /// </summary>
            void IDisposable.Dispose()
            {
                Dispose(false);
            }

            /// <summary>
            /// 
            /// </summary>
            internal string ProviderNameOrGuid { get; }

            /// <summary>
            /// 
            /// </summary>
            internal IDisposable Subscriber { get; }

        }

        #endregion

        #region TraceEvents

        private readonly Predicate<TraceEvent> m_Filter;

        private TraceEvent[] m_TraceEvents = new TraceEvent[] { };

        /// <summary>
        /// Gets the maximum number of TraceEvent caches.
        /// </summary>
        public int MaxTraceEventCount { get; private set; }

        /// <summary>
        /// Sets the maximum number of TraceEvent caches.
        /// </summary>
        /// <param name="maxCount">The maximum number of TraceEvent caches.</param>
        public void SetMaxTraceEventCount(int maxCount)
        {

            if (MaxTraceEventCount == maxCount) { return; }

            lock (this)
            {

                TraceEvent[] newArray = new TraceEvent[maxCount];

                if (CurrentTraceEventCount < maxCount)
                {

                    for (int i = 0; i < CurrentTraceEventCount; ++i)
                    {
                        newArray[i] = GetTraceEventAtInternal(i);
                    }

                    m_TraceEvents = newArray;
                    m_NextTraceEventIndex = CurrentTraceEventCount;

                }
                else
                {

                    for (int i = CurrentTraceEventCount - maxCount; i < CurrentTraceEventCount; ++i)
                    {
                        newArray[i-(CurrentTraceEventCount - maxCount)] = GetTraceEventAtInternal(i);
                    }

                    m_TraceEvents = newArray;
                    m_NextTraceEventIndex = 0;
                    CurrentTraceEventCount = maxCount;

                }

                MaxTraceEventCount = maxCount;

            }

            OnMaxTraceEventCountChanged(EventArgs.Empty);

        }

        private int m_NextTraceEventIndex = 0;

        /// <summary>
        /// Gets the number of cached TraceEvent.
        /// </summary>
        public int CurrentTraceEventCount { get; private set; } = 0;

        /// <summary>
        /// Gets the TraceEvent represented by the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public TraceEvent GetTraceEventAt(int index)
        {
            lock (this)
            {
                return GetTraceEventAtInternal(index);
            }
        }

        /// <summary>
        /// Gets the TraceEvent represented by the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        private TraceEvent GetTraceEventAtInternal(int index)
        {

            int ix = index + m_NextTraceEventIndex;

            if (ix >= CurrentTraceEventCount) { ix -= CurrentTraceEventCount; }

            return m_TraceEvents[ix];

        }

        /// <summary>
        /// Adds the specified TraceEvent.
        /// </summary>
        /// <param name="traceEvent"></param>
        private void AddTraceEvent(TraceEvent traceEvent)
        {

            if (m_TraceEvents.Length == 0) { return; }

            lock (this)
            {

                m_TraceEvents[m_NextTraceEventIndex] = traceEvent;

                if (m_NextTraceEventIndex < m_TraceEvents.Length - 1)
                {
                    ++m_NextTraceEventIndex;
                }
                else
                {
                    m_NextTraceEventIndex = 0;
                }

                if (CurrentTraceEventCount < m_TraceEvents.Length)
                {
                    ++CurrentTraceEventCount;
                }

            }

        }

        #endregion

        #region implement IObserver<TraceEvent>

        /// <summary>
        /// 
        /// </summary>
        void IObserver<TraceEvent>.OnCompleted()
        {
            System.Diagnostics.Debug.WriteLine("OnCompleted");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        void IObserver<TraceEvent>.OnError(Exception error)
        {
            System.Diagnostics.Debug.WriteLine("OnError : " + error.Message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        void IObserver<TraceEvent>.OnNext(TraceEvent value)
        {
            OnReceived(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        private void OnReceived(TraceEvent value)
        {

            if (m_Filter != null && !m_Filter(value)) { return; }

            try
            {
                if (MaxTraceEventCount > 0)
                {
                    AddTraceEvent(value);
                }

                OnReceivedLog?.Invoke(value);

                if (Received != null)
                {
                    OnReceived(new TraceEventArgs(value));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }

        #endregion

        /// <summary>
        /// Get the method to be executed when TraceEvent is received.
        /// </summary>
        public Action<TraceEvent> OnReceivedLog { get; private set; }

        #region events

        /// <summary>
        /// Occurs when a TraceEvent is received.
        /// </summary>
        [Obsolete("Use OnReceivedLog.")]
        public event TraceEventHandler Received;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        private void OnReceived(TraceEventArgs e)
        {
            Received?.Invoke(this, e);
        }

        /// <summary>
        /// Occurs when the value of the MaxTraceEventCount property changes.
        /// </summary>
        public event EventHandler MaxTraceEventCountChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        private void OnMaxTraceEventCountChanged(EventArgs e)
        {
            MaxTraceEventCountChanged?.Invoke(this, e);
        }

        #endregion

    }

}
