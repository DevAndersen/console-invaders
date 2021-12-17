using DevAndersen.ConsoleInvaders.Entities;
using DevAndersen.ConsoleInvaders.Entities.Living;
using DevAndersen.ConsoleInvaders.Native;
using DevAndersen.ConsoleInvaders.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAndersen.ConsoleInvaders
{
    internal class Game
    {
        public const int DefeatLine = 17;

        public int Width { get; init; }

        public int Height { get; init; }

        public List<Entity> Entities { get; } = new List<Entity>();

        public CharacterGrid CharGrid { get; set; }

        public EntityPlayer Player { get; private set; }

        public int GameTick { get; private set; }

        public int RenderTick { get; private set; }

        public bool running = false;
        private readonly Thread updateThread;
        private readonly Thread renderingThread;
        private StringBuilder stringBuilder = new StringBuilder();
        private IntPtr consoleOutHandle;
        public object gameLockObject = new object();

        public Game(int width, int height)
        {
            Width = width;
            Height = height;

            PopulateGame();

            updateThread = new Thread(GameLoop);
            renderingThread = new Thread(RenderLoop);
        }

        public void Run()
        {
            consoleOutHandle = Kernel32.GetStdHandle(Kernel32.StdHandle.STD_OUTPUT_HANDLE);
            Kernel32.GetConsoleMode(consoleOutHandle, out Kernel32.ConsoleMode consoleMode);
            consoleMode |= Kernel32.ConsoleMode.ENABLE_VIRTUAL_TERMINAL_PROCESSING;
            Kernel32.SetConsoleMode(consoleOutHandle, consoleMode);

            running = true;
            updateThread.Start();
            renderingThread.Start();
            InputLoop();

            if (Entities.Where(x => x is EntityInvader).Any())
            {
                RenderGameMessage(0xff0000, new string[]
                {
                    "############",
                    "# YOU LOSE #",
                    "############"
                });
            }
            else
            {
                RenderGameMessage(0x00ff00, new string[]
                {
                    "###########",
                    "# YOU WIN #",
                    "###########"
                });
            }
        }

        public List<Entity> GetEntityAtPosition(int posX, int posY)
        {
            return Entities.Where(e => e.PosX == posX && e.PosY == posY).ToList();
        }

        public void SpawnEntity(Entity entity)
        {
            lock (gameLockObject)
            {
                Entities.Add(entity);
            }
        }

        public void EndGame()
        {
            running = false;
            Kernel32.CancelIoEx(Kernel32.GetStdHandle(Kernel32.StdHandle.STD_INPUT_HANDLE), IntPtr.Zero);
        }

        private void PopulateGame()
        {
            lock(gameLockObject)
            {
                Player = new EntityPlayer(this, Width / 2, Height - 1);
                Entities.Add(Player);

                for (int i = 1; i < Width; i += Width / 5)
                {
                    Entities.Add(new EntityInvader(this, i, 10, true));
                    Entities.Add(new EntityInvader(this, i, 11, false));
                }

                for (int i = 1; i < Width; i += Width / 15)
                {
                    Entities.Add(new EntityBarrier(this, i, Height - 5));
                }
            }
        }

        private void InputLoop()
        {
            while (running)
            {
                try
                {
                    var readKey = Console.ReadKey(true);
                    switch (readKey.Key)
                    {
                        case ConsoleKey.LeftArrow:
                            Player.Move(-1, 0);
                            break;
                        case ConsoleKey.RightArrow:
                            Player.Move(1, 0);
                            break;
                        case ConsoleKey.UpArrow:
                            Player.Shoot();
                            break;
                        default:
                            break;
                    }
                }
                catch (InvalidOperationException) // Thrown when the ReadKey request is cancelled. Catch it and ignore it.
                {
                }
            }
        }

        private void GameLoop()
        {
            while (running)
            {
                List<Entity> deadEntities = new List<Entity>();
                lock (gameLockObject)
                {
                    Entity[] entitiesCopy = new Entity[Entities.Count];

                    if (!Entities.Where(x => x is EntityInvader).Any())
                    {
                        EndGame();
                        return;
                    }

                    Entities.CopyTo(entitiesCopy);

                    foreach (Entity entity in entitiesCopy)
                    {
                        entity.Update();

                        if (entity.IsDead)
                        {
                            deadEntities.Add(entity);
                        }
                    }

                    foreach (Entity deadEntity in deadEntities)
                    {
                        Entities.Remove(deadEntity);
                    }
                    deadEntities.Clear();
                }

                GameTick++;
                Thread.Sleep(20);
            }
        }

        private void RenderLoop()
        {
            while (running)
            {
                CharGrid = new CharacterGrid(new ConsoleChar[Width, Height]);

                lock(gameLockObject)
                {
                    foreach (Entity entity in Entities)
                    {
                        CharGrid[entity.PosX, entity.PosY] = entity.Visual;
                    }
                }

                // Render defeat line
                for (int x = 0; x < Width; x++)
                {
                    CharGrid.UpdateChar(x, DefeatLine, (c) => c with
                    {
                        background = 0x3b5c44,
                        options = c.options | ConsoleCharOptions.Background
                    });
                }

                // Render aim guide
                for (int y = 0; y < Height; y++)
                {
                    CharGrid.UpdateChar(Player.PosX, y, (c) => c with
                    {
                        background = 0x112233,
                        options = c.options | ConsoleCharOptions.Background
                    });
                }

                CharGrid[Player.PosX, Player.PosY] = Player.Visual;
                RenderConsoleCharGrid();
                RenderTick++;
                Thread.Sleep(5);
            }
        }

        private void RenderGameMessage(int color, string[] lines)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                string loseLine = lines[i];
                int xOffset = Width / 2 - loseLine.Length / 2;

                for (int c = 0; c < loseLine.Length; c++)
                {
                    CharGrid[xOffset + c, (Height / 2) + i] = new ConsoleChar(loseLine[c], color);
                }
            }
            RenderConsoleCharGrid();
        }

        private void RenderConsoleCharGrid()
        {
            stringBuilder = new StringBuilder();

            for (int y = 0; y < CharGrid.Height; y++)
            {
                for (int x = 0; x < CharGrid.Width; x++)
                {
                    CharGrid[x, y].AppendToStringBuilder(stringBuilder);
                }
            }

            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);
            Kernel32.WriteConsole(consoleOutHandle, stringBuilder.ToString(), (uint)stringBuilder.Length, out _);
        }
    }
}
