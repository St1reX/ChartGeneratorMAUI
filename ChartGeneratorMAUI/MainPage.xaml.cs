using ChartGeneratorMAUI.Charts;

namespace ChartGeneratorMAUI
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();

            chart.Items = new List<BarData>
            {
                new BarData {Label = "Psy", Value = 52},
                new BarData {Label = "Koty", Value = 12},
                new BarData {Label = "Konie", Value = 12},
                new BarData {Label = "Rybki", Value = 4}
            };
            chart.YAxisLabel = "Zwierzęta na farmie";
        }

       
    }

}
