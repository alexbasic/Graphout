using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotterLib
{
    public class Plotter
    {
        public ICollection<PlotData> PlotDataSeries { get; internal set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public int MarginTop { get; set; }
        public int MarginBottom { get; set; }
        public int MarginLeft { get; set; }
        public int MarginRight { get; set; }

        public int MarginBottomDiagram { get; set; }

        private int _legendHeight;

        public bool DrawLegend { get; set; }

        public Pen BorderPen { get; set; }

        public Plotter(ICollection<PlotData> plotDataSeries, int width, int height, bool drawLegend = false)
        {
            PlotDataSeries = plotDataSeries;

            _legendHeight = 50;

            DrawLegend = drawLegend;

            Width = width;
            Height = height;

            var margin = height / 20;

            MarginTop = margin;
            MarginBottom = margin;
            MarginBottomDiagram = (DrawLegend) ? MarginBottom + _legendHeight : MarginBottom;
            MarginLeft = margin;
            MarginRight = margin;

            BorderPen = new Pen(Color.Black, 1.0f);
        }

        public void DrawTo(string file)
        {
            using (Bitmap bitmap = new Bitmap(Width, Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.Clear(Color.White);

                    DrawHorLines(g, 10);

                    foreach (var item in PlotDataSeries.OrderBy(x => x.Order))
                    {
                        DrawOneCurve(g, item.Pen, item.DataRow);
                    }

                    DrawAxis(g, drawHorizontalMarkers: true);

                    DrawingLegend(g);

                }
                bitmap.Save(file, ImageFormat.Png);
            }
        }

        private void DrawingLegend(Graphics g)
        {
            var legendMargin = 10;

            float markerSize = 12;
            float markerSpace = 3;
            float rowSpace = 3;

            float initX = MarginLeft + legendMargin;
            float x = initX;
            float y = legendMargin;

            int maxX = Width - MarginRight;
            float doubleMarkerSpace = markerSpace * 2;

            var font = new Font("Arial", 12.0f, GraphicsUnit.Pixel);
            foreach (var item in PlotDataSeries)
            {
                x += markerSpace;

                g.FillRectangle(new SolidBrush(item.Pen.Color), x, y, markerSize, markerSize);

                x += markerSize + markerSpace;

                var caption = string.Format("{0} (MaxVal:{1})", item.Caption, item.DataRow.Max().ToString("F"));

                var stringSize = g.MeasureString(caption, font);

                g.DrawString(caption, font, Brushes.Black, x, y);

                x += stringSize.Width + markerSpace;
                if (x > maxX)
                {
                    x = initX;
                    y += stringSize.Height + rowSpace;
                };
            }
        }

        private void DrawAxis(Graphics g, bool drawHorizontalMarkers)
        {
            g.DrawLine(new Pen(Color.Black, 1.0f), MarginLeft, Height - MarginBottomDiagram, Width - MarginRight, Height - MarginBottomDiagram);

            if (drawHorizontalMarkers)
            {
                var xData = PlotDataSeries.First().XCoordData.ToArray();
                var fontForMarkers = new Font("Arial", 8f);
                var markerPen = new Pen(Color.Black, 1.0f);

                    var countMarkers = 3;
                    float markerInterval = (float)(Width - MarginRight - MarginLeft) / ((float)countMarkers - 1f);
                    float markerDataInterval = (float)(xData.Length) / ((float)countMarkers - 1f);

                    for (var i = 0; i < countMarkers; i++)
                    {
                        g.DrawLine(markerPen, MarginLeft + i * markerInterval, Height - MarginBottomDiagram + 3, MarginLeft + i * markerInterval, Height - MarginBottomDiagram);
                    }
            }
            g.DrawLine(new Pen(Color.Black, 1.0f), MarginLeft, Height - MarginBottomDiagram, MarginLeft, MarginTop);
        }

        private void DrawHorLines(Graphics g, int count)
        {
            var width = Width - MarginLeft - MarginRight;
            var height = Height - MarginTop - MarginBottomDiagram;
            var step = height / (count - 1);
            var y = MarginBottomDiagram + step;
            for (var c = 0; c < count; c++)
            {
                g.DrawLine(new Pen(Color.FromArgb(220, 220, 220), 1.0f), MarginLeft, Height - y, MarginLeft + width, Height - y);
                y += step;
            }
        }

        private void DrawOneCurve(Graphics g, Pen pen, IEnumerable<float> rowData)
        {
            var rowDataNorm = rowData.NormalizeVector();

            int scale = Height - MarginTop - MarginBottomDiagram;

            int clientWidth = Width - MarginLeft - MarginRight;

            var lengthOfVector = rowDataNorm.Count() - 1;
            float step = (float)clientWidth / lengthOfVector;
            float counter = -step;
            var points = rowDataNorm.Select(x => new Point(((int)(counter = counter + step)) + MarginLeft, Height - (int)(x * scale) - MarginBottomDiagram)).ToArray();

            g.DrawLines(pen, points);
        }
    }
}
