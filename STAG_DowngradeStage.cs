using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using Rhino.UI;
using STAG.ViewModels;
using STAG.Wrappers;
using System;
using System.Collections.Generic;

namespace STAG
{
    public class STAG_UpgradeStage : Command
    {
        ///<summary>The only instance of this command.</summary>
        public static STAG_UpgradeStage Instance { get; private set; }

        ///<returns>The command name as it appears on the Rhino command line.</returns>
        public override string EnglishName => "STAG_UpgradeStage";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            STAGPlugin.Instance.STAGPanelViewModel.DowngradeSelectedObjectsStages();
            return Result.Success;
            
        }
    }
}
