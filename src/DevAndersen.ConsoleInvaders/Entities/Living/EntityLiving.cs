using DevAndersen.ConsoleInvaders.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAndersen.ConsoleInvaders.Entities.Living
{
    internal abstract class EntityLiving : Entity
    {
        public int Health { get; set; }

        protected EntityLiving(Game game, int posX, int posY) : base(game, posX, posY)
        {
            Health = GetMaxHealth();
        }

        protected abstract int GetMaxHealth();

        public void Damage(int damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                Kill();
            }
        }
    }
}
