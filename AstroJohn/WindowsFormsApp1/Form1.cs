using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

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
                    int position = rnd.Next(600, 1000);

                    x.Left = 640 + (x.Left + position + x.Width * 3);
                }
            }

            gameTimer.Start();
        }

        private void score_table_show()
        {
            using (StreamReader Read = new StreamReader("Scores.txt"))
            {
                string line;

                while ((line = Read.ReadLine()) != null)
                {                    
                    Console.WriteLine(line);
                }
            }
        }
        private void score_table_write()
        {
            int n = 0;
            // Wczytywanie wynikow + zapisanie nowego + sortowanie(sortowanie dałbym jako nową klasę, albo funkcje)
            using (StreamReader Read = new StreamReader("Scores.txt"))
            {
                
                string line;
                //ilosc lini
                
                //Odczytujemy ilosc linii
                while ((line = Read.ReadLine()) != null)
                {
                    n++;                    
                }
                string[] lines = new string[n];
                int[] score = new int[n];
                
                int i = 0;
                //zapisujemy linie do lines,same wyniki do score
                while (i<n)
                {
                    lines[i] = Read.ReadLine();
                    string[] split = line.Split(' ');
                    score[i] = int.Parse(split[2]);
                    i++;
                }
                i++;
                n++;
                // Dodajemy nowy wynik
                // Tutaj trzeba bedzie do funkcji wszystko przekazac!!!!!!!!!!!!!!!!!!
                string nick= "Bartek";
                int new_score_int = 0;
                string new_score = (i +" "+ nick +" "+ new_score_int);
                lines[i] = new_score;
                

                //------------sortowanie-----------
                // TRZEBA PRZETESTOWAĆ ZAMIANĘ TABLICY LINES!!!!
                int k = 0, mini;
                i = 0;
                while (k < n - 1)
                {
                    k += 1;
                    mini = k;
                    i = k;
                    while (i < n)
                    {
                        i++;
                        if (score[i] < score[mini])
                        {
                            mini = i;
                        }
                    }
                    int pom = score[k];
                    score[k] = score[mini];
                    score[mini] = pom;
                    string temp = lines[k];
                    lines[k] = lines[mini];
                    lines[mini] = temp;
                }

                //-------------Zapisywanie do pliku-----------
                using (StreamWriter write = new StreamWriter("Scores.txt"))
                {
                    i=0;
                    while (i < n)
                    {
                        write.Write(lines[i]);
                        i++;
                    }
                }
            }
            
        }
    }
}
