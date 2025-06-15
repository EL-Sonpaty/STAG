using Rhino.Commands;
using Rhino;
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
        string STAG_BAKE_DOT = "StageTempDot";
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

        public ICommand SetDefaultValueCommand => new RelayCommand(SetDefaultValue);

        private void SetDefaultValue()
        {
            // get all objects in the model
            STAG_Core.FillAllEmptyStagesToDefault();
        }

        public ICommand DowngradeSelectedObjectsStagesCommand => new RelayCommand(DowngradeSelectedObjectsStages);
        public void DowngradeSelectedObjectsStages()
        {
            // get objects from rhino 
            var ids = STAG_Core.GetSelectedIds();
            // make sure the list is not empty
            if (ids.Count == 0)
            {
                RhinoApp.WriteLine("No objects selected.");
            }

            // upgrade stage
            STAG_Core.DowngradeStage(ids);
        }

        public ICommand UpgradeSelectedObjectsStagesCommand => new RelayCommand(UpgradeSelectedObjectsStages);
        public void UpgradeSelectedObjectsStages()
        {
            // get objects from rhino 
            var ids = STAG_Core.GetSelectedIds();
            // make sure the list is not empty
            if (ids.Count == 0)
            {
                RhinoApp.WriteLine("No objects selected.");
            }

            // downgrade stage
            STAG_Core.UpgradeStage(ids);
        }

        public ICommand ShowCurrentStagesCommand => new RelayCommand(ShowCurrentStages);
        private void ShowCurrentStages()
        {
            // get all objects in the model with a stage
            foreach (var obj in Rhino.RhinoDoc.ActiveDoc.Objects)
            {
                // get the object usertexts 
                string stage = STAG_Core.GetStage(obj.Id);
                if (!string.IsNullOrEmpty(stage))
                {
                    // get the bounding box 
                    var bbox = obj.Geometry.GetBoundingBox(false);
                    // get the center point of the bounding box
                    var center = bbox.Center;
                    // add a text dot in the model 
                    var dot = new Rhino.Geometry.TextDot(stage, center);
                    // add the text dot to the model
                    var id = Rhino.RhinoDoc.ActiveDoc.Objects.AddTextDot(dot);
                    // add a user string to the text dot with the object id
                    Rhino.RhinoDoc.ActiveDoc.Objects.FindId(id)?.Attributes.SetUserString(STAG_BAKE_DOT, obj.Id.ToString());
                }
            }
            // redraw the document
            Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
        }

        public ICommand ClearCurrentStagesDotsCommand => new RelayCommand(ClearCurrentStagesDots);
        private void ClearCurrentStagesDots()
        {
            // get all objects in the model with a stage
            foreach (var obj in Rhino.RhinoDoc.ActiveDoc.Objects)
            {
                // check if the object has a user string with the STAG_BAKE_DOT key
                if (obj.Attributes.GetUserString(STAG_BAKE_DOT) != null)
                {
                    // delete the object
                    Rhino.RhinoDoc.ActiveDoc.Objects.Delete(obj.Id, true);
                }
            }
            // redraw the document
            Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
        }

        public ICommand ShowGraphCommand => new RelayCommand(ShowGraph);
        private void ShowGraph()
        {
            //Add a window message Box
            System.Windows.MessageBox.Show("Busted!", "The graphs are not implemented yet", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
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
        public ICommand StageInputKeyDownCommand => new RelayCommand<KeyEventArgs>(OnStageInputKeyDown);

        private void OnStageInputKeyDown(KeyEventArgs e)
        {
            // Safety check in case parameter is null
            if (e == null)
                return;

            if (e.Key == Key.Enter)
            {

                if (AddNewStageCommand.CanExecute(null))
                    AddNewStageCommand.Execute(null);

                e.Handled = true;
            }
        }
    }
}