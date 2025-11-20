using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartGeneratorMAUI.Charts
{
    public struct BarData
    {
        public string Label { get; set; }
        public double Value { get; set; }
    }

    public class BarChart : GraphicsView
    {
        public List<BarData> Items { get; set; } = new();
        public string YAxisLabel { get; set; } = "";

        //Dynamic resize - depends on biggest value in dataset
        public void RefreshAutoHeight()
        {
            if (Items == null || Items.Count == 0) return;
            double max = Items.Max(x => x.Value);
            double scale = 20;
            HeightRequest = Math.Clamp(max * scale + 120, 150, 800);
        }

        public BarChart()
        {
            Drawable = new BarChartDrawable(this);
        }

    }

    public class BarChartDrawable : IDrawable
    {
        private readonly BarChart _chart;
        public BarChartDrawable(BarChart chart)
        {
            _chart = chart;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            //Not items provided case
            if (_chart.Items == null || _chart.Items.Count == 0)
            {
                canvas.FontSize = 18;
                canvas.FontColor = Application.Current!.RequestedTheme == AppTheme.Dark ? Colors.White : Colors.Black;
                canvas.DrawString("Brak danych", dirtyRect.Center.X, dirtyRect.Center.Y, HorizontalAlignment.Center);
                return;
            }

            //Container sizes
            float width = dirtyRect.Width;
            float height = dirtyRect.Height;
            float margin = 50;

            //Color choice
            bool dark = Application.Current!.RequestedTheme == AppTheme.Dark;
            var textColor = dark ? Colors.White : Colors.Black;
            var axisColor = dark ? Colors.White : Colors.Black;
            var barColor = dark ? Colors.LightGray : Colors.SteelBlue;

            //Bar dimensions limit
            double max = _chart.Items.Max(x => x.Value);
            double range = max;
            if (range == 0) range = 1;

            float barWidth = (width - margin * 2) / _chart.Items.Count;

            //Color settings
            canvas.FontSize = 16;
            canvas.FontColor = textColor;

            canvas.StrokeColor = axisColor;
            canvas.StrokeSize = 2;

            //Axis X
            canvas.DrawLine(margin, height - margin, width - margin, height - margin);
            //Axis Y
            canvas.DrawLine(margin, 0, margin, height - margin);

            // Axis Y Label
            canvas.SaveState();
            canvas.FontSize = 18;
            canvas.Translate(margin - margin/2, height / 2);
            canvas.Rotate(-90);
            canvas.DrawString(_chart.YAxisLabel, 0, 0, HorizontalAlignment.Center);
            canvas.RestoreState();

            _chart.RefreshAutoHeight();

            //Bars + labels rendering and calculations
            for (int i = 0; i < _chart.Items.Count; i++)
            {
                var item = _chart.Items[i];
                float x = margin + i * barWidth;

                float normalized = (float)(item.Value / range);
                float barHeight = normalized * (height - margin * 2);

                float y = height - margin - barHeight;

                canvas.FillColor = barColor;
                canvas.FillRectangle(x, y, barWidth * 0.8f, barHeight);
                canvas.StrokeColor = axisColor;
                canvas.StrokeSize = 2;
                canvas.DrawRectangle(x, y, barWidth * 0.8f, barHeight);

                canvas.DrawString(item.Label, x, height - margin + 8, barWidth * 0.8f, 20, HorizontalAlignment.Center, VerticalAlignment.Top);
            }
        }
    }
}
