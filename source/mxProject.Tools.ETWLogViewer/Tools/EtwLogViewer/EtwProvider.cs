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
            FriendlyName = friendlyName;
            Name = name;
            ID = id;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 
        /// </summary>
        public Guid ID { get; }

        /// <summary>
        /// 
        /// </summary>
        public string FriendlyName { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal string GetNameOrID()
        {
            if ( ID == Guid.Empty)
            {
                return Name;
            }
            else
            {
                return ID.ToString();
            }
        }

    }

}
