using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using cardsMatching.Properties;

namespace cardsMatching
{
    public partial class Form1 : Form
    {
        private bool allowClick = true;
        private PictureBox fistGuess;
        Random ran = new Random();
        Timer clickTimer = new Timer();
        int ticks = 45;
        Timer timer = new Timer{Interval = 1000};
        public Form1()
        {
            InitializeComponent();
            SetRandomImages();
            HideImages();
            StartGameTimer();
            clickTimer.Interval = 1000;
            clickTimer.Tick += clickTimer_Tick;
        }

        private PictureBox[] PictureBoxes
        {
            get { return Controls.OfType<PictureBox>().ToArray(); }
        }
        private static Image[]Images
        {
            get
            {
                return new Image[]
                {
                    Resources.img1,
                    Resources.img2,
                    Resources.img3,
                    Resources.img4,
                    Resources.img5,
                    Resources.img6,
                    Resources.img7,
                    Resources.img8
                };
            }
        }
        private void StartGameTimer()
        {
            timer.Start();
            timer.Tick += delegate
            {
                ticks--;
                if (ticks == -1)
                {
                    MessageBox.Show("time is up");
                    ResetImages();
                }
                var time = TimeSpan.FromSeconds(ticks);
                label1.Text = "00:" + time.ToString("ss");
            };
        }
        private void ResetImages()
        {
            foreach (PictureBox pic in PictureBoxes)
            {
                pic.Tag = null;
                pic.Visible = true;
            }
            HideImages();
            SetRandomImages();
            ticks = 45;
            timer.Start();
        }
        private void HideImages()
        {
            foreach (PictureBox pic in PictureBoxes)
            {
                pic.Image = Resources.img0;
            }
        }
        private PictureBox f()
        {
            int num;
            do
            {
                num = ran.Next(0, PictureBoxes.Count());
            } while (PictureBoxes[num].Tag != null); //pana cand gaseste un tag null
            return PictureBoxes[num];
        }
        private void SetRandomImages()
        {
            foreach(Image image in Images)
            {
                f().Tag = image;
                f().Tag = image;
            }
        }
        private void click(object sender, EventArgs e)
        {
            if (!allowClick) return;
            PictureBox pic = (PictureBox)sender; //cea pe care se face click
            if(fistGuess==null)
            {
                fistGuess = pic;
                pic.Image = (Image)pic.Tag; //imaginea acelui pic va fi tagul convertit in imagine
                return;
            }
            pic.Image = (Image)pic.Tag;
            if(pic.Image==fistGuess.Image && pic!=fistGuess)
            {
                pic.Visible = fistGuess.Visible = false; //daca sunt aceleasi le ascunde
            }
            else
            {
                allowClick = false;
                clickTimer.Start();
            }
            fistGuess = null;
            if (PictureBoxes.Any(p => p.Visible)) return;
            MessageBox.Show("You Won");
            ResetImages();
        }
        private void clickTimer_Tick(object sender,EventArgs e)
        {
            HideImages();
            allowClick = true;
            clickTimer.Stop();
        }
    }
}
