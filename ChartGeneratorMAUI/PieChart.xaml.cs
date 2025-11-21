using ChartGeneratorMAUI.Charts;
using ChartGeneratorMAUI.Models;

namespace ChartGeneratorMAUI
{
    public partial class PieChartPage : ContentPage
    {
        public PieChartPage(List<ChartData> chartData)
        {
            InitializeComponent();

            chart.Items = chartData;
        }
    }
}