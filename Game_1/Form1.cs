using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace Game_1
{
    public partial class Form1 : Form
    {
        //var pt nori
        PictureBox[] cloud;
        int backgroundspeed;
        Random rnd;
        int playerSpeed;

        //var pt cartuse
        PictureBox[] bullets;
        int bulletsSpeed;

        //variabile pt dusmanii care ataca
        PictureBox[] enemies;
        int sizeEnemy;
        int enemiesSpeed;

        //variabile pt scor si nivel
        int Score;
        int Level;

        WindowsMediaPlayer Shoot;
        WindowsMediaPlayer GameSong;
        WindowsMediaPlayer Rip;
        WindowsMediaPlayer Win;

        public Form1()
        {
            InitializeComponent();

            Game_menu.Hide();

            lbl_value.Text = Properties.Settings.Default.h_score; //
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            for(int i = 0; i < cloud.Length; i++)
            {
                //aici setam miscarea norilor
                cloud[i].Left += backgroundspeed;//backgroundspeed defineste viteza norilor

                if (cloud[i].Left >= 1280)
                {
                    cloud[i].Left = cloud[i].Height;
                }
            }

            for (int i = cloud.Length; i < cloud.Length; i++)
            {
                //aici setam miscarea norilor
                cloud[i].Left += backgroundspeed - 10;//backgroundspeed defineste viteza norilor

                if (cloud[i].Left >= 1280)
                {
                    cloud[i].Left = cloud[i].Left;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            backgroundspeed = 3;
            cloud = new PictureBox[15];
            rnd = new Random();
            playerSpeed = 3;

            bullets = new PictureBox[1];
            bulletsSpeed = 80;

            enemies = new PictureBox[4]; //nr de dusmani aparuti in acelasi timp
            int sizeEnemy = rnd.Next(60, 80); //marimea dusmanilor va fi random intre 60 si 80
            enemiesSpeed = 3; //viteza de deplasare = 3

            Score = 0; //initializam scorul initial
            Level = 1; // ---  nivelul initial

            Image easyEnemies = Image.FromFile("assets\\apap.gif");//calea catre imaginea dusmanilor


            for(int i = 0; i < enemies.Length; i++)
            {
                enemies[i] = new PictureBox();
                enemies[i].Size = new Size(sizeEnemy, sizeEnemy);
                enemies[i].SizeMode = PictureBoxSizeMode.Zoom;
                enemies[i].BackColor = Color.Transparent;
                enemies[i].Image = easyEnemies;
                enemies[i].Location = new Point((i + 1) * rnd.Next(90, 160) + 1080, rnd.Next(450, 600));

                this.Controls.Add(enemies[i]);
            }

            Shoot = new WindowsMediaPlayer();
            Shoot.URL = "sunete//shoot.mp3";
            Shoot.settings.volume = 5;

            Rip = new WindowsMediaPlayer();
            Rip.URL = "sunete//rip.mp3";
            Rip.settings.volume = 25;

            Win = new WindowsMediaPlayer();
            Win.URL = "sunete//win.mp3";
            Win.settings.volume = 25;

            GameSong = new WindowsMediaPlayer();
            GameSong.URL = "sunete//GameSong.mp3";
            GameSong.settings.setMode("loop", true);
            GameSong.settings.volume = 15;

            GameSong.controls.play();

            //bullets
            for (int i = 0; i < bullets.Length; i++)
            {
                bullets[i] = new PictureBox();
                bullets[i].BorderStyle = BorderStyle.None;
                bullets[i].Size = new Size(20, 5);
                bullets[i].BackColor = Color.White;

                this.Controls.Add(bullets[i]);
            }

            //nori
            for (int i = 0; i < cloud.Length; i++)
            {
                cloud[i] = new PictureBox();
                cloud[i].BorderStyle = BorderStyle.None;
                cloud[i].Location = new Point(rnd.Next(-1000, 1280), rnd.Next(140, 320)); //setam locatia norilor
                if (i % 2 == 1)//vor fi doua tipuri de nori(pari si impari), in legatura cu care vor fi diferiti prin transparenta si dimensiune
                {
                    cloud[i].Size = new Size(rnd.Next(100, 255), rnd.Next(30, 70));//dam norilor transparenta random
                    cloud[i].BackColor = Color.FromArgb(rnd.Next(50, 125), 255, 200, 200);
                }
                else
                {
                    cloud[i].Size = new Size(150, 25);
                    cloud[i].BackColor = Color.FromArgb(rnd.Next(50, 125), 255, 205, 205);
                }

                this.Controls.Add(cloud[i]);

            }

        }

        private void LeftMoveTimer_Tick(object sender, EventArgs e)
        {
            if (mainPlayer.Left > 5)//ca personajul sa nu iasa din marginile hartii
            {
                mainPlayer.Left -= playerSpeed;
            }
        }

        private void RightMoveTimer_Tick(object sender, EventArgs e)
        {
            if (mainPlayer.Left < 1150)//ca personajul sa nu iasa din marginile hartii
            {
                mainPlayer.Left += playerSpeed;
            }
        }

        private void UpMoveTimer_Tick(object sender, EventArgs e)
        {
            if (mainPlayer.Top > 400)
            {
                mainPlayer.Top -= playerSpeed;
            }
        }

        private void DownMoveTimer_Tick(object sender, EventArgs e)
        {
            if (mainPlayer.Top < 580)
            {
                mainPlayer.Top += playerSpeed;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            mainPlayer.Image = Properties.Resources.cowboy_run; // schimba imaginea cowboy-ului static cu cel care alearga

            //conditii pentru care se misca personajul
            //cat timp tin apasat butonul
            if (e.KeyCode == Keys.Left)
            {
                LeftMoveTimer.Start();
            }

            if (e.KeyCode == Keys.Right)
            {
                RightMoveTimer.Start();
            }

            if (e.KeyCode == Keys.Up)
            {
                UpMoveTimer.Start();
            }

            if (e.KeyCode == Keys.Down)
            {
                DownMoveTimer.Start();
            }

            if (e.KeyCode == Keys.Space)
            {
                Shoot.controls.play();
                for (int i = 0; i < bullets.Length; i++)
                {
                    Intersect();

                    if (bullets[i].Left > 1280)//instructune pt ca cartusele sa fie pozitionate unde este arma
                    {
                        bullets[i].Location = new Point(mainPlayer.Location.X + 100 + i * 50, mainPlayer.Location.Y + 50);
                    }
                }
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            mainPlayer.Image = Properties.Resources.cowboy_idble; // schimba imaginea cowboy-ului alergator cu cel static

            LeftMoveTimer.Stop();
            RightMoveTimer.Stop();
            UpMoveTimer.Stop();
            DownMoveTimer.Stop();
        }

        private void MoveBulletsTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < bullets.Length; i++)
            {
                bullets[i].Left += bulletsSpeed;
            }
        }

        private void MoveEnemiesTimer_Tick(object sender, EventArgs e)
        {
            MoveEnemis(enemies, enemiesSpeed);
        }

        private void MoveEnemis(PictureBox[] enemies, int speed)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                //functie de setare a vitezei aleatorie a dusmanilor
                if(Level == 1 )
                {
                    enemies[i].Left -= speed;
                }

                if (Level == 2)
                {
                    enemies[i].Left -= speed + 1;
                }

                if (Level == 3)
                {
                    enemies[i].Left -= speed + 2;
                }

                if (Level == 4)
                {
                    enemies[i].Left -= speed + 3;
                }

                if (Level == 5)
                {
                    enemies[i].Left -= speed + 4;
                }



                if (enemies[i].Left < this.Left)//enemy ajungand la margina din st apar din nou
                {
                    int sizeEnemy = rnd.Next(60, 90);
                    enemies[i].Size = new Size(sizeEnemy, sizeEnemy);
                    enemies[i].Location = new Point((i + 1) * rnd.Next(150, 250) + 1080, rnd.Next(450, 650));
                }
                Intersect();
            }
        }

        //in momentul interesctarii cartuselor cu eamicii
        private void Intersect()
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (bullets[0].Bounds.IntersectsWith(enemies[i].Bounds))
                {

                    Score += 1;
                    labelScore.Text = (Score < 10) ? "0" + Score.ToString() : Score.ToString();

                    if (Score == 30)
                    {
                        GameWin("Ai Câstigat");

                    }

                    if (Score % 10 == 0)
                    {
                        Level += 1;
                        labelLevel.Text = (Level < 10) ? "0" + Level.ToString() : Level.ToString();
                    }


                    enemies[i].Location = new Point((i + 1) * rnd.Next(150, 250) + 1280, rnd.Next(420, 600));
                    bullets[0].Location = new Point(2000, mainPlayer.Location.Y + 50);
                }

                if (mainPlayer.Bounds.IntersectsWith(enemies[i].Bounds))
                {
                    mainPlayer.Visible = false;

                    GameOver("Ai Pierdut");
                }

                int a = Int32.Parse(lbl_value.Text);

                if (Score > a)
                {
                    lbl_value.Text = Score.ToString();
                    Properties.Settings.Default.h_score = lbl_value.Text;
                    Properties.Settings.Default.Save();

                }
            }
        }

        private void GameWin(string str)
        {
            label1.Text = str;
            label1.Location = new Point(500, 50);
            label1.Visible = true;

            Game_menu.Show();

            GameSong.controls.stop();
            MoveEnemiesTimer.Stop();
            MoveBgTimer.Stop();

            Win.controls.play();
        }

        private void GameOver(string str)
        {
            label1.Text = str;
            label1.Location = new Point(500, 50);
            label1.Visible = true;

            Game_menu.Show();

            GameSong.controls.stop();
            MoveEnemiesTimer.Stop();
            MoveBgTimer.Stop();

            Rip.controls.play();
        }

        private void lbl_restart_Click(object sender, EventArgs e)
        {
            Game_menu.Hide();
            GameSong.controls.play();
            MoveEnemiesTimer.Start();
            MoveBgTimer.Start();
            mainPlayer.Location = new Point(565, 424);
            mainPlayer.Visible = true;
            enemies[0].Location = new Point(160, 600);
            enemies[1].Location = new Point(160, 600);
            enemies[2].Location = new Point(160, 600);
            enemies[3].Location = new Point(160, 600);

            labelScore.Text = "00";
            labelLevel.Text = "01";
            Score = 0;
            Level = 1;
        }

        private void lbl_quit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}