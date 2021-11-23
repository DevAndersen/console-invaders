using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAndersen.ConsoleInvaders.Rendering
{
    internal class CharacterGrid
    {
        public ConsoleChar this[int x, int y]
        {
            get
            {
                return IsWithinGrid(x, y)
                    ? Grid[x, y]
                    : default;
            }
            set
            {
                if (IsWithinGrid(x, y))
                {
                    Grid[x, y] = value;
                }
            }
        }

        public ConsoleChar[,] Grid;

        public int Width { get; set; }

        public int Height { get; set; }

        public void UpdateChar(int x, int y, Func<ConsoleChar, ConsoleChar> charFunc)
        {
            this[x, y] = charFunc(this[x, y]);
        }

        public CharacterGrid(ConsoleChar[,] grid)
        {
            Grid = grid;
            Width = grid.GetLength(0);
            Height = grid.GetLength(1);
        }

        private bool IsWithinGrid(int x, int y)
        {
            return x >= 0
                && x < Width
                && y >= 0
                && y < Height;
        }
    }
}
