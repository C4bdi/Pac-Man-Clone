using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Definerar namnrymden för spelklasserna
namespace Pacman
{
    // Spelarens huvudklass
    public class Player
    {
        // Positionen där spelaren befinner sig i koordinater
        private Vector2 position;

        // Riktningen som spelaren är vänd mot
        private Dir direction = Dir.Right;

        // Nästa riktning spelaren ska vänder i
        private Dir nextDirection = Dir.None;

        // Hastigheten som spelaren rör sig med
        private int speed = 150;

        // Avståndet från centrum av spelaren till dess gränser (används för dödsanimationer och liknande)
        public static int radiusOffSet = 19;

        // Rektanglar för dödsanimationen
        public static Rectangle[] deathAnimRect = new Rectangle[11];

        // Sista rektangeln används för att hålla spelenhetens position vid död
        private static Rectangle lastRect = new Rectangle(1467, 3, 39, 39);

        // Rektanglar för olika riktningar (up, ner, vänster, höger)
        public static Rectangle[] rectsDown = new Rectangle[3];
        public static Rectangle[] rectsUp = new Rectangle[3];
        public static Rectangle[] rectsLeft = new Rectangle[3];
        public static Rectangle[] rectsRight = new Rectangle[3];

        // Kontrollerar om spelaren kan röra sig eller inte
        bool canMove = true;

        // Tidare för att begränsa hur ofta spelaren kan röra sig
        float canMoveTimer = 0;
        float TimerThreshold = 0.2f;

        // Koordinaterna för den nuvarande och föregående plattan spelaren står på
        Vector2 previousTile;
        Vector2 currentTile;

        // Animering för spelarens sprite
        private SpriteAnimation playerAnim;

        // Antalet bonusliv spelaren har
        private int extraLives = 3;

        // Konstruktor för spelaren, initialiserar position och animationer
        public Player(int tileX, int tileY, Tile[,] tileArray)
        {
            position = tileArray[tileX, tileY].Position;
            position.X += 14;
            currentTile = new Vector2(tileX, tileY);
            previousTile = new Vector2(tileX - 1, tileY);

            // Initialisera rektanglarna för olika riktningar
            rectsDown[0] = new Rectangle(1371, 147, 39, 39);
            rectsDown[1] = new Rectangle(1419, 147, 39, 39);
            rectsDown[2] = lastRect;

            rectsUp[0] = new Rectangle(1371, 99, 39, 39);
            rectsUp[1] = new Rectangle(1419, 99, 39, 39);
            rectsUp[2] = lastRect;

            rectsLeft[0] = new Rectangle(1371, 51, 39, 39);
            rectsLeft[1] = new Rectangle(1419, 51, 39, 39);
            rectsLeft[2] = lastRect;

            rectsRight[0] = new Rectangle(1371, 3, 39, 39);
            rectsRight[1] = new Rectangle(1419, 3, 39, 39);
            rectsRight[2] = lastRect;

            // Initialisera rektanglarna för dödsanimationen
            deathAnimRect[0] = new Rectangle(1515, 3, 39, 39);
            deathAnimRect[1] = new Rectangle(1563, 3, 39, 39);
            deathAnimRect[2] = new Rectangle(1611, 3, 39, 39);
            deathAnimRect[3] = new Rectangle(1659, 3, 39, 39);
            deathAnimRect[4] = new Rectangle(1707, 6, 39, 39);
            deathAnimRect[5] = new Rectangle(1755, 9, 39, 39);
            deathAnimRect[6] = new Rectangle(1803, 12, 39, 39);
            deathAnimRect[7] = new Rectangle(1851, 12, 39, 39);
            deathAnimRect[8] = new Rectangle(1899, 12, 39, 39);
            deathAnimRect[9] = new Rectangle(1947, 9, 39, 39);
            deathAnimRect[10] = new Rectangle(1995, 15, 39, 39);

            // Initialisera spelarens animering
            playerAnim = new SpriteAnimation(0.08f, rectsRight, 2);
        }

