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
    internal interface ITraceEventField
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string GetCaption();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="traceEvent"></param>
        /// <returns></returns>
        object GetValue(TraceEvent traceEvent);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        int GetWidth();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        void SetWidth(int value);

    }

    /// <summary>
    /// 
    /// </summary>
    internal class TraceEventField : ITraceEventField
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="width"></param>
        /// <param name="accessor"></param>
        private TraceEventField(TraceEventFieldIndex index, int width, TraceEventAccessor accessor)
        {
            m_Index= index;
            m_Getter = accessor.GetFieldAccessor(index);
            m_Width = width;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="defaultWidth"></param>
        /// <returns></returns>
        internal static IDictionary<TraceEventFieldIndex, TraceEventField> GetAllFields(TraceEventListSetting setting, int defaultWidth)
        {

            TraceEventAccessor accessor = new TraceEventAccessor(setting);

            Array enumFields = Enum.GetValues(typeof(TraceEventFieldIndex));

            Dictionary<TraceEventFieldIndex, TraceEventField> fields = new Dictionary<TraceEventFieldIndex, TraceEventField>(enumFields.Length);

            for (int i=0; i < enumFields.Length; ++i)
            {

                TraceEventFieldIndex enumField = (TraceEventFieldIndex)enumFields.GetValue(i);

                fields.Add(enumField, new TraceEventField(enumField, defaultWidth, accessor));

            }

            return fields;

        }

        /// <summary>
        /// 
        /// </summary>
        public TraceEventFieldIndex Index
        {
            get { return m_Index; }
        }
        private TraceEventFieldIndex m_Index;

        /// <summary>
        /// 
        /// </summary>
        private Func<TraceEvent, object> m_Getter;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetCaption()
        {
            return m_Index.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="traceEvent"></param>
        /// <returns></returns>
        public object GetValue(TraceEvent traceEvent)
        {
            return m_Getter(traceEvent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetWidth()
        {
            return m_Width;
        }
        private int m_Width;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SetWidth(int value)
        {
            m_Width = value;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    internal class TraceEventPayload : ITraceEventField
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="width"></param>
        internal TraceEventPayload(string name, int width)
        {
            m_Name = name;
            m_Width = width;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return m_Name; }
        }
        private string m_Name;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetCaption()
        {
            return string.Format("[{0}]", m_Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="traceEvent"></param>
        /// <returns></returns>
        public object GetValue(TraceEvent traceEvent)
        {
            return TraceEventAccessor.GetPayloadValue(traceEvent, m_Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetWidth()
        {
            return m_Width;
        }
        private int m_Width;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SetWidth(int value)
        {
            m_Width = value;
        }

    }

}
