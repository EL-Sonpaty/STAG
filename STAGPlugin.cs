using System;
using System.Collections.Generic;
using System.Security.Policy;
using Eto.Forms;
using Rhino;
using Rhino.DocObjects;
using Rhino.PlugIns;
using static Rhino.RhinoDoc;

namespace STAG
{
    ///<summary>
    /// <para>Every RhinoCommon .rhp assembly must have one and only one PlugIn-derived
    /// class. DO NOT create instances of this class yourself. It is the
    /// responsibility of Rhino to create an instance of this class.</para>
    /// <para>To complete plug-in information, please also see all PlugInDescription
    /// attributes in AssemblyInfo.cs (you might need to click "Project" ->
    /// "Show All Files" to see it in the "Solution Explorer" window).</para>
    ///</summary>
    public class STAGPlugin : Rhino.PlugIns.PlugIn
    {

        public string LOCKED_BY_STAG_KEY = "TempStagLocked";
        public string LOCKED_BY_STAG_VALUE = "This object was locked by STAG plugin. It should have been unlocked by something went wrong, you can delete this usertext.";

        public Dictionary<Guid, bool> HasBeenProcessed = new Dictionary<Guid, bool>();

        public enum ModificationType
        {
            Transform,
            Replace,
            ModifyAttributes,
            ModifyUserStrings
        }

        public bool IsTransformAllowed => true;
        public bool IsReplaceAllowed => true;
        public bool IsModifyAttributesAllowed => true;
        public bool IsModifyUserStringsAllowed => true;

        public STAGPlugin()
        {
            Instance = this;

            // initialize transformation object list and matrices.
            RevertTransformObjects = new List<RhinoObject>();
            LastTransformation = Rhino.Geometry.Transform.Unset;
            LastInverseTransformation = Rhino.Geometry.Transform.Unset;
        }

        ///<summary>Gets the only instance of the STAGPlugin plug-in.</summary>
        public static STAGPlugin Instance { get; private set; }

        // You can override methods here to change the plug-in behavior on
        // loading and shut down, add options pages to the Rhino _Option command
        // and maintain plug-in wide options in a document.

        protected override Rhino.PlugIns.LoadReturnCode OnLoad(ref string errorMessage)
        {
            // Subscribe to the transform objects event
            RhinoDoc.BeforeTransformObjects += OnBeforeTransformObjects;
            RhinoDoc.ReplaceRhinoObject += onReplaceRhinoObject;
            RhinoDoc.AfterTransformObjects += onAfterTransformObjects;

            RhinoDoc.ModifyObjectAttributes += OnBeforeModifyAttributes;

            RhinoDoc.UserStringChanged += OnBeforeModifyUserStrings;

            return LoadReturnCode.Success;

        }

        protected override void OnShutdown()
        {
            // Unsubscribe from events to prevent memory leaks
            RhinoDoc.BeforeTransformObjects -= OnBeforeTransformObjects;
            base.OnShutdown();
        }

        public bool CheckPermission(RhinoObject obj)
        {
            return false;
        }

        public void AddToProcessedPool(RhinoObject obj)
        {
            Guid id = obj.Id;
            if (HasBeenProcessed.ContainsKey(id) == false)
            {
                HasBeenProcessed.Add(id, true);
            }
            else
            {
                HasBeenProcessed[id] = true;
            }

        }

        public void ClearProcessedPool()
        {
            HasBeenProcessed.Clear();
        }

        public List<RhinoObject> RevertTransformObjects;
        public Rhino.Geometry.Transform LastTransformation;
        public Rhino.Geometry.Transform LastInverseTransformation;


