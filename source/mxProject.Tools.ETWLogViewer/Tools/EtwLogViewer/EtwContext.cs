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
        /// 
        /// </summary>
        /// <param name="maxTraceEventCount"></param>
        /// <param name="filter"></param>
        public EtwContext(int maxTraceEventCount, Predicate<TraceEvent> filter)
        {
            m_Filter = filter;
            SetMaxTraceEventCount(maxTraceEventCount);
        }

        /// <summary>
        /// 
        /// </summary>
        ~EtwContext()
        {
            Dispose(false);
        }

        #endregion

        #region dispose

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
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
            UnregistProviders();
        }

        #endregion

        #region subscribe

        private readonly Dictionary<string, SubscriberState> m_Subscribers = new Dictionary<string, SubscriberState>();

        /// <summary>
        /// 
        /// </summary>
        public void RegistProvider(string providerNameOrGuid)
        {

            IObservable<TraceEvent> target = EtwStream.ObservableEventListener.FromTraceEvent(providerNameOrGuid);

            IDisposable subscriber = target.Subscribe(this);

            SubscriberState state = new SubscriberState(providerNameOrGuid, subscriber);

            m_Subscribers.Add(providerNameOrGuid, state);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="providerNameOrGuid"></param>
        public void UnregistProvider(string providerNameOrGuid)
        {

            if (m_Subscribers.TryGetValue(providerNameOrGuid, out SubscriberState state))
            {
                state.Subscriber.Dispose();
                m_Subscribers.Remove(providerNameOrGuid);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public void UnregistProviders()
        {

            string[] names = m_Subscribers.Keys.ToArray();

            for (int i = 0; i < names.Length; ++i)
            {
                UnregistProvider(names[i]);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private class SubscriberState
        {

            /// <summary>
            /// 
            /// </summary>
            /// <param name="providerNameOrGuid"></param>
            /// <param name="subscriber"></param>
            internal SubscriberState(string providerNameOrGuid, IDisposable subscriber)
            {
                m_ProviderName = providerNameOrGuid;
                m_Subscriber = subscriber;
            }

            /// <summary>
            /// 
            /// </summary>
            internal string ProviderNameOrGuid
            {
                get { return m_ProviderName; }
            }
            private string m_ProviderName;

            /// <summary>
            /// 
            /// </summary>
            internal IDisposable Subscriber
            {
                get { return m_Subscriber; }
            }
            private IDisposable m_Subscriber;

        }

        #endregion

        #region TraceEvents

        private Predicate<TraceEvent> m_Filter;

        private TraceEvent[] m_TraceEvents = new TraceEvent[10];

        /// <summary>
        /// 
        /// </summary>
        public int MaxTraceEventCount
        {
            get { return m_MaxTraceEventCount; }
        }
        private int m_MaxTraceEventCount = 10;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxCount"></param>
        public void SetMaxTraceEventCount(int maxCount)
        {

            if (m_MaxTraceEventCount == maxCount) { return; }

            lock (this)
            {

                TraceEvent[] newArray = new TraceEvent[maxCount];

                if (m_CurrentTraceEventCount < maxCount)
                {

                    for (int i = 0; i < m_CurrentTraceEventCount; ++i)
                    {
                        newArray[i] = GetTraceEventAtInternal(i);
                    }

                    m_TraceEvents = newArray;
                    m_NextTraceEventIndex = m_CurrentTraceEventCount;

                }
                else
                {

                    for (int i = m_CurrentTraceEventCount - maxCount; i < m_CurrentTraceEventCount; ++i)
                    {
                        newArray[i-(m_CurrentTraceEventCount - maxCount)] = GetTraceEventAtInternal(i);
                    }

                    m_TraceEvents = newArray;
                    m_NextTraceEventIndex = 0;
                    m_CurrentTraceEventCount = maxCount;

                }

                m_MaxTraceEventCount = maxCount;

            }

            OnMaxTraceEventCountChanged(EventArgs.Empty);

        }

        private int m_NextTraceEventIndex = 0;
        private int m_CurrentTraceEventCount = 0;

        /// <summary>
        /// 
        /// </summary>
        public int CurrentTraceEventCount
        {
            get { return m_CurrentTraceEventCount; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TraceEvent GetTraceEventAt(int index)
        {
            lock (this)
            {
                return GetTraceEventAtInternal(index);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private TraceEvent GetTraceEventAtInternal(int index)
        {

            int ix = index + m_NextTraceEventIndex;

            if (ix >= m_CurrentTraceEventCount) { ix -= m_CurrentTraceEventCount; }

            return m_TraceEvents[ix];

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="traceEvent"></param>
        private void AddTraceEvent(TraceEvent traceEvent)
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

            if (m_CurrentTraceEventCount < m_TraceEvents.Length)
            {
                ++m_CurrentTraceEventCount;
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

            OnNext(value);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        private void OnNext(TraceEvent value)
        {

            if (m_Filter != null && !m_Filter(value)) { return; }

            try
            {
                lock (this)
                {
                    AddTraceEvent(value);
                }

                OnReceived(new TraceEventArgs(value));
            }
            catch ( Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }

        }

        #endregion

        #region events

        /// <summary>
        /// 
        /// </summary>
        public event TraceEventHandler Received;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        private void OnReceived(TraceEventArgs e)
        {
            TraceEventHandler handler = this.Received;
            if (handler != null) { handler(this, e); }
        }

        public event EventHandler MaxTraceEventCountChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        private void OnMaxTraceEventCountChanged(EventArgs e)
        {
            EventHandler handler = this.MaxTraceEventCountChanged;
            if (handler != null) { handler(this, e); }
        }

        #endregion

    }

}
