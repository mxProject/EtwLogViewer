using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace mxProject.Tools.EtwLogViewer
{

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public sealed class EtwLogViewerConfig
    {

        #region providers

        /// <summary>
        /// 
        /// </summary>
        public ProviderConfig[] Providers
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlType("Provider")]
        public sealed class ProviderConfig
        {

            /// <summary>
            /// 
            /// </summary>
            [XmlAttribute]
            public string FriendlyName
            {
                get;
                set;
            }

            /// <summary>
            /// 
            /// </summary>
            [XmlAttribute]
            public string Name
            {
                get;
                set;
            }

            /// <summary>
            /// 
            /// </summary>
            [XmlAttribute]
            public Guid ID
            {
                get;
                set;
            }

        }

        #endregion

        #region fields

        /// <summary>
        /// 
        /// </summary>
        public string[] KnownPayloads
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public FieldConfig[] Fields
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlType("Field")]
        public sealed class FieldConfig
        {

            /// <summary>
            /// 
            /// </summary>
            [XmlAttribute]
            public string Name
            {
                get;
                set;
            }

            /// <summary>
            /// 
            /// </summary>
            [XmlAttribute]
            public int Width
            {
                get;
                set;
            }

            /// <summary>
            /// 
            /// </summary>
            internal bool IsTraceEventPayload
            {
                get
                {
                    if (string.IsNullOrEmpty(this.Name)) { return false; }
                    return this.Name != "." && this.Name.StartsWith(".");
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            internal string GetTraceEventPayload()
            {
                if (!IsTraceEventPayload) { return null; }
                return this.Name.Substring(1);
            }

            /// <summary>
            /// 
            /// </summary>
            internal bool IsTraceEventField
            {
                get
                {
                    if (string.IsNullOrEmpty(this.Name)) { return false; }
                    return Enum.IsDefined(typeof(TraceEventFieldIndex), this.Name);
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            internal TraceEventFieldIndex GetTraceEventField()
            {
                if (!IsTraceEventField) { return TraceEventFieldIndex.Unknown; }
                return (TraceEventFieldIndex)Enum.Parse(typeof(TraceEventFieldIndex), this.Name);
            }

        }

        #endregion

        #region serialize

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        public void SaveToFile(string filePath)
        {

            XmlSerializer serializer = new XmlSerializer(typeof(EtwLogViewerConfig));

            XmlWriterSettings settings = new XmlWriterSettings();

            settings.Encoding = Encoding.GetEncoding(932);
            settings.Indent = true;
            settings.IndentChars = "\t";
            settings.NewLineChars = Environment.NewLine;

            using (XmlWriter writer = XmlWriter.Create(filePath, settings))
            {
                serializer.Serialize(writer, this);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static EtwLogViewerConfig LoadFromFile(string filePath)
        {

            XmlSerializer serializer = new XmlSerializer(typeof(EtwLogViewerConfig));

            XmlReaderSettings settings = new XmlReaderSettings();

            using (XmlReader reader = XmlReader.Create(filePath, settings))
            {
                return (EtwLogViewerConfig)serializer.Deserialize(reader);
            }

        }

        #endregion

        #region default setting

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal static EtwLogViewerConfig CreateDefault()
        {

            TraceEventListSetting listSetting = new TraceEventListSetting();

            EtwLogViewerConfig config = new EtwLogViewerConfig();

            // providers
            List<ProviderConfig> providers = new List<ProviderConfig>();

            providers.Add(new ProviderConfig() { FriendlyName = "provider1", Name = "providerName1", ID = Guid.NewGuid() });
            providers.Add(new ProviderConfig() { FriendlyName = "provider2", Name = "providerName2" });
            providers.Add(new ProviderConfig() { FriendlyName = "provider3", ID = Guid.NewGuid() });

            config.Providers = providers.ToArray();

            // payloads
            config.KnownPayloads = new string[] {"payloadName1", "payloadName2", "payloadName3" };

            // fields
            List<FieldConfig> fields = new List<FieldConfig>();

            foreach ( KeyValuePair<TraceEventFieldIndex, TraceEventField> field in TraceEventField.GetAllFields(listSetting, TraceEventListSetting.DefaultColumnWidth))
            {
                fields.Add(new FieldConfig() { Name = field.Key.ToString(), Width = field.Value.GetWidth() });
            }

            config.Fields = fields.ToArray();

            return config;

        }

        #endregion

    }

}
