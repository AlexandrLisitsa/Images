using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.IO;
using Microsoft.VisualBasic;

namespace Game.Images
{
    public partial class MainFrame : Form
    {
        public const int EASY_COUNT = 8;
        public const int MEDIUM_COUNT = 12;
        public const int HARD_COUNT = 20;
        public const int EPYC_COUNT = 40;

        private List<ImageBox> pictures = new List<ImageBox>();
        public List<ImageBox> picturesCompareList = new List<ImageBox>();

        private int col, row;
        public int totalClickCount = 0;
        private int imageCount = 2;
        private System.Timers.Timer t,gameCycle;
        private int gameSeconds = 0;
        private string userName,difficultLevel;
        TextBox usernameBox;
        public MainFrame()
        {
            InitializeComponent();
            initMainMenu();      
        }

        private void initGameInterface()
        {
            this.Controls.Add(MainPanel);
            initImages();
            mixImages();
            paintImages();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            t = new System.Timers.Timer(5000);
            t.Elapsed += hideColors;
            t.Start();
        }

        private void initMainMenu()
        {
            this.Controls.Remove(this.MainPanel);
            Button easy, medium, hard, epyc,custom;
            //buttons memory allocation
            easy = new Button();
            medium = new Button();
            hard = new Button();
            epyc = new Button();
            custom = new Button();
            //add buttons click listener
            easy.Click += difficultSelect;
            hard.Click += difficultSelect;
            medium.Click += difficultSelect;
            epyc.Click += difficultSelect;
            custom.Click += difficultSelect;
            //init buttons name&text
            easy.Name = "Easy";
            easy.Text = "Easy";
            medium.Name = "Medium";
            medium.Text = "Medium";
            hard.Name = "Hard";
            hard.Text = "Hard";
            epyc.Name = "Epyc";
            epyc.Text = "Epyc";
            custom.Name = "Custom";
            custom.Text = "Custom";
            //select button location on frame
            easy.SetBounds(10, 10, 100, 100);
            medium.SetBounds(110, 10, 100, 100);
            hard.SetBounds(210, 10, 100, 100);
            epyc.SetBounds(310, 10, 100, 100);
            custom.SetBounds(10, 110, 400, 60);
            //add buttons in frame controls
            this.Controls.Add(easy);
            this.Controls.Add(medium);
            this.Controls.Add(hard);
            this.Controls.Add(epyc);
            this.Controls.Add(custom);
            //init UserName Field
            usernameBox = new TextBox();
            usernameBox.SetBounds(this.Size.Width/2-50, this.Size.Height/2, 100, 100);
            this.Controls.Add(usernameBox);
            //init username label
            Label usernameLabel = new Label();
            usernameLabel.SetBounds(this.Size.Width / 2 - 100, this.Size.Height / 2+2, 70, 20);
            usernameLabel.Text = "Player";
            this.Controls.Add(usernameLabel);
        }

        private void difficultSelect(object sender, EventArgs e)
        {
            if(sender is Button)
            {
                Button tmp = (Button)sender;
                Size sizeTmp = new Size();
                if (tmp.Name.Equals("Easy"))
                {
                    difficultLevel = tmp.Name;
                    sizeTmp.Height = 250;
                    sizeTmp.Width = 442;
                    loadDifficultSetings(EASY_COUNT,sizeTmp);       
                }
                if (tmp.Name.Equals("Medium"))
                {
                    difficultLevel = tmp.Name;
                    sizeTmp.Height = 357;
                    sizeTmp.Width = 442;
                    loadDifficultSetings(MEDIUM_COUNT, sizeTmp);
                }
                if (tmp.Name.Equals("Hard"))
                {
                    difficultLevel = tmp.Name;
                    sizeTmp.Height = 462;
                    sizeTmp.Width = 548;
                    loadDifficultSetings(HARD_COUNT, sizeTmp);
                }
                if (tmp.Name.Equals("Epyc"))
                {
                    difficultLevel = tmp.Name;
                    sizeTmp.Height = 569;
                    sizeTmp.Width = 866;
                    loadDifficultSetings(EPYC_COUNT, sizeTmp);
                }
                if (tmp.Name.Equals("Custom"))
                {
                    difficultLevel = tmp.Name;
                    loadCustomSettings();
                }
            }    
        }

        private void loadCustomSettings()
        {
            setColRow();
            clearMainPanel();
            this.Controls.Add(MainPanel);
            loadCustomImages();
            customMixImages();
            paintCustomImages();
            setOptimalSize();
            t = new System.Timers.Timer(5000);
            t.Elapsed += customHideColors;
            t.Start();
        }

        private void customHideColors(object sender, ElapsedEventArgs e)
        {
            for (int i = 0; i < col*row; i++)
            {
                pictures[i].hideColor();
            }
            t.Stop();
            t.Elapsed -= hideColors;
            t.Dispose();
            gameCycle = new System.Timers.Timer(1000);
            gameCycle.Elapsed += gameTimerMeth;
            gameCycle.Start();
        }

        private void clearMainPanel()
        {
            while (this.Controls.Count != 0)
            {
                for (int i = 0; i < Controls.Count; i++)
                {
                    this.Controls.RemoveAt(i);
                }
            }
        }

