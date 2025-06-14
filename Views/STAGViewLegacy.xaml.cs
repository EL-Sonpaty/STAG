using Rhino;
using STAG.Constants;
using STAG.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace STAG.Views
{
    /// <summary>
    /// Interaction logic for STAGViewLegacy.xaml
    /// </summary>
    public partial class STAGViewLegacy
    {

        public STAGViewLegacy(uint documentSerialNumber)
        {
            HardCodedData.DocumentSerialNumber = documentSerialNumber;
            DataContext = new STAGPanelViewModel(documentSerialNumber);
            InitializeComponent();
        }

        //private STAGPanelViewModel ViewModel => DataContext as STAGPanelViewModel;

        //private ToggleButton CreateToggleIconButton(string iconName, string tooltip, RoutedEventHandler handler, object tag = null)
        //{
        //    var icon = new FontAwesome.Sharp.IconBlock
        //    {
        //        Icon = (FontAwesome.Sharp.IconChar)Enum.Parse(typeof(FontAwesome.Sharp.IconChar), iconName),
        //        Foreground = Brushes.Gray,
        //        Width = 16,
        //        Height = 16,
        //        HorizontalAlignment = HorizontalAlignment.Center,
        //        VerticalAlignment = VerticalAlignment.Center
        //    };

        //    var toggle = new ToggleButton
        //    {
        //        Width = 32,
        //        Height = 28,
        //        Margin = new Thickness(2),
        //        Padding = new Thickness(4, 2, 4, 2),
        //        ToolTip = tooltip,
        //        Tag = tag,
        //        Content = icon,
        //        Background = Brushes.Transparent,
        //        BorderBrush = Brushes.Transparent,
        //        BorderThickness = new Thickness(0),
        //        FocusVisualStyle = null
        //    };

        //    // Remove default blue highlight
        //    var style = new Style(typeof(ToggleButton));
        //    style.Setters.Add(new Setter(Control.BackgroundProperty, Brushes.Transparent));
        //    style.Setters.Add(new Setter(Control.BorderBrushProperty, Brushes.Transparent));
        //    style.Setters.Add(new Setter(Control.BorderThicknessProperty, new Thickness(0)));
        //    style.Setters.Add(new Setter(Control.FocusVisualStyleProperty, null));

        //    var triggerChecked = new Trigger
        //    {
        //        Property = ToggleButton.IsCheckedProperty,
        //        Value = true
        //    };
        //    triggerChecked.Setters.Add(new Setter(Control.BackgroundProperty, Brushes.LightGray));
        //    style.Triggers.Add(triggerChecked);

        //    toggle.Style = style;

        //    toggle.Checked += (s, e) =>
        //    {
        //        icon.Foreground = Brushes.Black;
        //        handler?.Invoke(s, e); // Call handler when selected
        //    };

        //    toggle.Unchecked += (s, e) =>
        //    {
        //        icon.Foreground = Brushes.Gray;
        //        handler?.Invoke(s, e); // Call handler when deselected
        //    };

        //    return toggle;
        //}

        //private Button CreateIconButton(string iconName, string tooltip, RoutedEventHandler handler, object tag = null)
        //{
        //    var icon = new FontAwesome.Sharp.IconBlock
        //    {
        //        Icon = (FontAwesome.Sharp.IconChar)Enum.Parse(typeof(FontAwesome.Sharp.IconChar), iconName),
        //        Foreground = Brushes.Black,
        //        HorizontalAlignment = HorizontalAlignment.Center,
        //        VerticalAlignment = VerticalAlignment.Center
        //    };

        //    var grid = new Grid
        //    {
        //        Width = 20,
        //        Height = 20,
        //        HorizontalAlignment = HorizontalAlignment.Center,
        //        VerticalAlignment = VerticalAlignment.Center
        //    };
        //    grid.Children.Add(icon);

        //    var viewbox = new Viewbox
        //    {
        //        Stretch = Stretch.Uniform,
        //        Child = grid
        //    };

        //    var button = new Button
        //    {
        //        Width = 25,
        //        Height = 25,
        //        Padding = new Thickness(0),
        //        ToolTip = tooltip,
        //        Background = new SolidColorBrush(Color.FromRgb(0xEE, 0xEE, 0xEE)),
        //        BorderBrush = Brushes.Gray,
        //        Tag = tag,
        //        Content = viewbox
        //    };

        //    button.Click += handler;
        //    return button;
        //}

        //private void AddStage_Click(object sender, RoutedEventArgs e)
        //{
        //    string stageText = StageInput.Text.Trim();

        //    if (!string.IsNullOrEmpty(stageText))
        //    {
        //        // Add a placeholder; we'll refresh all numbers next
        //        AddStageItem(stageText);
        //        StageInput.Clear();
        //        RefreshStageNumbers();
        //    }
        //}

        //private void AddStageItem(string text)
        //{
        //    var outerPanel = new StackPanel
        //    {
        //        Orientation = Orientation.Vertical,
        //        Margin = new Thickness(0, 5, 0, 5)
        //    };

        //    // First row: number + text input + control buttons
        //    var rowPanel = new StackPanel
        //    {
        //        Orientation = Orientation.Horizontal,
        //        VerticalAlignment = VerticalAlignment.Center,
        //        Margin = new Thickness(0, 0, 0, 2)
        //    };

        //    var numberBlock = new TextBlock
        //    {
        //        Width = 25,
        //        Height = 25,
        //        VerticalAlignment = VerticalAlignment.Center,
        //        TextAlignment = TextAlignment.Right,
        //        Padding = new Thickness(0, 0, 4, 0),
        //        FontWeight = FontWeights.SemiBold,
        //        FontSize = 12
        //    };

        //    var stageBox = new TextBox
        //    {
        //        Text = text,
        //        Width = 230,
        //        Height = 25,
        //        FontSize = 12,
        //        VerticalContentAlignment = VerticalAlignment.Center,
        //        Margin = new Thickness(0, 0, 5, 0)
        //    };

        //    var upButton = CreateIconButton("ArrowUp", "Move Up", MoveUp_Click, outerPanel);
        //    var downButton = CreateIconButton("ArrowDown", "Move Down", MoveDown_Click, outerPanel);
        //    var removeButton = CreateIconButton("Minus", "Remove", RemoveStage_Click, outerPanel);

        //    // Ensure uniform button sizing and alignment
        //    foreach (var btn in new[] { upButton, downButton, removeButton })
        //    {
        //        btn.Width = 25;
        //        btn.Height = 25;
        //        btn.Padding = new Thickness(0);
        //        btn.Margin = new Thickness(0, 0, 2, 0);
        //        btn.VerticalAlignment = VerticalAlignment.Center;
        //    }

        //    rowPanel.Children.Add(numberBlock);
        //    rowPanel.Children.Add(stageBox);
        //    rowPanel.Children.Add(upButton);
        //    rowPanel.Children.Add(downButton);
        //    rowPanel.Children.Add(removeButton);

        //    // Second row: toggle icon buttons (Geometrical, etc.)
        //    var iconPanel = new StackPanel
        //    {
        //        Orientation = Orientation.Horizontal,
        //        Margin = new Thickness(30, 2, 0, 0)
        //    };

        //    iconPanel.Children.Add(CreateToggleIconButton("Cubes", "Geometrical", OnGeometricalClick, outerPanel));
        //    iconPanel.Children.Add(CreateToggleIconButton("ArrowsAlt", "Translation", OnTranslationClick, outerPanel));
        //    iconPanel.Children.Add(CreateToggleIconButton("Tags", "Attributes", OnAttributesClick, outerPanel));
        //    iconPanel.Children.Add(CreateToggleIconButton("Font", "User Texts", OnUserTextsClick, outerPanel));

        //    outerPanel.Children.Add(rowPanel);
        //    outerPanel.Children.Add(iconPanel);

        //    StageListBox.Items.Add(outerPanel);
        //    RefreshStageNumbers();
        //}


        //private void OnGeometricalClick(object sender, RoutedEventArgs e)
        //{
        //    MessageBox.Show("Geometrical clicked");
        //}

        //private void OnTranslationClick(object sender, RoutedEventArgs e)
        //{
        //    MessageBox.Show("Translation clicked");
        //}

        //private void OnAttributesClick(object sender, RoutedEventArgs e)
        //{
        //    MessageBox.Show("Attributes clicked");
        //}

        //private void OnUserTextsClick(object sender, RoutedEventArgs e)
        //{
        //    MessageBox.Show("User Texts clicked");
        //}

        //private void MoveUp_Click(object sender, RoutedEventArgs e)
        //{
        //    if (sender is Button btn && btn.Tag is StackPanel panel)
        //    {
        //        int index = StageListBox.Items.IndexOf(panel);
        //        if (index > 0)
        //        {
        //            StageListBox.Items.RemoveAt(index);
        //            StageListBox.Items.Insert(index - 1, panel);
        //            RefreshStageNumbers();
        //        }
        //    }
        //}

        //private void MoveDown_Click(object sender, RoutedEventArgs e)
        //{
        //    if (sender is Button btn && btn.Tag is StackPanel panel)
        //    {
        //        int index = StageListBox.Items.IndexOf(panel);
        //        if (index < StageListBox.Items.Count - 1)
        //        {
        //            StageListBox.Items.RemoveAt(index);
        //            StageListBox.Items.Insert(index + 1, panel);
        //            RefreshStageNumbers();
        //        }
        //    }
        //}

        //private void RefreshStageNumbers()
        //{
        //    for (int i = 0; i < StageListBox.Items.Count; i++)
        //    {
        //        if (StageListBox.Items[i] is StackPanel outer &&
        //            outer.Children[0] is StackPanel row &&
        //            row.Children[0] is TextBlock number)
        //        {
        //            number.Text = $"{i + 1}.";
        //        }
        //    }
        //}
        //private void RemoveStage_Click(object sender, RoutedEventArgs e)
        //{
        //    if (sender is Button removeButton && removeButton.Tag is StackPanel panel)
        //    {
        //        StageListBox.Items.Remove(panel);
        //        RefreshStageNumbers();
        //    }
        //}

        //private void Control_VisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    if ((bool)e.NewValue)
        //    {
        //        // Visible code here
        //        RhinoApp.WriteLine("STAGViewLegacy now visible");
        //    }
        //    else
        //    {
        //        // Hidden code here
        //        RhinoApp.WriteLine("STAGViewLegacy now hidden");
        //    }
        //}
    }
}