        // listen to object transformation events
        public void OnBeforeTransformObjects(object sender, RhinoTransformObjectsEventArgs e)
        {

            // is replaced alowed 
            if (IsTransformAllowed == false)
            {
                return; // Skip replacement if not allowed
            }
            Rhino.RhinoApp.WriteLine("OnBeforeTransformObjects");

            // Reset the last transformation and inverse transformation
            LastTransformation = Rhino.Geometry.Transform.Unset;
            LastInverseTransformation = Rhino.Geometry.Transform.Unset;
            RevertTransformObjects.Clear();

            // Access the objects being transformed
            RhinoObject[] objects = e.Objects;

            LastTransformation = e.Transform;
            bool InverseSuccess = LastTransformation.TryGetInverse(out LastInverseTransformation);


            // You can also access object IDs
            foreach (var obj in e.Objects)
            {
                // Check if the object is already processed
                if(HasBeenProcessed.ContainsKey(obj.Id) && HasBeenProcessed[obj.Id] == true)
                {
                    continue; // Skip this object if it has been processed
                }
                // check permission
                bool permission = CheckPermission(obj);
                // block or let go
                if (permission == false)
                {
                    
                    if (RevertTransformObjects.Contains(obj) == false)
                    {
                        RevertTransformObjects.Add(obj);
                        AddToProcessedPool(obj);
        
                    }
                }
                else
                {
                    RhinoApp.WriteLine("Transformation allowed.");
                }
            }

        }

        public void onAfterTransformObjects(object sender, RhinoAfterTransformObjectsEventArgs e)
        {
            Rhino.RhinoApp.WriteLine("onAfterTransformObjects");

            if (RevertTransformObjects != null && RevertTransformObjects.Count > 0)
            {
                foreach (var obj in RevertTransformObjects)
                {
                    RhinoDoc.ActiveDoc.Objects.Transform(obj.Id, LastInverseTransformation, true);

                    // if we reached this point, remove the object from the processed pool
                    if (HasBeenProcessed.ContainsKey(obj.Id))
                    {
                        HasBeenProcessed.Remove(obj.Id);
                    }
                }
                RhinoApp.WriteLine($"Blocked tranformation for {RevertTransformObjects.Count} objects.");
            }
        }

        public void onReplaceRhinoObject(object sender, RhinoReplaceObjectEventArgs e)
        {
            // is replaced alowed 
            if (IsReplaceAllowed == false)
            {
                return; // Skip replacement if not allowed
            }

            Rhino.RhinoApp.WriteLine("onReplaceRhinoObject");
            // Access the object being replaced
            RhinoObject oldObj = e.OldRhinoObject;
            RhinoObject newObj = e.NewRhinoObject;

            if (oldObj != null && newObj != null)
            {
                // has this object already been processed ? 
                if (HasBeenProcessed.ContainsKey(e.ObjectId) && HasBeenProcessed[e.ObjectId] == true)
                {
                    //if we reached this point, remove the object from the processed pool
                    if (HasBeenProcessed.ContainsKey(e.ObjectId))
                    {
                        HasBeenProcessed.Remove(e.ObjectId);
                    }
                    return; // Skip this object if it has been processed
                }
                // Add to processed pool if it hasn't been added yet.
                AddToProcessedPool(oldObj);

                // check permission
                RhinoObject obj = RhinoDoc.ActiveDoc.Objects.Find(e.ObjectId);
                bool permission = CheckPermission(obj);

                ObjRef objRef = new ObjRef(obj.Id);

                // block or let go
                if (permission == false)
                {
                    var oldGeom = oldObj.DuplicateGeometry();

                    Rhino.RhinoDoc.ActiveDoc.Objects.Replace(obj.Id, oldGeom    , true);
                    obj = e.OldRhinoObject;
                    obj.CommitChanges();
                    RhinoApp.WriteLine($"Blocked Operation for ID: {obj.Id}");
                }
            }
        }

        // listen to attribute modification events
        private void OnBeforeModifyAttributes(object sender, RhinoModifyObjectAttributesEventArgs e)
        {
            if (IsModifyAttributesAllowed == false) return;
            // Access the object being transformed
            RhinoObject obj = e.RhinoObject;

            // what has been modified
            if (obj != null)
            {

                var oldAtt = e.OldAttributes;
                var newAtt = e.NewAttributes;

                // check permission
                bool permission = CheckPermission(obj);

                // block or let go
                if (permission == false)
                {
                    // revert to "old attributes" if permission is not granted
                    obj.Attributes = oldAtt;
                    RhinoApp.WriteLine($"Blocked attribute modification for ID: {obj.Id}");
                }
                else
                {
                    RhinoApp.WriteLine($"Object ID: {obj.Id}");
                }
            }
        }

        public void OnBeforeModifyUserStrings(object sender, UserStringChangedArgs e)
        {
            if (IsModifyUserStringsAllowed == false) return;

            RhinoApp.WriteLine($"Changing user string");
        }

    }
}