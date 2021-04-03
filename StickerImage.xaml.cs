using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CartoonMemento
{
    /// <summary>
    /// Interaction logic for StickerImage.xaml
    /// </summary>
    public partial class StickerImage : Canvas
    {

        public StickerImage(Image image)
        {
            InitializeComponent();
            myImage.Source = image.Source;
            myImage.MouseLeftButtonDown += EditMode;
            myImage.MouseEnter += ChangeCursor;
        }

        private void ChangeCursor(object sender, MouseEventArgs e)
        {
             myImage.Cursor = Cursors.Hand;
        }

        private void EditMode(object sender, MouseButtonEventArgs e)
        {
            points.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
