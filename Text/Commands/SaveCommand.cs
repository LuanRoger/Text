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
    enum SaveMode { SaveWith, Save }
    class SaveCommand : ICommandAsyncWithReturn<TextArchive>
    {
        public TextArchive Result { get; private set;}
        public string text { get; set; }
        public TextArchive textArchive { get; set; }
        private SaveMode saveMode;

        public SaveCommand(SaveMode mode, TextArchive textArchive, string text)
        {
            saveMode = mode;
            this.text = text;
            this.textArchive = textArchive;
        }
        public async Task Execute()
        {
            switch (saveMode)
            {
                case SaveMode.Save:
                    await Save();
                    break;
                case SaveMode.SaveWith:
                    await SaveWith();
                    break;
            }
        }
        private async Task Save()
        {
            if (textArchive == null)
            {
                await SaveWith();
                return;
            }

            await WriteTextFile();
        }
        private async Task SaveWith()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = Consts.TXTFILTER;
            DialogResult dialogResult = saveFileDialog.ShowDialog();

            if (dialogResult != DialogResult.OK) return;

            textArchive.fileSource = saveFileDialog.FileName;
            await WriteTextFile();

            textArchive = new TextArchive
            (
                fileName: Path.GetFileName(saveFileDialog.FileName),
                fileSource: saveFileDialog.FileName,
                hasSaved: true,
                text: File.ReadAllText(saveFileDialog.FileName),
                lastModified: File.GetLastWriteTimeUtc(saveFileDialog.FileName)
            );

            Result = textArchive;
        }
        private async Task WriteTextFile()
        {
            List<Task> tasks = new()
            {
                File.WriteAllTextAsync(textArchive.fileSource, text),
                Task.Run(() => Thread.Sleep(2000))
            };
            while(tasks.Count != 0)
            {
                Task taskObserver = await Task.WhenAny(tasks);
                tasks.Remove(taskObserver);
            }
        }
    }
}
