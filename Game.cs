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

        //public static BaseObject[] _objs;
        private static List<Star> _stars = new List<Star>();
        private static List<Bullet> _bullets = new List<Bullet>();
        private static List<Medpack> _medpacks = new List<Medpack>();
        private static List<Asteroid> _asteroids = new List<Asteroid>();
        //private static Asteroid[] _asteroids;
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
            Load();

            timer.Start();
            timer.Tick += Timer_Tick;

            form.KeyDown += Form_KeyDown;

            Ship.MessageDie += Finish;
        }

        private static void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey) _bullets.Add(new Bullet(
                new Point(_ship.Rect.X + _ship.Rect.Width, _ship.Rect.Y + _ship.Rect.Height / 2), 
                new Point(4, 0), 
                new Size(4, 1))); 
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
            foreach (BaseObject d in _stars) d.Draw();
            foreach (Asteroid a in _asteroids) a?.Draw();
            foreach (Bullet b in _bullets) b?.Draw();
            foreach (Medpack c in _medpacks) c?.Draw();
            _ship?.Draw();
            if (_ship != null)
                Buffer.Graphics.DrawString("Energy:" + _ship.Energy, SystemFonts.DefaultFont, Brushes.White, 0, 0);
            Buffer.Render();
        }

        public static void Update()
        {
            foreach (Star a in _stars) a.Update();
            foreach (Bullet b in _bullets) b.Update();
            foreach (Medpack c in _medpacks) c.Update();
            var temp_b = new List<Bullet>();
            var temp_a = new List<Asteroid>();
            var temp_m = new List<Medpack>();

            if (_asteroids.Count == 0)
            {
                Asteroid.asteroidCount++;
                for (int i = 0; i < Asteroid.asteroidCount; i++) _asteroids.Add(new Asteroid(
                                    new Point(RandomInt.Next(Game.Width / 2, Game.Width), RandomInt.Next(0, Game.Height)),
                                    new Point(RandomInt.Next(1, 10), RandomInt.Next(1, 10)),
                                    new Size(RandomInt.Next(10, 50), RandomInt.Next(10, 50))));
            }
            if (_medpacks.Count == 0)
            {
                _medpacks.Add(new Medpack(
                            new Point(RandomInt.Next(Game.Width / 2, Game.Width), RandomInt.Next(20, Game.Height)),
                            new Point(RandomInt.Next(1, 5), 0),
                            new Size(30, 25)));
            }

            foreach (var asteroid in _asteroids)
            {
                asteroid.Update();
                foreach (var bullet in _bullets)
                    if (bullet.Collision(asteroid))
                    {
                        System.Media.SystemSounds.Hand.Play();
                        temp_a.Add(asteroid);
                        temp_b.Add(bullet);
                    }
                if (_ship.Collision(asteroid))
                {
                    _ship.EnergyLow(RandomInt.Next(1, 10));
                    System.Media.SystemSounds.Asterisk.Play();
                    temp_a.Add(asteroid);
                    if (_ship.Energy <= 0) _ship.Die();
                }
            }
            foreach (var medpack in _medpacks)
            {
                if (medpack.Collision(_ship))
                {
                    temp_m.Add(medpack);
                    _ship.EnergyUp(RandomInt.Next(10, 30));
                }
            }
            foreach (Bullet b in temp_b) _bullets.Remove(b);
            foreach (Asteroid a in temp_a) _asteroids.Remove(a);
            foreach (Medpack m in temp_m) _medpacks.Remove(m);
        }

        public static void Load()
        {

            for (int i = 0; i < Asteroid.asteroidCount; i++) _asteroids.Add( new Asteroid(
                    new Point(RandomInt.Next(Game.Width / 2, Game.Width), RandomInt.Next(0, Game.Height)),
                    new Point(RandomInt.Next(1, 10), RandomInt.Next(1, 10)),
                    new Size(RandomInt.Next(10, 50), RandomInt.Next(10, 50))));

            for (int i = 0; i < Star.starCount; i++) _stars.Add( new Star(
                    new Point(RandomInt.Next(0, Game.Width), RandomInt.Next(0, Game.Height)),
                    new Point(RandomInt.Next(1, 10), 0),
                    new Size(RandomInt.Next(50, 80), RandomInt.Next(60, 100))));

            _medpacks.Add(new Medpack(
                new Point(RandomInt.Next(Game.Width/2, Game.Width), RandomInt.Next(20, Game.Height)),
                new Point(RandomInt.Next(1, 5), 0),
                new Size(30, 25)));

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
