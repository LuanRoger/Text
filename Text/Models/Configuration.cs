using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Text.Models
{
    public class Configuration
    {
        private bool saveForEachChange { get; set; }
        private readonly System.Configuration.Configuration config = 
            ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        public bool saveFont { get; private set; }
        public void SetSaveFont(bool saveFont)
        {
            this.saveFont = saveFont;
            if (saveForEachChange) SaveConfiguration();
        }
        public bool saveFontSize { get; private set; }
        public void SetSaveFontSize(bool saveFontSize)
        {
            this.saveFontSize = saveFontSize;
            if (saveForEachChange) SaveConfiguration();
        }
        public string font { get; private set; }
        public void SetFont(string font)
        {
            this.font = font;
            if (saveForEachChange) SaveConfiguration();
        }
        public double fontSize { get; private set; }
        public void SetFontSize(double fontSize)
        {
            this.fontSize = fontSize;
            if (saveForEachChange) SaveConfiguration();
        }
        public bool saveLastFile { get; private set; }
        public void SetSaveLastFile(bool saveLastFile)
        {
            this.saveLastFile = saveLastFile;
            if (saveForEachChange) SaveConfiguration();
        }
        public string lastTextFile { get; private set; }
        public void SetLastTextFile(string filePath)
        {
            lastTextFile = filePath;
            if (saveForEachChange) SaveConfiguration();
        }
        public Configuration(bool saveForEachChange)
        {
            this.saveForEachChange = saveForEachChange;

            saveFont = ConfigurationManager.AppSettings["saveFont"].ToString() == "1";
            saveFontSize = ConfigurationManager.AppSettings["saveFontSize"].ToString() == "1";
            saveLastFile = ConfigurationManager.AppSettings["saveLastFile"].ToString() == "1";

            font = ConfigurationManager.AppSettings["font"].ToString();
            fontSize = double.Parse(ConfigurationManager.AppSettings["fontSize"].ToString());
            lastTextFile = ConfigurationManager.AppSettings["lastTextFile"];
        }

        public void SaveConfiguration()
        {
            config.AppSettings.Settings["saveFont"].Value = saveFont ? "1" : "0";
            config.AppSettings.Settings["saveFontSize"].Value = saveFontSize ? "1" : "0";
            config.AppSettings.Settings["saveLastFile"].Value = saveLastFile ? "1" : "0";
            if (saveFont) config.AppSettings.Settings["font"].Value = font;
            if (saveFontSize) config.AppSettings.Settings["fontSize"].Value = fontSize.ToString();
            if (saveLastFile) config.AppSettings.Settings["lastTextFile"].Value = lastTextFile;

            config.Save();
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
