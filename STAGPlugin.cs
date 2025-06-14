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

        public STAGPlugin()
        {
            Instance = this;
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


        public List<RhinoObject> RevertTransformObjects;
        public Rhino.Geometry.Transform LastTransformation;
        public Rhino.Geometry.Transform LastInverseTransformation;


        // listen to object transformation events
        public void OnBeforeTransformObjects(object sender, RhinoTransformObjectsEventArgs e)
        {
            LastTransformation = Rhino.Geometry.Transform.Unset;
            LastInverseTransformation = Rhino.Geometry.Transform.Unset;
            RevertTransformObjects.Clear();

            // Handle the transform event
            RhinoApp.WriteLine($"Transform event: {e.Objects.Length} objects transformed");

            // Access the objects being transformed
            RhinoObject[] objects = e.Objects;

            LastTransformation = e.Transform;
            bool InverseSuccess = LastTransformation.TryGetInverse(out LastInverseTransformation);


            // You can also access object IDs
            foreach (var obj in e.Objects)
            {
                RhinoApp.WriteLine($"Object ID: {obj.Id}");
                // check permission
                bool permission = false;
                // block or let go
                if (permission == false)
                {
                    //BlockObject(obj);
                    if (RevertTransformObjects.Contains(obj) == false)
                    {
                        RevertTransformObjects.Add(obj);
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

            foreach (var obj in RevertTransformObjects)
            {
                // unblock object
                //UnblockObject(obj);
                RhinoDoc.ActiveDoc.Objects.Transform(obj.Id, LastInverseTransformation, true);

            }

        }

        // listen to attribute modification events
        private void OnBeforeModifyAttributes(object sender, RhinoModifyObjectAttributesEventArgs e)
        {
            // Access the object being transformed
            RhinoObject obj = e.RhinoObject;

            // what has been modified
            if (obj != null)
            {

                var oldAtt = e.OldAttributes;
                var newAtt = e.NewAttributes;

                // check permission
                bool permission = false;

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
            RhinoApp.WriteLine($"Changing user string");
        }

        public void BlockObject(RhinoObject obj)
        {
            // check if the object was locked before, if locked, we don't need to handle it.
            if (obj.IsLocked)
            {
                return;
            }
            // Set user string to lock the object
            obj.Attributes.SetUserString(LOCKED_BY_STAG_KEY, LOCKED_BY_STAG_VALUE);
            RhinoDoc.ActiveDoc.Objects.Lock(obj.Id, true);
            RhinoDoc.ActiveDoc.Objects.ModifyAttributes(obj, obj.Attributes, true);
        }

        public void UnblockObject(RhinoObject obj)
        {
            // check if the object was locked before
            if (!obj.IsLocked)
            {
                return;
            }
            // was it blocked by us ? 
            var userString = obj.Attributes.GetUserString(LOCKED_BY_STAG_KEY);
            if (userString != null && userString != string.Empty)
            {
                // Remove user string to unlock the object
                obj.Attributes.SetUserString(LOCKED_BY_STAG_KEY, null);
                RhinoDoc.ActiveDoc.Objects.Lock(obj.Id, false);
                RhinoDoc.ActiveDoc.Objects.ModifyAttributes(obj, obj.Attributes, true);
            }
        }
    }
}