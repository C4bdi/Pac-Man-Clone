using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
    public class Enemy
    {
        // Definition av olika typer av spöken.
        public enum GhostType { Inky, Blinky, Pinky, Clyde };
        public GhostType type;

        // Definition av olika tillstånd som ett spöke kan vara i.
        public enum EnemyState { Scatter, Chase, Frightened, Eaten };
        public EnemyState state = EnemyState.Scatter;

        // Måltile för scatter-läge.
        public Vector2 ScatterTargetTile;

        // Måltile för när spöket har blivit uppätet.
        public Vector2 eatenTargetTile = new Vector2(13, 14);

        // Positionen för spöket på spelplanen.
        protected Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        // Indikator för om spöket kolliderar med något.
        public bool colliding = false;

        // Aktuell tile som spöket befinner sig på.
        protected Vector2 currentTile;
        public Vector2 CurrentTile
        {
            get { return currentTile; }
        }

        // Föregående tile som spöket befann sig på.
        protected Vector2 previousTile;
        protected Tile.TileType previousTileType;

        // Spökets nuvarande riktning.
        protected Dir direction;

        // Spelarens senaste riktning, används för att spöket ska kunna veta vilken tile det ska sikta på.
        protected Dir playerLastDir;

        // Lista över tiles på väg till PacMan.
        protected List<Vector2> pathToPacMan;
        protected Vector2 foundpathTile;

        public List<Vector2> PathToPacMan
        {
            get { return pathToPacMan; }
            set { pathToPacMan = value; }
        }

        // Hastighetsinställningar för spöket.
        public int speed = 140;
        public int normalSpeed = 140;
        public int frightenedSpeed = 90;
        public int eatenSpeed = 240;

        // Rectanglar för att rita spöket i olika riktningar och tillstånd.
        public Rectangle[] rectsDown = new Rectangle[2];
        public Rectangle[] rectsUp = new Rectangle[2];
        public Rectangle[] rectsLeft = new Rectangle[2];
        public Rectangle[] rectsRight = new Rectangle[2];

        public Rectangle rectDownEaten;
        public Rectangle rectUpEaten;
        public Rectangle rectLeftEaten;
        public Rectangle rectRightEaten;

        public Rectangle[] frightenedRects = new Rectangle[2];
        public Rectangle[] frightenedRectsEnd = new Rectangle[8];

        // Offset för att rita spöket korrekt på spelplanen.
        protected int drawOffSetX = -9;
        protected int drawOffSetY = -9;

        // Animering för spöket.
        protected SpriteAnimation enemyAnim;
        public SpriteAnimation EnemyAnim
        {
            get { return enemyAnim; }
        }

        // Timer för rädd tillståndet.
        public float timerFrightened;
        public float timerFrightenedLength = 8f;

        // Konstruktor för att initialisera spöket.
        public Enemy(int tileX, int tileY, Tile[,] tileArray)
        {
            position = tileArray[tileX, tileY].Position;
            currentTile = new Vector2(tileX, tileY);
            previousTile = new Vector2(-1, -1);
            direction = Dir.None;

            // Initialisera rektanglar för de olika tillstånden.
            rectRightEaten = new Rectangle(1755, 243, 42, 42);
            rectLeftEaten = new Rectangle(1803, 243, 42, 42);
            rectUpEaten = new Rectangle(1851, 243, 42, 42);
            rectDownEaten = new Rectangle(1899, 243, 42, 42);

            frightenedRects[0] = new Rectangle(1755, 195, 42, 42);
            frightenedRects[1] = new Rectangle(1803, 195, 42, 42);

            frightenedRectsEnd[0] = new Rectangle(1755, 195, 42, 42);
            frightenedRectsEnd[1] = new Rectangle(1803, 195, 42, 42);
            frightenedRectsEnd[2] = new Rectangle(1755, 195, 42, 42);
            frightenedRectsEnd[3] = new Rectangle(1803, 195, 42, 42);
            frightenedRectsEnd[4] = new Rectangle(1851, 195, 42, 42);
            frightenedRectsEnd[5] = new Rectangle(1899, 195, 42, 42);
            frightenedRectsEnd[6] = new Rectangle(1851, 195, 42, 42);
            frightenedRectsEnd[7] = new Rectangle(1899, 195, 42, 42);

            enemyAnim = new SpriteAnimation(0.08f, rectsUp);

            position.X += 12;
        }

        // Metod för att rita spöket på skärmen.
        public void Draw(SpriteBatch spriteBatch, SpriteSheet spriteSheet)
        {
            if (state != EnemyState.Eaten)
                enemyAnim.Draw(spriteBatch, spriteSheet, new Vector2(position.X + drawOffSetX, position.Y + drawOffSetY));
            else
            {
                // Rita spöket baserat på dess riktning om det är i tillståndet "Eaten".
                switch (direction)
                {
                    case Dir.Up:
                        spriteSheet.drawSprite(spriteBatch, rectUpEaten, new Vector2(position.X + drawOffSetX, position.Y + drawOffSetY));
                        break;
                    case Dir.Down:
                        spriteSheet.drawSprite(spriteBatch, rectDownEaten, new Vector2(position.X + drawOffSetX, position.Y + drawOffSetY));
                        break;
                    case Dir.Left:
                        spriteSheet.drawSprite(spriteBatch, rectLeftEaten, new Vector2(position.X + drawOffSetX, position.Y + drawOffSetY));
                        break;
                    case Dir.Right:
                        spriteSheet.drawSprite(spriteBatch, rectRightEaten, new Vector2(position.X + drawOffSetX, position.Y + drawOffSetY));
                        break;
                }
            }
        }

        // Metod för att uppdatera spökets tillstånd och position.
        public void Update(GameTime gameTime, Controller gameController, Vector2 playerTilePos, Dir playerDir, Vector2 blinkyPos)
        {
            if (state == EnemyState.Frightened)
            {
                timerFrightened += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (timerFrightened > timerFrightenedLength)
                {
                    state = EnemyState.Chase;
                    speed = normalSpeed;
                    timerFrightened = 0;
                }
            }
            updateTilePosition(gameController.tileArray);
            enemyAnim.Update(gameTime);

            decideDirection(playerTilePos, playerDir, gameController, blinkyPos);
            Move(gameTime, gameController.tileArray);
        }

        // Returnerar målpositionen för olika tillstånd (scatter, chase, etc).
        public virtual Vector2 getScatterTargetPosition()
        {
            return ScatterTargetTile;
        }

        public virtual Vector2 getChaseTargetPosition(Vector2 playerTilePos, Dir playerDir, Tile[,] tileArray)
        {
            return playerTilePos;
        }

        public virtual Vector2 getChaseTargetPosition(Vector2 playerTilePos, Dir playerDir, Tile[,] tileArray, Vector2 blinkyPos)
        {
            return playerTilePos;
        }

        public virtual Vector2 getFrightenedTargetPosition()
        {
            List<Dir> dirs = new List<Dir>();

            if (currentTile.Equals(new Vector2(0, 14)) || currentTile.Equals(new Vector2(Controller.numberOfTilesX - 1, 14)))
            {
                return currentTile;
            }

            // Kontrollerar om spöket är i spökhuset, och om det är det, returnerar en tile utanför spökhuset.
            if (Game1.gameController.tileArray[(int)currentTile.X, (int)currentTile.Y].tileType == Tile.TileType.GhostHouse)
                return new Vector2(13, 11);

            // Lägg till möjliga riktningar baserat på om de inte leder till en vägg eller spökhus.
            if (Controller.returnOppositeDir(direction) != Dir.Left && Game1.gameController.isNextTileAvailableGhosts(Dir.Left, currentTile) && Game1.gameController.tileArray[(int)currentTile.X - 1, (int)currentTile.Y].tileType != Tile.TileType.GhostHouse)
                dirs.Add(Dir.Left);
            if (Controller.returnOppositeDir(direction) != Dir.Right && Game1.gameController.isNextTileAvailableGhosts(Dir.Right, currentTile) && Game1.gameController.tileArray[(int)currentTile.X + 1, (int)currentTile.Y].tileType != Tile.TileType.GhostHouse)
                dirs.Add(Dir.Right);
            if (Controller.returnOppositeDir(direction) != Dir.Down && Game1.gameController.isNextTileAvailableGhosts(Dir.Down, currentTile) && Game1.gameController.tileArray[(int)currentTile.X, (int)currentTile.Y + 1].tileType != Tile.TileType.GhostHouse)
                dirs.Add(Dir.Down);
            if (Controller.returnOppositeDir(direction) != Dir.Up && Game1.gameController.isNextTileAvailableGhosts(Dir.Up, currentTile) && Game1.gameController.tileArray[(int)currentTile.X, (int)currentTile.Y - 1].tileType != Tile.TileType.GhostHouse)
                dirs.Add(Dir.Up);

            // Om det finns möjliga riktningar, välj en slumpmässigt.
            if (dirs.Count > 0)
            {
                Random rd = new Random();
                int randDirNum = rd.Next(0, dirs.Count);

                Vector2 rPos = currentTile;

                switch (dirs[randDirNum])
                {
                    case Dir.Left:
                        rPos.X--;
                        break;
                    case Dir.Right:
                        rPos.X++;
                        break;
                    case Dir.Down:
                        rPos.Y++;
                        break;
                    case Dir.Up:
                        rPos.Y--;
                        break;
                }

                return rPos;
            }
            // Om inga riktningar är tillgängliga, returnera den aktuella tile.
            return currentTile;
        }

        // Returnerar målpositionen när spöket har blivit uppätet.
        public virtual Vector2 getEatenTargetPosition()
        {
            return eatenTargetTile;
        }

        // Metod för när spöket blir uppätet.
        public virtual void getEaten()
        {
            switch (Game1.gameController.ghostScoreMultiplier)
            {
                case 1:
                    Game1.score += 200;
                    break;
                case 2:
                    Game1.score += 400;
                    break;
                case 3:
                    Game1.score += 800;
                    break;
                case 4:
                    Game1.score += 1600;
                    break;
            }
            Game1.gameController.ghostScoreMultiplier += 1;
            state = EnemyState.Eaten;
            speed = eatenSpeed;
            timerFrightened = 0;
            MySounds.eat_ghost.Play();
            MySounds.retreatingInstance.Play();
            MySounds.power_pellet_instance.Stop();
            return;
        }
        public void decideDirection(Vector2 playerTilePos, Dir playerDir, Controller gameController, Vector2 blinkyPos)
        {
            // Kontrollera om spöket har nått den nuvarande vägen
            if (!foundpathTile.Equals(currentTile))
            {
                // Baserat på spökets tillstånd, beräkna vägen till måltile
                if (state == EnemyState.Scatter)
                {
                    pathToPacMan = Pathfinding.findPath(currentTile, getScatterTargetPosition(), gameController.tileArray, direction);
                }
                else if (state == EnemyState.Chase)
                {
                    // Om spöket är i Chase-läge, beräkna vägen till spelarens position
                    pathToPacMan = Pathfinding.findPath(currentTile, getChaseTargetPosition(playerTilePos, playerDir, gameController.tileArray), gameController.tileArray, direction);
                    // Specialfall för spöket Inky, som behöver positionen av Blinky för att beräkna sin väg
                    if (type == GhostType.Inky)
                    {
                        pathToPacMan = Pathfinding.findPath(currentTile, getChaseTargetPosition(playerTilePos, playerDir, gameController.tileArray, blinkyPos), gameController.tileArray, direction);
                    }
                }
                else if (state == EnemyState.Frightened)
                {
                    // Om spöket är i Frightened-läge, beräkna en slumpmässig väg
                    pathToPacMan = Pathfinding.findPath(currentTile, getFrightenedTargetPosition(), gameController.tileArray, direction);
                }
                else if (state == EnemyState.Eaten)
                {
                    // Om spöket har blivit uppätet, beräkna vägen tillbaka till spökhuset
                    pathToPacMan = Pathfinding.findPath(currentTile, getEatenTargetPosition(), gameController.tileArray, direction);
                }
                foundpathTile = currentTile;
            }

            // Kontrollera om spöket har nått sitt mål och byt tillstånd tillbaka till Chase om det är i Eaten-läge
            if (currentTile.Equals(getEatenTargetPosition()) && state == EnemyState.Eaten)
            {
                state = EnemyState.Chase;
                speed = normalSpeed;
                MySounds.power_pellet_instance.Play();
            }

            // Om spöket är på spelarens position, hantera kollision beroende på tillstånd
            if (playerTilePos.Equals(currentTile))
            {
                if (state == EnemyState.Frightened)
                {
                    getEaten();
                }
                if (state != EnemyState.Eaten)
                {
                    colliding = true;
                    return;
                }
            }

            // Om det inte finns någon väg till PacMan, returnera
            if (pathToPacMan.Count == 0) { return; }

            // Uppdatera riktningen baserat på nästa tile i vägen till PacMan
            if (pathToPacMan[0].X > currentTile.X)
            {
                direction = Dir.Right;
                position.Y = gameController.tileArray[(int)currentTile.X, (int)currentTile.Y].Position.Y;
            }
            else if (pathToPacMan[0].X < currentTile.X)
            {
                direction = Dir.Left;
                position.Y = gameController.tileArray[(int)currentTile.X, (int)currentTile.Y].Position.Y;
            }
            else if (pathToPacMan[0].Y > currentTile.Y)
            {
                direction = Dir.Down;
                position.X = gameController.tileArray[(int)currentTile.X, (int)currentTile.Y].Position.X;
            }
            else if (pathToPacMan[0].Y < currentTile.Y)
            {
                direction = Dir.Up;
                position.X = gameController.tileArray[(int)currentTile.X, (int)currentTile.Y].Position.X;
            }
        }

        public void Move(GameTime gameTime, Tile[,] tileArray)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Flytta spöket baserat på dess nuvarande riktning och hastighet
            switch (direction)
            {
                case Dir.Right:
                    position.X += speed * dt;
                    // Hantera animering för Frightened-tillstånd
                    if (state == EnemyState.Frightened)
                    {
                        if (timerFrightened > 5)
                        {
                            enemyAnim.setSourceRects(frightenedRectsEnd);
                            break;
                        }
                        enemyAnim.setSourceRects(frightenedRects);
                    }
                    else
                        enemyAnim.setSourceRects(rectsRight);
                    break;

                case Dir.Left:
                    position.X -= speed * dt;
                    // Hantera animering för Frightened-tillstånd
                    if (state == EnemyState.Frightened)
                    {
                        if (timerFrightened > 5)
                        {
                            enemyAnim.setSourceRects(frightenedRectsEnd);
                            break;
                        }
                        enemyAnim.setSourceRects(frightenedRects);
                    }
                    else
                        enemyAnim.setSourceRects(rectsLeft);
                    break;

                case Dir.Down:
                    position.Y += speed * dt;
                    // Hantera animering för Frightened-tillstånd
                    if (state == EnemyState.Frightened)
                    {
                        if (timerFrightened > 5)
                        {
                            enemyAnim.setSourceRects(frightenedRectsEnd);
                            break;
                        }
                        enemyAnim.setSourceRects(frightenedRects);
                    }
                    else
                        enemyAnim.setSourceRects(rectsDown);
                    break;

                case Dir.Up:
                    position.Y -= speed * dt;
                    // Hantera animering för Frightened-tillstånd
                    if (state == EnemyState.Frightened)
                    {
                        if (timerFrightened > 5)
                        {
                            enemyAnim.setSourceRects(frightenedRectsEnd);
                            break;
                        }
                        enemyAnim.setSourceRects(frightenedRects);
                    }
                    else
                        enemyAnim.setSourceRects(rectsUp);
                    break;

                case Dir.None:
                    position = tileArray[(int)currentTile.X, (int)currentTile.Y].Position;
                    break;
            }
        }

        public int checkForTeleportPos(Tile[,] tileArray)
        {
            // Kontrollera om spöket är på en teleporteringsposition och returnera motsvarande teleporteringsnummer
            if (new int[2] { (int)currentTile.X, (int)currentTile.Y }.SequenceEqual(new int[2] { 0, 14 }))
            {
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

        public void teleport(Vector2 pos, Vector2 tilePos)
        {
            // Teleportera spöket till en ny position och uppdatera dess tile-position
            position = pos;
            previousTile = currentTile;
            currentTile = tilePos;
        }

        public void updateTilePosition(Tile[,] tileArray)
        {
            // Hantera teleportering om spöket är på en teleporteringsposition
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

            // Uppdatera spökets nuvarande tile baserat på dess position
            for (int x = 0; x < tileArray.GetLength(0); x++)
            {
                for (int y = 0; y < tileArray.GetLength(1); y++)
                {
                    float tilePosX = tileArray[x, y].Position.X;
                    float tilePosY = tileArray[x, y].Position.Y;

                    float nextTilePosX = tileArray[x, y].Position.X + Controller.tileWidth;
                    float nextTilePosY = tileArray[x, y].Position.Y + Controller.tileHeight;

                    float posOffSetX = 10;
                    float posOffSetY = 10;

                    // Justera nästa tile position baserat på riktningen
                    switch (direction)
                    {
                        case Dir.Right:
                            nextTilePosX = tileArray[x, y].Position.X + Controller.tileWidth;
                            break;
                        case Dir.Left:
                            nextTilePosX = tileArray[x, y].Position.X - Controller.tileWidth;
                            posOffSetX *= -1;
                            break;
                        case Dir.Down:
                            nextTilePosY = tileArray[x, y].Position.Y + Controller.tileHeight;
                            break;
                        case Dir.Up:
                            nextTilePosY = tileArray[x, y].Position.Y - Controller.tileHeight;
                            posOffSetY *= -1;
                            break;
                    }

                    float posX = position.X + posOffSetX;
                    float posY = position.Y + posOffSetY;

                    // Kontrollera om spöket är på en ny tile och uppdatera tile-positionen
                    if (direction == Dir.Right || direction == Dir.Down)
                    {
                        if (posX >= tilePosX && posX < nextTilePosX)
                        {
                            if (posY >= tilePosY && posY < nextTilePosY)
                            {
                                previousTile = currentTile;
                                tileArray[(int)previousTile.X, (int)previousTile.Y].tileType = previousTileType;
                                currentTile = new Vector2(x, y);
                                previousTileType = tileArray[(int)currentTile.X, (int)currentTile.Y].tileType;
                                tileArray[(int)currentTile.X, (int)currentTile.Y].tileType = Tile.TileType.Ghost;
                            }
                        }
                    }
                    else if (direction == Dir.Left)
                    {
                        if (posX <= tilePosX && posX > nextTilePosX)
                        {
                            if (posY >= tilePosY && posY < nextTilePosY)
                            {
                                previousTile = currentTile;
                                tileArray[(int)previousTile.X, (int)previousTile.Y].tileType = previousTileType;
                                currentTile = new Vector2(x, y);
                                previousTileType = tileArray[(int)currentTile.X, (int)currentTile.Y].tileType;
                                tileArray[(int)currentTile.X, (int)currentTile.Y].tileType = Tile.TileType.Ghost;
                            }
                        }
                    }
                    else if (direction == Dir.Up)
                    {
                        if (posX >= tilePosX && posX < nextTilePosX)
                        {
                            if (posY <= tilePosY && posY > nextTilePosY)
                            {
                                previousTile = currentTile;
                                tileArray[(int)previousTile.X, (int)previousTile.Y].tileType = previousTileType;
                                currentTile = new Vector2(x, y);
                                previousTileType = tileArray[(int)currentTile.X, (int)currentTile.Y].tileType;
                                tileArray[(int)currentTile.X, (int)currentTile.Y].tileType = Tile.TileType.Ghost;
                            }
                        }
                    }
                }
            }
        }
    }
}