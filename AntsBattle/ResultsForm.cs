using System;
using System.Drawing;
using System.Windows.Forms;

namespace AntsBattle
{
    public partial class ResultsForm : Form
    {
        private int WhiteScore;
        private int BlackScore;
        private World world;
        private Timer Timer;
        public ResultsForm(World world)
        {
            Timer = new Timer {Interval = 10};
            Timer.Tick += TimerTick;
            Timer.Start();
            
            this.world = world;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            Invalidate();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DoubleBuffered = true;
            Text = "Results";
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            WhiteScore = world.WhiteScore;
            BlackScore = world.BlackScore;
            var g = e.Graphics;
            if (world.LifeTime <= 0)
            {
                foreach (var obj in world.Objects)
                {
                    if (obj.GetColourOrNone() == AntColour.Black)
                        BlackScore += 3;
                    if (obj.GetColourOrNone() == AntColour.White)
                        WhiteScore += 3;
                }
                g.DrawString("END", new Font("Arial", 16), new SolidBrush(Color.Black), 55, 105);
            }
            Width = 600;
            //g.DrawString(world.WhiteAntAI.PlayerName + " score: " + WhiteScore, new Font("Arial", 16), new SolidBrush(Color.Black), 5, 5);
            //g.DrawString(world.BlackAntAI.PlayerName + " score: " + BlackScore, new Font("Arial", 16), new SolidBrush(Color.Black), 5, 55);
        }
    }
}
