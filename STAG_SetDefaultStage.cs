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
    public class STAG_SetDefaultStage : Command
    {
        ///<summary>The only instance of this command.</summary>
        public static STAG_SetDefaultStage Instance { get; private set; }

        ///<returns>The command name as it appears on the Rhino command line.</returns>
        public override string EnglishName => "STAG_SetDefaultStage";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            STAG_Core.FillAllEmptyStagesToDefault();
            return Result.Success;
        }
    }
}
