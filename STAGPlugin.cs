using System;
using Rhino;
using Rhino.DocObjects;
using Rhino.PlugIns;

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
        public STAGPlugin()
        {
            Instance = this;
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
            
            return LoadReturnCode.Success;

        }

        private void OnBeforeTransformObjects(object sender, RhinoTransformObjectsEventArgs e)
        {
            // Handle the transform event
            RhinoApp.WriteLine($"Transform event: {e.Objects.Length} objects transformed");

            // Access the objects being transformed
            RhinoObject[] objects = e.Objects;

            // You can also access object IDs
            foreach (var obj in e.Objects)
            {
                RhinoApp.WriteLine($"Object ID: {obj.Id}");
            }
        }
    }
}