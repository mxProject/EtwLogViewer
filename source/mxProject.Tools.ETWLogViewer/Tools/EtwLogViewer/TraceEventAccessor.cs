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
    internal class TraceEventAccessor
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setting"></param>
        internal TraceEventAccessor(TraceEventListSetting setting)
        {
            m_Setting = setting;
            m_FieldAccessors = CreateFieldAccessors(setting);
        }

        private TraceEventListSetting m_Setting;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="traceEvent"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public object GetFieldValue(TraceEvent traceEvent, TraceEventFieldIndex field)
        {

            if (m_FieldAccessors.TryGetValue(field, out Func<TraceEvent, object> accessor))
            {
                return accessor(traceEvent);
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        internal Func<TraceEvent, object> GetFieldAccessor(TraceEventFieldIndex field)
        {

            if (m_FieldAccessors.TryGetValue(field, out Func<TraceEvent, object> accessor))
            {
                return accessor;
            }
            else
            {
                return GetUnknownFieldValue;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="traceEvent"></param>
        /// <returns></returns>
        private object GetUnknownFieldValue(TraceEvent traceEvent)
        {
            return null;
        }

        private readonly IDictionary<TraceEventFieldIndex, Func<TraceEvent, object>> m_FieldAccessors;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        private IDictionary<TraceEventFieldIndex, Func<TraceEvent, object>> CreateFieldAccessors(TraceEventListSetting setting)
        {

            Dictionary<TraceEventFieldIndex, Func<TraceEvent, object>> accessors = new Dictionary<TraceEventFieldIndex, Func<TraceEvent, object>>();

            accessors.Add(TraceEventFieldIndex.ActivityID, o => o.ActivityID);
            accessors.Add(TraceEventFieldIndex.Channel, o => o.Channel);
            accessors.Add(TraceEventFieldIndex.Dump, o => o.Dump());
            accessors.Add(TraceEventFieldIndex.EventData, o => o.EventData());
            accessors.Add(TraceEventFieldIndex.EventDataLength, o => o.EventDataLength);
            accessors.Add(TraceEventFieldIndex.EventIndex, o => o.EventIndex);
            accessors.Add(TraceEventFieldIndex.EventName, o => o.EventName);
            accessors.Add(TraceEventFieldIndex.EventTypeUserData, o => o.EventTypeUserData);
            accessors.Add(TraceEventFieldIndex.FormattedMessage, o => o.FormattedMessage);
            accessors.Add(TraceEventFieldIndex.ID, o => o.ID);
            accessors.Add(TraceEventFieldIndex.IsClassicProvider, o => o.IsClassicProvider);
            accessors.Add(TraceEventFieldIndex.Keywords, o => o.Keywords);
            accessors.Add(TraceEventFieldIndex.Level, o => o.Level);
            accessors.Add(TraceEventFieldIndex.Opcode, o => o.Opcode);
            accessors.Add(TraceEventFieldIndex.OpcodeName, o => o.OpcodeName);
            accessors.Add(TraceEventFieldIndex.PointerSize, o => o.PointerSize);
            accessors.Add(TraceEventFieldIndex.ProcessID, o => o.ProcessID);
            accessors.Add(TraceEventFieldIndex.ProcessName, o => o.ProcessName);
            accessors.Add(TraceEventFieldIndex.ProcessorNumber, o => o.ProcessorNumber);
            accessors.Add(TraceEventFieldIndex.ProviderGuid, o => o.ProviderGuid);
            accessors.Add(TraceEventFieldIndex.ProviderName, o => o.ProviderName);
            accessors.Add(TraceEventFieldIndex.RelatedActivityID, o => o.RelatedActivityID);
            accessors.Add(TraceEventFieldIndex.Source, o => o.Source);
            accessors.Add(TraceEventFieldIndex.Task, o => o.Task);
            accessors.Add(TraceEventFieldIndex.TaskName, o => o.TaskName);
            accessors.Add(TraceEventFieldIndex.ThreadID, o => o.ThreadID);
            accessors.Add(TraceEventFieldIndex.TimeStamp, o => o.TimeStamp);
            accessors.Add(TraceEventFieldIndex.TimeStampRelativeMSec, o => o.TimeStampRelativeMSec);
            accessors.Add(TraceEventFieldIndex.Version, o => o.Version);

            accessors.Add(TraceEventFieldIndex.Xml, delegate (TraceEvent o)
            {
                StringBuilder sb = new StringBuilder();
                o.ToXml(sb);
                return sb.ToString();
            }
            );

            accessors.Add(TraceEventFieldIndex.Payloads, delegate (TraceEvent o)
            {
                StringBuilder sb = new StringBuilder();

                for (int i=0; i< o.PayloadNames.Length; ++i)
                {
                    if (sb.Length > 0) { sb.Append("; "); }
                    sb.AppendFormat("{0}={1}", o.PayloadNames[i], o.PayloadByName(o.PayloadNames[i]));
                }

                return sb.ToString();
            }
            );

            IDictionary<string, EtwProvider> providers = GetProviders(setting);

            accessors.Add(TraceEventFieldIndex.ProviderFriendlyName, delegate (TraceEvent o)
            {

                EtwProvider provider;

                if (providers.TryGetValue(o.ProviderGuid.ToString(), out provider))
                {
                    return provider.FriendlyName;
                }
                else if (providers.TryGetValue(o.ProviderName, out provider))
                {
                    return provider.FriendlyName;
                }

                return null;

            }
            );

            return accessors;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        private IDictionary<string, EtwProvider> GetProviders(TraceEventListSetting setting)
        {

            if (setting == null || setting.Providers == null) { return new Dictionary<string, EtwProvider>(); }

            return setting.Providers.ToDictionary(o => o.GetNameOrID());

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="traceEvent"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object GetPayloadValue(TraceEvent traceEvent, string name)
        {

            int index = traceEvent.PayloadIndex(name);

            if (index < 0) { return null; }

            return traceEvent.PayloadValue(index);

        }

    }

}
