using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman
{
    // Klassdefinition för Clyde, som ärver från Enemy-klassen.
    public class Clyde : Enemy
    {
        // Konstruktor för Clyde-klassen.
        public Clyde(int tileX, int tileY, Tile[,] tileArray) : base(tileX, tileY, tileArray)
        {
            // Sätter Scatter-lägets målbricka för Clyde.
            ScatterTargetTile = new Vector2(2, 29);
            // Sätter typen av spöke till Clyde.
            type = GhostType.Clyde;

            // Definierar rektanglarna för Clydes nedåtrörelse animation.
            rectsDown[0] = new Rectangle(1659, 339, 42, 42);
            rectsDown[1] = new Rectangle(1707, 339, 42, 42);

            // Definierar rektanglarna för Clydes upprörelse animation.
            rectsUp[0] = new Rectangle(1563, 339, 42, 42);
            rectsUp[1] = new Rectangle(1611, 339, 42, 42);

            // Definierar rektanglarna för Clydes vänsterrörelse animation.
            rectsLeft[0] = new Rectangle(1467, 339, 42, 42);
            rectsLeft[1] = new Rectangle(1515, 339, 42, 42);

            // Definierar rektanglarna för Clydes högerrörelse animation.
            rectsRight[0] = new Rectangle(1371, 339, 42, 42);
            rectsRight[1] = new Rectangle(1419, 339, 42, 42);
        }

        // Överskrider basmetoden för att få Clydes jagarmålposition.
        public override Vector2 getChaseTargetPosition(Vector2 playerTilePos, Dir playerDir, Tile[,] tileArray)
        {
            // Om avståndet mellan spelaren och Clyde är större än 8 rutor, jaga spelaren.
            if (Tile.getDistanceBetweenTiles(playerTilePos, currentTile) > 8)
            {
                return playerTilePos;
            }
            // Annars, återgå till Scatter-lägets målbricka.
            return ScatterTargetTile;
        }
    }
}