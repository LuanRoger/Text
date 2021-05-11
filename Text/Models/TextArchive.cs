using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public delegate void OnModifyHasSavedEventHandler(object sender, HasSavedEventArgs e);
        public event OnModifyHasSavedEventHandler OnModifyHasSaved;
        protected virtual void OnModifyHasSavedCall(bool saveState)
        {
            OnModifyHasSaved?.Invoke(this, new HasSavedEventArgs { saveState =  saveState});
        }
    }
}
