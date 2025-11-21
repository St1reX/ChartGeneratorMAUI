using ChartGeneratorMAUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartGeneratorMAUI.Charts
{
    public class LineChart : GraphicsView
    {
        public List<ChartData> Items { get; set; } = new();
        public string YAxisLabel { get; set; } = "";

        //Dynamic resize - depends on biggest value in dataset
        public void RefreshAutoHeight()
        {
            if (Items == null || Items.Count == 0) return;
            double max = Items.Max(x => x.Value);
            double scale = 20;
            HeightRequest = Math.Clamp(max * scale + 120, 150, 800);
        }

        public LineChart()
        {
            Drawable = new LineChartDrawable(this);
        }
    }

    public class LineChartDrawable : IDrawable
    {
        private readonly LineChart _chart;
        public LineChartDrawable(LineChart chart)
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
            var lineColor = dark ? Colors.LightGray : Colors.SteelBlue;

            //Data range calculation
            double max = _chart.Items.Max(x => x.Value);
            double min = 0;
            double range = max - min;
            if (range == 0) range = 1;

            float chartWidth = width - margin * 2;
            float chartHeight = height - margin * 2;

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
            canvas.Translate(margin - margin / 2, height / 2);
            canvas.Rotate(-90);
            canvas.DrawString(_chart.YAxisLabel, 0, 0, HorizontalAlignment.Center);
            canvas.RestoreState();

            _chart.RefreshAutoHeight();

            //Calculate points for line chart with side margins
            var points = new PointF[_chart.Items.Count];
            float sideMargin = 50; // Additional margin for first and last point
            float availableWidth = chartWidth - 2 * sideMargin;
            float pointSpacing = availableWidth / (_chart.Items.Count - 1);

            for (int i = 0; i < _chart.Items.Count; i++)
            {
                var item = _chart.Items[i];
                float x = margin + sideMargin + i * pointSpacing;

                //Normalize value to chart height
                float normalized = (float)((item.Value - min) / range);
                float y = height - margin - normalized * chartHeight;

                points[i] = new PointF(x, y);
            }

            //Draw line chart
            canvas.StrokeColor = lineColor;
            canvas.StrokeSize = 3;

            for (int i = 0; i < points.Length - 1; i++)
            {
                canvas.DrawLine(points[i], points[i + 1]);
            }

            //Draw data points and labels
            canvas.StrokeColor = axisColor;
            canvas.StrokeSize = 2;

            for (int i = 0; i < points.Length; i++)
            {
                var point = points[i];
                var item = _chart.Items[i];

                //Draw data point
                canvas.FillColor = lineColor;
                canvas.FillCircle(point, 5);

                //Draw label below X-axis
                float labelWidth = pointSpacing * 0.8f;
                float labelX = point.X - labelWidth / 2;
                canvas.DrawString(item.Label, labelX, height - margin + 8, labelWidth, 20, HorizontalAlignment.Center, VerticalAlignment.Top);
            }
        }
    }
}
