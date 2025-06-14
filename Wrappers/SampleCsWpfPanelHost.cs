using Microsoft.Windows;
using STAG.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;



namespace STAG.Wrappers
{
    [Guid("0874C691-A083-4C05-9E31-96C2C63F7C7E")]
    public class STAGViewHost : RhinoWindows.Controls.WpfElementHost
    {
        public STAGViewHost(uint docSn)
        : base(new STAGView(docSn), null)
        {
        }
    }
}
