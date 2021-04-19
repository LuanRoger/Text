﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.IO;
using System.Windows;
using Text.Models;

namespace Text
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow mainWindow = new MainWindow();
            if (e.Args.Length > 0)
            {
                TextArchive textArchive = new TextArchive
                (
                    fileName: Path.GetFileName(e.Args[0]),
                    fileSource: e.Args[0],
                    hasSaved: true,
                    text: File.ReadAllText(e.Args[0]),
                    lastModified: File.GetLastWriteTimeUtc(e.Args[0])
                );
                mainWindow = new MainWindow(textArchive);
            }
            mainWindow.Closed += MainWindow_Closed;
            mainWindow.Show();
        }

        private void MainWindow_Closed(object sender, EventArgs e) => Environment.Exit(0);
    }
}