        // Egenskaper för att få åtkomst till spelarens attribut
        public int ExtraLives
        {
            get { return extraLives; }
            set { extraLives = value; }
        }

        public Vector2 CurrentTile
        {
            get { return currentTile; }
            set { currentTile = value; }
        }

        public Dir Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public SpriteAnimation PlayerAnim
        {
            get { return playerAnim; }
        }

        public Vector2 PreviousTile
        {
            get { return previousTile; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        // Metoder för att ändra spelarens position
        public void setX(float newX)
        {
            position.X = newX;
        }

        public void setY(float newY)
        {
            position.Y = newY;
        }
        // Metoden Update uppdaterar spelarens tillstånd baserat på tid och input
        public void Update(GameTime gameTime, Tile[,] tileArray)
        {
            // Hämtar det aktuella tillståndet av tangentbordet
            KeyboardState kState = Keyboard.GetState();

            // Beräknar tiden sedan senast uppdateringen
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Om spelaren inte kan röra sig, ökar vi tiden sedan senast spelaren kunde röra sig
            if (!canMove)
            {
                canMoveTimer += dt;
                // Efter en viss tid kan spelaren röra sig igen
                if (canMoveTimer >= TimerThreshold)
                {
                    canMove = true;
                    canMoveTimer = 0;
                }
            }

            // Uppdaterar spelarens animering baserat på tiden
            playerAnim.Update(gameTime);

            // Sätter nästa riktning baserat på vilken tangent som trycks ner
            if (kState.IsKeyDown(Keys.D))
            {
                nextDirection = Dir.Right;
            }
            if (kState.IsKeyDown(Keys.A))
            {
                nextDirection = Dir.Left;
            }
            if (kState.IsKeyDown(Keys.W))
            {
                nextDirection = Dir.Up;
            }
            if (kState.IsKeyDown(Keys.S))
            {
                nextDirection = Dir.Down;
            }

            // Om spelaren kan röra sig, flyttas spelaren i den angivna riktningen
            if (canMove)
            {
                switch (nextDirection)
                {
                    case Dir.Right:
                        if (Game1.gameController.isNextTileAvailable(nextDirection, currentTile))
                        {
                            canMove = false;
                            direction = nextDirection;
                            position.Y = tileArray[(int)currentTile.X, (int)currentTile.Y].Position.Y + 1;
                            nextDirection = Dir.None;
                        }
                        break;
                    case Dir.Left:
                        if (Game1.gameController.isNextTileAvailable(nextDirection, currentTile))
                        {
                            canMove = false;
                            direction = nextDirection;
                            position.Y = tileArray[(int)currentTile.X, (int)currentTile.Y].Position.Y + 1;
                            nextDirection = Dir.None;
                        }
                        break;
                    case Dir.Down:
                        if (Game1.gameController.isNextTileAvailable(nextDirection, currentTile))
                        {
                            canMove = false;
                            direction = nextDirection;
                            position.X = tileArray[(int)currentTile.X, (int)currentTile.Y].Position.X + 2;
                            nextDirection = Dir.None;
                        }
                        break;
                    case Dir.Up:
                        if (Game1.gameController.isNextTileAvailable(nextDirection, currentTile))
                        {
                            canMove = false;
                            direction = nextDirection;
                            position.X = tileArray[(int)currentTile.X, (int)currentTile.Y].Position.X + 2;
                            nextDirection = Dir.None;
                        }
                        break;
                }
            }

            // Om nästa platta inte är tillgänglig, sätts riktningen till None
            if (!Game1.gameController.isNextTileAvailable(direction, currentTile))
                direction = Dir.None;

            // Baserat på den valda riktningen, flyttas spelaren och uppdateras animeringsramverket
            switch (direction)
            {
                case Dir.Right:
                    if (Game1.gameController.isNextTileAvailable(Dir.Right, currentTile))
                    {
                        position.X += speed * dt;
                        playerAnim.setSourceRects(rectsRight);
                    }
                    break;
                case Dir.Left:
                    if (Game1.gameController.isNextTileAvailable(Dir.Left, currentTile))
                    {
                        position.X -= speed * dt;
                        playerAnim.setSourceRects(rectsLeft);
                    }
                    break;
                case Dir.Down:
                    if (Game1.gameController.isNextTileAvailable(Dir.Down, currentTile))
                    {
                        position.Y += speed * dt;
                        playerAnim.setSourceRects(rectsDown);
                    }
                    break;
                case Dir.Up:
                    if (Game1.gameController.isNextTileAvailable(Dir.Up, currentTile))
                    {
                        position.Y -= speed * dt;
                        playerAnim.setSourceRects(rectsUp);
                    }
                    break;
                case Dir.None:
                    Vector2 p = tileArray[(int)currentTile.X, (int)currentTile.Y].Position;
                    position = new Vector2(p.X + 2, p.Y + 1);
                    MySounds.munchInstance.Stop();
                    break;
            }
        }

        // Metoden eatSnack hanterar när spelaren äter ett godisbit
        public void eatSnack(int listPosition)
        {
            // Ökar poängen baserat på typen av godisbit som ätits
            Game1.score += Game1.gameController.snackList[listPosition].scoreGain;

            // Om en stor godisbit ätits, startas en specifik ljudsekvens
            if (Game1.gameController.snackList[listPosition].snackType == Snack.SnackType.Big)
            {
                Game1.gameController.eatenBigSnack = true;
                MySounds.eat_fruit.Play();
            }

            // Tar bort godisbiten från listan över godisbitar
            Game1.gameController.snackList.RemoveAt(listPosition);
            MySounds.munchInstance.Play();
        }

        // Metoden Draw renderar spelaren på skärmen
        public void Draw(SpriteBatch spriteBatch, SpriteSheet spriteSheet)
        {
            playerAnim.Draw(spriteBatch, spriteSheet, new Vector2(position.X - radiusOffSet / 2, position.Y - radiusOffSet / 2 + 1));
        }

        // Metoden checkForTeleportPos kontrollerar om spelaren ska teleporteras
        public int checkForTeleportPos(Tile[,] tileArray)
        {
            // Kontrollerar om spelaren är på en teleportationsplats
            if (new int[2] { (int)currentTile.X, (int)currentTile.Y }.SequenceEqual(new int[2] { 0, 14 }))
            {
                // Om spelaren är på en teleportationsplats, beräknar omständigheterna för teleportering
                if (position.X < -30)
                {
                    return 1;
                }
            }
            else if (new int[2] { (int)currentTile.X, (int)currentTile.Y }.SequenceEqual(new int[2] { Controller.numberOfTilesX - 1, 14 }))
            {
                if (position.X > tileArray[(int)currentTile.X, (int)currentTile.Y].Position.X + 30)
                {
                    return 2;
                }
            }
            return 0;
        }

        // Metoden teleport flyttar spelaren till en ny position
        public void teleport(Vector2 pos, Vector2 tilePos)
        {
            position = pos;
            previousTile = currentTile;
            currentTile = tilePos;
        }

        // Metoden updatePlayerTilePosition uppdaterar positionen av spelarens platta i spelet
        public void updatePlayerTilePosition(Tile[,] tileArray)
        {
            // Uppdaterar positionen av spelarens platta i spelets 2D-array
            tileArray[(int)previousTile.X, (int)previousTile.Y].tileType = Tile.TileType.None;
            tileArray[(int)currentTile.X, (int)currentTile.Y].tileType = Tile.TileType.Player;

            // Hanterar teleportationer baserat på spelarens position
            if (checkForTeleportPos(tileArray) == 1)
            {
                if (direction == Dir.Left)
                    teleport(new Vector2(Game1.windowWidth + 30, position.Y), new Vector2(Controller.numberOfTilesX - 1, 14));
            }
            else if (checkForTeleportPos(tileArray) == 2)
            {
                if (direction == Dir.Right)
                    teleport(new Vector2(-30, position.Y), new Vector2(0, 14));
            }

            // Loopar genom alla platser i spelets 2D-array
            for (int x = 0; x < tileArray.GetLength(0); x++)
            {
                for (int y = 0; y < tileArray.GetLength(1); y++)
                {
                    // Hanterar interaktion mellan spelare och godisbitar
                    if (Game1.gameController.checkTileType(new Vector2(x, y), Tile.TileType.Player))
                    {
                        int snackListPos = Game1.gameController.findSnackListPosition(tileArray[x, y].Position);
                        if (snackListPos != -1)
                        {
                            eatSnack(snackListPos);
                        }
                    }

                    // Uppdaterar positionen av spelarens platta baserat på riktningen
                    float tilePosX = tileArray[x, y].Position.X;
                    float tilePosY = tileArray[x, y].Position.Y;

                    float nextTilePosX = tileArray[x, y].Position.X + Controller.tileWidth;
                    float nextTilePosY = tileArray[x, y].Position.Y + Controller.tileHeight;

                    float pacmanPosOffSetX = radiusOffSet / 2;
                    float pacmanPosOffSetY = radiusOffSet / 2;

                    switch (direction)
                    {
                        case Dir.Right:
                            nextTilePosX = tileArray[x, y].Position.X + Controller.tileWidth;
                            break;
                        case Dir.Left:
                            nextTilePosX = tileArray[x, y].Position.X - Controller.tileWidth;
                            pacmanPosOffSetX *= -1;
                            break;
                        case Dir.Down:
                            nextTilePosY = tileArray[x, y].Position.Y + Controller.tileHeight;
                            break;
                        case Dir.Up:
                            nextTilePosY = tileArray[x, y].Position.Y - Controller.tileHeight;
                            pacmanPosOffSetY *= -1;
                            break;
                    }

                    float pacmanPosX = position.X + pacmanPosOffSetX;
                    float pacmanPosY = position.Y + pacmanPosOffSetY;

                    // Hanterar när spelaren passerar över en platta
                    if (direction == Dir.Right || direction == Dir.Down)
                    {
                        if (pacmanPosX >= tilePosX && pacmanPosX < nextTilePosX)
                        {
                            if (pacmanPosY >= tilePosY && pacmanPosY < nextTilePosY)
                            {
                                previousTile = currentTile;
                                currentTile = new Vector2(x, y);
                                if (Game1.gameController.checkTileType(currentTile, Tile.TileType.None))
                                {
                                    MySounds.munchInstance.Stop();
                                }
                            }
                        }
                    }
                    else if (direction == Dir.Left)
                    {
                        if (pacmanPosX <= tilePosX && pacmanPosX > nextTilePosX)
                        {
                            if (pacmanPosY >= tilePosY && pacmanPosY < nextTilePosY)
                            {
                                previousTile = currentTile;
                                currentTile = new Vector2(x, y);
                                if (Game1.gameController.checkTileType(currentTile, Tile.TileType.None))
                                {
                                    MySounds.munchInstance.Stop();
                                }
                            }
                        }
                    }
                    else if (direction == Dir.Up)
                    {
                        if (pacmanPosX >= tilePosX && pacmanPosX < nextTilePosX)
                        {
                            if (pacmanPosY <= tilePosY && pacmanPosY > nextTilePosY)
                            {
                                previousTile = currentTile;
                                currentTile = new Vector2(x, y);
                                if (Game1.gameController.checkTileType(currentTile, Tile.TileType.None))
                                {
                                    MySounds.munchInstance.Stop();
                                }
                            }
                        }
                    }
                }
            }
        }

        // Metoden debugPacmanPosition visar spelarens position för debugging
        public void debugPacmanPosition(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.debuggingDot, position, Color.White);
        }
    }
}