using STAG.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STAG.Models
{
    public class StageConstraintViewModel: BaseViewModel
    {
        public int Index => STAGPlugin.Instance.STAGPanelViewModel.StageConstraints.IndexOf(this);

        private string stageName;
        public string StageName { 
            get => stageName;
            set
            {
                if (stageName != value)
                {
                    SetProperty(value, ref stageName, nameof(StageName));
                }
            }
        }

        private bool isGeometricalEditable;
        public bool IsGeometricalEditable { 
            get => isGeometricalEditable;
            set
            {
                if (isGeometricalEditable != value)
                {
                    SetProperty(value, ref isGeometricalEditable, nameof(IsGeometricalEditable));
                }
            }
        }
        private bool isTranslationEditable;
        public bool IsTranslationEditable { get => isTranslationEditable;
            set
            {
                if (isTranslationEditable != value)
                {
                    SetProperty(value, ref isTranslationEditable, nameof(IsTranslationEditable));
                }
            }
        }

        private bool isAttributesEditable;
        public bool IsAttributesEditable { get => isAttributesEditable;
            set
            {
                if (isAttributesEditable != value)
                {
                    SetProperty(value, ref isAttributesEditable, nameof(IsAttributesEditable));
                }
            }
        }

        private bool isUserTextsEditable;
        public bool IsUserTextsEditable { get=>isUserTextsEditable;
            set
            {
                if (isUserTextsEditable != value)
                {
                    SetProperty(value, ref isUserTextsEditable, nameof(IsUserTextsEditable));
                }
            }
        }
        public DateTime DateAdded { get; set; }
        public string AddedBy { get; set; }


        

    }
}
