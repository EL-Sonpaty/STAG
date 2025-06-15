using STAG.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace STAG.ViewModels
{
    public class StageConstraintViewModel: BaseViewModel
    {

        public StageConstraintViewModel()
        {
            StageColor = pastelBrushes[new Random().Next(pastelBrushes.Count)];
        }
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

        private bool isGeometricalEditable = true;
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
        private bool isTranslationEditable = true;
        public bool IsTranslationEditable { get => isTranslationEditable;
            set
            {
                if (isTranslationEditable != value)
                {
                    SetProperty(value, ref isTranslationEditable, nameof(IsTranslationEditable));
                }
            }
        }

        private bool isAttributesEditable = true;
        public bool IsAttributesEditable { get => isAttributesEditable;
            set
            {
                if (isAttributesEditable != value)
                {
                    SetProperty(value, ref isAttributesEditable, nameof(IsAttributesEditable));
                }
            }
        }

        private bool isUserTextsEditable = true;
        public bool IsUserTextsEditable { get=>isUserTextsEditable;
            set
            {
                if (isUserTextsEditable != value)
                {
                    SetProperty(value, ref isUserTextsEditable, nameof(IsUserTextsEditable));
                }
            }
        }

        public Brush StageColor { get; private set; }




        List<Brush> pastelBrushes = new List<Brush>
        {
        new SolidColorBrush(Color.FromRgb(0xB6, 0xD7, 0xFF)), // Powder Blue
        new SolidColorBrush(Color.FromRgb(0xE6, 0xE6, 0xFA)), // Lavender Mist
        new SolidColorBrush(Color.FromRgb(0xB8, 0xF2, 0xB8)), // Mint Cream
        new SolidColorBrush(Color.FromRgb(0xC5, 0xC5, 0xFF)), // Periwinkle
        new SolidColorBrush(Color.FromRgb(0xFF, 0xB3, 0xBA)), // Blush Pink
        new SolidColorBrush(Color.FromRgb(0xFF, 0xDF, 0xBA)), // Peach Whisper
        new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0xBA)), // Butter Yellow
        new SolidColorBrush(Color.FromRgb(0xFF, 0xB3, 0xDE)), // Coral Dream
        new SolidColorBrush(Color.FromRgb(0xBA, 0xE1, 0xBA)), // Sage Green
        new SolidColorBrush(Color.FromRgb(0xF0, 0xC2, 0xC2)), // Dusty Rose
        new SolidColorBrush(Color.FromRgb(0xE0, 0xD0, 0xE0)), // Lilac Gray
        new SolidColorBrush(Color.FromRgb(0xFF, 0xF8, 0xDC))  // Cream
        };

    }
}
