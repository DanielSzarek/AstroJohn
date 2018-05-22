using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        bool jumping = false;
        int jumpSpeed = 10;
        int force = 12;
        int score = 0;
        int obstacleSpeed = 10;
        Random rnd = new Random();

        public Form1()
        {
            InitializeComponent();

            resetGame();
        }

        private void gameEvent(object sender, EventArgs e)
        {
            astro.Top += jumpSpeed;

            scoreText.Text = "Score: " + score;

            if (jumping && force < 0)
            {
                jumping = false;
            }

            if (jumping)
            {
                jumpSpeed = -12;
                force -= 1;
            }
            else
            {
                jumpSpeed = 12;
            }

            foreach(Control x in this.Controls)
            {
                if(x is PictureBox && x.Tag == "obstacle")
                {
                    x.Left -= obstacleSpeed; //przesuwa przeszkody w lewo

                    if(x.Left + x.Width < -120)
                    {
                        x.Left = this.ClientSize.Width + rnd.Next(200, 800);
                        score++;
                    }

                    if(astro.Bounds.IntersectsWith(x.Bounds))
                    {
                        gameTimer.Stop();

                        astro.Image = Properties.Resources.player_static;

                        scoreText.Text += " Press R to restart";
                    }
                }
            }

            if (astro.Top >= 380 && !jumping)
            {
                force = 12;
                astro.Top = floor.Top - astro.Height;
                jumpSpeed = 0;
            }

            if (score >= 10)
            {
                obstacleSpeed = 15;
            }
        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Space && !jumping)
            {
                jumping = true;
            }

        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.R)
            {
                resetGame();
            }

            if(jumping)
            {
                jumping = false;
            }

        }

        private void resetGame()
        {
            force = 12;
            astro.Top = floor.Top - astro.Height;
            jumpSpeed = 0;
            jumping = false;
            score = 0;
            obstacleSpeed = 10;
            scoreText.Text = "Score: " + score;
            astro.Image = Properties.Resources.plyaer_walk_3;

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x.Tag == "obstacle")
                {
                    int position = rnd.Next(600, 3000);

                    x.Left = 640 + (x.Left + position + x.Width * 3);
                }
            }

            gameTimer.Start();
        }

        private void astro_Click(object sender, EventArgs e)
        {

        }
    }
}
