using Rhino;
using Rhino.DocObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
namespace STAG
{
    public class STAG_Core
    {
        public const string STAG_KEY = "StagKey";
        private  static  List<string> _stageOrder = new List<string> { "Design", "Engineering", "Production", "Shipping" };
        public STAG_Core(RhinoDoc doc)
        {
        }
        public static void FillAllEmptyStagesToDefault()
        {
            List<Guid> MissingStageIds = new List<Guid>();
            foreach (var obj in Rhino.RhinoDoc.ActiveDoc.Objects.GetObjectList(Rhino.DocObjects.ObjectType.AnyObject))
            {
                string currentStage = obj.Attributes.GetUserString(STAG_KEY);
                if (string.IsNullOrEmpty(currentStage))
                {
                    string firstName = STAGPlugin.Instance.STAGPanelViewModel.StageNames.First();
                    obj.Attributes.SetUserString(STAG_KEY, firstName);
                    obj.CommitChanges();
                    RhinoApp.WriteLine($"Object {obj.Id} stage set to '{firstName}'.");
                }
            }
            Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
        }

        // This probably belong somewhere else.
        public static List<Guid> GetSelectedIds()
        {
            var selectedObjects = Rhino.RhinoDoc.ActiveDoc.Objects.GetSelectedObjects(false, false);
            List<Guid> rhinoIds = new List<Guid>();
            if (selectedObjects != null)
            {
                foreach (var obj in selectedObjects)
                {
                    rhinoIds.Add(obj.Id);
                }
            }
            return rhinoIds;
        }
        private static void SetStage(List<Guid> rhinoIds, string newStage)
        {
            foreach (var id in rhinoIds)
            {
                var obj = Rhino.RhinoDoc.ActiveDoc.Objects.FindId(id);
                if (obj != null)
                {
                    obj.Attributes.SetUserString(STAG_KEY, newStage);
                    obj.CommitChanges();
                    RhinoApp.WriteLine($"Object {id} stage set to '{newStage}'.");
                }
                else
                {
                    RhinoApp.WriteLine("Object not found.");
                    return;
                }
            }
            Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
        }

        public static string GetStage(Guid rhinoId, bool setIfMissing = false)
        {
            var obj = Rhino.RhinoDoc.ActiveDoc.Objects.FindId(rhinoId);
            if (obj != null)
            {
                string stage = obj.Attributes.GetUserString(STAG_KEY);
                if (stage == null)
                {
                    if (setIfMissing)
                    {
                        stage = STAGPlugin.Instance.STAGPanelViewModel.StageNames.First();
                        obj.Attributes.SetUserString(STAG_KEY, stage);
                        obj.CommitChanges();
                        RhinoApp.WriteLine($"Object {rhinoId} stage set to '{stage}'.");
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else
                {
                    RhinoApp.WriteLine($"Object {rhinoId} is at stage '{stage}'.");
                }
                return stage ?? string.Empty;
            }
            else
            {
                RhinoApp.WriteLine("Object not found.");
                return null;
            }
        }

        public static void UpgradeStage(List<Guid> rhinoIds)
        {
            var stageNames = STAGPlugin.Instance.STAGPanelViewModel.StageNames;
            foreach (var id in rhinoIds)
            {
                var obj = Rhino.RhinoDoc.ActiveDoc.Objects.FindId(id);
                if (obj == null)
                {
                    RhinoApp.WriteLine("Object not found.");
                    return;
                }
                string currentStage = GetStage(obj.Id);
                int index = stageNames.IndexOf(currentStage);
                if (index != -1 && index < stageNames.Count - 1)
                {
                    string nextStage = stageNames[index + 1];
                    SetStage(new List<Guid> { id }, nextStage);
                }
                else
                {
                    RhinoApp.WriteLine($"Object {id} is already at highest stage or not recognized.");
                    return;
                }
                Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
            }
        }
        public static void DowngradeStage(List<Guid> rhinoIds)
        {
            var stageNames = STAGPlugin.Instance.STAGPanelViewModel.StageNames;

            foreach (var id in rhinoIds)
            {
                var obj = Rhino.RhinoDoc.ActiveDoc.Objects.FindId(id);
                if (obj == null)
                {
                    Console.WriteLine("Object is not found.");
                    return;
                }
                string currentStage = GetStage(obj.Id);
                int index = stageNames.IndexOf(currentStage);
                if (index > 0)
                {
                    string previousStage = stageNames[index - 1];
                    SetStage(new List<Guid> { id }, previousStage);
                }
                else
                {
                    RhinoApp.WriteLine($"Object {id} is already at lowest stage or not recognized.");
                    return;
                }
            }
        }

        public static void SelectObjectsForStage(string stageName)
        {
            var stageNames = STAGPlugin.Instance.STAGPanelViewModel.StageNames;

            if (!stageNames.Contains(stageName))
            {
                RhinoApp.WriteLine($"Stage '{stageName}' is not recognized.");
                return;
            }

            var objects = Rhino.RhinoDoc.ActiveDoc.Objects.GetObjectList(Rhino.DocObjects.ObjectType.AnyObject);
            List<Guid> selectedIds = new List<Guid>();

            foreach (var obj in objects)
            {
                string stage = GetStage(obj.Id);
                if (stage == stageName)
                {
                    selectedIds.Add(obj.Id);
                }
            }

            if (selectedIds.Count > 0)
            {
                Rhino.RhinoDoc.ActiveDoc.Objects.Select(selectedIds);
                RhinoApp.WriteLine($"Selected {selectedIds.Count} objects at stage '{stageName}'.");
            }
            else
            {
                RhinoApp.WriteLine($"No objects found at stage '{stageName}'.");
            }

            // Redraw Rhino view 
            Rhino.RhinoDoc.ActiveDoc.Views.Redraw();

        }

        public static Dictionary<string, int> CountObjectsByStages()
        {
            Dictionary<string, int> count = new Dictionary<string, int>();

            var objects = Rhino.RhinoDoc.ActiveDoc.Objects.GetObjectList(Rhino.DocObjects.ObjectType.AnyObject);

            foreach (var obj in objects)
            {
                string stage = GetStage(obj.Id);
                if (string.IsNullOrEmpty(stage))
                {
                    continue;
                }
                if (count.ContainsKey(stage))
                {
                    count[stage]++;
                }
                else
                {
                    count[stage] = 1;
                }
            }
            return count;
        }

        public static ViewModels.StageConstraintViewModel GetStageConstraintByName(string stageName)
        {
            return STAGPlugin.Instance.STAGPanelViewModel.StageConstraints.FirstOrDefault(sc => sc.StageName == stageName);
        }



    }
}