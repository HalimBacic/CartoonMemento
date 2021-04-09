using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using Point = System.Windows.Point;
using Image = System.Windows.Controls.Image;

namespace CartoonMemento
{
    public class DrawingCanvas : Canvas
    {
        private List<StickerImage> elems = new List<StickerImage>();
        private Canvas canvas;
        private StickerImage activeSticker = null;
        private Boolean save = false;

        public DrawingCanvas(Canvas c)
        {
            canvas = c;
            canvas.AllowDrop = true;
            canvas.MouseMove += MoveActiveSticker;
        }

        public void LoadImage(Image img)
        {
            //TODO Scale image function
            img.Height = canvas.Height;
            img.Width = canvas.Width;
            canvas.Children.Add(img);
        }

        public StickerImage getActive()
        {
            return activeSticker;
        }

        public void AddSticker(StickerImage image)
        {
            image.MouseLeftButtonDown += EditMode;
            canvas.Children.Add(image.stickerCanvas);
            elems.Add(image);
        }

        public void removeActive()
        {
            foreach (StickerImage sti in elems)
                sti.StopEdit();
        }

        private void EditMode(object sender, MouseButtonEventArgs e)
        {
            removeActive();

            MoveSticker((StickerImage)sender);
        }

        private void MoveSticker(StickerImage sticker)
        {
            activeSticker = sticker;
            activeSticker.StartEdit();
            activeSticker.delete.MouseLeftButtonDown += RemoveSticker;
        }

        public void RemoveSticker(object sender, MouseButtonEventArgs e)
        {
            RemoveElement(activeSticker);
            activeSticker = null;
        }

        private void MoveActiveSticker(object sender, MouseEventArgs e)
        {
            if (activeSticker != null && e.LeftButton == MouseButtonState.Pressed)
            {
                Point point = Mouse.GetPosition((Canvas)sender);
                Canvas.SetLeft(activeSticker,point.X);
                Canvas.SetTop(activeSticker,point.Y);
            }
        }

        public void RemoveElement(StickerImage element)
        {
            canvas.Children.Remove(element);
            elems.Remove(element);
        }
    }
}
