using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STAG.ViewModels
{
    internal class STAGPanelViewModel : Rhino.UI.ViewModel
    {
        private uint DocumentRuntimeSerialNumber { get; }
        private string m_message;
        private string m_settings_changed_message;
        private bool m_use_multiple_counters;

        public STAGPanelViewModel(uint documentSerialNumber)
        {
            DocumentRuntimeSerialNumber = documentSerialNumber;
            Rhino.UI.Panels.Show += OnShowPanel;
        }

        private void OnShowPanel(object sender, Rhino.UI.ShowPanelEventArgs e)
        {
            uint sn = e.DocumentSerialNumber;
        }
    }
}