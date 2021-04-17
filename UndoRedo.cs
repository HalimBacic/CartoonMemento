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

        public void AddUndo(StickerImage Sticker)
        {
            Console.WriteLine("Dodajem undo:"+Sticker.GetHashCode()+" "+Sticker.Height+" "+Sticker.Width);
            undoStack.Push(Sticker.Copy());
        }

        public void AddRedo(StickerImage Sticker)
        {
            Console.WriteLine("Dodajem undo:" + Sticker.GetHashCode() + " " + Sticker.Height + " " + Sticker.Width);
            redoStack.Push(Sticker.Copy());
        }

        public StickerImage Undo()
        {
            StickerImage undo =  (StickerImage)undoStack.Pop();
            AddRedo(undo.Copy());

            return undo;
        }

        public StickerImage Redo()
        {
            StickerImage redo = (StickerImage)redoStack.Pop();
            AddUndo(redo.Copy());

            return redo;
        }
    }
}
