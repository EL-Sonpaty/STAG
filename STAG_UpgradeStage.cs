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
    public class STAG_DowngradeStage : Command
    {
        ///<summary>The only instance of this command.</summary>
        public static STAG_DowngradeStage Instance { get; private set; }

        ///<returns>The command name as it appears on the Rhino command line.</returns>
        public override string EnglishName => "STAG_DowngradeStage";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // get objects from rhino 
            var ids = STAG_Core.GetSelectedIds();
            // make sure the list is not empty
            if (ids.Count == 0)
            {
                RhinoApp.WriteLine("No objects selected.");
                return Result.Failure;
            }

            // upgrade stage
            STAG_Core.DowngradeStage(ids);
            return Result.Success;
            
        }
    }
}
