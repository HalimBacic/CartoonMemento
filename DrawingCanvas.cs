using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using Point = System.Windows.Point;
using Image = System.Windows.Controls.Image;
using System.Windows;

namespace CartoonMemento
{
    public class DrawingCanvas : Canvas
    {
        private List<StickerImage> elems = new List<StickerImage>();
        private Canvas canvas;
        private StickerImage activeSticker = null;
        private Boolean save = false;
        public static readonly DependencyProperty HeightProperty = DependencyProperty.Register("Height", typeof(double), typeof(DrawingCanvas));
        private UndoRedo UndoRedo = new UndoRedo();

        public double Height {
            get { return (double)GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }

        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(double), typeof(DrawingCanvas));

        public double Width
        {
            get { return (double)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }


        public DrawingCanvas(Canvas c)
        {
            canvas = c;
            canvas.AllowDrop = true;
            canvas.MouseMove += MoveActiveSticker;
        }

        public void LoadImage(Image img)
        {
            this.Height = img.Height;
            this.Width = img.Width;

            double ratioX = (double)625 / img.Width;
            double ratioY = (double)1041 / img.Height;
            double ratio = Math.Min(ratioX, ratioY);

            this.Width = (int)(img.Width * ratio);
            this.Height = (int)(img.Height * ratio);

            canvas.Height = this.Height;
            canvas.Width = this.Width;
            img.Height = this.Height;
            img.Width = this.Width;

            canvas.Children.Add(img);
        }

        public StickerImage GetActive()
        {
            return activeSticker;
        }

        public void AddSticker(StickerImage image)
        {
            image.MouseLeftButtonDown += EditMode;
            image.pH.MouseLeftButtonDown += plusHeight;
            image.mH.MouseLeftButtonDown += minusHeight;
            image.pW.MouseLeftButtonDown += plusWidth;
            image.mW.MouseLeftButtonDown += minusWidth;
            canvas.Children.Add(image.stickerCanvas);
            elems.Add(image);   
        }

        private void minusHeight(object sender, MouseButtonEventArgs e)
        {
            AddToUndo();
            activeSticker.configureHeight(-5);
        }

        private void plusHeight(object sender, MouseButtonEventArgs e)
        {
            AddToUndo();
            activeSticker.configureHeight(5);
        }

        private void minusWidth(object sender, MouseButtonEventArgs e)
        {
            AddToUndo();
            activeSticker.configureWidth(-5);
        }

        private void plusWidth(object sender, MouseButtonEventArgs e)
        {
            AddToUndo();
            activeSticker.configureWidth(5);
        }

        private void AddToUndo()
        {
            Console.WriteLine("Added Added");
            UndoRedo.AddUndo(activeSticker.Copy());
        }

        public void RemoveActive()
        {
            foreach (StickerImage sti in elems)
                sti.StopEdit();
        }

        private void EditMode(object sender, MouseButtonEventArgs e)
        {
            RemoveActive();
            activeSticker = (StickerImage)sender;
            activeSticker.StartEdit();
            activeSticker.delete.MouseLeftButtonDown += RemoveSticker;
        }

        private void MoveActiveSticker(object sender, MouseEventArgs e)
        {
            if (activeSticker != null && e.LeftButton == MouseButtonState.Pressed)
            {
                Point point = Mouse.GetPosition((Canvas)sender);
                Canvas.SetLeft(activeSticker, point.X);
                Canvas.SetTop(activeSticker, point.Y);
            }
        }

        public void RemoveSticker(object sender, MouseButtonEventArgs e)
        {
            RemoveElement(activeSticker);
        }

        public void RemoveElement(StickerImage element)
        {
            AddToUndo();
            canvas.Children.Remove(element);
            elems.Remove(element);
        }

        public void PerformUndo()
        {
            if (activeSticker != null)
            {
                Console.WriteLine(activeSticker.Height + "  " + activeSticker.Width);
                canvas.Children.Remove(activeSticker);
                StickerImage sti = UndoRedo.Undo();
                Console.WriteLine(sti.Height + "  " + sti.Width);
                activeSticker = sti;
                AddSticker(sti);
            }
        }

        public void PerformRedo()
        {
            if (activeSticker != null)
            {
                canvas.Children.Remove(activeSticker);
                StickerImage sti = UndoRedo.Redo();
                activeSticker = sti;
                AddSticker(sti);
            }
        }

    }
}
