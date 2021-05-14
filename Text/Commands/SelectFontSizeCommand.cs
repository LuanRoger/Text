using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text.Commands
{
    class SelectFontSizeCommand : Query<double>
    {
        private string fontSize { get; }

        public SelectFontSizeCommand(string fontSize)
        {
            this.fontSize = fontSize;
        }
        public double Execute()
        {
            double fontiSizeConvertDouble = new Reciver().ConvertDouble(fontSize);


            Consts.CONFIGURATION.SetFontSize(fontiSizeConvertDouble);

            return fontiSizeConvertDouble;
        }
    }
}
