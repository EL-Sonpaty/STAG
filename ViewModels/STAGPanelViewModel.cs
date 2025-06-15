using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace STAG.ViewModels
{
    public class STAGPanelViewModel : BaseViewModel
    {
        private uint DocumentRuntimeSerialNumber { get; }

        public STAGPanelViewModel(uint documentSerialNumber)
        {
            DocumentRuntimeSerialNumber = documentSerialNumber;
            Rhino.UI.Panels.Show += OnShowPanel;
            StageConstraints = new ObservableCollection<Models.StageConstraintViewModel>
            {
                new Models.StageConstraintViewModel { StageName = "Stage 1" },
                new Models.StageConstraintViewModel { StageName = "Stage 2" },
                new Models.StageConstraintViewModel { StageName = "Stage 3" }
            };
        }

        private void OnShowPanel(object sender, Rhino.UI.ShowPanelEventArgs e)
        {
            uint sn = e.DocumentSerialNumber;
        }

        private string _newStageName;
        public string NewStageName
        {
            get => _newStageName;
            set
            {
                if (_newStageName != value)
                {
                    SetProperty(value, ref  _newStageName, nameof(NewStageName));
                }
            }
        }

        public ICommand AddNewStageCommand => new RelayCommand(AddNewStage);
        private void AddNewStage()
        {
            if (string.IsNullOrWhiteSpace(NewStageName))
            {
                return;
            }

            // Check if a stage with the same name already exists
            if (StageConstraints.Any(s => s.StageName.Equals(NewStageName, StringComparison.OrdinalIgnoreCase)))
            {
                System.Windows.MessageBox.Show("A stage with this name already exists.", "Duplicate Stage Name", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            var newStage = new Models.StageConstraintViewModel
            {
                StageName = NewStageName
            };

            StageConstraints.Add(newStage);
        }

        public ICommand RemoveStageCommand => new RelayCommand<Models.StageConstraintViewModel>(RemoveStage);

        private void RemoveStage(Models.StageConstraintViewModel stage)
        {
            if (stage != null && StageConstraints.Contains(stage))
            {
                StageConstraints.Remove(stage);
            }
        }

        public ICommand MoveStageUpCommand => new RelayCommand<Models.StageConstraintViewModel>(MoveStageUp);
        private void MoveStageUp(Models.StageConstraintViewModel stage)
        {
            int index = StageConstraints.IndexOf(stage);
            if (index > 0)
            {
                StageConstraints.Move(index, index - 1);
            }
        }

        public ICommand MoveStageDownCommand => new RelayCommand<Models.StageConstraintViewModel>(MoveStageDown);
        private void MoveStageDown(Models.StageConstraintViewModel stage)
        {
            int index = StageConstraints.IndexOf(stage);
            if (index < StageConstraints.Count - 1)
            {
                StageConstraints.Move(index, index + 1);
            }
        }

        public ObservableCollection<Models.StageConstraintViewModel> StageConstraints { get; set; }
    }
}