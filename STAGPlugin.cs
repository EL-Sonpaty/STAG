﻿using Eto.Forms;
using Rhino;
using Rhino.DocObjects;
using Rhino.PlugIns;
using Rhino.UI;
using STAG.Constants;
using STAG.Models;
using STAG.Views;
using STAG.Wrappers;
using System;
using System.Collections.Generic;
using System.Security.Policy;
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
        public override PlugInLoadTime LoadTime => PlugInLoadTime.AtStartup;

        public bool ListenToRhino = false;

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
            // initialize the STAGPanelViewModel
            STAGPanelViewModel = new ViewModels.STAGPanelViewModel(HardCodedData.DocumentSerialNumber);

            ListenToRhino = true;

            // initialize transformation object list and matrices.
            RevertTransformObjects = new List<RhinoObject>();
            LastTransformation = Rhino.Geometry.Transform.Unset;
            LastInverseTransformation = Rhino.Geometry.Transform.Unset;
        }

        ///<summary>Gets the only instance of the STAGPlugin plug-in.</summary>
        public static STAGPlugin Instance { get; private set; }

        public ViewModels.STAGPanelViewModel STAGPanelViewModel { get; private set; }


        private System.Drawing.Icon LoadIcon()
        {
            System.Drawing.Icon icon = null;

            try
            {
                // Load PNG from embedded resources
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                var resourceName = "STAG.EmbeddedResources.STAG.png";

                using (var stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream != null)
                    {
                        RhinoApp.WriteLine("Resource found! Loading icon...");

                        // Load the bitmap from stream
                        var bitmap = new System.Drawing.Bitmap(stream);

                        // Create icon from bitmap - don't dispose bitmap immediately
                        var iconHandle = bitmap.GetHicon();
                        icon = System.Drawing.Icon.FromHandle(iconHandle);

                        RhinoApp.WriteLine("Icon loaded successfully!");
                    }
                    else
                    {
                        RhinoApp.WriteLine($"Resource '{resourceName}' not found!");
                        icon = System.Drawing.SystemIcons.Application;
                    }
                }
            }
            catch (Exception ex)
            {
                RhinoApp.WriteLine($"Error loading embedded icon: {ex.Message}");
                RhinoApp.WriteLine($"Stack trace: {ex.StackTrace}");
                icon = System.Drawing.SystemIcons.Application;
            }

            return icon;
        }
        // You can override methods here to change the plug-in behavior on
        // loading and shut down, add options pages to the Rhino _Option command
        // and maintain plug-in wide options in a document.

        protected override Rhino.PlugIns.LoadReturnCode OnLoad(ref string errorMessage)
        {

            var icon = LoadIcon();
                        

            // Register the panel (only needs to be done once)
            Panels.RegisterPanel(
                this,
                typeof(STAGViewHost),
                "STAG",
                icon,
                PanelType.System
            );

            // Show the panel on startup
            //Panel T = Panels.GetPanel(typeof(STAGViewHost).GUID, HardCodedData.DocumentSerialNumber) as Panel;


            // Subscribe to the transform objects event
            RhinoDoc.BeforeTransformObjects += OnBeforeTransformObjects;
            RhinoDoc.ReplaceRhinoObject += onReplaceRhinoObject;
            RhinoDoc.AfterTransformObjects += onAfterTransformObjects;

            RhinoDoc.ModifyObjectAttributes += OnBeforeModifyAttributes;

            RhinoDoc.UserStringChanged += OnBeforeModifyUserStrings;

            return LoadReturnCode.Success;

        }

        public bool StartListening()
        {
            if (ListenToRhino == false)
            {
                ListenToRhino = true;
                RhinoDoc.BeforeTransformObjects += OnBeforeTransformObjects;
                RhinoDoc.ReplaceRhinoObject += onReplaceRhinoObject;
                RhinoDoc.AfterTransformObjects += onAfterTransformObjects;

                RhinoDoc.ModifyObjectAttributes += OnBeforeModifyAttributes;

                RhinoDoc.UserStringChanged += OnBeforeModifyUserStrings;

                return true;
            }
            return false;
        }

        public bool StopListening()
        {
            if (ListenToRhino == true)
            {
                ListenToRhino = false;
                RhinoDoc.BeforeTransformObjects -= OnBeforeTransformObjects;
                RhinoDoc.ReplaceRhinoObject -= onReplaceRhinoObject;
                RhinoDoc.AfterTransformObjects -= onAfterTransformObjects;

                RhinoDoc.ModifyObjectAttributes -= OnBeforeModifyAttributes;

                RhinoDoc.UserStringChanged -= OnBeforeModifyUserStrings;

                return true;
            }
            return false;
        }

        protected override void OnShutdown()
        {
            // Unsubscribe from events to prevent memory leaks
            StopListening();
            base.OnShutdown();
        }

        public bool CheckPermission(RhinoObject obj, ModificationType modifType)
        {
            
            string stageName = STAG_Core.GetStage(obj.Id);
            if (string.IsNullOrEmpty(stageName))
            {
                return true; // Default to true if no stage is assigned
            }

            ViewModels.StageConstraintViewModel stageModel = STAG_Core.GetStageConstraintByName(stageName);
            if (stageModel == null)
            {
                return true; // Default to true if no stage model is found
            }
            switch(modifType)
            {
                case ModificationType.Transform:
                    return stageModel.IsTranslationEditable;
                case ModificationType.Replace:
                    return stageModel.IsGeometricalEditable;
                case ModificationType.ModifyAttributes:
                    return stageModel.IsAttributesEditable;
                case ModificationType.ModifyUserStrings:
                    return stageModel.IsUserTextsEditable;
                default:
                    return true; // Default to false if no valid modification type is provided
            }


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

            //// is replaced alowed 
            //if (IsTransformAllowed == false)
            //{
            //    return; // Skip replacement if not allowed
            //}
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
                if (HasBeenProcessed.ContainsKey(obj.Id) && HasBeenProcessed[obj.Id] == true)
                {
                    continue; // Skip this object if it has been processed
                }
                // check permission
                bool permission = CheckPermission(obj, ModificationType.Transform);
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
                    // Add to HasBeenProcessed pool if it hasn't been added yet.
                    AddToProcessedPool(obj);

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
                bool permission = CheckPermission(obj, ModificationType.Replace);

                ObjRef objRef = new ObjRef(obj.Id);

                // block or let go
                if (permission == false)
                {
                    var oldGeom = oldObj.DuplicateGeometry();

                    Rhino.RhinoDoc.ActiveDoc.Objects.Replace(obj.Id, oldGeom, true);
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
                bool permission = CheckPermission(obj, ModificationType.ModifyAttributes);

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
            // TODO 
        }

    }
}