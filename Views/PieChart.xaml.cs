using OxyPlot.Series;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using STAG.ViewModels;
using OxyPlot.Wpf;

namespace STAG.Views
{
    /// <summary>
    /// Interaction logic for PieChart.xaml
    /// </summary>
    public partial class PieChart : Window
    {
        public PieChart()
        {
            InitializeComponent();
            this.DataContext = this;
            PlotModel = CreatePlotModel();
        }

        public PlotModel PlotModel { get; private set; }

        public PlotModel CreatePlotModel()
        {
            PlotModel model = new PlotModel { Title = "Stage Distribution" };

            PieSeries pie = new PieSeries
            {
                StrokeThickness = 1,
                AngleSpan = 360,
                StartAngle = 0,
                InsideLabelFormat = "{0}",
            };
            var count = STAG_Core.CountObjectsByStages();

            

            foreach(var key in count.Keys)
            {
                StageConstraintViewModel stage = STAG_Core.GetStageConstraintByName(key);

                pie.Slices.Add(new PieSlice(key, count[key])
                {
                    IsExploded = true,
                    Fill = stage.StageColor.ToOxyColor(),
                });
            }

            model.Series.Add(pie);
            return model;

        }
    }
}
