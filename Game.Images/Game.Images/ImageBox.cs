using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Game.Images
{
    public class ImageBox:Panel
    {
        public bool isSelected = false;
        public MainFrame parent { get; set; }
        public Color Color { get; set; }
        public int MyProperty { get; set; }
        private const string rubashka = "./background.jpg";
        public int Index { get; set; }

        public ImageBox()
        {
            this.Size = new Size(new Point(100, 100));
            this.Click += new System.EventHandler(boxClick);
        }

        public void hideColor()
        {
            this.BackgroundImage = Image.FromFile(rubashka);
            isSelected = false;
        }

        public void boxClick(object sender,EventArgs e)
        {
            if (!isSelected&&parent.picturesCompareList.Count<2)
            {
                parent.totalClickCount++;
                isSelected = true;
                this.BackgroundImage = null;
                parent.picturesCompareList.Add(this);
                parent.compareImages();
            }
            else
            {
                return;
            }       
        }
    }
}
