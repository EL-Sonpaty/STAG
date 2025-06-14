//using System;
//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using Rhino;
//using Rhino.UI;
//using Rhino.DocObjects;
//using System.Linq;

//namespace MyRhinoWPFPlugin
//{
//    // 1. Data Model for our panel
//    public class ObjectInfo : INotifyPropertyChanged
//    {
//        private string _objectId;
//        private DateTime _date;
//        private string _userName;

//        public string ObjectId
//        {
//            get => _objectId;
//            set
//            {
//                _objectId = value;
//                OnPropertyChanged(nameof(ObjectId));
//            }
//        }

//        public DateTime Date
//        {
//            get => _date;
//            set
//            {
//                _date = value;
//                OnPropertyChanged(nameof(Date));
//            }
//        }

//        public string UserName
//        {
//            get => _userName;
//            set
//            {
//                _userName = value;
//                OnPropertyChanged(nameof(UserName));
//            }
//        }

//        public event PropertyChangedEventHandler PropertyChanged;
//        protected virtual void OnPropertyChanged(string propertyName)
//        {
//            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//        }
//    }

//    // 2. WPF UserControl for the panel content
//    public partial class ObjectInfoPanel : UserControl, INotifyPropertyChanged
//    {
//        public ObservableCollection<ObjectInfo> ObjectInfos { get; set; }

//        private ObjectInfo _selectedObjectInfo;
//        public ObjectInfo SelectedObjectInfo
//        {
//            get => _selectedObjectInfo;
//            set
//            {
//                _selectedObjectInfo = value;
//                OnPropertyChanged(nameof(SelectedObjectInfo));
//            }
//        }

//        public ObjectInfoPanel()
//        {
//            InitializeComponent();
//            ObjectInfos = new ObservableCollection<ObjectInfo>();
//            DataContext = this;

//            // Subscribe to Rhino document events
//            RhinoDoc.AddRhinoObject += OnRhinoObjectAdded;
//            RhinoDoc.DeleteRhinoObject += OnRhinoObjectDeleted;
//        }

//        private void InitializeComponent()
//        {
//            // Create the WPF layout programmatically
//            var grid = new Grid();

//            // Define rows
//            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
//            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
//            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
//            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

//            // Title
//            var title = new TextBlock
//            {
//                Text = "Object Information",
//                FontSize = 16,
//                FontWeight = FontWeights.Bold,
//                Margin = new Thickness(10),
//                HorizontalAlignment = HorizontalAlignment.Center
//            };
//            Grid.SetRow(title, 0);
//            grid.Children.Add(title);

//            // DataGrid for displaying object info
//            var dataGrid = new DataGrid
//            {
//                AutoGenerateColumns = false,
//                CanUserAddRows = false,
//                Margin = new Thickness(10),
//                GridLinesVisibility = DataGridGridLinesVisibility.Horizontal
//            };

//            // Define columns
//            dataGrid.Columns.Add(new DataGridTextColumn
//            {
//                Header = "Object ID",
//                Binding = new Binding("ObjectId"),
//                Width = new DataGridLength(120)
//            });

//            dataGrid.Columns.Add(new DataGridTextColumn
//            {
//                Header = "Date",
//                Binding = new Binding("Date") { StringFormat = "yyyy-MM-dd HH:mm:ss" },
//                Width = new DataGridLength(140)
//            });

//            dataGrid.Columns.Add(new DataGridTextColumn
//            {
//                Header = "User Name",
//                Binding = new Binding("UserName"),
//                Width = new DataGridLength(100)
//            });

//            // Bind the DataGrid to our collection
//            dataGrid.SetBinding(DataGrid.ItemsSourceProperty, new Binding("ObjectInfos"));
//            dataGrid.SetBinding(DataGrid.SelectedItemProperty, new Binding("SelectedObjectInfo"));

//            Grid.SetRow(dataGrid, 1);
//            grid.Children.Add(dataGrid);

//            // Button panel
//            var buttonPanel = new StackPanel
//            {
//                Orientation = Orientation.Horizontal,
//                HorizontalAlignment = HorizontalAlignment.Center,
//                Margin = new Thickness(10)
//            };

//            var refreshButton = new Button
//            {
//                Content = "Refresh",
//                Width = 80,
//                Height = 30,
//                Margin = new Thickness(5)
//            };
//            refreshButton.Click += RefreshButton_Click;

//            var selectButton = new Button
//            {
//                Content = "Select Object",
//                Width = 100,
//                Height = 30,
//                Margin = new Thickness(5)
//            };
//            selectButton.Click += SelectButton_Click;

//            var clearButton = new Button
//            {
//                Content = "Clear List",
//                Width = 80,
//                Height = 30,
//                Margin = new Thickness(5)
//            };
//            clearButton.Click += ClearButton_Click;

//            buttonPanel.Children.Add(refreshButton);
//            buttonPanel.Children.Add(selectButton);
//            buttonPanel.Children.Add(clearButton);

//            Grid.SetRow(buttonPanel, 2);
//            grid.Children.Add(buttonPanel);

