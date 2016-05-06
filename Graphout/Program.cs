using System;
using PlotterLib;

namespace Graphout
{
    static class Program
    {
        static void Main(params string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Parameters not exists");
                Console.WriteLine(ConsoleParamParser.GetParametersHelp());
                return;
            }

            var parser = new ConsoleParamParser(args);

            var plotData = new PhoutTable(parser.InputFileName).GetPlotData(parser.DrawColumnNames);

            var plotter = new Plotter(plotData, parser.HorisontalSize, parser.VerticalSize, drawLegend: true);
            plotter.DrawTo(parser.DrawToFileName);
        }
    }
}
