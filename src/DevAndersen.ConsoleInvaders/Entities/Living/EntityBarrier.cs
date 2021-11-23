using DevAndersen.ConsoleInvaders.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAndersen.ConsoleInvaders.Entities.Living
{
    internal class EntityBarrier : EntityLiving
    {
        public EntityBarrier(Game game, int posX, int posY) : base(game, posX, posY)
        {
        }

        public override ConsoleChar Visual => new ConsoleChar(GetCharacter(), 0xaaaaaa, 0x0000ff);

        public override void Update()
        {
        }

        protected override int GetMaxHealth() => 4;

        private char GetCharacter() => Health switch
        {
            <= 0 => ' ',
            1 => '▗',
            2 => '▚',
            3 => '▙',
            4 => '▉',
            _ => '?'
        };
    }
}
