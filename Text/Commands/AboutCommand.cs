using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Text.Commands
{
    class AboutCommand : ICommand
    {
        public void Execute()
        {
            MessageBox.Show($"Text v{Assembly.GetExecutingAssembly().GetName().Version}" +
                            $"\nLicença: MIT License", 
                "Sobre", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
