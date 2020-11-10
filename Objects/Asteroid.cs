using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace Asteroids.Objects
{
    class Asteroid : BaseObject
    {
        static public int asteroidCount = 10;
        private Random r = new Random();
        private Image[] pics = { 
            Image.FromFile(@"Images\a1.png"),
            Image.FromFile(@"Images\a2.png"),
            Image.FromFile(@"Images\a3.png"),
            Image.FromFile(@"Images\a4.png"),
            Image.FromFile(@"Images\a5.png"),
            Image.FromFile(@"Images\a6.png"),
            Image.FromFile(@"Images\a10.png")
        };
        public int Power { get; set; }
        public Asteroid(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Power = 1;
        }
        public override void Draw()
        {          
            Game.Buffer.Graphics.DrawImage(pics[r.Next(6)], Pos.X, Pos.Y, Size.Width, Size.Height);
        }
    }
}
