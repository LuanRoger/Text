using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Text.Commands
{
    class SelectFontCommand : ICommand
    {
        public FontFamily font { get; }
        public SelectFontCommand(FontFamily font)
        {
            this.font = font;
        }

        public void Execute() => Consts.CONFIGURATION.SetFont(font.ToString());
    }
}
