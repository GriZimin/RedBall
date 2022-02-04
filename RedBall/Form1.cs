using System;
using System.Drawing;
using System.Windows.Forms;

namespace RedBall
{
    public partial class Form1 : Form
    {
        Graphics g, gf;
        Bitmap frame;
        GameManager gm = new GameManager();
        public Form1()
        {
            InitializeComponent();
            g = CreateGraphics();
            frame = new Bitmap(Width, Height);
            gf = Graphics.FromImage(frame);
            LevelBuild();
        }

        private void LevelBuild()
        {
            new RBall(gm, gf, new PointF(500, 500), new SizeF(500, 500));
            new Platform(gm, gf, new PointF( -500, 1500), new SizeF(1000, 1));
            new Platform(gm, gf, new PointF( -500, 500), new SizeF(1000, 1));
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            Step();
            Render();

        }
        public void Step()
        {
            foreach (GameObject gameObject in gm.objs) gameObject.Step();
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            gm.keys.Add(e.KeyCode);
            if (e.KeyCode == Keys.PageUp && gm.scale > 1) gm.scale--;
            if (e.KeyCode == Keys.PageDown) gm.scale++;
            if (e.KeyCode == Keys.Down) gm.posCamera.Y += 2;
            if (e.KeyCode == Keys.Up) gm.posCamera.Y -= 2;
            if (e.KeyCode == Keys.Left) gm.posCamera.X -= 2;
            if (e.KeyCode == Keys.Right) gm.posCamera.X += 2;
            if ((e.KeyCode == Keys.Space || e.KeyCode == Keys.W)) gm.ball.AddForse(0, -Data.forseJump);
            if (e.KeyCode == Keys.D) gm.ball.AddForse(Data.speedBall, 0);
            if (e.KeyCode == Keys.A) gm.ball.AddForse(-Data.speedBall, 0);

        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            gm.keys.Remove(e.KeyCode);
        }

        public void Render()
        {
            gf.Clear(Color.LightBlue);
            foreach (GameObject gameObject in gm.objs) gameObject.Render();
            gf.DrawString(gm.ball.forse.ToString(), new Font("Calibri", 16), new SolidBrush(Color.Black), new Point(5, 5));
            g.DrawImage(frame, 0, 0);
        }
    }
    static class Data
    {
        public static float gravityValue = 4.5f;
        public static float forseJump = 100;
        public static float speedBall = 25;
        public static float XSpeedMax = 30;
        public static float YSpeedMax = 50;
    }
}
