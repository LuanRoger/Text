using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using Text.Models;
using Text.View;

namespace Text.Commands
{
    public class OpenCommand : Query<TextArchive>
    {
        public TextArchive Execute() => TextArchive.OpenFileArchive();
    }
    public class OpenInWindowCommand : ICommand
    {
        public void Execute()
        {
            TextArchive textArchiveNewWindow = TextArchive.OpenFileArchive();

            if (textArchiveNewWindow == null) return;

            MainWindow mainWindow = new MainWindow(textArchiveNewWindow);
            mainWindow.Show();
        }
    }
}
