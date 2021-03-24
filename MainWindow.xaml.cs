using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace CartoonMemento
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

        private void buttonLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter="Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*";

            if (ofd.ShowDialog() == true)
            {
                Uri uri = new Uri(ofd.FileName);
                image.Source = new BitmapImage(uri);
                imageStatus.Text = ofd.FileName;
            }
            else
            {
                imageStatus.Text = "Failure";
            }
        }
    }
}
