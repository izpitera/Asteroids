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
        static public int starCount = 20;
        private readonly Image star = Image.FromFile(@"Images\s1.png");
        public Star(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }
        public override void Draw()
        {
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
