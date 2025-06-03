using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MusikPlayer
{
    public partial class Form1 : Form
    {
        private MusicPlayer player = new MusicPlayer();

        private Button btnOpen, btnPause, btnResume, btnStop;
        private TrackBar volumeSlider;
        private Label lblTrack, lblTime;
        private ProgressBar progressBar;
        private System.Windows.Forms.Timer timer;
        private double trackLength;

        public Form1()
        {
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "🎧 Musikplayer";
            this.BackColor = Color.WhiteSmoke;
            this.Size = new Size(500, 300);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            Font defaultFont = new Font("Segoe UI", 10, FontStyle.Regular);
            int spacing = 10;
            int top = 30;
            int left = 20;

            // Buttons
            btnOpen = new Button() { Text = "Öffnen", Font = defaultFont };
            btnPause = new Button() { Text = "Pause", Font = defaultFont };
            btnResume = new Button() { Text = "Resume", Font = defaultFont };
            btnStop = new Button() { Text = "Stop", Font = defaultFont };

            Button[] buttons = { btnOpen, btnPause, btnResume, btnStop };
            for (int i = 0; i < buttons.Length; i++)
            {
                var btn = buttons[i];
                btn.Size = new Size(85, 35);
                btn.Location = new Point(left + i * (btn.Width + spacing), top);
                StyleButton(btn);
                Controls.Add(btn);
            }

            btnOpen.Click += btnOpen_Click;
            btnPause.Click += btnPause_Click;
            btnResume.Click += btnResume_Click;
            btnStop.Click += btnStop_Click;

            // Volume
            volumeSlider = new TrackBar()
            {
                Minimum = 0,
                Maximum = 100,
                Value = 50,
                TickFrequency = 10,
                Location = new Point(left, top + 60),
                Width = 200
            };
            volumeSlider.Scroll += VolumeSlider_Scroll;
            Controls.Add(volumeSlider);

            // Labels
            lblTrack = new Label()
            {
                Text = "🎵 Spielt: -",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 40, 40),
                Location = new Point(left, top + 110),
                AutoSize = true
            };
            Controls.Add(lblTrack);

            lblTime = new Label()
            {
                Text = "⏱️ 00:00 / 00:00",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.FromArgb(90, 90, 90),
                Location = new Point(left, top + 140),
                AutoSize = true
            };
            Controls.Add(lblTime);

            // Fortschrittsbalken
            progressBar = new ProgressBar()
            {
                Location = new Point(left, top + 170),
                Size = new Size(400, 15),
                Minimum = 0,
                Maximum = 100,
                Style = ProgressBarStyle.Continuous
            };
            Controls.Add(progressBar);

            // Timer
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 500;
            timer.Tick += Timer_Tick;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            using OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "MP3 Files (*.mp3)|*.mp3";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var track = new Track(dialog.FileName);
                player.Play(track);
                lblTrack.Text = $"🎵 Spielt: {track.Title}";
                trackLength = player.GetTotalTime().TotalSeconds;
                timer.Start();
            }
        }

        private void btnPause_Click(object sender, EventArgs e) => player.Pause();
        private void btnResume_Click(object sender, EventArgs e) => player.Resume();

        private void btnStop_Click(object sender, EventArgs e)
        {
            player.Stop();
            progressBar.Value = 0;
            lblTime.Text = "⏱️ 00:00 / 00:00";
            timer.Stop();
        }

        private void VolumeSlider_Scroll(object sender, EventArgs e)
        {
            var slider = sender as TrackBar;
            float volume = slider.Value / 100f;
            player.SetVolume(volume);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (player.GetCurrentTime() is TimeSpan current && trackLength > 0)
            {
                lblTime.Text = $"⏱️ {current:mm\\:ss} / {TimeSpan.FromSeconds(trackLength):mm\\:ss}";
                progressBar.Value = Math.Min(100, (int)(current.TotalSeconds / trackLength * 100));
            }
        }

        private void StyleButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.BackColor = Color.FromArgb(30, 144, 255);
            btn.ForeColor = Color.White;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;
            btn.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btn.Width, btn.Height, 12, 12));
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);
    }
}
