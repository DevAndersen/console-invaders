using DevAndersen.ConsoleInvaders.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAndersen.ConsoleInvaders.Entities.Animations
{
    internal class Animation : Entity
    {
        public int Frame { get; set; }

        public Func<int, ConsoleChar> Frames { get; set; }

        public override ConsoleChar Visual => Frames(Frame);

        public Animation(Game game, int posX, int posY, Func<int, ConsoleChar> frames) : base(game, posX, posY)
        {
            Frames = frames;
        }

        public override void Update()
        {
            Frame++;
            if (Frames(Frame).character == (char)0)
            {
                Kill();
            }
        }
    }
}
