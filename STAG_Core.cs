using Rhino;
using Rhino.DocObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STAG

{

    public class STAG_Core

    {

        private readonly RhinoDoc _doc;

        public string STAG_KEY = "StagKey";

        private readonly List<string> _stageOrder = new List<string> { "Design", "Engineering", "Production", "Shipping" };

        public STAG_Core(RhinoDoc doc)

        {

            _doc = doc;

        }

        public void FillAllEmptyStagesToDefault()
        {
            // get all objects in the model 

            // retrieve the ones that have no stage 

            // set to first stage.
        }

        public void SetStage(List<Guid> rhinoIds, string newStage)

        {

            foreach (var id in rhinoIds)

            {

                var obj = _doc.Objects.FindId(id);

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

            _doc.Views.Redraw();

        }

        public void UpgradeObject(List<Guid> rhinoIds)

        {

            foreach (var id in rhinoIds)

            {

                var obj = _doc.Objects.FindId(id);

                if (obj == null)

                {

                    RhinoApp.WriteLine("Object not found.");

                    return;

                }

                string currentStage = obj.Attributes.GetUserString(STAG_KEY);

                int index = _stageOrder.IndexOf(currentStage);

                if (index != -1 && index < _stageOrder.Count - 1)

                {

                    string nextStage = _stageOrder[index + 1];

                    SetStage(new List<Guid> { id }, nextStage);

                }

                else

                {

                    RhinoApp.WriteLine($"Object {id} is already at highest stage or not recognized.");

                    return;

                }

                _doc.Views.Redraw();

            }

        }

        public void DowngradeObject(List<Guid> rhinoIds)

        {

            foreach (var id in rhinoIds)

            {

                var obj = _doc.Objects.FindId(id);

                if (obj == null)

                {

                    Console.WriteLine("Object is not found.");

                    return;

                }

                string currentStage = obj.Attributes.GetUserString(STAG_KEY);

                int index = _stageOrder.IndexOf(currentStage);

                if (index > 0)

                {

                    string previousStage = _stageOrder[index - 1];

                    SetStage(new List<Guid> { id }, previousStage);

                }

                else

                {

                    RhinoApp.WriteLine($"Object {id} is already at lowest stage or not recognized.");

                    return;

                }

            }

        }

    }

}

