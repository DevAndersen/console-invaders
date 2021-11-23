using DevAndersen.ConsoleInvaders.Entities.Living;
using DevAndersen.ConsoleInvaders.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAndersen.ConsoleInvaders.Entities
{
    internal class EntityLaser : Entity
    {
        public bool IsGood { get; set; }

        public override ConsoleChar Visual => new ConsoleChar('|', IsGood ? 0x00ff00 : 0xff0000);

        public EntityLaser(Game game, int posX, int posY, bool isGood) : base(game, posX, posY)
        {
            IsGood = isGood;
        }

        public override void Update()
        {
            bool hitLiving = false;
            int relativeY = IsGood ? -1 : 1;
            List<Entity> hitEntities = game.GetEntityAtPosition(PosX, PosY + relativeY);

            lock(game.gameLockObject)
            {
                foreach (Entity hitEntity in hitEntities)
                {
                    if (hitEntity is EntityLiving living)
                    {
                        hitLiving = true;

                        if (living is EntityInvader invader)
                        {
                            if (IsGood)
                            {
                                invader.Damage(1);
                            }
                            else
                            {
                                hitLiving = false;
                            }
                        }
                        else if (living is EntityPlayer player)
                        {
                            if (!IsGood)
                            {
                                player.Damage(1);
                            }
                            else
                            {
                                hitLiving = false;
                            }
                        }
                        else
                        {
                            living.Damage(1);
                        }
                    }
                }
            }

            if (hitLiving || PosY > game.Height)
            {
                Kill();
            }
            else if (!Move(0, relativeY))
            {
                Kill();
            }
        }
    }
}
