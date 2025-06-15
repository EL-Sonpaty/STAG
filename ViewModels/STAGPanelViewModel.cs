using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
                System.Diagnostics.Debug.WriteLine($"NewStageName setter called: old='{_newStageName}', new='{value}'");
                if (_newStageName != value)
                {
                    _newStageName = value;
                    OnPropertyChanged(nameof(NewStageName));
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

            var newStage = new Models.StageConstraint
            {
                StageName = NewStageName
            };

            StageConstraints.Add(newStage);
        }

        public ICommand RemoveStageCommand => new RelayCommand<Models.StageConstraint>(RemoveStage);

        private void RemoveStage(Models.StageConstraint stage)
        {
            if (stage != null && StageConstraints.Contains(stage))
            {
                StageConstraints.Remove(stage);
            }
        }

        public ICommand MoveStageUpCommand => new RelayCommand<Models.StageConstraint>(MoveStageUp);
        private void MoveStageUp(Models.StageConstraint stage)
        {
            int index = StageConstraints.IndexOf(stage);
            if (index > 0)
            {
                StageConstraints.Move(index, index - 1);
            }
        }

        public ICommand MoveStageDownCommand => new RelayCommand<Models.StageConstraint>(MoveStageDown);
        private void MoveStageDown(Models.StageConstraint stage)
        {
            int index = StageConstraints.IndexOf(stage);
            if (index < StageConstraints.Count - 1)
            {
                StageConstraints.Move(index, index + 1);
            }
        }

        public ObservableCollection<Models.StageConstraint> StageConstraints { get; } = new ObservableCollection<Models.StageConstraint>();
    }
}