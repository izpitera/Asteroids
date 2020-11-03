using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Asteroids.Objects;

namespace Asteroids
{
    class Game
    {
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;
        static public Random RandomInt { get; } = new Random();
        public static int Width { get; set; }
        public static int Height { get; set; }

        static public Image background = Image.FromFile(@"Images\galaxy.jpg");

        public static BaseObject[] _objs;
        private static Bullet _bullet;
        private static Asteroid[] _asteroids;
        private static Ship _ship = new Ship(new Point(10, 400), new Point(5, 5), new Size(100, 60));

        public static Timer timer = new Timer { Interval = 100 };

        static Game()
        {
        }
        public static void Init(Form form)
        {
            // Графическое устройство для вывода графики
            Graphics g;
            // Предоставляет доступ к главному буферу графического контекста для текущего приложения
            _context = BufferedGraphicsManager.Current;
            g = form.CreateGraphics();
            // Создаем объект (поверхность рисования) и связываем его с формой
            // Запоминаем размеры формы
            Width = form.ClientSize.Width;
            Height = form.ClientSize.Height;
            if (Width > 1000 || Width < 0) throw new ArgumentOutOfRangeException("Превышена максимальная высота формы");
            if (Height > 1000 || Height < 0) throw new ArgumentOutOfRangeException("Превышена максимальная ширина формы");

            // Связываем буфер в памяти с графическим объектом, чтобы рисовать в буфере
            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));
            // baseobjects initialization
            Load(30);

            timer.Start();
            timer.Tick += Timer_Tick;

            form.KeyDown += Form_KeyDown;

            Ship.MessageDie += Finish;
        }

        private static void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey) _bullet = new Bullet(
                new Point(_ship.Rect.X + _ship.Rect.Width, _ship.Rect.Y + _ship.Rect.Height/2), 
                new Point(4, 0), 
                new Size(4, 1));
            if (e.KeyCode == Keys.Up) _ship.Up();
            if (e.KeyCode == Keys.Down) _ship.Down();
        }

        public static void Draw()
        {
            // Проверяем вывод графики
            //Buffer.Graphics.Clear(Color.Black);
            //Buffer.Graphics.DrawRectangle(Pens.White, new Rectangle(100, 100, 200, 200));
            //Buffer.Graphics.FillEllipse(Brushes.Wheat, new Rectangle(100, 100, 200, 200));
            //Buffer.Render();

            //Buffer.Graphics.Clear(Color.Black)

            Buffer.Graphics.DrawImage(background, 0, 0);
            foreach (BaseObject obj in _objs) obj.Draw();
            foreach (Asteroid a in _asteroids)
            {
                a?.Draw();
            }
            _bullet?.Draw();
            _ship?.Draw();
            if (_ship != null)
                Buffer.Graphics.DrawString("Energy:" + _ship.Energy, SystemFonts.DefaultFont, Brushes.White, 0, 0);
            Buffer.Render();
        }

        public static void Update()
        {
            foreach (BaseObject obj in _objs) obj.Update();
            _bullet?.Update();
            for (var i = 0; i < _asteroids.Length; i++)
            {
                if (_asteroids[i] == null) continue;
                _asteroids[i].Update();
                if (_bullet != null && _bullet.Collision(_asteroids[i]))
                {
                    System.Media.SystemSounds.Hand.Play();
                    _asteroids[i] = null;
                    _bullet = null;
                    continue;
                }
                if (!_ship.Collision(_asteroids[i])) continue;
                var rnd = new Random();
                _ship?.EnergyLow(rnd.Next(1, 10));
                System.Media.SystemSounds.Asterisk.Play();
                if (_ship.Energy <= 0) _ship?.Die();
            }

        }

        public static void Load(int numberOfObjects)
        {
            _objs = new BaseObject[numberOfObjects];
            _asteroids = new Asteroid[12];

            for (int i = 0; i < _asteroids.Length; i++)
                _asteroids[i] = new Asteroid(new Point(RandomInt.Next(0, Game.Width), RandomInt.Next(0, Game.Height)),
                    new Point(RandomInt.Next(1, 10), RandomInt.Next(1, 10)),
                    new Size(RandomInt.Next(10, 50), RandomInt.Next(10, 50)));
            for (int i = 0; i < _objs.Length; i++)
                _objs[i] = new Star(new Point(RandomInt.Next(0, Game.Width), RandomInt.Next(0, Game.Height)),
                    new Point(RandomInt.Next(1, 10), 0),
                    new Size(60, 70));

        }
        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }
        public static void Finish()
        {
            timer.Stop();
            Buffer.Graphics.DrawString("The End", new Font(FontFamily.GenericSansSerif, 60, FontStyle.Underline), Brushes.White, 200, 100);
            Buffer.Render();
        }
    }
}
