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
    /// <param name="sender"></param>
    /// <param name="e"></param>
    internal delegate void TraceEventHandler(object sender, TraceEventArgs e);

    /// <summary>
    /// 
    /// </summary>
    internal class TraceEventArgs : EventArgs
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="traceEvent"></param>
        public TraceEventArgs(TraceEvent traceEvent) : base()
        {
            m_TraceEvent = traceEvent;
        }

        /// <summary>
        /// 
        /// </summary>
        public TraceEvent TraceEvent
        {
            get { return m_TraceEvent; }
        }
        private TraceEvent m_TraceEvent;

    }

}
