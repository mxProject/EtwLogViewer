using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace mxProject.Tools.EtwLogViewer
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();

            EtwLogViewerConfig config = null;

            try
            {
                config = GetConfig();
            }
            catch ( Exception ex)
            {
                MessageBox.Show(ex.Message, "EtwLogViewer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new MainForm(config));

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static EtwLogViewerConfig GetConfig()
        {

            string filePath = "EtwLogViewer.config";

            if (File.Exists(filePath))
            {
                return EtwLogViewerConfig.LoadFromFile(filePath);
            }

            EtwLogViewerConfig config = EtwLogViewerConfig.CreateDefault();

            config.SaveToFile(filePath);

            return config;

        }

    }
}
