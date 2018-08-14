using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mxProject.Tools.EtwLogViewer
{

    /// <summary>
    /// 
    /// </summary>
    internal sealed class TraceEventListSetting
    {

        #region providers

        /// <summary>
        /// 
        /// </summary>
        internal IList<EtwProvider> Providers
        {
            get { return m_Providers; }
        }
        private readonly List<EtwProvider> m_Providers = new List<EtwProvider>();

        #endregion

        #region fields

        /// <summary>
        /// 
        /// </summary>
        internal IList<ITraceEventField> HideFields
        {
            get { return m_HideFields; }
        }
        private readonly List<ITraceEventField> m_HideFields = new List<ITraceEventField>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fields"></param>
        internal void AddHideFields(IEnumerable<ITraceEventField> fields)
        {
            m_HideFields.AddRange(fields);
        }

        /// <summary>
        /// 
        /// </summary>
        internal void ClearHideFields()
        {
            m_HideFields.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        internal IList<ITraceEventField> VisibleFields
        {
            get { return m_VisibleFields; }
        }
        private readonly List<ITraceEventField> m_VisibleFields = new List<ITraceEventField>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fields"></param>
        internal void AddVisibleFields(IEnumerable<ITraceEventField> fields)
        {
            m_VisibleFields.AddRange(fields);
        }

        /// <summary>
        /// 
        /// </summary>
        internal void ClearVisibleFields()
        {
            m_VisibleFields.Clear();
        }

        #endregion

        #region misc

        /// <summary>
        /// 
        /// </summary>
        public int MaxRowCount
        {
            get { return m_MaxRowCount; }
            set { m_MaxRowCount = value; }
        }
        private int m_MaxRowCount = DefaultMaxRowCount;

        /// <summary>
        /// 
        /// </summary>
        internal static readonly int DefaultMaxRowCount = 1000;

        /// <summary>
        /// 
        /// </summary>
        internal static readonly int DefaultColumnWidth = 100;

        #endregion

    }

}
