using DevAndersen.ConsoleInvaders.Entities.Animations;
using DevAndersen.ConsoleInvaders.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAndersen.ConsoleInvaders.Entities.Living
{
    internal class EntityInvader : EntityLiving
    {
        private bool goingRight;
        private int cooldown;

        public override ConsoleChar Visual => new ConsoleChar('ᗖ', 0xff0000);

        public EntityInvader(Game game, int posX, int posY, bool goingRight) : base(game, posX, posY)
        {
            this.goingRight = goingRight;
        }

        public override void Update()
        {
            if (game.GameTick % 3 == 0)
            {
                if (PosY >= game.DefeatLine)
                {
                    game.EndGame();
                }

                if (!Move(1 * (goingRight ? 1 : -1), 0))
                {
                    goingRight = !goingRight;
                    Move(0, 1);
                }

                if (cooldown > 0)
                {
                    cooldown--;
                }
                else if (new Random().Next() % 10 == 0)
                {
                    game.SpawnEntity(new EntityLaser(game, PosX, PosY + 1, false));
                    cooldown = 5;
                }
            }
        }

        protected override int GetMaxHealth() => 1;

        public override Func<int, ConsoleChar>? GetDeathAnimation() => (frame) => frame switch
        {
            0 => new ConsoleChar('O', 0xff8800),
            1 => new ConsoleChar('*', 0xaa4400),
            2 => new ConsoleChar('•', 0x660000),
            _ => default
        };
    }
}
