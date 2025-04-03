using System;

namespace Pacman
{
    // Denna klass representerar huvudprogrammet för spelet Pacman.
    public static class Program
    {
        // Huvudmetoden som körs när programmet startas.
        [STAThread]
        static void Main()
        {
            // Skapar en instans av spelet och startar det.
            using (var game = new Game1())
                game.Run();
        }
    }
}
