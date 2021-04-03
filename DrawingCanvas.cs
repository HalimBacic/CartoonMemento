using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

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
            elems.Add(image);
        }

        public void RemoveElement(StickerImage element)
        {
            canvas.Children.Remove(element);
            elems.Remove(element);
        }
    }
}
