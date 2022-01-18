using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            new RBall(gm, gf, new Point(300, 200), new Size(10, 10));        
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
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            gm.keys.Remove(e.KeyCode);        
        }

        public void Render()
        {
            gf.Clear(Color.LightBlue);
            foreach (GameObject gameObject in gm.objs) gameObject.Render();
            g.DrawImage(frame, 0, 0);
        }
    }
}
