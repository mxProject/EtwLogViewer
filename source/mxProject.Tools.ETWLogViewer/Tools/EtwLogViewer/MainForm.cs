using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.Diagnostics.Tracing;

namespace mxProject.Tools.EtwLogViewer
{

    /// <summary>
    /// 
    /// </summary>
    internal partial class MainForm : Form
    {

        #region ctor

        /// <summary>
        /// 
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            Init();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public MainForm(EtwLogViewerConfig config) : this()
        {
            m_Config = config ?? new EtwLogViewerConfig();
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        private void Init()
        {

            RegistEventHandlers();

            InitETW();

        }

        private EtwLogViewerConfig m_Config;

        #region control eventhandlers

        /// <summary>
        /// 
        /// </summary>
        private void RegistEventHandlers()
        {

            this.Load += Form_Load;
            this.FormClosing += Form_FormClosing;
            this.FormClosed += Form_FormClosed;

            mnuListSetting.Click += MnuListSetting_Click;

            lstProvider.ItemChecked += LstProvider_ItemChecked;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_Load(object sender, EventArgs e)
        {

            InitGridSetting();

            InitProviderList(this.lstProvider, m_ListSetting);
            InitGridView(this.grdLog, m_ListSetting);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_FormClosing = true;
        }

        private bool m_FormClosing;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (m_Context != null)
            {
                m_Context.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MnuListSetting_Click(object sender, EventArgs e)
        {

            using (TraceEventListSettingForm frm = new TraceEventListSettingForm())
            {
                if (frm.ShowDialog(this, m_ListSetting) != DialogResult.OK) { return; }
            }

            InitGridViewFields(grdLog, m_ListSetting);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LstProvider_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            RegistProvider(e.Item.Tag as EtwProvider, e.Item.Checked);
        }

        #endregion

        #region setting

        private TraceEventListSetting m_ListSetting;

        /// <summary>
        /// 
        /// </summary>
        private void InitGridSetting()
        {

            m_ListSetting = new TraceEventListSetting();

            // providers

            if (m_Config.Providers != null)
            {
                foreach (EtwLogViewerConfig.ProviderConfig provider in m_Config.Providers)
                {
                    m_ListSetting.Providers.Add(new EtwProvider(provider.FriendlyName, provider.Name, provider.ID));
                }
            }

            // fields

            IDictionary<TraceEventFieldIndex, TraceEventField> allFields = TraceEventField.GetAllFields(m_ListSetting, TraceEventListSetting.DefaultColumnWidth);

            IDictionary<string, TraceEventPayload> allPayloads = new Dictionary<string, TraceEventPayload>();

            if (m_Config.KnownPayloads != null)
            {
                foreach (string name in m_Config.KnownPayloads)
                {
                    allPayloads[name] = new TraceEventPayload(name, TraceEventListSetting.DefaultColumnWidth);
                }
            }

            m_ListSetting.AddHideFields(allFields.Values);
            m_ListSetting.AddHideFields(allPayloads.Values);

            if (m_Config.Fields != null)
            {
                foreach (EtwLogViewerConfig.FieldConfig field in m_Config.Fields)
                {
                    if (field.IsTraceEventField)
                    {
                        if (allFields.TryGetValue(field.GetTraceEventField(), out TraceEventField found))
                        {
                            m_ListSetting.VisibleFields.Add(found);
                            m_ListSetting.HideFields.Remove(found);
                            found.SetWidth(field.Width);
                        }
                    }
                    else if (field.IsTraceEventPayload)
                    {
                        if (allPayloads.TryGetValue(field.GetTraceEventPayload(), out TraceEventPayload found))
                        {
                            m_ListSetting.VisibleFields.Add(found);
                            m_ListSetting.HideFields.Remove(found);
                            found.SetWidth(field.Width);
                        }
                        else
                        {
                            m_ListSetting.VisibleFields.Add(new TraceEventPayload(field.GetTraceEventPayload(), field.Width));
                        }
                    }
                }
            }

        }

        #endregion

        #region provider list

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listView"></param>
        /// <param name="setting"></param>
        private void InitProviderList(ListView listView, TraceEventListSetting setting)
        {

            listView.BeginUpdate();

            try
            {

                listView.Items.Clear();

                foreach (EtwProvider provider in setting.Providers)
                {
                    if (provider == null) { continue; }
                    listView.Items.Add(CreateProviderListItem(provider));
                }

            }
            finally
            {
                listView.EndUpdate();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        private ListViewItem CreateProviderListItem(EtwProvider provider)
        {

            ListViewItem item = new ListViewItem(new string[] {
                provider.FriendlyName
                , provider.Name
                , provider.ID == Guid.Empty ? "" : provider.ID.ToString()
            })
            {
                Tag = provider
            };

            return item;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="regist"></param>
        private void RegistProvider(EtwProvider provider, bool regist)
        {

            if (provider == null) { return; }

            try
            {

                Cursor = Cursors.WaitCursor;

                if (regist)
                {
                    m_Context.RegistProvider(provider.GetNameOrID());
                }
                else
                {
                    m_Context.UnregistProvider(provider.GetNameOrID());
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }

        }

        #endregion

        #region grid

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gridView"></param>
        /// <param name="setting"></param>
        private void InitGridView(DataGridView gridView, TraceEventListSetting setting)
        {

            gridView.ReadOnly = true;
            gridView.MultiSelect = true;
            gridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridView.AllowUserToAddRows = false;
            gridView.AllowUserToDeleteRows = false;

            gridView.RowCount = 0;
            gridView.CellValueNeeded += GridView_CellValueNeeded;
            gridView.VirtualMode = true;

            InitGridViewFields(gridView, setting);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gridView"></param>
        /// <param name="fields"></param>
        private void InitGridViewFields(DataGridView gridView, TraceEventListSetting setting)
        {

            m_Context.SetMaxTraceEventCount(setting.MaxRowCount);

            gridView.Columns.Clear();

            foreach (ITraceEventField field in setting.VisibleFields)
            {

                int width = field.GetWidth();
                if (width <= 0) { width = TraceEventListSetting.DefaultColumnWidth; }

                gridView.Columns[gridView.Columns.Add(field.GetCaption(), field.GetCaption())].Width = width;

            }

            gridView.RowCount = m_Context.CurrentTraceEventCount;

            gridView.Invalidate();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {

            if (e.ColumnIndex < m_ListSetting.VisibleFields.Count)
            {
                if (e.RowIndex < m_Context.CurrentTraceEventCount)
                {
                    TraceEvent log = m_Context.GetTraceEventAt(e.RowIndex);
                    e.Value = m_ListSetting.VisibleFields[e.ColumnIndex].GetValue(log);
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private void RefreshGridView()
        {

            int count = m_Context.CurrentTraceEventCount;

            grdLog.RowCount = count;

            if (count > 0)
            {
                grdLog.FirstDisplayedScrollingRowIndex = count - 1;
            }

            grdLog.Invalidate();

        }

        #endregion

        #region ETW

        private EtwContext m_Context;

        /// <summary>
        /// 
        /// </summary>
        private void InitETW()
        {

            EtwContext.Initialize();

            m_Context = new EtwContext(10, null, OnReceivedLog);

            m_Context.MaxTraceEventCountChanged += ETwContext_MaxTraceEventCountChanged;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ETwContext_MaxTraceEventCountChanged(object sender, EventArgs e)
        {

            if (InvokeRequired)
            {
                Invoke(new EventHandler(ETwContext_MaxTraceEventCountChanged), new object[] { sender, e });
                return;
            }

            RefreshGridView();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="traceEvent"></param>
        private void OnReceivedLog(TraceEvent traceEvent)
        {

            if (m_FormClosing) { return; }
            if (IsDisposed) { return; }

            if (m_Context.MaxTraceEventCount < 1) { return; }

            if (InvokeRequired)
            {
                try
                {
                    Invoke(new Action<TraceEvent>(OnReceivedLog), new object[] { traceEvent });
                }
                catch (ObjectDisposedException)
                {
                    System.Diagnostics.Debug.WriteLine("ETWContext_Received: This form has already been disposed.");
                }
                return;
            }

            RefreshGridView();

        }

        #endregion

    }

}
