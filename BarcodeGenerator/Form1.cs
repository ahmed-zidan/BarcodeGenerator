using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;
using ZXing;

namespace BarcodeGenerator
{
    public partial class Form1 : Form
    {
        private IKeyboardMouseEvents globalMouseHook;
        public Form1()
        {
            InitializeComponent();
            // Note: for the application hook, use the Hook.AppEvents() instead.
            globalMouseHook = Hook.GlobalEvents();

            // Bind MouseDoubleClick event with a function named MouseDoubleClicked.
            globalMouseHook.MouseDoubleClick += MouseDoubleClicked;

            // Bind DragFinished event with a function.
            // Same as double click, so I didn't write here.
            //globalMouseHook.MouseDragFinished += MouseDragFinished;
        }


        private void Form1_Load(object sender, EventArgs e)
        {

            this.KeyPreview = true;
        }


        // I make the function async to avoid GUI lags.
        private async void MouseDoubleClicked(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Save clipboard's current content to restore it later.
            IDataObject tmpClipboard = Clipboard.GetDataObject();

            Clipboard.Clear();

            // I think a small delay will be more safe.
            // You could remove it, but be careful.
            await Task.Delay(50);

            // Send Ctrl+C, which is "copy"
            System.Windows.Forms.SendKeys.SendWait("^c");

            // Same as above. But this is more important.
            // In some softwares like Word, the mouse double click will not select the word you clicked immediately.
            // If you remove it, you will not get the text you selected.
            await Task.Delay(50);

            if (Clipboard.ContainsText())
            {
                string text = Clipboard.GetText();

                BarcodeWriter writer = new BarcodeWriter() { Format = BarcodeFormat.CODE_128 };
                
                picBarcode.Image = writer.Write(text);
                picBarcode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;

                // Your code                                                                                                                                    



            }
            else
            {
                // Restore the Clipboard.
                Clipboard.SetDataObject(tmpClipboard);
            }
        }




    }
}
