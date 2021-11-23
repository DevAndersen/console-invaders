using DevAndersen.ConsoleInvaders.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAndersen.ConsoleInvaders.Entities.Living
{
    internal class EntityPlayer : EntityLiving
    {
        public int cooldown;

        public override ConsoleChar Visual => new ConsoleChar('ᗗ', GetHealthIndicatorColor());

        public EntityPlayer(Game game, int posX, int posY) : base(game, posX, posY)
        {
        }

        public override void Update()
        {
            if (cooldown > 0)
            {
                cooldown--;
            }
        }

        public void Shoot()
        {
            if (cooldown <= 0)
            {
                lock (game.gameLockObject)
                {
                    game.SpawnEntity(new EntityLaser(game, PosX, PosY - 1, true));
                }
                cooldown = 10;
            }
        }

        protected override int GetMaxHealth() => 3;

        public override void OnDeath()
        {
            game.EndGame();
        }

        private int GetHealthIndicatorColor()
        {
            return Health switch
            {
                3 => 0x00ff00,
                2 => 0xaaff00,
                1 => 0xaa4400,
                _ => 0x000000
            };
        }
    }
}
