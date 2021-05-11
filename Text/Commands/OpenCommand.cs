using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using Text.Models;

namespace Text.Commands
{
    public enum OpenFileMode { NewWindow, Open }
    public class OpenCommand : ICommandWithReturn<TextArchive>
    {
        private readonly OpenFileMode openFileMode;
        private TextArchive textArchive { get; }
        public TextArchive Result { get; private set;}

        public OpenCommand(OpenFileMode openFileMode, TextArchive textArchive)
        {
            this.openFileMode = openFileMode;
            this.textArchive = textArchive;
        }
        public void Execute()
        {
            switch (openFileMode)
            {
                case OpenFileMode.NewWindow:
                    OpenInNewWindow();
                    break;
                case OpenFileMode.Open:
                    Open();
                    break;
            }
        }

        private TextArchive Open() => Result = OpenTextFile();
        private void OpenInNewWindow()
        {
            TextArchive textArchiveNewWindow = OpenTextFile();

            if (textArchiveNewWindow == null) return;

            MainWindow mainWindow = new MainWindow(textArchiveNewWindow);
            mainWindow.Show();
        }
        private TextArchive OpenTextFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = Consts.TXTFILTER;
            bool? result = openFileDialog.ShowDialog();

            if (result is null or false) return null;

            TextArchive fileTextToOpen = new TextArchive
            (
                fileName: Path.GetFileName(openFileDialog.FileName),
                fileSource: openFileDialog.FileName,
                hasSaved: true,
                text: File.ReadAllText(openFileDialog.FileName),
                lastModified: File.GetLastWriteTimeUtc(openFileDialog.FileName)
            );
            return fileTextToOpen;
        }
    }
}
