using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AntsBattle
{
    public partial class AntForm : Form
    {
        private const int ImageSize = 32;
        private int _steps = 8;
        private readonly Timer timer = new Timer();
        private int _timeFractions;
        private readonly World _world;// = new World();
        private readonly int _stepsPerSecond;
        private bool IsOpen;

        public Images Images { get; set; }

        //public AntForm() : this(new Images("."), new World())
        //{
        //}

        public AntForm(Images images, World world, int stepsPerSecond = 10)
        {
            //FormClosing += ClosingForm;
            _world = world;
            _stepsPerSecond = stepsPerSecond;
            Images = images;
            ClientSize = world.Size.Scale(ImageSize);
        }

        private void ClosingForm(object sender, FormClosingEventArgs e)
        {
            _world.CloseAllProcess();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            timer.Tick += OnTimer;
            timer.Interval = 20;
            _steps = 1000 / _stepsPerSecond / timer.Interval;
            timer.Start();
            DoubleBuffered = true;
            Text = "Ants";
        }

        private void OnTimer(object sender, EventArgs e)
        {
            _timeFractions = (_timeFractions + 1) % _steps;
            if (_world.LifeTime <= 0) return;
            if (_timeFractions == 0)
                _world.MakeStep();
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            BackColor = Color.AliceBlue;
            var g = e.Graphics;
            foreach (var obj in _world.Objects)
            {
                var delta = (float)_timeFractions / _steps;
                var x = obj.Destination.X * delta + obj.Location.X * (1 - delta);
                var y = obj.Destination.Y * delta + obj.Location.Y * (1 - delta);
                g.DrawImage(obj.GetImage(Images, _world.Time),
                    new RectangleF(x * ImageSize, y * ImageSize, ImageSize, ImageSize));
            }
        }
    }
}
