using System;
using System.Collections.Generic;
using System.Drawing;
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
        public SizeF size;
        public bool isFly;


        protected void init(GameManager gm, Graphics g, PointF p, SizeF size)
        {
            this.g = g;
            this.gm = gm;
            this.pos = p;
            this.size = size;
            forse = new Vector(0, 0);
            gm.objs.Add(this);
        }
        public PointF GetRenderPos() => new PointF(pos.X / gm.scale + 500 - gm.posCamera.X, pos.Y / gm.scale + 500 - gm.posCamera.Y);
        public SizeF GetRenderSize() => new SizeF(size.Width / gm.scale, size.Height / gm.scale);
        public void Gravity() => forse.y = Math.Min(forse.y + Data.gravityValue, Data.YSpeedMax);
        public void UnGravity() => forse.y -= Data.gravityValue;
        public float Dist(PointF p1, PointF p2) => (float)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        public void AddForse(float X, float Y) => forse += new Vector(X, Y);
        public void AddForse(Vector v) => forse += v;
        //public bool isfly() => !(pos.Y + size.Height / 2 >= gm.objs[1].pos.Y);
        public abstract void Render();
        public abstract void Step();

    }

    class RBall : GameObject
    {
        public RBall(GameManager gm, Graphics g, PointF p, SizeF size)
        {
            init(gm, g, p, size);
            elasticity = 0.4F;
            gm.ball = this;
        }
        public override void Render()
        {
            g.DrawEllipse(new Pen(Color.Red), GetRenderPos().X - GetRenderSize().Width / 2, GetRenderPos().Y - GetRenderSize().Height / 2, GetRenderSize().Width, GetRenderSize().Height);
        }
        public override void Step()
        {
            forse.y = (int)forse.y;
            forse.x = Math.Min(forse.x, Data.XSpeedMax);
            forse.x = Math.Max(forse.x, -Data.XSpeedMax);
            pos += forse;
            //if (isfly())
            isFly = true;
            foreach (var v in gm.platforms)
            {
                if (pos.Y + size.Height / 2 >= v.pos.Y && pos.Y + size.Height / 2 <= v.pos.Y + Data.YSpeedMax && forse.y >= 0 && pos.X >= v.pos.X && pos.X <= (v.pos + v.size).X)
                {

                    pos.Y = v.pos.Y - size.Height / 2;
                    forse.y = -forse.y * elasticity;
                    forse.x = forse.x / 2;
                    isFly = false;
                }
                else
                {
                    if (pos.Y < v.pos.Y && (Math.Abs(new Vector(pos, v.pos).getLength() - size.Height / 2) < size.Height / 25))
                    {

                        forse = new Vector(v.pos, pos).ToUnitVector() * forse.getLength();
                        isFly = false;
                    }
                    if (pos.Y < v.pos.Y && (Math.Abs(new Vector(pos, v.pos + size).getLength() - size.Height / 2) < size.Height / 25))
                    {

                        forse = new Vector(v.pos + size, pos).ToUnitVector() * forse.getLength();
                        isFly = false;
                    }

                }

            }
            if (isFly)
                Gravity();

            gm.posCamera = new PointF(pos.X / gm.scale + 150, pos.Y / gm.scale);
        }


    }
    class Platform : GameObject
    {
        public Platform(GameManager gm, Graphics g, PointF p, SizeF size)
        {
            init(gm, g, p, size);
            gm.platforms.Add(this);
        }
        public override void Render()
        {
            g.DrawLine(new Pen(Color.Brown), GetRenderPos(), GetRenderPos() + GetRenderSize());
        }
        public override void Step() { }
    }
    class GameManager
    {
        public List<GameObject> objs = new List<GameObject>();
        public List<Platform> platforms = new List<Platform>();
        public List<Keys> keys = new List<Keys>();
        public RBall ball;
        public PointF posCamera = new PointF(0, 0);
        public int scale = 6;
    }
    class Vector
    {
        public Vector(float X, float Y)
        {
            x = X;
            y = Y;
        }
        public Vector(PointF p1, PointF p2) => new Vector(p2.X - p1.X, p2.Y - p1.Y);
        public float x, y;
        public float getLength() => (float)Math.Sqrt(x * x + y * y);
        public static Vector operator +(Vector V1, Vector V2) => new Vector(V1.x + V2.x, V1.y + V2.y);
        public static Vector operator -(Vector V1, Vector V2) => new Vector(V1.x - V2.x, V1.y - V2.y);
        public static PointF operator +(PointF p, Vector v) => new PointF(v.x + p.X, v.y + p.Y);
        public static PointF operator -(PointF p, Vector v) => new PointF(v.x - p.X, v.y - p.Y);
        public static Vector operator *(Vector v, float f) => new Vector(v.x * f, v.y * f);
        public static Vector operator /(Vector v, float f) => new Vector(v.x / f, v.y / f);
        public static Vector operator -(Vector V) => new Vector(-V.x, -V.y);
        public PointF ToPointF() => new PointF(x, y);
        public Vector ToUnitVector() => this / getLength();

        public override string ToString()
        {
            return $"({x};{y})";
        }
    }

}
