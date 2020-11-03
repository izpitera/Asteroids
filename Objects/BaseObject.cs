using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Asteroids.Objects
{
    public delegate void Message();
    interface ICollision
    {
        bool Collision(ICollision obj);
        Rectangle Rect { get; }
    }
    abstract class BaseObject: ICollision
    {
        protected Point Pos;
        protected Point Dir;
        protected Size Size;

        protected BaseObject(Point pos, Point dir, Size size)
        {
            Pos = pos;
            Dir = dir;
            Size = size;
        }
        public abstract void Draw();
        public virtual void Update()
        {
            Pos.X += Dir.X;
            Pos.Y += Dir.Y;
            //if (Pos.X < 0) Pos.X = Game.Width + Size.Width;
            if (Pos.X < 0 || Pos.X > Game.Width) Dir.X *= -1;
            if (Pos.Y < 0 || Pos.Y > Game.Height) Dir.Y *= -1;
        }
        public bool Collision(ICollision o) => o.Rect.IntersectsWith(this.Rect);

        public Rectangle Rect => new Rectangle(Pos, Size);
    }
}
