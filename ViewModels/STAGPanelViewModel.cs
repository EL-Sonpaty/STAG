using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STAG.ViewModels
{
    internal class STAGPanelViewModel : BaseViewModel
    {
        private uint DocumentRuntimeSerialNumber { get; }
        public STAGPanelViewModel(uint documentSerialNumber)
        {
            DocumentRuntimeSerialNumber = documentSerialNumber;
            Rhino.UI.Panels.Show += OnShowPanel;
        }

        private void OnShowPanel(object sender, Rhino.UI.ShowPanelEventArgs e)
        {
            uint sn = e.DocumentSerialNumber;
        }

        public string NewStageName { get; set; }


        public ObservableCollection<Models.StageConstraint> StageConstraints { get; } = new ObservableCollection<Models.StageConstraint>();


    }
}