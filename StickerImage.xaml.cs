using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CartoonMemento
{
    /// <summary>
    /// Interaction logic for StickerImage.xaml
    /// </summary>
    public partial class StickerImage : Canvas
    {
        public static  readonly DependencyProperty CanMove = DependencyProperty.Register("canMove",typeof(Boolean), typeof(StickerImage));

        public Boolean canMove
        {
            get { return (Boolean)this.GetValue(CanMove); }
            set { this.SetValue(CanMove,value); }
        }

        public StickerImage(Image image)
        {
            InitializeComponent();
            myImage.Source = image.Source;
            myImage.MouseEnter += ChangeCursor;
            myImage.MouseLeftButtonDown += StartMove;
            myImage.MouseLeftButtonUp += StopMove;
        }

        private void StopMove(object sender, MouseButtonEventArgs e)
        {
            canMove = false;
        }

        private void StartMove(object sender, MouseButtonEventArgs e)
        {
            canMove = true;
        }

        private void ChangeCursor(object sender, MouseEventArgs e)
        {
             myImage.Cursor = Cursors.Hand;
        }

        private void ResizeCursor(object sender, MouseEventArgs e)
        {
            ((Ellipse)sender).Cursor = Cursors.SizeNWSE;
        }
    }
}
