using ChartGeneratorMAUI.Charts;
using ChartGeneratorMAUI.Models;

namespace ChartGeneratorMAUI
{
    public partial class LineChartPage : ContentPage
    {
        public LineChartPage(List<ChartData> chartData)
        {
            InitializeComponent();

            chart.Items = chartData;
            chart.YAxisLabel = "Wartoœci";
        }
    }
}