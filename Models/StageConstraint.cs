using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STAG.Models
{
    public class StageConstraint
    {
        public int Index => STAGPlugin.Instance.STAGPanelViewModel.StageConstraints.IndexOf(this);
        public string StageName { get; set; }
        public bool IsGeometricalEditable { get; set; }
        public bool IsTranslationEditable { get; set; }
        public bool IsAttributesEditable { get; set; }
        public bool IsUserTextsEditable { get; set; }
        public DateTime DateAdded { get; set; }
        public string AddedBy { get; set; }


        

    }
}
