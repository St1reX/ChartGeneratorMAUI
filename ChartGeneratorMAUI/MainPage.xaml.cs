using ChartGeneratorMAUI.Charts;

namespace ChartGeneratorMAUI
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();

            chart.Items = new List<LineData>
{
                new LineData { Label = "Sty", Value = 45 },
                new LineData { Label = "Lut", Value = 62 },
                new LineData { Label = "Mar", Value = 38 },
                new LineData { Label = "Kwi", Value = 71 },
                new LineData { Label = "Maj", Value = 55 },
                new LineData { Label = "Cze", Value = 82 },
                new LineData { Label = "Lip", Value = 647 },
                new LineData { Label = "Sie", Value = 49 },
                new LineData { Label = "Wrz", Value = 58 },
                new LineData { Label = "Paź", Value = 73 },
                new LineData { Label = "Lis", Value = 41 },
                new LineData { Label = "Gru", Value = 76 }
};
            chart.YAxisLabel = "Sprzedaż (w tys.)";
        }

       
    }

}
