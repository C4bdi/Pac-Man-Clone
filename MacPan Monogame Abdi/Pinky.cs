using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// Definiera en klass för Pinky, en fiende i spelet Pacman.
namespace Pacman
{
    // Deklarera en klass som ärver från basen Enemy för att representera Pinky, en specifik typ av fiende.
    public class Pinky : Enemy
    {
        // Konstruktor för Pinky som initialiserar basen och inställningar specifika för Pinky.
        public Pinky(int tileX, int tileY, Tile[,] tileArray) : base(tileX, tileY, tileArray)
        {
            // Inställningar för scatter-target-tilen, vilket kan användas för vissa fiendemönster.
            ScatterTargetTile = new Vector2(1, 2);

            // Specificera typen av fiende.
            type = GhostType.Pinky;

            // Definiera rektanglar för animationer när Pinky rör sig åt olika håll.
            rectsDown[0] = new Rectangle(1659, 243, 42, 42);
            rectsDown[1] = new Rectangle(1707, 243, 42, 42);

            rectsUp[0] = new Rectangle(1563, 243, 42, 42);
            rectsUp[1] = new Rectangle(1611, 243, 42, 42);

            rectsLeft[0] = new Rectangle(1467, 243, 42, 42);
            rectsLeft[1] = new Rectangle(1515, 243, 42, 42);

            rectsRight[0] = new Rectangle(1371, 243, 42, 42);
            rectsRight[1] = new Rectangle(1419, 243, 42, 42);

            // Initialisera animasjonen för Pinky när den rör sig neråt.
            enemyAnim = new SpriteAnimation(0.08f, rectsDown);
        }

        // Överskriva metoden för att få chasets målposition baserat på spelarens position och riktning.
        public override Vector2 getChaseTargetPosition(Vector2 playerTilePos, Dir playerDir, Tile[,] tileArray)
        {
            // Starta med spelarens nuvarande position och riktning.
            Vector2 pos = playerTilePos;
            Dir PlayerDir = playerDir;

            // Om spelarens riktning är None, använd den senaste kända riktningen.
            if (PlayerDir == Dir.None)
            {
                PlayerDir = playerLastDir;
            }

            // Beroende på spelarens riktning, justera målpositionen.
            switch (PlayerDir)
            {
                case Dir.Right:
                    pos = new Vector2(playerTilePos.X + 4, playerTilePos.Y);
                    playerLastDir = Dir.Right;
                    break;
                case Dir.Left:
                    pos = new Vector2(playerTilePos.X - 4, playerTilePos.Y);
                    playerLastDir = Dir.Left;
                    break;
                case Dir.Down:
                    pos = new Vector2(playerTilePos.X, playerTilePos.Y + 4);
                    playerLastDir = Dir.Down;
                    break;
                case Dir.Up:
                    pos = new Vector2(playerTilePos.X, playerTilePos.Y - 4);
                    playerLastDir = Dir.Up;
                    break;
            }

            // Kontrollera om den nya positionen är utanför kartgränserna eller blockerad av ett vägg, om så är fallet, använd spelarens nuvarande position istället.
            if (pos.X < 0 || pos.Y < 0 || pos.X > Controller.numberOfTilesX - 1 || pos.Y > Controller.numberOfTilesY - 1)
            {
                return playerTilePos;
            }
            if (tileArray[(int)pos.X, (int)pos.Y].tileType == Tile.TileType.Wall)
            {
                return playerTilePos;
            }
            return pos;
        }
    }
}
