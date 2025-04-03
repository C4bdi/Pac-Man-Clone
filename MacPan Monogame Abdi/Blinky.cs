using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman
{
    // Klassdefinition för Blinky, som ärver från Enemy-klassen.
    public class Blinky : Enemy
    {
        // Konstruktor för Blinky-klassen.
        public Blinky(int tileX, int tileY, Tile[,] tileArray) : base(tileX, tileY, tileArray)
        {
            // Sätter Scatter-lägets målbricka för Blinky.
            ScatterTargetTile = new Vector2(26, 2);
            // Sätter typen av spöke till Blinky.
            type = GhostType.Blinky;

            // Definierar rektanglarna för Blinky's nedåtrörelse animation.
            rectsDown[0] = new Rectangle(1659, 195, 42, 42);
            rectsDown[1] = new Rectangle(1707, 195, 42, 42);

            // Definierar rektanglarna för Blinky's upprörelse animation.
            rectsUp[0] = new Rectangle(1563, 195, 42, 42);
            rectsUp[1] = new Rectangle(1611, 195, 42, 42);

            // Definierar rektanglarna för Blinky's vänsterrörelse animation.
            rectsLeft[0] = new Rectangle(1467, 195, 42, 42);
            rectsLeft[1] = new Rectangle(1515, 195, 42, 42);

            // Definierar rektanglarna för Blinky's högerrörelse animation.
            rectsRight[0] = new Rectangle(1371, 195, 42, 42);
            rectsRight[1] = new Rectangle(1419, 195, 42, 42);

            // Skapar en ny SpriteAnimation för Blinky med initial vänster rörelse.
            enemyAnim = new SpriteAnimation(0.08f, rectsLeft);
        }
    }
}
