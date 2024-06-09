using Microsoft.Win32;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace uml_hw_two
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private string _file;

        private void selectFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                _file = openFileDialog.FileName;
                fileTextBlock.Text = _file;
            }
        }

        private async void uploadButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_file))
            {
                MessageBox.Show("Выберите файл!");
                return;
            }

            BaseFileUploader uploader = new GoogleDriveFileUploader();
            uploader = new DropBoxFileUploaderDecorator(uploader, accessTokenTextBox.Text);

            try
            {
                await uploader.UploadFileAsync(_file);
                MessageBox.Show("Файл успешно загружен.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки файла: {ex.Message}");
            }
        }
    }
}