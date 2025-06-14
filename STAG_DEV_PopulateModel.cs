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
    public class STAG_DEV_PopulateModel : Command
    {
        ///<summary>The only instance of this command.</summary>
        public static STAG_DEV_PopulateModel Instance { get; private set; }

        ///<returns>The command name as it appears on the Rhino command line.</returns>
        public override string EnglishName => "STAG_DEV_PopulateModel";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {

            // Create 10 points randomly located in the model
            Random random = new Random();

            for (int i = 0; i < 10; i++)
            {
                double x = random.NextDouble() * 100;
                double y = random.NextDouble() * 100;
                double z = random.NextDouble() * 100;
                Rhino.RhinoDoc.ActiveDoc.Objects.AddPoint(new Point3d(x, y, z));
            }


            return Result.Success;

        }
    }
}
