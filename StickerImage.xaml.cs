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
        public static  readonly DependencyProperty CanMove = DependencyProperty.Register("canMove",typeof(Boolean), typeof(StickerImage));
        private DrawingCanvas canvas;

        public Boolean canMove
        {
            get { return (Boolean)this.GetValue(CanMove); }
            set { this.SetValue(CanMove,value); }
        }

        public StickerImage(Image image,DrawingCanvas canvas)
        {
            InitializeComponent();
            this.canvas = canvas;
            myImage.Source = image.Source;
            myImage.MouseEnter += ChangeCursor;
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

        private void plusHeight(object sender, MouseEventArgs e)
        {
            configureHeight(5);
        }

        private void plusWidth(object sender, MouseEventArgs e)
        {
            configureWidth(5);
        }

        private void minusHeight(object sender, MouseEventArgs e)
        {
            configureHeight(-5);
        }

        private void minusWidth(object sender, MouseEventArgs e)
        {
            configureWidth(-5);
        }

        private void configureWidth(int value)
        {
            stickerCanvas.Width += value;
            points.Width += value;
            myImage.Width += value;
        }

        private void configureHeight(int value)
        {
            stickerCanvas.Height += value;
            points.Height += value;
            myImage.Height += value;
        }
    }
}
