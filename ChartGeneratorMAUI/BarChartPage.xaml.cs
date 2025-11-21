using ChartGeneratorMAUI.Charts;
using ChartGeneratorMAUI.Models;

namespace ChartGeneratorMAUI
{
    public partial class BarChartPage : ContentPage
    {
        public BarChartPage(List<ChartData> chartData)
        {
            InitializeComponent();

            chart.Items = chartData;
            chart.YAxisLabel = "Wartoœci";
        }
    }
}