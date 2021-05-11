using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text.Commands
{
    class SelectFontSizeCommand : ICommand
    {
        public double fontSize { get; }
        public SelectFontSizeCommand(double fontSize)
        {
            this.fontSize = fontSize;
        }
        public void Execute()
        {
            Consts.CONFIGURATION.SetFontSize(fontSize);
        }
    }
}
