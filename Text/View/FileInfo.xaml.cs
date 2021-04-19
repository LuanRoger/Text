using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using Text.Models;
using System.Windows.Media.Imaging;

namespace Text.View
{    
    public partial class FileInfo
    {
        public FileInfo(TextArchive textArchive)
        {
            InitializeComponent();

            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"\assets\imgs\txt_file.png");
            image.EndInit();
            imgFileText.Source = image;

            txtFileName.Text = textArchive.fileName;
            txtFileSize.Text = new System.IO.FileInfo(textArchive.fileSource).Length.ToString() + "KB";
            txtFileSource.Text = textArchive.fileSource;
            txtLastUpdate.Text = File.GetLastWriteTimeUtc(textArchive.fileSource).ToString();
            txtLastAccess.Text = File.GetLastAccessTimeUtc(textArchive.fileSource).ToString();
            txtCreateDate.Text = File.GetCreationTimeUtc(textArchive.fileSource).ToString();

            btnFecharInfo.Click += BtnFecharInfo_Click;
        }

        private void BtnFecharInfo_Click(object sender, RoutedEventArgs e) => Close();
    }
}
