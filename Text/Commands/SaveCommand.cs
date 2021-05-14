using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using Text.Models;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;

namespace Text.Commands
{
    class SaveCommand : ICommandAsync
    {
        public TextArchive textArchive { get; set; }
        public SaveCommand(TextArchive textArchive)
        {
            this.textArchive = textArchive;
        }
        public async Task Execute()
        {
            if (textArchive == null)
            {
                new Reciver().OpenNonOpenFileDialog();
                return;
            }
            await textArchive.WriteTextFileAsync();
        }
    }

    class SaveWithCommand : Query<Task<TextArchive>>
    {
        public TextArchive textArchive { get; set; }
        public SaveWithCommand(TextArchive textArchive)
        {
            this.textArchive = textArchive;
        }

        public async Task<TextArchive> Execute()
        {
            string fileName = new Reciver().OpenSaveFileDialog();
            if (fileName == null) return null;

            textArchive.fileSource = fileName;
            await textArchive.WriteTextFileAsync();

            return textArchive = new TextArchive
            (
                fileName: Path.GetFileName(fileName),
                fileSource: fileName,
                hasSaved: true,
                text: File.ReadAllText(fileName),
                lastModified: File.GetLastWriteTimeUtc(fileName)
            );
        }
    }
}
