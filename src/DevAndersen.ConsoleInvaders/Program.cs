using DevAndersen.ConsoleInvaders.Entities;
using DevAndersen.ConsoleInvaders.Rendering;
using System.Text;

namespace DevAndersen.ConsoleInvaders;

public static class Program
{
    public static void Main()
    {
        Game game = new Game(Console.WindowWidth, Console.WindowHeight);
        game.Run();
        Console.ReadLine();
    }
}
