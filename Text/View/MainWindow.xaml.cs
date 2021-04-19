using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using Text.Models;
using System.IO;
using Microsoft.Win32;
using System.Drawing.Text;
using System.Drawing;
using System.Reflection;
using System.Threading;

namespace Text
{
    public partial class MainWindow
    {
        private readonly string _txtFilter = "Documento de Texto | *.txt";

        private TextArchive textArchive { get; set; }
        public MainWindow(TextArchive textArchive = null)
        {
            InitializeComponent();

            this.textArchive = textArchive;
            LoadWindowData();
            if (textArchive != null) LoadInfoText();
        }
        private void LoadInfoText()
        {
            Title = textArchive.fileName;
            txtMain.Text = textArchive.text;
            lblLastModified.Content = textArchive.lastModified.ToString();
        }
        private void LoadWindowData()
        {
            cmbFonts.SelectedItem = new System.Windows.Media.FontFamily("Arial");
            cmbTamanhoFonte.SelectedIndex = 4;

            #region EventHandlers
            btnAbrirArquivo.Click += BtnAbrirArquivo_Click;
            btnAbrirArquivoJanela.Click += BtnAbrirArquivoJanela_Click;
            btnSave.Click += BtnSave_Click;
            btnSaveWith.Click += BtnSaveWith_Click;
            btnShowFileInfo.Click += BtnShowFileInfo_Click;
            btnPrint.Click += BtnPrint_Click;
            btnExit.Click += BtnExit_Click;

            btnAddDate.Click += btnAddDate_Click;

            btnSobre.Click += btnSobre_Click;

            btnQaAbrir.Click += BtnAbrirArquivo_Click;
            btnQaSalvar.Click += BtnSave_Click;
            btnQaColar.Click += BtnPaste_Click;
            btnQaCortar.Click += BtnCut_Click;

            if (textArchive != null)textArchive.OnModifyHasSaved += TextArchive_OnModifyHasSaved;

            txtMain.TextChanged += delegate 
            {
                if (textArchive == null) return;
                textArchive.SetHasSaved(false); 
            };
            #endregion

            #region Shortcuts
            RoutedCommand shortcutCommand = new RoutedCommand();

            shortcutCommand.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(shortcutCommand, BtnSave_Click));

            shortcutCommand = new RoutedCommand();
            shortcutCommand.InputGestures.Add(new KeyGesture(Key.O, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(shortcutCommand, BtnAbrirArquivo_Click));
            #endregion
        }

        private void TextArchive_OnModifyHasSaved(object sender, HasSavedEventArgs e)
        {
            if(!e.saveState)
            {
                if (Title.Contains('*')) return;
                Title += '*';
            }
            else
            {
                try { Title.Remove(Title.IndexOf('*')); }
                catch { /*Nada*/ }
            }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnShowFileInfo_Click(object sender, RoutedEventArgs e)
        {
            if(textArchive == null)
            {
                MessageBox.Show("Não há arquivo carregado", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            View.FileInfo fileInfo = new(textArchive);
            fileInfo.Show();
        }

        #region SaveFile
        private async void BtnSaveWith_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = _txtFilter;
            bool? dialogResult = saveFileDialog.ShowDialog();

            if (dialogResult == null || dialogResult == false) return;

            await WriteTextFile(saveFileDialog.FileName, txtMain.Text);

            textArchive = new TextArchive
                (
                    fileName: Path.GetFileName(saveFileDialog.FileName),
                    fileSource: saveFileDialog.FileName,
                    hasSaved: true,
                    text: File.ReadAllText(saveFileDialog.FileName),
                    lastModified: File.GetLastWriteTimeUtc(saveFileDialog.FileName)
                );
            textArchive.OnModifyHasSaved += TextArchive_OnModifyHasSaved;
            LoadInfoText();
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (textArchive == null)
            {
                BtnSaveWith_Click(this, null);
                return;
            }

            await WriteTextFile(textArchive.fileSource, txtMain.Text);

            textArchive.text = txtMain.Text;
            LoadInfoText();
            textArchive.SetHasSaved(true);
        }
        private async Task WriteTextFile(string fileSource, string text)
        {
            pgbAsyncTasks.Visibility = Visibility.Visible;

            List<Task> tasks = new()
            {
                File.WriteAllTextAsync(fileSource, text)
            };
            while(tasks.Count != 0)
            {
                Task taskObserver = await Task.WhenAny(tasks);
                tasks.Remove(taskObserver);
            }
            pgbAsyncTasks.Visibility = Visibility.Hidden;
        }
        #endregion

        #region OpenFile
        private void BtnAbrirArquivoJanela_Click(object sender, RoutedEventArgs e)
        {
            TextArchive textArchiveNewWindow = OpenTextFile();

            if (textArchiveNewWindow == null) return;

            MainWindow mainWindow = new MainWindow(textArchiveNewWindow);
            mainWindow.Show();
        }

        private void BtnAbrirArquivo_Click(object sender, RoutedEventArgs e)
        {
            textArchive = OpenTextFile();
            if (textArchive == null) return;

            LoadInfoText();
        }
        private TextArchive OpenTextFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = _txtFilter;
            bool? result = openFileDialog.ShowDialog();

            if (result == null || result == false) return null;

            TextArchive fileTextToOpen = new TextArchive
                (
                    fileName: Path.GetFileName(openFileDialog.FileName),
                    fileSource: openFileDialog.FileName,
                    hasSaved: true,
                    text: File.ReadAllText(openFileDialog.FileName),
                    lastModified: File.GetLastWriteTimeUtc(openFileDialog.FileName)
                );
            fileTextToOpen.OnModifyHasSaved += TextArchive_OnModifyHasSaved;
            return fileTextToOpen;
        }
        #endregion

        #region Paste Cut Copy
        private void BtnPaste_Click(object sender, RoutedEventArgs e) => txtMain.Text += Clipboard.GetText();
        private void BtnCut_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(txtMain.SelectedText);
            txtMain.SelectedText = string.Empty;
        }
        #endregion

        private void BtnCopy_Click(object sender, RoutedEventArgs e) => Clipboard.SetText(txtMain.SelectedText);

        #region cmbSelectionChanged
        private void cmbFonts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtMain.FontFamily = (System.Windows.Media.FontFamily)cmbFonts.SelectedItem;
            lblFontStyle.Content = ((System.Windows.Media.FontFamily)cmbFonts.SelectedItem).ToString();
        }
        private void cmbTamanhoFonte_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string fontSize = ((ComboBoxItem)cmbTamanhoFonte.SelectedItem).Content.ToString();
            txtMain.FontSize = double.Parse(fontSize.Replace("px", string.Empty));
            lblFontSize.Content = fontSize;
        }
        #endregion

        private void btnAddDate_Click(object sender, RoutedEventArgs e) => txtMain.Text += DateTime.Now;
        private void btnSelectAll_Click(object sender, RoutedEventArgs e) => txtMain.SelectAll();
        private void btnSobre_Click(object sender, RoutedEventArgs e) =>
            MessageBox.Show($"Text v{Assembly.GetExecutingAssembly().GetName().Version}", 
                "Sobre", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}
