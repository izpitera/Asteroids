using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Asteroids.Objects
{
    class Medpack: BaseObject
    {
        private Image medpack = Image.FromFile(@"Images\med.png");

        public Medpack(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }
        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(medpack, Pos.X, Pos.Y, Size.Width, Size.Height);
        }
        public override void Update()
        {
            Pos.X -= 3;
            if (Pos.X < 0) Pos.X = Game.Width - 3;
        }
    }
}