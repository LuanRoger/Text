using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Text.Models;

namespace Text.Commands
{
    class ShowFileInfoCommand : ICommand
    {
        TextArchive textFileToOpen {get;}
        public ShowFileInfoCommand(TextArchive textFileToOpen)
        {
            this.textFileToOpen = textFileToOpen;
        }
        public void Execute()
        {
            if(textFileToOpen == null)
            {
                MessageBox.Show("Não há arquivo carregado", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            View.FileInfo fileInfo = new(textFileToOpen);
            fileInfo.Show();
        }
    }
}
