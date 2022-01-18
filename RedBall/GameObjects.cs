using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedBall
{
    
    abstract class GameObject
    {
        public bool isGravity;
        public float elasticity;
        public Vector forse;
        public Graphics g;
        public GameManager gm;
        public PointF pos;
        public Size size;

        protected void init(GameManager gm, Graphics g,Point p, Size size)
        {
            this.g = g;
            this.gm = gm;
            this.pos = p;
            this.size = size;
            forse = new Vector(0, 0);
            gm.objs.Add(this); 
        }
        public void Gravity() => forse += new Vector(0, 0.09f);
        public void AddForse(float X, float Y) => forse += new Vector(X, Y);
        public void AddForse(Vector v) => forse += v;
        public abstract void Render();
        public abstract void Step();
    }
    class RBall : GameObject
    {
        public RBall(GameManager gm, Graphics g,Point p,Size size)
        {
            init(gm, g,p,size);
            elasticity = 0.4;
        }
        public override void Render()
        {
            g.DrawEllipse(new Pen(Color.Red), pos.X - size.Width / 2, pos.Y - size.Height / 2, size.Width,size.Height);
        } 
        public override void Step()
        {
            pos += forse;
            Gravity();
            if (pos.Y + size.Height/2 >= 1000)
            {
                pos.Y = 1000 - size.Height / 2;
                forse = -forse*elasticity;
            }
        }

    }
    class GameManager
    {
        public List<GameObject> objs = new List<GameObject>();
        public List<Keys> keys = new List<Keys>();

    }
    class Vector
    {
        public Vector(float X, float Y)
        {
            x = X;
            y = Y;
        }
        public float x, y;
        public float getLength() =>(float) Math.Sqrt(x * x + y * y);
        public static Vector operator +(Vector V1, Vector V2) => new Vector(V1.x + V2.x, V1.y + V2.y);
        public static Vector operator -(Vector V1, Vector V2) => new Vector(V1.x - V2.x, V1.y - V2.y);
        public static PointF operator +(PointF p,Vector v) => new PointF(v.x + p.X, v.y + p.Y);
        public static PointF operator -(PointF p,Vector v) => new PointF(v.x - p.X, v.y - p.Y);
        public static Vector operator *(Vector v, float f) => new Vector(v.x * f, v.y * f);
        public static Vector operator /(Vector v, float f) => new Vector(v.x / f, v.y / f);
        public static Vector operator -(Vector V) => new Vector(-V.x,-V.y);
    }
}
