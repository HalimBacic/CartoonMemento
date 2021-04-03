using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Input;
using Image = System.Windows.Controls.Image;

namespace CartoonMemento
{
    class DrawingCanvas : Canvas
    {
        private List<StickerImage> elems = new List<StickerImage>();
        private Canvas canvas;

        public DrawingCanvas(Canvas c)
        {
            canvas = c;
            canvas.AllowDrop = true;
        }

        public void LoadImage(Image img)
        {
            canvas.Children.Add(img);
        }

        public void AddSticker(StickerImage image)
        {
            canvas.Children.Add(image.stickerCanvas);
            image.MouseLeftButtonDown += EditMode;
            elems.Add(image);
        }

        private void MoveImage(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                ((StickerImage)sender).Cursor = Cursors.SizeAll;
                System.Windows.Point point = Mouse.GetPosition(canvas);
                Canvas.SetLeft(((StickerImage)sender),point.X);
                Canvas.SetTop(((StickerImage)sender), point.Y);
            }
        }

        private void EditMode(object sender, MouseButtonEventArgs e)
        {
            foreach (StickerImage sticker in elems)
            {
                sticker.points.Visibility = System.Windows.Visibility.Hidden;
                sticker.canMove = false;
            }
            ((StickerImage)sender).points.Visibility = System.Windows.Visibility.Visible;
            ((StickerImage)sender).MouseMove += MoveImage;
        }

        public void RemoveElement(StickerImage element)
        {
            canvas.Children.Remove(element);
            elems.Remove(element);
        }
    }
}
