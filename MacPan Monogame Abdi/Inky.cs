using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

// Definiera en klass för fienden Inky i spelet Pacman, som ärver från en basklass Enemy.
namespace Pacman
{
    // Deklarera en klass som representerar fienden Inky.
    public class Inky : Enemy
    {
        // Konstruktor för Inky-klassen som tar position och en referens till en array av Tiles.
        public Inky(int tileX, int tileY, Tile[,] tileArray) : base(tileX, tileY, tileArray)
        {
            // Sätt måltavlan för scatter-beteendet och typen av ghost.
            ScatterTargetTile = new Vector2(25, 29);
            type = GhostType.Inky;

            // Definiera rektanglar för animationer när Inky rör sig upp, ner, vänster eller höger.
            rectsDown[0] = new Rectangle(1659, 291, 42, 42);
            rectsDown[1] = new Rectangle(1707, 291, 42, 42);

            rectsUp[0] = new Rectangle(1563, 291, 42, 42);
            rectsUp[1] = new Rectangle(1611, 291, 42, 42);

            rectsLeft[0] = new Rectangle(1467, 291, 42, 42);
            rectsLeft[1] = new Rectangle(1515, 291, 42, 42);

            rectsRight[0] = new Rectangle(1371, 291, 42, 42);
            rectsRight[1] = new Rectangle(1419, 291, 42, 42);
        }

        // Överskriva metoden för att få målpositionen för Inky baserat på spelarens position och riktning.
        public override Vector2 getChaseTargetPosition(Vector2 playerTilePos, Dir playerDir, Tile[,] tileArray, Vector2 blinkyPos)
        {
            // Initialisera variabler för spelarens position, riktning och Blinkys position.
            Dir PlayerDir = playerDir;
            Vector2 PacmanPos = playerTilePos;
            Vector2 BlinkyPos = blinkyPos;

            // Om spelaren inte rör sig, använd den senaste kända riktningen.
            if (PlayerDir == Dir.None)
            {
                PlayerDir = playerLastDir;
            }

            // Skapa en initial målposition baserad på spelarens riktning.
            Vector2 finalTarget = new Vector2(0, 0);

            // Uppdatera spelarens sista kända riktning baserat på nuvarande riktning.
            switch (PlayerDir)
            {
                case Dir.Down:
                    finalTarget.Y += 2;
                    playerLastDir = Dir.Down;
                    break;
                case Dir.Up:
                    finalTarget.Y -= 2;
                    playerLastDir = Dir.Up;
                    break;
                case Dir.Left:
                    finalTarget.X -= 2;
                    playerLastDir = Dir.Left;
                    break;
                case Dir.Right:
                    finalTarget.X += 2;
                    playerLastDir = Dir.Right;
                    break;
            }

            // Beräkna skillnad mellan Inky och Pacman i x- och y-led.
            if (PacmanPos.X < BlinkyPos.X)
            {
                finalTarget.X = BlinkyPos.X - PacmanPos.X;
            }
            else
            {
                finalTarget.X = PacmanPos.X - BlinkyPos.X;
            }

            if (PacmanPos.Y < BlinkyPos.Y)
            {
                finalTarget.Y = BlinkyPos.Y - PacmanPos.Y;
            }
            else
            {
                finalTarget.Y = PacmanPos.Y - BlinkyPos.Y;
            }

            // Dubbela målpositionen för att få en större rörelse.
            finalTarget *= 2;

            // Anpassa målpositionen till Inkys nuvarande position på kartan.
            finalTarget.X += currentTile.X;
            finalTarget.Y += currentTile.Y;

            // Kontrollera om målpositionen går utanför gränserna eller mot ett vägg, om så är fallet, återställ till spelarens position.
            if (finalTarget.X < 0 || finalTarget.Y < 0 || finalTarget.X > Controller.numberOfTilesX - 1 || finalTarget.Y > Controller.numberOfTilesY - 1)
            {
                return playerTilePos;
            }
            if (tileArray[(int)finalTarget.X, (int)finalTarget.Y].tileType == Tile.TileType.Wall)
            {
                return playerTilePos;
            }

            // Returnera det beräknade målpositionen.
            return finalTarget;
        }
    }
}