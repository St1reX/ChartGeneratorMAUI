using ChartGeneratorMAUI.Charts;
using ChartGeneratorMAUI.Models;

namespace ChartGeneratorMAUI
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnLineChartClicked(object sender, EventArgs e)
        {
            if (!TryGetInputValues(out var chartData))
            {
                await DisplayAlert("Błąd", "Proszę wprowadzić poprawne wartości liczbowe we wszystkich polach", "OK");
                return;
            }

            await Navigation.PushAsync(new LineChartPage(chartData));
        }

        private async void OnBarChartClicked(object sender, EventArgs e)
        {
            if (!TryGetInputValues(out var chartData))
            {
                await DisplayAlert("Błąd", "Proszę wprowadzić poprawne wartości liczbowe we wszystkich polach", "OK");
                return;
            }

            await Navigation.PushAsync(new BarChartPage(chartData));
        }

        private async void OnPieChartClicked(object sender, EventArgs e)
        {
            if (!TryGetInputValues(out var chartData))
            {
                await DisplayAlert("Błąd", "Proszę wprowadzić poprawne wartości liczbowe we wszystkich polach", "OK");
                return;
            }

            await Navigation.PushAsync(new PieChartPage(chartData));
        }

        private bool TryGetInputValues(out List<ChartData> chartData)
        {
            chartData = new List<ChartData>();

            // Sprawdź czy wszystkie wartości są poprawne
            if (string.IsNullOrWhiteSpace(Value1Entry.Text) ||
                string.IsNullOrWhiteSpace(Value2Entry.Text) ||
                string.IsNullOrWhiteSpace(Value3Entry.Text) ||
                string.IsNullOrWhiteSpace(Label1Entry.Text) ||
                string.IsNullOrWhiteSpace(Label2Entry.Text) ||
                string.IsNullOrWhiteSpace(Label3Entry.Text))
            {
                return false;
            }

            if (!double.TryParse(Value1Entry.Text, out double value1) ||
                !double.TryParse(Value2Entry.Text, out double value2) ||
                !double.TryParse(Value3Entry.Text, out double value3))
            {
                return false;
            }

            // Utwórz listę danych z labelami i wartościami
            chartData.Add(new ChartData { Label = Label1Entry.Text, Value = value1 });
            chartData.Add(new ChartData { Label = Label2Entry.Text, Value = value2 });
            chartData.Add(new ChartData { Label = Label3Entry.Text, Value = value3 });

            return true;
        }
    }
}