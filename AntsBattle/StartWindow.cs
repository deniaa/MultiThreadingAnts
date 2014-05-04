using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AntsBattle
{
    public partial class StartWindow : Form
    {
        private StartWindowData Data;
        private ComboBox FirstPlayerChoosen;
        private ComboBox SecondPlayerChoosen;
        private ComboBox MapChoosen;
        private TextBox LifeLength;
        private TextBox MouthLength;
        private TextBox FrogSleepTime;
        private Label _battleMap;
        private Label BlackVsWhite;
        private Label Message;
        private Label lifeTimeLable;
        private Label mouthLengthLable;
        private Label frogSleepLable;
        private Button startButton;
        private bool IsReady;
        // private string Args;

        public StartWindow(StartWindowData data)
        {
            Data = data;
            startButton = new Button {Location = new Point(150, 200), Text = @"Start!", Width = 125, Height = 50};
            startButton.Click += ClickStart;
            var setStatementButton = new Button {Location = new Point(250, 5), Text = @"Set statements", Width = 155, Height = 115};
            setStatementButton.Click += SetStatements;

            Width = 450;
            Height = 330;
            MinimumSize = new Size(Width, Height);
            MaximumSize = new Size(Width, Height);
            
            FirstPlayerChoosen = new ComboBox { Text = @"Choose first player name", Location = new Point(5, 5), Width = 200 };
            SecondPlayerChoosen = new ComboBox { Text = @"Choose second player name", Location = new Point(5, 35), Width = 200 };
            foreach (var player in Data.Players)
            {
                //if (player.EndsWith("1"))
                    FirstPlayerChoosen.Items.Add(player);
                //if (player.EndsWith("2"))
                    SecondPlayerChoosen.Items.Add(player);
            }


            MapChoosen = new ComboBox { Text = @"Choose map", Location = new Point(5, 95), Width = 200 };
            foreach (var map in Data.Maps)
            {
                MapChoosen.Items.Add(map);
            }

            LifeLength = new TextBox { Text = "35", Location = new Point(5, 130), Width = 120 };
            MouthLength = new TextBox { Text = "4", Location = new Point(145, 130), Width = 120 };
            FrogSleepTime = new TextBox { Text = "16", Location = new Point(285, 130), Width = 120 };

            lifeTimeLable = new Label { Text = @"set lifetime", Location = new Point(5, 150), Width = 120 };
            mouthLengthLable = new Label { Text = @"set mouth length", Location = new Point(145, 150), Width = 120 };
            frogSleepLable = new Label { Text = @"set sleep time", Location = new Point(285, 150), Width = 120 };
            Message = new Label { Location = new Point(150, 255), Width = 170 };

            Controls.Add(FirstPlayerChoosen);
            Controls.Add(SecondPlayerChoosen);
            Controls.Add(MapChoosen);
            Controls.Add(LifeLength);
            Controls.Add(MouthLength);
            Controls.Add(FrogSleepTime);
            Controls.Add(lifeTimeLable);
            Controls.Add(mouthLengthLable);
            Controls.Add(frogSleepLable);
            Controls.Add(startButton);
            //Controls.Add(setStatementButton);
            Controls.Add(Message);
        }

        private void SetStatements(object sender, EventArgs e)
        {
            //Message.Text = (string) FirstPlayerChoosen.SelectedItem;
            int lifeLength;
            int mouthLength;
            int sleepTime;

            if (FirstPlayerChoosen.SelectedItem == null && FirstPlayerChoosen.Items.Count > 0)
                FirstPlayerChoosen.SelectedItem = FirstPlayerChoosen.Items[0];
            if (SecondPlayerChoosen.SelectedItem == null && SecondPlayerChoosen.Items.Count > 0)
                SecondPlayerChoosen.SelectedItem = SecondPlayerChoosen.Items[0];
            if (MapChoosen.SelectedItem == null && MapChoosen.Items.Count > 0)
                MapChoosen.SelectedItem = MapChoosen.Items[0];

            if (FirstPlayerChoosen.SelectedItem == null ||
                SecondPlayerChoosen.SelectedItem == null ||
                MapChoosen.SelectedItem == null ||
                !int.TryParse(LifeLength.Text, out lifeLength) ||
                !int.TryParse(MouthLength.Text, out mouthLength) ||
                !int.TryParse(FrogSleepTime.Text, out sleepTime))
            {
                Message.Text = @"Not all field are correct";
                IsReady = false;
            }
            else
            {
                Message.Text = @"   Ready to start!";
                Data.Args = new[]
                {
                    LifeLength.Text + ' ',
                    MouthLength.Text + ' ',
                    FrogSleepTime.Text + ' ',
                    (string) FirstPlayerChoosen.SelectedItem + ".dll ",
                    (string) SecondPlayerChoosen.SelectedItem  + ".dll ",
                    (string) MapChoosen.SelectedItem + ".txt"
                };

                IsReady = true;
            }

        }

        public override sealed Size MaximumSize
        {
            get { return base.MaximumSize; }
            set { base.MaximumSize = value; }
        }

        public override sealed Size MinimumSize
        {
            get { return base.MinimumSize; }
            set { base.MinimumSize = value; }
        }

        private void ClickStart(object sender, EventArgs e)
        {
            try
            {
                SetStatements(sender, e);
                if (IsReady)
                {
                    FreezSettings();
                    var world = new World(Data.Args);
                    new WorldLoader()
                        .AddRule('#', loc => new Wall(loc))
                        .AddRule('F', loc => new Frog(loc))
                        .AddRule('E', loc => new Food(loc))
                        .AddRule('W', loc => new WhiteAnt(loc))
                        .AddRule('B', loc => new BlackAnt(loc))
                        .Load("Maps\\" + Data.Args[5], world);
                    var mainForm = new AntForm(new Images(".\\images"), world);
                    var resultsForm = new ResultsForm(world);
                    mainForm.Show();
                    resultsForm.Show();
                    Message.Text = "";
                }
                else
                {
                    Message.Text = @"Statements not filled";
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText("StartWindowsExc.txt", ex.ToString());
            }
        }

        private void FreezSettings()
        {
            startButton.Visible = false;
            SecondPlayerChoosen.Visible = false;
            FirstPlayerChoosen.Visible = false;
            MapChoosen.Visible = false;
            _battleMap = new Label
            {
                Text = "Map : " + MapChoosen.SelectedItem,
                Location = new Point(30, 90),
                Width = 500,
                Height = 100
            };
            
            BlackVsWhite = new Label
            {
                Text = FirstPlayerChoosen.SelectedItem + "    VS    " + SecondPlayerChoosen.SelectedItem,
                Location = new Point(30, 30),
                Width = 500,
                Height = 100
            };
            Controls.Add(BlackVsWhite);
            FrogSleepTime.KeyPress += BlockKeyPressed;
            MouthLength.KeyPress += BlockKeyPressed;
            LifeLength.KeyPress += BlockKeyPressed;
            mouthLengthLable.Text = "Mouth length";
            lifeTimeLable.Text = "Life Time";
            frogSleepLable.Text = "Frog sleep time";
        }

        private void BlockKeyPressed(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
