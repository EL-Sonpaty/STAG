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
                    stageName = value;
                    OnPropertyChanged(nameof(StageName));
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
                    isGeometricalEditable = value;
                    OnPropertyChanged(nameof(IsGeometricalEditable));
                }
            }
        }
        private bool isTranslationEditable;
        public bool IsTranslationEditable { get => isTranslationEditable;
            set
            {
                if (isTranslationEditable != value)
                {
                    isTranslationEditable = value;
                    OnPropertyChanged(nameof(IsTranslationEditable));
                }
            }
        }

        private bool isAttributesEditable;
        public bool IsAttributesEditable { get => isAttributesEditable;
            set
            {
                if (isAttributesEditable != value)
                {
                    isAttributesEditable = value;
                    OnPropertyChanged(nameof(IsAttributesEditable));
                }
            }
        }

        private bool isUserTextsEditable;
        public bool IsUserTextsEditable { get=>isUserTextsEditable;
            set
            {
                if (isUserTextsEditable != value)
                {
                    isUserTextsEditable = value;
                    OnPropertyChanged(nameof(IsUserTextsEditable));
                }
            }
        }
        public DateTime DateAdded { get; set; }
        public string AddedBy { get; set; }


        

    }
}
