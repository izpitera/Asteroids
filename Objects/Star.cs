using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Asteroids.Objects
{
    class Star : BaseObject
    {
        private Image star = Image.FromFile(@"Images\s1.png");
        public Star(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }
        public override void Draw()
        {
            //Game.Buffer.Graphics.DrawLine(Pens.White, Pos.X, Pos.Y, Pos.X + Size.Width, Pos.Y + Size.Height);
            //Game.Buffer.Graphics.DrawLine(Pens.White, Pos.X + Size.Width, Pos.Y, Pos.X, Pos.Y + Size.Height);
            Game.Buffer.Graphics.DrawImage(star, Pos.X, Pos.Y, Size.Width, Size.Height);
        }
        public override void Update()
        {
            if (Pos.X > 0)
            {
                Pos.X -= Dir.X;
            }
            else
            {
                Pos.X = Game.Width + Size.Width;
            }
        }
    }
}
