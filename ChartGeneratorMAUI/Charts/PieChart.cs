using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartGeneratorMAUI.Charts
{
    public struct PieData
    {
        public string Label { get; set; }
        public double Value { get; set; }
    }

    public class PieChart : GraphicsView
    {
        public List<PieData> Items { get; set; } = new();
        public string YAxisLabel { get; set; } = "";

        public PieChart()
        {
            Drawable = new PieChartDrawable(this);
        }
    }

    public class PieChartDrawable : IDrawable
    {
        private readonly PieChart _chart;

        public PieChartDrawable(PieChart chart)
        {
            _chart = chart;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            // No items provided case
            if (_chart.Items == null || _chart.Items.Count == 0)
            {
                canvas.FontSize = 18;
                canvas.FontColor = Application.Current!.RequestedTheme == AppTheme.Dark ? Colors.White : Colors.Black;
                canvas.DrawString("Brak danych", dirtyRect.Center.X, dirtyRect.Center.Y, HorizontalAlignment.Center);
                return;
            }

            // Check if sum equals 100
            double sum = _chart.Items.Sum(x => x.Value);
            if (Math.Abs(sum - 100) > 0.001)
            {
                canvas.FontSize = 16;
                canvas.FontColor = Application.Current!.RequestedTheme == AppTheme.Dark ? Colors.White : Colors.Black;
                canvas.DrawString("Błędny zestaw danych - suma musi wynosić 100%",
                                dirtyRect.Center.X, dirtyRect.Center.Y, HorizontalAlignment.Center);
                return;
            }

            // Container sizes
            float width = dirtyRect.Width;
            float height = dirtyRect.Height;
            float margin = 50;

            // Color choice
            bool dark = Application.Current!.RequestedTheme == AppTheme.Dark;
            var textColor = dark ? Colors.White : Colors.Black;
            var titleColor = dark ? Colors.White : Colors.Black;

            // Predefined colors for pie slices
            Color[] sliceColors = new Color[]
            {
            Colors.Red,
            Colors.Blue,
            Colors.Green,
            Colors.Orange,
            Colors.Purple,
            Colors.Teal,
            Colors.Magenta,
            Colors.Brown,
            Colors.Navy,
            Colors.Olive
            };

            // Draw title above the chart
            if (!string.IsNullOrEmpty(_chart.YAxisLabel))
            {
                canvas.FontSize = 20;
                canvas.FontColor = titleColor;
                canvas.DrawString(_chart.YAxisLabel, width / 2, 20, HorizontalAlignment.Center);
            }

            // Pie chart dimensions
            float chartSize = Math.Min(width - margin * 2, height - margin * 2 - 40);
            float centerX = width / 2;
            float centerY = (height - 40) / 2 + 40;
            float radius = chartSize / 2;

            // Draw pie slices
            float startAngle = 0;

            for (int i = 0; i < _chart.Items.Count; i++)
            {
                var item = _chart.Items[i];
                float sweepAngle = (float)(item.Value * 3.6); // 3.6 = 360/100

                // Use predefined colors, cycle if more items than colors
                Color sliceColor = sliceColors[i % sliceColors.Length];

                // Draw pie slice
                canvas.FillColor = sliceColor;
                canvas.StrokeColor = dark ? Colors.White : Colors.Black;
                canvas.StrokeSize = 1;

                var path = new PathF();
                path.MoveTo(centerX, centerY);
                path.AddArc(centerX - radius, centerY - radius, centerX + radius, centerY + radius,
                           startAngle, startAngle + sweepAngle, false);
                path.Close();

                canvas.FillPath(path);
                canvas.DrawPath(path);

                startAngle += sweepAngle;
            }

            // Draw legend
            float legendX = margin;
            float legendY = centerY + radius + 20;
            float legendItemHeight = 25;

            for (int i = 0; i < _chart.Items.Count; i++)
            {
                var item = _chart.Items[i];
                Color sliceColor = sliceColors[i % sliceColors.Length];

                // Draw color box
                canvas.FillColor = sliceColor;
                canvas.FillRectangle(legendX, legendY, 20, 15);
                canvas.StrokeColor = dark ? Colors.White : Colors.Black;
                canvas.StrokeSize = 1;
                canvas.DrawRectangle(legendX, legendY, 20, 15);

                // Draw legend text
                canvas.FontSize = 12;
                canvas.FontColor = textColor;
                canvas.DrawString($"{item.Label} ({item.Value:F1}%)", legendX, legendY - 20, HorizontalAlignment.Left);

                legendY += legendItemHeight;

                // Move to next column if needed
                if (legendY > height - margin && i < _chart.Items.Count - 1)
                {
                    legendX += 120;
                    legendY = centerY + radius + 20;
                }
            }
        }
    }
}