        private void setColRow()
        {
            try
            {
                col = int.Parse(Interaction.InputBox("", "Images", "Set col count", -1, -1));
                row = int.Parse(Interaction.InputBox("", "Images", "Set row count", -1, -1));
                if ((col * row) % 2 != 0)
                {
                    MessageBox.Show("Multiply of col and row must be divided by 2 without remain");
                    setColRow();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Wrong number");
            }
        }

        private void setOptimalSize()
        {
            Size sizeTmp = new Size();
            sizeTmp.Height = (col*100)+(((col*2)*3)+38);
            sizeTmp.Width = (row*100)+(((row*2)*3)+16);
            this.Size = sizeTmp;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        private void paintCustomImages()
        {
            for (int i = 0; i < col*row; i++)
            {
                MainPanel.Controls.Add(pictures[i]);
            }
            
        }

        private void loadCustomImages()
        {
            Random r = new Random();
            for (int i = 0; i < (col*row)/2; i++)
            {
                Color color = Color.FromArgb(r.Next(255), r.Next(255), r.Next(255));
                for (int k = 0; k < 2; k++)
                {
                    pictures.Add(new ImageBox());
                    pictures[pictures.Count - 1].Index = i;
                    pictures[pictures.Count - 1].BackColor = color;
                    pictures[pictures.Count - 1].BackgroundImage = null;
                    pictures[pictures.Count - 1].parent = this;
                }
            }
        }

        private void setUsername(string username)
        {
            if (username.Length == 0)
            {
                userName = "Default";
            }
            else
            {
                this.userName = username;
            }
        }

        private void loadDifficultSetings(int count,Size frameSize)
        {
            this.imageCount = count;
            setUsername(usernameBox.Text);
            while (this.Controls.Count != 0)
            {
                for (int i = 0; i < Controls.Count; i++)
                {
                    this.Controls.RemoveAt(i);
                }
            }
            this.Size = frameSize;
            initGameInterface();
        }


        private void clearPictureCompare()
        {
            for (int i = 0; i < picturesCompareList.Count; i++)
            {
                picturesCompareList.RemoveAt(i);
            }
        }

        public void compareImages()
        {
            if (picturesCompareList.Count >1&&picturesCompareList.Count<3)
            {
                if (picturesCompareList[0].Index.Equals(picturesCompareList[1].Index))
                {
                    while (picturesCompareList.Count != 0)
                    {
                        clearPictureCompare();
                    }
                }else
                {
                   
                    t = new System.Timers.Timer(500);
                    t.Elapsed += hidePicturesCompare;
                    t.Start();
                }
            }
            if (isWin())
            {
                gameCycle.Stop();
                MessageBox.Show("You Win,your time is: "+gameSeconds+" seconds.");
                log();
                restart();
            }
            foreach (var item in picturesCompareList)
            {
                item.isSelected = true;
            }
        }

        private void restart()
        {
            var confirm = MessageBox.Show("Try again", "Images", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                while (this.Controls.Count != 0)
                {
                    for (int i = 0; i < Controls.Count; i++)
                    {
                        this.Controls.RemoveAt(i);
                    }
                }
                Size sizeTmp = new Size();
                sizeTmp.Height = 357;
                sizeTmp.Width = 442;
                this.Size = sizeTmp;
                initMainMenu();
            }else
            {
                this.Dispose();
            }
        }

        private void log()
        {
            File.AppendAllText("./log.txt", DateTime.Now.ToString() +" "+userName+" |"+difficultLevel+ "| Result:" + gameSeconds + " Seconds| "+"Click count:"+totalClickCount+"\n");
        }

        private bool isWin()
        {
            foreach (var pic in pictures)
            {
                if (pic.isSelected == false) return false;
            }
            return true;         
        }

        private void hidePicturesCompare(object sender, ElapsedEventArgs e)
        {
            for (int i = 0; i < picturesCompareList.Count; i++)
            {
                picturesCompareList[i].hideColor();
            }
            t.Elapsed -= hidePicturesCompare;
            t.Dispose();
            while (picturesCompareList.Count != 0)
            {
                clearPictureCompare();
            }
        }

        private void hideColors(Object source, ElapsedEventArgs e)
        {
            for (int i = 0; i < imageCount; i++)
            {
                pictures[i].hideColor();
            }
            t.Stop();
            t.Elapsed -= hideColors;
            t.Dispose();
            gameCycle = new System.Timers.Timer(1000);
            gameCycle.Elapsed += gameTimerMeth;
            gameCycle.Start();
        }

        private void gameTimerMeth(object sender, ElapsedEventArgs e)
        {
            gameSeconds++;
        }

        private void paintImages()
        {
            for (int i = 0; i < imageCount; i++)
            {
                MainPanel.Controls.Add(pictures[i]);
            }
        }

        private void mixImages()
        {
            Random r = new Random();
            for (int i = 0; i < 100000; i++)
            {
                int index = r.Next(imageCount);
                ImageBox tmp=pictures[0];
                pictures[0] = pictures[index];
                pictures[index] = tmp;
            }
        }

        private void customMixImages()
        {
            Random r = new Random();
            for (int i = 0; i < 100000; i++)
            {
                int index = r.Next(col*row);
                ImageBox tmp = pictures[0];
                pictures[0] = pictures[index];
                pictures[index] = tmp;
            }
        }

        private void initImages()
        {
            Random r = new Random();
            for (int i = 0; i < imageCount / 2; i++)
            {
                Color color = Color.FromArgb(r.Next(255), r.Next(255), r.Next(255));
                for (int k = 0; k < 2; k++)
                {
                    pictures.Add(new ImageBox());
                    pictures[pictures.Count - 1].Index = i;
                    pictures[pictures.Count - 1].BackColor = color;
                    pictures[pictures.Count - 1].BackgroundImage = null;
                    pictures[pictures.Count - 1].parent = this;
                }
                
            }

        }

        private void MainPanel_Click(object sender, EventArgs e)
        {
            MessageBox.Show(MainPanel.Parent.Size.ToString());
        }
    }
}
