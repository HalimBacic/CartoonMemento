using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace CartoonMemento
{
    /// <summary>
    /// Interaction logic for StickerImage.xaml
    /// </summary>
    public partial class StickerImage : Canvas
    {
        public int id;
        public static  readonly DependencyProperty CanMove = DependencyProperty.Register("canMove",typeof(Boolean), typeof(StickerImage));

        public Boolean canMove
        {
            get { return (Boolean)this.GetValue(CanMove); }
            set { this.SetValue(CanMove,value); }
        }

        public StickerImage(Image image,DrawingCanvas canvas)
        {
            Random random = new Random();
            InitializeComponent();
            myImage.Source = image.Source;
            myImage.MouseEnter += ChangeCursor;
        }

        public StickerImage Copy()
        {
            return (StickerImage)this.MemberwiseClone();
        }

        public void StopEdit()
        {
            canMove = false;
            points.Visibility = System.Windows.Visibility.Hidden;
        }

        public void StartEdit()
        {
            canMove = true;
            points.Visibility = System.Windows.Visibility.Visible;
        }

        private void ChangeCursor(object sender, MouseEventArgs e)
        {
             myImage.Cursor = Cursors.Hand;
        }

        private void ResizeCursor(object sender, MouseEventArgs e)
        {
            ((Image)sender).Cursor = Cursors.SizeNWSE;
        }


 
        public void configureWidth(int value)
        {
            stickerCanvas.Width += value;
            points.Width += value;
            myImage.Width += value;
        }

        public void configureHeight(int value)
        {
            stickerCanvas.Height += value;
            points.Height += value;
            myImage.Height += value;
        }
    }
}
