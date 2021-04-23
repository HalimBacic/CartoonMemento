﻿using System;
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

namespace CartoonMemento
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        private int first = 0;
        private DrawingCanvas dc;
        private string path = "";

        public MainWindow()
        {
            InitializeComponent();
            InitializeStickers();

        }

        private void InitializeStickers()
        {
            canvasImage.AllowDrop = true;
            //TODO Change this to avoid hardcore programming
            string path = "D:\\Fakultet\\HCI\\PROJEKAT\\CartoonMemento\\CartoonMemento\\resources\\Stickers";

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
                image.Width = bmp.Width;
                Console.WriteLine();
                dc.LoadImage(image);
                imageStatus.Text = ofd.FileName;
            }
            else
            {
                imageStatus.Text = "Failure";
            }
        }

        private void CreateSticker(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("Dimenzije:"+((Image)sender).Width+" "+ ((Image)sender).Height);
            imageStatus.Text = ((Image)sender).Source.ToString();
            StickerImage stickerImage = new StickerImage((Image)sender, this.dc);
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

            RenderTargetBitmap rtb = new RenderTargetBitmap((Int32)dc.Width, (Int32)dc.Height, 96, 96, PixelFormats.Pbgra32);
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
                    path = saveFileDialog1.FileName;
                    png.Save(myStream);
                    myStream.Close();
                }
            }

            if (path != "")
            {
                DialogResult res = MessageBox.Show("Picture saved in folder", "Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            dc.RemoveSticker(dc.GetActive(), e);
        }

        private void SaveAs(object sender, MouseButtonEventArgs e)
        {
            ButtonSave_Click(sender, e);
        }

        private void Save(object sender, MouseButtonEventArgs e)
        {
            if (path == "")
                ButtonSave_Click(sender, e);
            else
            {

                dc.RemoveActive();
                MemoryStream myStream = new MemoryStream(); ;


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

                png.Save(myStream);

                System.Drawing.Image img = System.Drawing.Image.FromStream(myStream);
                img.Save(path);


            }

        }

        private void NewFile(object sender, MouseButtonEventArgs e)
        {
            if (path != "")
            {
                DialogResult res = MessageBox.Show("Da li zelite trenutnu sliku sacuvati na sistem?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (res == System.Windows.Forms.DialogResult.OK)
                {
                    SaveAs(sender, e);
                }
                if (res == System.Windows.Forms.DialogResult.Cancel)
                {
                    ButtonLoad_Click(sender, e);
                }
                path = "";
            }
                ButtonLoad_Click(sender, e);
        }

        private void UndoButton(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("Radim undo");
            dc.PerformUndo();
        }

        private void RedoButton(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("Radim redo");
            dc.PerformRedo();
        }

        private void HelpWindow(object sender, MouseButtonEventArgs e)
        {
            Help helpWindow = new Help();
            helpWindow.Topmost = true;
            helpWindow.ShowActivated = true;
            helpWindow.Show();
        }
    }
}