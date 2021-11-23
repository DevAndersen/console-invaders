using DevAndersen.ConsoleInvaders.Entities.Animations;
using DevAndersen.ConsoleInvaders.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAndersen.ConsoleInvaders.Entities
{
    abstract class Entity
    {
        public bool IsDead { get; set; }

        public int PosX { get; set; }

        public int PosY { get; set; }

        public abstract ConsoleChar Visual { get; }

        protected Game game;

        public Entity(Game game, int posX, int posY)
        {
            this.game = game;
            PosX = posX;
            PosY = posY;
        }

        public abstract void Update();

        public void Kill()
        {
            IsDead = true;
            OnDeath();
        }

        public bool Move(int relativeX, int relativeY)
        {
            if (PosX + relativeX >= 0 && PosX + relativeX < game.Width && PosY + relativeY >= 0 && PosY + relativeY < game.Width)
            {
                PosX += relativeX;
                PosY += relativeY;
                return true;
            }
            return false;
        }

        public virtual void OnDeath()
        {
            Func<int, ConsoleChar>? deathAnimationFrames = GetDeathAnimation();
            if (deathAnimationFrames != null)
            {
                lock (game.gameLockObject)
                {
                    game.SpawnEntity(new Animation(game, PosX, PosY, deathAnimationFrames));
                }
            }
        }

        public virtual Func<int, ConsoleChar>? GetDeathAnimation() => null;
    }
}
