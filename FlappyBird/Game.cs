using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlappyBird
{
    public partial class Game : Form
    {
        const int GraviationalConstant  = 5;
        int gravity = GraviationalConstant;

        int pipeSpeed = 20;

        int score = 0;
        int previousScore = 0;

        Random randomNumber = new Random();

        public Game()
        {
            InitializeComponent();
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            bird.Top += gravity;

            pipeTop.Left -= pipeSpeed;
            pipeBottom.Left -= pipeSpeed;

            if (pipeTop.Left < -100)
            {
                pipeTop.Left = 720;
                MovePipe(pipeBottom);
                score++;
                lblScore.Text = "Score: " + score;
            }
            if (pipeBottom.Left < -73)
            {
                pipeBottom.Left = 700;
                MovePipe(pipeTop);
                score++;
                lblScore.Text = "Score: " + score;
            }


            //Speeding up
            if(score - previousScore > 5)
            {
                pipeSpeed += 10;
                previousScore = score;
            }


            //if (bird.Bounds.IntersectsWith(ground.Bounds)||
            //    bird.Bounds.IntersectsWith(pipeBottom.Bounds)||
            //    bird.Bounds.IntersectsWith(pipeTop.Bounds)||
            //    bird.Top < -5)
            //{
            //    EndGame();
            //}
        }

        private void Game_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Space)
            {
                gravity = -GraviationalConstant;
            }
        }

        private void Game_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Space)
            {
                gravity = GraviationalConstant;
            }
        }

        private void MovePipesUpAndDown()
        {
            Random randomNumber = new Random();
            int verticalChange = randomNumber.Next(-80, 80);

            int topPrevious = pipeTop.Top;
            int bottomPrevious = pipeBottom.Top;

            if(score % 2.0 == 0) //if score is even, move pipes up, if score is odd then move pipes down
            {
                pipeTop.Top += verticalChange;
                pipeBottom.Top += verticalChange;
            }
            else
            {
                pipeTop.Top -= verticalChange;
                pipeBottom.Top -= verticalChange;
            }

            if(pipeTop.Bottom < 30) //if top pipe goes out of bounds
            {
                pipeTop.Top = topPrevious;
                pipeBottom.Top = bottomPrevious;
            }

            if (pipeBottom.Top > 490) //if bottom pipe goes below the ground
            {
                pipeTop.Top = topPrevious;
                pipeBottom.Top = bottomPrevious;
            }

        }

        private void MovePipe(PictureBox pipe)
        {
            //This works really shitty. 
            //Sometimes the pipes are close together, other times they are far away. 
            //I think it may be better to just have their height difference be the same, just have them staggered in x direction
            //Not sure how to do that though?

            if (pipe == pipeTop)
            {
                //Move pipe top up or down
                int max = 20;
                int min = -230;
                pipeTop.Top = randomNumber.Next(min, max);
            }
            else if (pipe == pipeBottom) 
            {
                //Move pipe bottom up or down
                int max = 490;
                int min = 230;
                pipeBottom.Top = randomNumber.Next(min, max);
            }
        }

        private void EndGame()
        {
            gameTimer.Stop();
        }
    }
}
