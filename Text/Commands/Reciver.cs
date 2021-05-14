using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Text.Commands
{
    class Reciver
    {
        /// <summary>
        /// Open a SaveFileDialog
        /// </summary>
        /// <returns>SaveFileDialog.FileName</returns>
        public string OpenSaveFileDialog()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = Consts.TXTFILTER;
            DialogResult dialogResult = saveFileDialog.ShowDialog();

            if (dialogResult != DialogResult.OK) return null;
            return saveFileDialog.FileName;
        }
        public void OpenNonOpenFileDialog() => 
            MessageBox.Show("Você deve abrir um arquivo para salvar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        public double ConvertDouble(string fontSize) => double.Parse(fontSize.Replace("px", string.Empty));
    }
}
