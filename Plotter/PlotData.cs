using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotterLib
{
    public class PlotData
    {
        public int Order { get; set; }

        public Pen Pen { get; set; }

        public string Caption { get; set; }

        public ICollection<float> DataRow { get; set; }

        public ICollection<string> XCoordData { get; set; }

        public PlotData()
        {
            DataRow = new List<float>();
            Pen = new Pen(Color.Red, 1.0f);
            XCoordData = new List<string>();
        }
    }
}
