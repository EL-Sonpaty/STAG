using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using Rhino.UI;
using STAG.Wrappers;
using System;
using System.Collections.Generic;

namespace STAG
{
    public class STAG_StartListening : Command
    {
        ///<summary>The only instance of this command.</summary>
        public static STAG_StartListening Instance { get; private set; }

        ///<returns>The command name as it appears on the Rhino command line.</returns>
        public override string EnglishName => "STAG_StartListening";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            if (STAGPlugin.Instance.StartListening())
            {
                return Result.Success;
            }
            else
            {
                Rhino.RhinoApp.WriteLine("STAG was already listening");
                return Result.Failure;
            }
        }
    }
}
