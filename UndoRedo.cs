using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartoonMemento
{
    public class UndoRedo
    {
        private Stack undoStack = new Stack();
        private Stack redoStack = new Stack();


        public UndoRedo()
        { }

        public void AddUndo(StickerImage sti)
        {
            Console.WriteLine("Dodajem undo:"+sti.GetHashCode()+sti.Height+"  "+sti.Width+"  "+ DrawingCanvas.GetLeft(sti), DrawingCanvas.GetTop(sti));
            undoStack.Push(sti);
        }

        public void AddRedo(StickerImage sti)
        {
            Console.WriteLine("Dodajem undo:" + sti.GetHashCode() + sti.Height + "  " + sti.Width + "  " + DrawingCanvas.GetLeft(sti), DrawingCanvas.GetTop(sti));
            Console.WriteLine("Dodajem:" + sti.GetHashCode());
            redoStack.Push(sti);
        }

        public StickerImage Undo()
        {
            StickerImage undo =  (StickerImage)undoStack.Pop();
            AddRedo(undo);

            return undo;
        }

        public StickerImage Redo()
        {
            return (StickerImage)redoStack.Pop();
        }
    }
}
