using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Microsoft.Win32;
using Point = System.Drawing.Point;
using PointWin = System.Windows.Point;
using Image = System.Windows.Controls.Image;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using MessageBox = System.Windows.Forms.MessageBox;
using System.Resources;

namespace CartoonMemento
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private int first = 0;
        private DrawingCanvas dc;
        private Boolean isSaved = false;

        public MainWindow()
        {
            InitializeComponent();
            InitializeStickers();

        }

        private void InitializeStickers()
        {
            canvasImage.AllowDrop = true;
            //TODO Change this to avoid hardcore programming

            string resXfile = @".\Resources.resx";
            string path;

            using (ResXResourceSet resultSet = new ResXResourceSet(resXfile))
            {
                path = resultSet.GetString("stickers");
            }

            dc = new DrawingCanvas(canvasImage);
            string[] dirs = Directory.GetDirectories(path);

            foreach (string dir in dirs)
            {
                InitializeStickerPack(dir);
            }
        }

        private void ButtonLoad_Click(object sender, RoutedEventArgs e)
        {


            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*";

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                canvasImage.Children.Clear();
                Image image = new Image();
                Uri uri = new Uri(ofd.FileName);
                BitmapImage bmp = new BitmapImage(uri);
                image.Source = bmp;
                image.Height = bmp.Height;
                image.Width = bmp.Width;
                dc.LoadImage(image);
            }
            else if(ofd.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                DialogResult res = MessageBox.Show("Picture loading canceled.", "Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void CreateSticker(object sender, MouseButtonEventArgs e)
        {
            StickerImage stickerImage = new StickerImage((Image)sender,this.dc);
            dc.AddSticker(stickerImage);
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            dc.RemoveActive();
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            Rect rect = VisualTreeHelper.GetDescendantBounds(canvasImage);
            RenderTargetBitmap rtb = new RenderTargetBitmap((Int32)canvasImage.Width, (Int32)canvasImage.Height, 96, 96, PixelFormats.Pbgra32);
            DrawingVisual dv = new DrawingVisual();

            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(canvasImage);
                dc.DrawRectangle(vb, null, new Rect(new PointWin(), rect.Size));
            }

            rtb.Render(dv);

            PngBitmapEncoder png = new PngBitmapEncoder();

            png.Frames.Add(BitmapFrame.Create(rtb));

            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    png.Save(myStream);
                    myStream.Close();
                }
                DialogResult res = MessageBox.Show("Picture saved in folder", "Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                DialogResult res = MessageBox.Show("Problem with saving", "Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ButtonImport_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.Description =
                "Select directory with png pictures";

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                String path = fbd.SelectedPath;
                InitializeStickerPack(path);
            }
        }
        
        private void InitializeStickerPack(String dir)
        {
            FileInfo dirInfo = new FileInfo(dir);
            Expander exp = new Expander();
            exp.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(190,197,173));
            exp.Padding = new Thickness(2);
            exp.Margin = new Thickness(3);
            exp.Header = dirInfo.Name;
            exp.Width = 330;
            if (first == 0)
            {
                exp.IsExpanded = true;
                first++;
            }
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
                Grid.SetRow(image, rowNum);
                Grid.SetColumn(image, columnNum);

                if (columnNum % 3 == 2)
                {
                    columnNum = 0;
                    rowNum++;
                }
                else
                    columnNum++;
                image.Cursor = System.Windows.Input.Cursors.Hand;
                image.MouseDown += CreateSticker;
                grid.Children.Add(image);
            }

            ScrollViewer scroll = new ScrollViewer();
            scroll.Content = grid;
            scroll.Height = 200;
            exp.Content = scroll;
            stickers.Children.Add(exp);
        }

        private void Exit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void DeleteActiveSticker(object sender, MouseButtonEventArgs e)
        {
            dc.RemoveSticker(dc.GetActive(),e);
        }

        private void SaveAs(object sender, MouseButtonEventArgs e)
        {
            ButtonSave_Click(sender,e);
        }

        private void ClearSticker(object sender,MouseButtonEventArgs e)
        {
            textBox.Text = "";
        }

        private void SearchButtonAction(object sender, MouseButtonEventArgs e)
        {
            string findSomething = textBox.Text;

            UIElementCollection expanders = stickers.Children;

            if (!findSomething.Equals(""))
            {
                foreach (Expander expander in expanders)
                {
                    string header = (string)expander.Header;
                    if (header.Contains(findSomething))
                    {
                        expander.Visibility = Visibility.Visible;
                        expander.IsExpanded = true;
                    }
                    else
                    {
                        expander.Visibility = Visibility.Hidden;
                    }
                }
            }
            else
            {
                ((Expander)expanders[0]).IsExpanded = true;
                foreach (Expander expander in expanders)
                    expander.Visibility = Visibility.Visible;
            }
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow hw = new HelpWindow();
            hw.Topmost = true;
            hw.Show();
        }
    }
}