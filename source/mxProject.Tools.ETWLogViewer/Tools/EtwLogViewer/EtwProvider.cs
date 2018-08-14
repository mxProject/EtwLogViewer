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
    internal sealed class EtwProvider
    {

        #region ctor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="friendlyName"></param>
        /// <param name="name"></param>
        public EtwProvider(string friendlyName, string name) : this(friendlyName, name, Guid.Empty)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="friendlyName"></param>
        /// <param name="id"></param>
        public EtwProvider(string friendlyName, Guid id) : this(friendlyName, null, id)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="friendlyName"></param>
        /// <param name="name"></param>
        /// <param name="id"></param>
        public EtwProvider(string friendlyName, string name, Guid id)
        {
            m_FriendlyName = friendlyName;
            m_Name = name;
            m_ID = id;
        }

        #endregion

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
        public Guid ID
        {
            get { return m_ID; }
        }
        private Guid m_ID;

        /// <summary>
        /// 
        /// </summary>
        public string FriendlyName
        {
            get { return m_FriendlyName; }
        }
        private string m_FriendlyName;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal string GetNameOrID()
        {
            if ( m_ID == Guid.Empty)
            {
                return m_Name;
            }
            else
            {
                return m_ID.ToString();
            }
        }

    }

}
