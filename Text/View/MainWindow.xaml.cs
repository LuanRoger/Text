using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Text.Commands;
using Text.Models;

namespace Text.View
{
    public partial class MainWindow
    {
        private TextArchive textArchive { get; set; }

        public MainWindow(TextArchive textArchive = null)
        {
            InitializeComponent();

            this.textArchive = textArchive;
            LoadWindowData();
            LoadConfiguration();
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
            #region EventHandlers
            btnQaAbrir.Click += BtnAbrirArquivo_Click;
            btnQaSalvar.Click += BtnSave_Click;
            btnQaColar.Click += BtnPaste_Click;
            btnQaCortar.Click += BtnCut_Click;

            if (textArchive != null)
            {
                textArchive.OnModifyHasSaved += (_, e) => {
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
                };
            }
            txtMain.TextChanged += delegate 
            {
                if (textArchive == null) return;

                textArchive.SetHasSaved(false); 
                textArchive.text = txtMain.Text;
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

        private void LoadConfiguration()
        {
            chbSaveFont.IsChecked = Consts.CONFIGURATION.saveFont;
            chbSaveFontSize.IsChecked = Consts.CONFIGURATION.saveFontSize;
            chbSaveState.IsChecked = Consts.CONFIGURATION.saveLastFile;

            cmbFonts.SelectedItem = new System.Windows.Media.FontFamily(Consts.CONFIGURATION.font);
            ComboBoxItem comboBoxModelFontSizeItem = ((ComboBoxItem)cmbTamanhoFonte.Items[0]);
            comboBoxModelFontSizeItem.Content = Consts.CONFIGURATION.fontSize + "px";
            cmbTamanhoFonte.SelectedItem = comboBoxModelFontSizeItem;
        }
        private void RibbonWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (textArchive != null && !textArchive.hasSaved)
            {
                MessageBoxResult dialogResult = MessageBox.Show("O arquivo não foi salvo, deseja salvar antes de sair?", "Aviso",
                    MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                switch(dialogResult)
                {
                    case MessageBoxResult.Yes:
                        BtnSave_Click(this, null);
                        break;
                    case MessageBoxResult.No:
                        break;
                    case MessageBoxResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
            if (textArchive == null || !Consts.CONFIGURATION.saveLastFile) return;
            Consts.CONFIGURATION.SetLastTextFile(textArchive.fileSource);
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnShowFileInfo_Click(object sender, RoutedEventArgs e) => new ShowFileInfoCommand(textArchive).Execute();

        #region SaveFile
        private async void BtnSaveWith_Click(object sender, RoutedEventArgs e)
        {
            pgbAsyncTasks.Visibility = Visibility.Visible;

            QueryInvoker<Task<TextArchive>> queryInvoker = new(new SaveWithCommand(textArchive));
            textArchive = await queryInvoker.Invoker();
            if (textArchive != null) LoadInfoText();

            pgbAsyncTasks.Visibility = Visibility.Hidden;
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            pgbAsyncTasks.Visibility = Visibility.Visible;

            CommandInvokerAsync commandInvokerAsync = new CommandInvokerAsync(new SaveCommand(textArchive));
            await commandInvokerAsync.Invoker();

            pgbAsyncTasks.Visibility = Visibility.Hidden;
        }
        #endregion

        #region OpenFile
        private void BtnAbrirArquivoJanela_Click(object sender, RoutedEventArgs e) => 
            new CommandInvoker(new OpenInWindowCommand()).Invoker();

        private void BtnAbrirArquivo_Click(object sender, RoutedEventArgs e)
        {
            QueryInvoker<TextArchive> queryInvoker = new(new OpenCommand());
            textArchive = queryInvoker.Invoker();

            LoadInfoText();
        }
        #endregion

        #region Paste Cut Copy
        private void BtnPaste_Click(object sender, RoutedEventArgs e) => txtMain.Text += Clipboard.GetText();
        private void BtnCut_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(txtMain.SelectedText);
            txtMain.SelectedText = string.Empty;
        }
        private void BtnCopy_Click(object sender, RoutedEventArgs e) => Clipboard.SetText(txtMain.SelectedText);
        #endregion

        #region cmbSelectionChanged
        private void cmbFonts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Windows.Media.FontFamily font = (System.Windows.Media.FontFamily)cmbFonts.SelectedItem;
            new CommandInvoker(new SelectFontCommand(font)).Invoker();

            txtMain.FontFamily = font;
            lblFontStyle.Content = font.ToString();
        }
        private void cmbTamanhoFonte_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            QueryInvoker<double> queryInvoker = new(new SelectFontSizeCommand
                (((ComboBoxItem)cmbTamanhoFonte.SelectedItem).Content.ToString()));
            double fontSize = queryInvoker.Invoker();


            txtMain.FontSize = fontSize;
            lblFontSize.Content = fontSize.ToString();
        }
        #endregion

        #region ConfigurationCheckboxChange
        private void chbSaveFont_Click(object sender, RoutedEventArgs e) =>
            Consts.CONFIGURATION.SetSaveFont(chbSaveFont.IsChecked != null && (bool)chbSaveFont.IsChecked);
        private void chbSaveFontSize_Click(object sender, RoutedEventArgs e) =>
            Consts.CONFIGURATION.SetSaveFontSize(chbSaveFontSize.IsChecked != null && (bool)chbSaveFontSize.IsChecked);
        private void chbSaveState_Click(object sender, RoutedEventArgs e)
        {
            Consts.CONFIGURATION.SetLastTextFile(string.Empty);
            Consts.CONFIGURATION.SetSaveLastFile(chbSaveState.IsChecked != null && (bool)chbSaveState.IsChecked);
        }
        #endregion

        private void btnAddDate_Click(object sender, RoutedEventArgs e) => txtMain.Text += DateTime.Now;
        private void btnSelectAll_Click(object sender, RoutedEventArgs e) => txtMain.SelectAll();
        private void btnSobre_Click(object sender, RoutedEventArgs e) => new AboutCommand().Execute();
    }
}
