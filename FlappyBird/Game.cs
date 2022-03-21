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
        const int GraviationalConstant = 5;
        int gravity = GraviationalConstant;

        int pipeSpeed = 20;

        int score = 0;
        int previousScore = 0;

        Dictionary<string, int> scores = new Dictionary<string, int>();

        public Game()
        {
            InitializeComponent();
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            bird.Top += gravity;

            pipeTop.Left -= pipeSpeed;
            pipeBottom.Left -= pipeSpeed;

            if (pipeTop.Left < -100) //Resetting pipe to right side of screen
            {
                pipeTop.Left = 700;
                pipeBottom.Left = 700;
                MovePipesUpAndDown();
                score++;
                lblScore.Text = "Score: " + score;
            }


            if (score - previousScore > 5) //Bird is speeding up
            {
                pipeSpeed += 10;
                previousScore = score;
            }


            if (bird.Bounds.IntersectsWith(ground.Bounds) ||
                bird.Bounds.IntersectsWith(pipeBottom.Bounds) ||
                bird.Bounds.IntersectsWith(pipeTop.Bounds) ||
                bird.Top < -5)
            {
                EndGame();
            }
        }

        private void Game_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                gravity = -GraviationalConstant;
            }
        }

        private void Game_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                gravity = GraviationalConstant;
            }
        }

        private void MovePipesUpAndDown()
        {
            Random randomNumber = new Random();
            int verticalChange = randomNumber.Next(-100, 100); 

            int topPrevious = pipeTop.Top;
            int bottomPrevious = pipeBottom.Top;

            if (score % 2.0 == 0) //if score is even, move pipes up, if score is odd then move pipes down
            {
                pipeTop.Top += verticalChange;
                pipeBottom.Top += verticalChange;
            }
            else
            {
                pipeTop.Top -= verticalChange;
                pipeBottom.Top -= verticalChange;
            }

            if (pipeTop.Bottom < 30) //if top pipe goes out of bounds
            {
                pipeTop.Top = topPrevious + 100;
                pipeBottom.Top = bottomPrevious + 100;
            }

            if (pipeBottom.Top > 490) //if bottom pipe goes below the ground
            {
                pipeTop.Top = topPrevious - 100;
                pipeBottom.Top = bottomPrevious - 100;
            }

        }

        private void EndGame()
        {
            gameTimer.Stop();
            bird.Visible = false;
            lblGameover.Visible = true;
            btnPlayAgain.Visible = true;
            btnLeaderboard.Visible = true;
            lblScore.Text = "Score: " + score;
        }

        private void btnLeaderboard_Click(object sender, EventArgs e)
        {
            lblGameover.Visible = false;
            btnLeaderboard.Visible = false;

            lblName.Visible = true;
            txtName.Visible = true;
            btnSubmitScore.Visible = true;

            
        }

        private void btnSubmitScore_Click(object sender, EventArgs e)
        {
            string name = txtName.Text;
            lstLeaderboard.Visible = true;
            scores.Add(name, score);

            scores.OrderBy(x => x.Value);

            lstLeaderboard.Items.Add("Name       | Score      ");

            foreach (KeyValuePair<string, int> s in scores)
            {
                lstLeaderboard.Items.Add(string.Format("{0, -10} | {1, 5}", s.Key, s.Value));
            }
        }

        private void btnPlayAgain_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
    }
}
