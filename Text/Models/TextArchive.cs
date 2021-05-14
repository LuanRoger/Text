using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace Text.Models
{
    public class HasSavedEventArgs : EventArgs
    {
        public bool saveState { get; set; }
    }
    public class TextArchive
    {
        public string fileName { get; set; }
        public string fileSource { get; set; }
        public bool hasSaved { get; private set; }
        public void SetHasSaved(bool hasSaved) 
        {
            this.hasSaved = hasSaved;
            OnModifyHasSavedCall(hasSaved);
        }
        public string text { get; set; }
        public DateTime lastModified { get; set; }

        public TextArchive(string fileName, string fileSource, bool hasSaved, string text, DateTime lastModified)
        {
            this.fileName = fileName;
            this.fileSource = fileSource;
            this.hasSaved = hasSaved;
            this.text = text;
            SetHasSaved(hasSaved);
            this.lastModified = lastModified;
        }
        public static TextArchive OpenFileArchive()
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

        public async Task WriteTextFileAsync()
        {
            List<Task> tasks = new()
            {
                File.WriteAllTextAsync(fileSource, text),
            };
            while(tasks.Count != 0)
            {
                Task taskObserver = await Task.WhenAny(tasks);
                tasks.Remove(taskObserver);
            }

            SetHasSaved(true);
        }
        public delegate void OnModifyHasSavedEventHandler(object sender, HasSavedEventArgs e);
        public event OnModifyHasSavedEventHandler OnModifyHasSaved;
        protected virtual void OnModifyHasSavedCall(bool saveState)
        {
            OnModifyHasSaved?.Invoke(this, new HasSavedEventArgs { saveState =  saveState});
        }
    }
}