//            // Status bar
//            var statusBar = new TextBlock
//            {
//                Text = "Ready",
//                Margin = new Thickness(10, 5, 10, 10),
//                FontStyle = FontStyles.Italic,
//                Foreground = System.Windows.Media.Brushes.Gray
//            };
//            Grid.SetRow(statusBar, 3);
//            grid.Children.Add(statusBar);

//            this.Content = grid;
//        }

//        private void OnRhinoObjectAdded(object sender, RhinoObjectEventArgs e)
//        {
//            Application.Current.Dispatcher.Invoke(() =>
//            {
//                var objectInfo = new ObjectInfo
//                {
//                    ObjectId = e.TheObject.Id.ToString(),
//                    Date = DateTime.Now,
//                    UserName = Environment.UserName
//                };
//                ObjectInfos.Add(objectInfo);
//            });
//        }

//        private void OnRhinoObjectDeleted(object sender, RhinoObjectEventArgs e)
//        {
//            Application.Current.Dispatcher.Invoke(() =>
//            {
//                var itemToRemove = ObjectInfos.FirstOrDefault(x => x.ObjectId == e.TheObject.Id.ToString());
//                if (itemToRemove != null)
//                {
//                    ObjectInfos.Remove(itemToRemove);
//                }
//            });
//        }

//        private void RefreshButton_Click(object sender, RoutedEventArgs e)
//        {
//            ObjectInfos.Clear();
//            var doc = RhinoDoc.ActiveDoc;
//            if (doc != null)
//            {
//                foreach (var obj in doc.Objects)
//                {
//                    var objectInfo = new ObjectInfo
//                    {
//                        ObjectId = obj.Id.ToString(),
//                        Date = DateTime.Now,
//                        UserName = Environment.UserName
//                    };
//                    ObjectInfos.Add(objectInfo);
//                }
//            }
//        }

//        private void SelectButton_Click(object sender, RoutedEventArgs e)
//        {
//            if (SelectedObjectInfo != null)
//            {
//                var doc = RhinoDoc.ActiveDoc;
//                if (doc != null && Guid.TryParse(SelectedObjectInfo.ObjectId, out Guid objectId))
//                {
//                    var rhinoObject = doc.Objects.Find(objectId);
//                    if (rhinoObject != null)
//                    {
//                        rhinoObject.Select(true);
//                        doc.Views.Redraw();
//                        RhinoApp.WriteLine($"Selected object: {objectId}");
//                    }
//                }
//            }
//        }

//        private void ClearButton_Click(object sender, RoutedEventArgs e)
//        {
//            ObjectInfos.Clear();
//        }

//        public event PropertyChangedEventHandler PropertyChanged;
//        protected virtual void OnPropertyChanged(string propertyName)
//        {
//            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//        }
//    }

//    // 3. Rhino Panel wrapper
//    [System.Runtime.InteropServices.Guid("A1B2C3D4-E5F6-7890-ABCD-EF1234567890")]
//    public class ObjectInfoRhinoPanel : Panel, IPanel
//    {
//        public static Guid PanelId => typeof(ObjectInfoRhinoPanel).GUID;

//        public ObjectInfoRhinoPanel(uint documentSerialNumber) : base(documentSerialNumber)
//        {
//        }

//        public override object PanelObject()
//        {
//            return new ObjectInfoPanel();
//        }

//        public override string PanelTitle => "Object Information";
//    }

//    // 4. Main Plugin Class
//    public class ObjectInfoPlugin : Rhino.PlugIns.PlugIn
//    {
//        public ObjectInfoPlugin()
//        {
//            Instance = this;
//        }

//        public static ObjectInfoPlugin Instance { get; private set; }

//        protected override LoadReturnCode OnLoad(ref string errorMessage)
//        {
//            try
//            {
//                Panels.RegisterPanel(this, typeof(ObjectInfoRhinoPanel), "Object Information", null);
//                return LoadReturnCode.Success;
//            }
//            catch (Exception ex)
//            {
//                errorMessage = ex.Message;
//                return LoadReturnCode.ErrorShowDialog;
//            }
//        }
//    }

//    // 5. Command to show the panel
//    [System.Runtime.InteropServices.Guid("B2C3D4E5-F6G7-8901-BCDE-F23456789012")]
//    public class ShowObjectInfoPanelCommand : Rhino.Commands.Command
//    {
//        public override string EnglishName => "ShowObjectInfoPanel";

//        protected override Rhino.Commands.Result RunCommand(RhinoDoc doc, Rhino.Commands.RunMode mode)
//        {
//            var panelId = ObjectInfoRhinoPanel.PanelId;
//            var visible = Panels.IsPanelVisible(panelId);

//            if (!visible)
//            {
//                Panels.OpenPanel(panelId);
//                RhinoApp.WriteLine("Object Information panel opened.");
//            }
//            else
//            {
//                Panels.ClosePanel(panelId);
//                RhinoApp.WriteLine("Object Information panel closed.");
//            }

//            return Rhino.Commands.Result.Success;
//        }
//    }
//}