using System;
using System.Collections.Generic;
using System.Drawing;
using Graphout;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlotterLib;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        // For manual use
        [Ignore]
        [TestMethod]
        public void TestMethod1()
        {
            var inputFile = @"D:\YandexTankAmmo\2016-06-01_15-31-14.PyukpA\phout_oPQuQO.log";
            var outputFile = @"D:\YandexTankAmmo\2016-06-01_15-31-14.PyukpA\graphout.png";
            var columns = new string[]{"interval_real", "proto_code", "net_code"};

            var plotData = new PhoutTable(inputFile).GetPlotData(columns);

            var plotter = new Plotter(plotData, 2048, 480, drawLegend: true);
            plotter.DrawTo(outputFile);
        }

        // For manual use
        [Ignore]
        [TestMethod]
        public void TestMethod2()
        {
            var plotData = new List<PlotData>() 
            {
                new PlotData{Caption="123", 
                    DataRow=new List<float>{0,1,2,3,4,5,6},
                    Pen = new Pen(Color.Black),
                    XCoordData=new List<string>{"0","1","2","3","4","5","6"}
                }
            };
            var outputFile = "D:\\graphout.png";
            var plotter = new Plotter(plotData, 2048, 480, drawLegend: true);
            plotter.DrawTo(outputFile);
        }
    }
}
