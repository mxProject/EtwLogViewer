using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mxProject.Tools.EtwLogViewer
{

    /// <summary>
    /// List setting form.
    /// </summary>
    internal partial class TraceEventListSettingForm : Form
    {

        #region ctor

        /// <summary>
        /// 
        /// </summary>
        public TraceEventListSettingForm()
        {
            InitializeComponent();
            Init();
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        private void Init()
        {

            this.CancelButton = btnCancel;

            btnSelect.Click += BtnSelect_Click;
            btnDeselect.Click += BtnDeselect_Click;
            btnMoveUp.Click += BtnMoveUp_Click;
            btnMoveDown.Click += BtnMoveDown_Click;

            txtMaxCount.Validating += TxtMaxCount_Validating;

            btnOk.Click += BtnOk_Click;
            btnCancel.Click += BtnCancel_Click;

        }

        #region control eventHandlers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSelect_Click(object sender, EventArgs e)
        {
            MoveSelectedField(lstUnselected, lstSelected);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDeselect_Click(object sender, EventArgs e)
        {
            MoveSelectedField(lstSelected, lstUnselected);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMoveUp_Click(object sender, EventArgs e)
        {
            MoveUpField(lstSelected);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMoveDown_Click(object sender, EventArgs e)
        {
            MoveDownField(lstSelected);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtMaxCount_Validating(object sender, CancelEventArgs e)
        {
            if (!ValidateMaxRowCount())
            {
                e.Cancel = true;
            }
        }

        #endregion

        #region showDialog

        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        internal DialogResult ShowDialog(IWin32Window owner, TraceEventListSetting settings)
        {

            try
            {

                ShowFieldList(lstUnselected, settings.HideFields);
                ShowFieldList(lstSelected, settings.VisibleFields);
                SetMaxRowCount(settings.MaxRowCount);

                DialogResult result = base.ShowDialog(owner);

                if (result == DialogResult.OK)
                {
                    CopyItems(GetUnselectedFields(), settings.HideFields);
                    CopyItems(GetSelectedFields(), settings.VisibleFields);
                    settings.MaxRowCount = GetMaxRowCount();
                }

                return result;

            }
            finally
            {
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        private void CopyItems<T>(IList<T> source, IList<T> destination)
        {

            destination.Clear();

            for (int i = 0; i < source.Count; ++i)
            {
                destination.Add(source[i]);
            }

        }

        #endregion

        #region fields

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="fields"></param>
        private void ShowFieldList(ListView target, IList<ITraceEventField> fields)
        {

            target.Items.Clear();

            for (int i = 0; i < fields.Count; ++i)
            {
                target.Items.Add(CreateFieldListItem(fields[i]));
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private ListViewItem CreateFieldListItem(ITraceEventField field)
        {

            ListViewItem item = new ListViewItem(field.GetCaption())
            {
                Tag = field
            };

            return item;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        private void MoveSelectedField(ListView source, ListView destination)
        {

            if (source.SelectedItems.Count == 0) { return; }

            ListViewItem[] items = new ListViewItem[source.SelectedItems.Count];

            source.SelectedItems.CopyTo(items, 0);

            for (int i = 0; i < items.Length; ++i)
            {
                source.Items.Remove(items[i]);
                destination.Items.Add(items[i]);
                items[i].Selected = true;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listView"></param>
        private void MoveUpField(ListView listView)
        {

            if (listView.SelectedItems.Count == 0) { return; }

            ListViewItem[] items = new ListViewItem[listView.SelectedItems.Count];

            listView.SelectedItems.CopyTo(items, 0);

            if (items[0].Index == 0) { return; }

            for (int i = 0; i < items.Length; ++i)
            {
                int index = items[i].Index;
                items[i].Remove();
                listView.Items.Insert(index - 1, items[i]).Selected = true;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listView"></param>
        private void MoveDownField(ListView listView)
        {

            if (listView.SelectedItems.Count == 0) { return; }

            ListViewItem[] items = new ListViewItem[listView.SelectedItems.Count];

            listView.SelectedItems.CopyTo(items, 0);

            if (items[items.Length - 1].Index == listView.Items.Count - 1) { return; }

            for (int i = items.Length - 1; i >= 0; --i)
            {
                int index = items[i].Index;
                items[i].Remove();
                listView.Items.Insert(index + 1, items[i]).Selected = true;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IList<ITraceEventField> GetSelectedFields()
        {
            return GetFields(lstSelected);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IList<ITraceEventField> GetUnselectedFields()
        {
            return GetFields(lstUnselected);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private IList<ITraceEventField> GetFields(ListView target)
        {

            List<ITraceEventField> fields = new List<ITraceEventField>(target.Items.Count);

            foreach ( ListViewItem item in target.Items)
            {
                if (item.Tag is ITraceEventField field) { fields.Add(field); }
            }

            return fields;

        }

        #endregion

        #region max row count

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        private void SetMaxRowCount(int value)
        {
            txtMaxCount.Text = value.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int GetMaxRowCount()
        {
            if (!string.IsNullOrEmpty(txtMaxCount.Text) && int.TryParse(txtMaxCount.Text, out int value) && value >= 0)
            {
                return value;
            }
            else
            {
                return TraceEventListSetting.DefaultMaxRowCount;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool ValidateMaxRowCount()
        {
            if (!string.IsNullOrEmpty(txtMaxCount.Text) && int.TryParse(txtMaxCount.Text, out int value) && value >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

    }

}
