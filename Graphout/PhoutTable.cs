using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using PlotterLib;

namespace Graphout
{
    public class PhoutTable
    {
        private static char[] _phoutInLineDelimiter = new char[] { '\t' };

        public ICollection<PhoutRow> PhoutRows { get; internal set; }

        private double _firstTimeRun;

        public PhoutTable(string fileName)
        {
            var fileData = File.ReadAllLines(fileName);

            PhoutRows = new List<PhoutRow>();

            foreach (var line in fileData)
            {
                string[] dataRow = line.Split(_phoutInLineDelimiter, StringSplitOptions.None);

                var phoutRow = new PhoutRow
                {
                    time = double.Parse(dataRow[0], CultureInfo.InvariantCulture),
                    tag = dataRow[1],
                    interval_real = float.Parse(dataRow[2]),
                    connect_time = float.Parse(dataRow[3]),
                    send_time = float.Parse(dataRow[4]),
                    latency = float.Parse(dataRow[5]),
                    receive_time = float.Parse(dataRow[6]),
                    interval_event = float.Parse(dataRow[7]),
                    size_out = float.Parse(dataRow[8]),
                    size_in = float.Parse(dataRow[9]),
                    net_code = int.Parse(dataRow[10]),
                    proto_code = int.Parse(dataRow[11])
                };

                PhoutRows.Add(phoutRow);
            }

            _firstTimeRun = PhoutRows.First().time;
        }

        public ICollection<PlotData> GetPlotData(string[] columnNames = null)
        {
            if (columnNames == null || columnNames.Length == 0)
            {
                columnNames = new string[]{
                    "interval_real",
                    "connect_time",
                    "send_time",
                    "latency",
                    "receive_time",
                    "interval_event",
                    "size_out",
                    "size_in",
                    "net_code",
                    "proto_code"
                };
            }

            return GetPlotDataByName(columnNames);
        }

        private ICollection<PlotData> GetPlotDataByName(string[] columnNames)
        {
            var result = new List<PlotData>();
            var rnd = new Random();
            foreach (var name in columnNames)
            {
                result.Add(new PlotData { Caption = name, Pen = new Pen(Color.FromArgb(190, rnd.Next(255), rnd.Next(255), rnd.Next(255)), 1.0f) });
            }

            foreach (var item in PhoutRows)
            {
                foreach (var name in columnNames)
                {
                    AddByReflection(result, item, name);
                }
            }

            return result;
        }

        private void AddData(ICollection<PlotData> plotCollections, string name, float data, string xCoordData)
        {
            var plotData = plotCollections.Single(x => x.Caption.Equals(name));
            //Set X
            plotData.DataRow.Add(data);
            //Set Y
            plotData.XCoordData.Add(xCoordData);
        }

        private void AddByReflection(ICollection<PlotData> plotCollections, PhoutRow phoutRow, string name)
        {
            var type = phoutRow.GetType();
            var property = type.GetProperty(name);

            float value;
            if (property.PropertyType.Equals(typeof(int)))
            {
                value = (float)((int)property.GetValue(phoutRow));
            }
            else if (property.PropertyType.Equals(typeof(string)))
            {
                value = float.Parse((string)property.GetValue(phoutRow));
            }
            else if (property.PropertyType.Equals(typeof(float)))
            {
                value = (float)property.GetValue(phoutRow);
            }
            else
            {
                throw new Exception("Imposibble convert type");
            }

            var xProperty = type.GetProperty("time");
            var t = new TimeSpan(ConvertFromUnixTimestamp((double)xProperty.GetValue(phoutRow) - _firstTimeRun).Ticks);
            var xValue = string.Format("{0}:{1}:{2}.{3}", t.Hours, t.Minutes, t.Seconds, t.Milliseconds);

            AddData(plotCollections, name, value, xValue);
        }

        private DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }
    }
}
