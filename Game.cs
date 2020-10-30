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

            Timer timer = new Timer { Interval = 100 };
            timer.Start();
            timer.Tick += Timer_Tick;
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
            foreach (Asteroid obj in _asteroids) obj.Draw();
            _bullet.Draw();
            Buffer.Render();
        }

        public static void Update()
        {
            foreach (BaseObject obj in _objs)
                obj.Update();
            foreach (Asteroid a in _asteroids)
            {
                a.Update();
                if (a.Collision(_bullet)) { System.Media.SystemSounds.Hand.Play(); }
            }
            _bullet.Update();
        }

        public static void Load(int numberOfObjects)
        {
            _objs = new BaseObject[numberOfObjects];
            _bullet = new Bullet(new Point(0, 200), new Point(5, 0), new Size(4, 1));
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
    }
}
