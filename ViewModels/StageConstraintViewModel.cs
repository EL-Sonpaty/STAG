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
            // pick a new color 
            var color = GetRandomColor();
            // make sure this color is not already used in the STAGPanelViewModel.StageConstraint
            // add a counter to use the color anyway after 5 tries
            int attempts = 0;
            if (STAGPlugin.Instance != null && STAGPlugin.Instance.STAGPanelViewModel != null)
            {
                if (STAGPlugin.Instance.STAGPanelViewModel.StageConstraints != null ||
                    STAGPlugin.Instance.STAGPanelViewModel.StageConstraints.Count != 0)
                {
                    while (attempts < 5 && STAGPlugin.Instance.STAGPanelViewModel.StageConstraints.Any(s => s.StageColor == color))
                    {
                        color = pastelBrushes[new Random().Next(pastelBrushes.Count)];
                        attempts++;
                    }
                }
            }
            StageColor = color;
        }

        public static Brush GetRandomColor()
        {
            return pastelBrushes[new Random().Next(pastelBrushes.Count)];
        }

        public void ResetColor()
        {
            StageColor = GetRandomColor();
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

        private Brush _color;
        public Brush StageColor
        {
            get => _color;
            private set
            {
                SetProperty(value, ref _color, nameof(StageColor));
            }
        }




        public static List<Brush> pastelBrushes = new List<Brush>
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
