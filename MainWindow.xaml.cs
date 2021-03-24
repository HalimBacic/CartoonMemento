using System;
using System.IO;
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
            InitializeStickers();
        }

        private void InitializeStickers()
        {
            //TODO Change this to avoid hardcore programming
            string path = "D:\\Fakultet\\HCI\\PROJEKAT\\CartoonMemento\\CartoonMemento\\resources";

            string[] dirs = Directory.GetDirectories(path);

            foreach (string dir in dirs)
            {
                FileInfo dirInfo = new FileInfo(dir);
                Expander exp = new Expander();
                exp.Header = dirInfo.Name;
                exp.Width = 330;
                exp.Height = 170;
                Grid grid = new Grid();
                grid.Width = 328;
                string[] files = Directory.GetFiles(dir);


                int numOfFiles = files.Length;
                int numOfRows;
                if (numOfFiles % 3 == 0)
                    numOfRows = numOfFiles / 3;
                else
                    numOfRows = numOfFiles / 3 + 1;

                for (int i = 0; i < 3; i++)
                {
                    ColumnDefinition col = new ColumnDefinition();
                    grid.ColumnDefinitions.Add(col);
                }

                for (int i = 0; i < numOfRows; i++)
                {
                    RowDefinition row = new RowDefinition();
                    grid.RowDefinitions.Add(row);
                }

                int rowNum = 0; int columnNum = 0;
                foreach (string file in files)
                {
                    FileInfo fi = new FileInfo(file);
                    Image image = new Image();
                    Uri uri = new Uri(fi.FullName);
                    image.Source = new BitmapImage(uri);
                    image.Height = 100;
                    image.Width = 100;

                    Grid.SetRow(image,rowNum);
                    Grid.SetColumn(image,columnNum);

                    if (columnNum % 3 == 2)
                    {
                        columnNum = 0;
                        rowNum++;
                    }
                    else
                        columnNum++;

                    grid.Children.Add(image);
                }

                ScrollViewer scroll = new ScrollViewer();
                scroll.Content = grid;

                exp.Content = scroll;
                stickers.Children.Add(exp);
            }
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
