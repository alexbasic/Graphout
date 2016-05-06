using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphout
{
    public class ConsoleParamParser
    {
        public string InputFileName { get; set; }
        public string DrawToFileName { get; set; }
        public string[] DrawColumnNames { get; set; }
        public int HorisontalSize { get; set; }
        public int VerticalSize { get; set; }

        public ConsoleParamParser(string[] args)
        {
            var notCorrectSizeParameterString = "Not correct size parameter";

            InputFileName = ExtractParameterValue(args, "-i=");
            DrawToFileName = ExtractParameterValue(args, "-o=");

            var drawColumnNamesString = ExtractParameterValue(args, "-c=");
            if (!string.IsNullOrEmpty(drawColumnNamesString))
            {
                DrawColumnNames = drawColumnNamesString.Split(new char[] { ',' });
            }

            var sizeString = ExtractParameterValue(args, "-s=");
            if (!string.IsNullOrEmpty(sizeString))
            {
                var sizes = sizeString.Split(new char[] { 'x', '*', ':', 'X', '/' });
                if (sizes.Length != 2) throw new Exception(notCorrectSizeParameterString);

                int horisontalSize = 0;
                int verticalSize = 0;
                var succedConvert = int.TryParse(sizes[0], out horisontalSize) &&
                    int.TryParse(sizes[1], out verticalSize);

                if (!succedConvert) throw new Exception(notCorrectSizeParameterString);

                HorisontalSize = horisontalSize;
                VerticalSize = verticalSize;
            }
            else
            {
                HorisontalSize = 2048;
                VerticalSize = 480;
            }
        }

        public static string GetParametersHelp()
        {
            var formatHelpRow = "     {0}\t{1}\r\n";
            return
            "Parameters: \r\n" +
            string.Format(formatHelpRow, "-i=filepath", "Input phout file.") +
            string.Format(formatHelpRow, "-o=filepath", "Output image file.") +
            string.Format(formatHelpRow, "[-c=column1,column2,column3]", "Use only this column from phout file.") +
            string.Format(formatHelpRow, "[-s=Horizontal*Vertical]", 
            "Set output image size. Default image size is 2048x480. As delimiter of vertical and horizontal dimensions mast be used - 'x', '*', ':', 'X', '/'. Example: -s=1024*768 or -s=1024x768 or -s=1024:768 or -s=1024/768");
        }

        private string ExtractParameterValue(string[] args, string parameterName)
        {
            var parameter = args.Where(x => x.StartsWith(parameterName)).SingleOrDefault();
            return string.IsNullOrEmpty(parameter) ? string.Empty : parameter.Substring(args.Length);
        }
    }
}
