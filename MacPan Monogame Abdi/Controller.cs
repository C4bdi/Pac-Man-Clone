using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
    public class Controller
    {
        // Definierar kartdesignen med en tvådimensionell array. Varje siffra representerar en typ av spelbricka.
        private int[,] mapDesign = new int[,]{
            { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            { 1,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,1},
            { 1,0,1,1,1,1,0,1,1,1,1,1,0,1,1,0,1,1,1,1,1,0,1,1,1,1,0,1},
            { 1,3,1,1,1,1,0,1,1,1,1,1,0,1,1,0,1,1,1,1,1,0,1,1,1,1,3,1},
            { 1,0,1,1,1,1,0,1,1,1,1,1,0,1,1,0,1,1,1,1,1,0,1,1,1,1,0,1},
            { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            { 1,0,1,1,1,1,0,1,1,0,1,1,1,1,1,1,1,1,0,1,1,0,1,1,1,1,0,1},
            { 1,0,1,1,1,1,0,1,1,0,1,1,1,1,1,1,1,1,0,1,1,0,1,1,1,1,0,1},
            { 1,0,0,0,0,0,0,1,1,0,0,0,0,1,1,0,0,0,0,1,1,0,0,0,0,0,0,1},
            { 1,1,1,1,1,1,0,1,1,1,1,1,5,1,1,5,1,1,1,1,1,0,1,1,1,1,1,1},
            { 1,1,1,1,1,1,0,1,1,1,1,1,5,1,1,5,1,1,1,1,1,0,1,1,1,1,1,1},
            { 1,1,1,1,1,1,0,1,1,5,5,5,5,5,5,5,5,5,5,1,1,0,1,1,1,1,1,1},
            { 1,1,1,1,1,1,0,1,1,5,1,1,1,2,2,1,1,1,5,1,1,0,1,1,1,1,1,1},
            { 1,1,1,1,1,1,0,1,1,5,1,2,2,2,2,2,2,1,5,1,1,0,1,1,1,1,1,1},
            { 0,0,0,0,0,0,0,5,5,5,1,2,2,2,2,2,2,1,5,5,5,0,0,0,0,0,0,0},
            { 1,1,1,1,1,1,0,1,1,5,1,2,2,2,2,2,2,1,5,1,1,0,1,1,1,1,1,1},
            { 1,1,1,1,1,1,0,1,1,5,1,1,1,1,1,1,1,1,5,1,1,0,1,1,1,1,1,1},
            { 1,1,1,1,1,1,0,1,1,5,5,5,5,5,5,5,5,5,5,1,1,0,1,1,1,1,1,1},
            { 1,1,1,1,1,1,0,1,1,5,1,1,1,1,1,1,1,1,5,1,1,0,1,1,1,1,1,1},
            { 1,1,1,1,1,1,0,1,1,5,1,1,1,1,1,1,1,1,5,1,1,0,1,1,1,1,1,1},
            { 1,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,1},
            { 1,0,1,1,1,1,0,1,1,1,1,1,0,1,1,0,1,1,1,1,1,0,1,1,1,1,0,1},
            { 1,0,1,1,1,1,0,1,1,1,1,1,0,1,1,0,1,1,1,1,1,0,1,1,1,1,0,1},
            { 1,3,0,0,1,1,0,0,0,0,0,0,0,5,5,0,0,0,0,0,0,0,1,1,0,0,3,1},
            { 1,1,1,0,1,1,0,1,1,0,1,1,1,1,1,1,1,1,0,1,1,0,1,1,0,1,1,1},
            { 1,1,1,0,1,1,0,1,1,0,1,1,1,1,1,1,1,1,0,1,1,0,1,1,0,1,1,1},
            { 1,0,0,0,0,0,0,1,1,0,0,0,0,1,1,0,0,0,0,1,1,0,0,0,0,0,0,1},
            { 1,0,1,1,1,1,1,1,1,1,1,1,0,1,1,0,1,1,1,1,1,1,1,1,1,1,0,1},
            { 1,0,1,1,1,1,1,1,1,1,1,1,0,1,1,0,1,1,1,1,1,1,1,1,1,1,0,1},
            { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
        };

        // Definierar olika tillstånd som spelet kan vara i.
        public enum GameState { Normal, GameOver, Menu };
        public GameState gameState = GameState.Menu; // Initialt tillstånd är Meny.

        public static int numberOfTilesY; // Antal brickor i Y-led.
        public static int numberOfTilesX; // Antal brickor i X-led.
        public static int tileWidth; // Bredd på en spelbricka.
        public static int tileHeight; // Höjd på en spelbricka.
        public Tile[,] tileArray; // Array som innehåller alla spelbrickor.
        public List<Snack> snackList = new List<Snack>();
        // Lista för att lagra alla snacks (små och stora) i spelet.

        public Enemy.EnemyState enemiesState = Enemy.EnemyState.Scatter;
        // Variabel för att hålla reda på tillståndet för fienderna (t.ex. scatter-läge).

        public float ghostInitialTimer;
        // Timer för initialt spöktillstånd.

        public float ghostInitialTimerLength = 2f;
        // Standardlängd för den initiala spöktimern.

        public float ghostTimerScatter;
        // Timer för scatter-läget för spökena.

        public float ghostTimerScatterLength = 15f;
        // Standardlängd för scatter-timern.

        public float ghostTimerChaser;
        // Timer för chaser-läget för spökena.

        public float ghostTimerChaserLength = 20f;
        // Standardlängd för chaser-timern.

        public bool eatenBigSnack = false;
        // Flagga för att indikera om en stor snack har blivit uppäten.

        public bool startPacmanDeathAnim = false;
        // Flagga för att indikera om Pacmans dödsanimation ska starta.

        public Vector2 pacmanDeathPosition;
        // Positionen där Pacman dog.

        public int ghostScoreMultiplier = 1;
        // Multiplikator för poäng när spöken äts.

        public Controller()
        {
            numberOfTilesX = 28;
            numberOfTilesY = 31;
            tileWidth = Game1.windowWidth / numberOfTilesX;
            tileHeight = (Game1.windowHeight - Game1.scoreOffSet) / numberOfTilesY;
            tileArray = new Tile[numberOfTilesX, numberOfTilesY];
            // Initialiserar antalet tiles på X- och Y-axeln samt deras bredd och höjd.
            // Skapar en 2D-array av Tile-objekt för att representera spelbrädet.
        }

        public void createGrid() // Skapar ett rutnät med Tile-objekt som representerar 24x24 pixlars kvadrater i spelet, alla med olika typer som väggar, snacks osv.
        {
            for (int y = 0; y < numberOfTilesY; y++)
            {
                for (int x = 0; x < numberOfTilesX; x++)
                {
                    if (mapDesign[y, x] == 0) // liten snack
                    {
                        tileArray[x, y] = new Tile(new Vector2(x * tileWidth, y * tileHeight + Game1.scoreOffSet), Tile.TileType.Snack);
                        tileArray[x, y].isEmpty = false;
                        snackList.Add(new Snack(Snack.SnackType.Small, new Vector2(x * tileWidth, y * tileHeight + Game1.scoreOffSet), new int[] { x, y }));
                    }
                    else if (mapDesign[y, x] == 1) // väggkolliderare
                    {
                        tileArray[x, y] = new Tile(new Vector2(x * tileWidth, y * tileHeight + Game1.scoreOffSet), Tile.TileType.Wall);
                        tileArray[x, y].isEmpty = false;
                    }
                    else if (mapDesign[y, x] == 2) // spökhus
                    {
                        tileArray[x, y] = new Tile(new Vector2(x * tileWidth, y * tileHeight + Game1.scoreOffSet), Tile.TileType.GhostHouse);
                        tileArray[x, y].isEmpty = false;
                    }
                    else if (mapDesign[y, x] == 3) // stor snack
                    {
                        tileArray[x, y] = new Tile(new Vector2(x * tileWidth, y * tileHeight + Game1.scoreOffSet), Tile.TileType.Snack);
                        tileArray[x, y].isEmpty = false;
                        snackList.Add(new Snack(Snack.SnackType.Big, new Vector2(x * tileWidth, y * tileHeight + Game1.scoreOffSet), new int[] { x, y }));
                    }
                    else if (mapDesign[y, x] == 5) // tom
                    {
                        tileArray[x, y] = new Tile(new Vector2(x * tileWidth, y * tileHeight + Game1.scoreOffSet));
                    }
                }
            }
            // Loopar igenom varje tile på spelbrädet och tilldelar rätt typ och position beroende på mapDesign-värdet.
        }

        public void createSnacks() // Skapar snacks på nytt när spelaren har ätit alla snacks på skärmen.
        {
            for (int y = 0; y < numberOfTilesY; y++)
            {
                for (int x = 0; x < numberOfTilesX; x++)
                {
                    if (mapDesign[y, x] == 0) // liten snack
                    {
                        tileArray[x, y] = new Tile(new Vector2(x * tileWidth, y * tileHeight + Game1.scoreOffSet), Tile.TileType.Snack);
                        tileArray[x, y].isEmpty = false;
                        snackList.Add(new Snack(Snack.SnackType.Small, new Vector2(x * tileWidth, y * tileHeight + Game1.scoreOffSet), new int[] { x, y }));
                    }
                    else if (mapDesign[y, x] == 3) // stor snack
                    {
                        tileArray[x, y] = new Tile(new Vector2(x * tileWidth, y * tileHeight + Game1.scoreOffSet), Tile.TileType.Snack);
                        tileArray[x, y].isEmpty = false;
                        snackList.Add(new Snack(Snack.SnackType.Big, new Vector2(x * tileWidth, y * tileHeight + Game1.scoreOffSet), new int[] { x, y }));
                    }
                }
            }
            // Loopar igenom varje tile och återskapar små och stora snacks baserat på mapDesign-värdena.
        }

        public void win(Inky i, Blinky b, Pinky p, Clyde c, Player pacman)
        {
            createSnacks();
            resetGhosts(i, b, p, c);

            ghostTimerChaser = 0;
            ghostTimerScatter = 0;
            ghostInitialTimer = 0;

            eatenBigSnack = false;

            pacman.Position = new Vector2(tileArray[13, 23].Position.X + 14, tileArray[13, 23].Position.Y);
            pacman.CurrentTile = new Vector2(13, 23);
            pacman.PlayerAnim.setSourceRects(Player.rectsRight);
            pacman.PlayerAnim.setAnimIndex(2);
            pacman.Direction = Dir.Right;

            MySounds.munchInstance.Stop();
            MySounds.power_pellet_instance.Stop();
            MySounds.retreatingInstance.Stop();
            // Återställer spelet till ett nytt tillstånd när spelaren vinner.
            // Skapar snacks på nytt, återställer spökena, och återställer olika timers och tillstånd.
            // Flyttar Pacman till startpositionen och stoppar ljud som spelar.
        }
        public void gameOver(Inky i, Blinky b, Pinky p, Clyde c, Player pacman)
        {
            gameState = GameState.GameOver;
            // Sätt spelets tillstånd till "Game Over".

            Game1.hasPassedInitialSong = false;
            // Återställ flaggan för initial sång.

            Game1.score = 0;
            // Återställ poängen till 0.

            Game1.pacmanDeathAnimation.IsPlaying = false;
            // Stäng av Pacmans dödsanimation.

            Game1.gamePauseTimer = Game1.gameStartSongLength;
            // Återställ paustimer för spelet.

            pacman.ExtraLives = 3;
            // Återställ Pacmans extra liv till 3.

            createSnacks();
            // Återskapa snacks på spelplanen.

            resetGhosts(i, b, p, c);
            // Återställ alla spöken.

            ghostTimerChaser = 0;
            ghostTimerScatter = 0;
            ghostInitialTimer = 0;
            // Återställ alla spöktimrar.

            eatenBigSnack = false;
            // Återställ flaggan för stora snacks.

            pacman.Position = new Vector2(tileArray[13, 23].Position.X + 14, tileArray[13, 23].Position.Y);
            pacman.CurrentTile = new Vector2(13, 23);
            pacman.PlayerAnim.setSourceRects(Player.rectsRight);
            pacman.PlayerAnim.setAnimIndex(2);
            pacman.Direction = Dir.Right;
            // Återställ Pacmans position, aktuella tile, animation och riktning.

            MySounds.munchInstance.Stop();
            MySounds.power_pellet_instance.Stop();
            MySounds.retreatingInstance.Stop();
            // Stoppa alla spelande ljud.
        }

        public void killPacman(Inky i, Blinky b, Pinky p, Clyde c, Player pacman)
        {
            pacman.ExtraLives -= 1;
            // Minska Pacmans extra liv med 1.

            startPacmanDeathAnim = true;
            // Starta Pacmans dödsanimation.

            pacmanDeathPosition = new Vector2(pacman.Position.X - Player.radiusOffSet / 2, pacman.Position.Y - Player.radiusOffSet / 2 + 1);
            // Sätt Pacmans dödsposition.

            MySounds.death_1.Play(); // Length = 2.78
                                     // Spela upp dödsljudet.

            Game1.gamePauseTimer = 4f;
            // Sätt paustimer för spelet.

            resetGhosts(i, b, p, c);
            // Återställ alla spöken.

            ghostTimerChaser = 0;
            ghostTimerScatter = 0;
            ghostInitialTimer = 0;
            // Återställ alla spöktimrar.

            eatenBigSnack = false;
            // Återställ flaggan för stora snacks.

            pacman.Position = new Vector2(tileArray[13, 23].Position.X + 14, tileArray[13, 23].Position.Y);
            pacman.CurrentTile = new Vector2(13, 23);
            pacman.PlayerAnim.setSourceRects(Player.rectsRight);
            pacman.PlayerAnim.setAnimIndex(2);
            pacman.Direction = Dir.Right;
            // Återställ Pacmans position, aktuella tile, animation och riktning.

            MySounds.munchInstance.Stop();
            MySounds.power_pellet_instance.Stop();
            MySounds.retreatingInstance.Stop();
            // Stoppa alla spelande ljud.
        }

        public void drawGridDebugger(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < numberOfTilesX; x++)
            {
                for (int y = 0; y < numberOfTilesY; y++)
                {
                    Vector2 dotPosition = tileArray[x, y].Position;
                    spriteBatch.Draw(Game1.debugLineX, dotPosition, Color.White);
                    spriteBatch.Draw(Game1.debugLineY, dotPosition, Color.White);
                    // Rita gridlinjer för felsökning vid varje tile-position.
                }
            }
        }

        public void drawGhosts(Inky i, Blinky b, Pinky p, Clyde c, SpriteBatch spriteBatch, SpriteSheet spriteSheet)
        {
            i.Draw(spriteBatch, spriteSheet);
            b.Draw(spriteBatch, spriteSheet);
            p.Draw(spriteBatch, spriteSheet);
            c.Draw(spriteBatch, spriteSheet);
            // Rita varje spöke på skärmen.
        }

        public void updateGhosts(Inky i, Blinky b, Pinky p, Clyde c, GameTime gameTime, Player Pacman, Vector2 blinkyPos)
        {
            if (eatenBigSnack)
            {
                eatenBigSnack = false;
                setGhostStates(i, b, p, c, Enemy.EnemyState.Frightened);
                MySounds.power_pellet_instance.Play();
                // Sätt spökena till "Frightened"-läge och spela upp ljud om en stor snack har ätits.
            }

            if (i.state != Enemy.EnemyState.Frightened && b.state != Enemy.EnemyState.Frightened && p.state != Enemy.EnemyState.Frightened && c.state != Enemy.EnemyState.Frightened)
            {
                MySounds.power_pellet_instance.Stop();
                ghostScoreMultiplier = 1;
                // Stoppa ljudet för "Frightened"-läge och återställ poängmultiplikatorn om inget spöke är skrämt.
            }

            if (i.state != Enemy.EnemyState.Eaten && b.state != Enemy.EnemyState.Eaten && p.state != Enemy.EnemyState.Eaten && c.state != Enemy.EnemyState.Eaten)
                MySounds.retreatingInstance.Stop();
            // Stoppa ljudet för återdragning om inget spöke har ätits.

            if (ghostInitialTimer < ghostInitialTimerLength)
            {
                ghostInitialTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                c.EnemyAnim.Update(gameTime);
                i.EnemyAnim.Update(gameTime);
                // Uppdatera initialtimer och animationer för spökena.
            }
            if (ghostInitialTimer > ghostInitialTimerLength / 2 && ghostInitialTimer < ghostInitialTimerLength)
            {
                i.Update(gameTime, this, Pacman.CurrentTile, Pacman.Direction, blinkyPos);
                // Uppdatera Inky efter halva initialtimer.
            }
            else if (ghostInitialTimer > ghostInitialTimerLength) // När initialtimern är slut, starta timrar för scatter och chaser-lägen.
            {
                c.Update(gameTime, this, Pacman.CurrentTile, Pacman.Direction, blinkyPos);
                i.Update(gameTime, this, Pacman.CurrentTile, Pacman.Direction, blinkyPos);
                switchBetweenStates(i, b, p, c, gameTime);
                // Uppdatera Clyde och Inky och byt tillstånd mellan scatter och chaser.
            }

            p.Update(gameTime, this, Pacman.CurrentTile, Pacman.Direction, blinkyPos);
            b.Update(gameTime, this, Pacman.CurrentTile, Pacman.Direction, blinkyPos);
            // Uppdatera Pinky och Blinky.

            if (i.colliding == true || b.colliding == true || p.colliding == true || c.colliding == true)
            {
                killPacman(i, b, p, c, Pacman);
                i.colliding = false;
                b.colliding = false;
                p.colliding = false;
                c.colliding = false;
                // Döda Pacman om någon av spökena kolliderar med honom och återställ kollisionsflaggan.
            }
        }

        public void switchBetweenStates(Inky i, Blinky b, Pinky p, Clyde c, GameTime gameTime)
        {
            if (i.state == Enemy.EnemyState.Frightened || i.state == Enemy.EnemyState.Eaten ||
                b.state == Enemy.EnemyState.Frightened || b.state == Enemy.EnemyState.Eaten ||
                c.state == Enemy.EnemyState.Frightened || c.state == Enemy.EnemyState.Eaten ||
                p.state == Enemy.EnemyState.Frightened || p.state == Enemy.EnemyState.Eaten)
                return;
            // Om något spöke är skrämt eller äts, byt inte tillstånd.

            if (enemiesState == Enemy.EnemyState.Scatter)
            {
                ghostTimerScatter += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (ghostTimerScatter > ghostTimerScatterLength)
                {
                    ghostTimerScatter = 0;
                    enemiesState = Enemy.EnemyState.Chase;
                    setGhostStates(i, b, p, c, Enemy.EnemyState.Chase);
                    // Byt tillstånd till chaser efter scatter-timern.
                }
            }
            else if (enemiesState == Enemy.EnemyState.Chase)
            {
                ghostTimerChaser += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (ghostTimerChaser > ghostTimerChaserLength)
                {
                    ghostTimerChaser = 0;
                    enemiesState = Enemy.EnemyState.Scatter;
                    setGhostStates(i, b, p, c, Enemy.EnemyState.Scatter);
                    // Byt tillstånd till scatter efter chaser-timern.
                }
            }
        }

        public void setGhostStates(Inky i, Blinky b, Pinky p, Clyde c, Enemy.EnemyState eState)
        {
            if (eState == Enemy.EnemyState.Chase || eState == Enemy.EnemyState.Scatter || eState == Enemy.EnemyState.Eaten)
            {
                i.speed = i.normalSpeed;
                b.speed = b.normalSpeed;
                p.speed = p.normalSpeed;
                c.speed = c.normalSpeed;
                // Sätt normal hastighet för spöken om de är i chase, scatter eller eaten-läge.
            }
            else
            {
                if (i.state != Enemy.EnemyState.Eaten)
                    i.speed = i.frightenedSpeed;
                if (b.state != Enemy.EnemyState.Eaten)
                    b.speed = b.frightenedSpeed;
                if (p.state != Enemy.EnemyState.Eaten)
                    p.speed = p.frightenedSpeed;
                if (c.state != Enemy.EnemyState.Eaten)
                    c.speed = c.frightenedSpeed;
                // Sätt frightened hastighet för spöken om de inte är i eaten-läge.
            }
            if (i.state != Enemy.EnemyState.Eaten)
                i.state = eState;
            if (b.state != Enemy.EnemyState.Eaten)
                b.state = eState;
            if (p.state != Enemy.EnemyState.Eaten)
                p.state = eState;
            if (c.state != Enemy.EnemyState.Eaten)
                c.state = eState;
            // Sätt det nya tillståndet för alla spöken om de inte är i eaten-läge.
        }

        public void drawPacmanGridDebugger(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < numberOfTilesX; x++)
            {
                for (int y = 0; y < numberOfTilesY; y++)
                {
                    Vector2 dotPosition = tileArray[x, y].Position;
                    if (tileArray[x, y].tileType == Tile.TileType.Player)
                    {
                        spriteBatch.Draw(Game1.playerDebugLineX, dotPosition, Color.White);
                        spriteBatch.Draw(Game1.playerDebugLineY, dotPosition, Color.White);
                        spriteBatch.Draw(Game1.playerDebugLineX, new Vector2(dotPosition.X, dotPosition.Y + 24), Color.White);
                        spriteBatch.Draw(Game1.playerDebugLineY, new Vector2(dotPosition.X + 24, dotPosition.Y), Color.White);
                        // Rita gridlinjer för felsökning runt Pacmans position.
                    }
                }
            }
        }

        public void resetGhosts(Inky i, Blinky b, Pinky p, Clyde c)
        {
            ghostInitialTimer = 0;
            // Återställ initialtimern för spöken.

            setGhostStates(i, b, p, c, Enemy.EnemyState.Scatter);
            // Sätt alla spöken till scatter-läge.

            i.EnemyAnim.setSourceRects(i.rectsUp);
            b.EnemyAnim.setSourceRects(b.rectsLeft);
            p.EnemyAnim.setSourceRects(p.rectsDown);
            c.EnemyAnim.setSourceRects(c.rectsUp);
            // Återställ animationer för alla spöken.

            i.timerFrightened = 0;
            b.timerFrightened = 0;
            p.timerFrightened = 0;
            c.timerFrightened = 0;
            // Återställ frightened-timrar för alla spöken.

            i.PathToPacMan = new List<Vector2>();
            b.PathToPacMan = new List<Vector2>();
            p.PathToPacMan = new List<Vector2>();
            c.PathToPacMan = new List<Vector2>();
            // Återställ vägar till Pacman för alla spöken.

            i.Position = new Vector2(tileArray[11, 14].Position.X + 12, tileArray[11, 14].Position.Y);
            b.Position = new Vector2(tileArray[13, 11].Position.X + 12, tileArray[13, 11].Position.Y);
            p.Position = new Vector2(tileArray[13, 14].Position.X + 12, tileArray[13, 14].Position.Y);
            c.Position = new Vector2(tileArray[15, 14].Position.X + 12, tileArray[15, 14].Position.Y);
            // Återställ positioner för alla spöken.
        }

        public void drawPathFindingDebugger(SpriteBatch spriteBatch, List<Vector2> path)
        {
            if (path == null) return;
            // Om ingen väg finns, avbryt funktionen.

            foreach (Vector2 gridPos in path)
            {
                Vector2 pos = tileArray[(int)gridPos.X, (int)gridPos.Y].Position;
                spriteBatch.Draw(Game1.pathfindingDebugLineX, pos, Color.White);
                spriteBatch.Draw(Game1.pathfindingDebugLineY, pos, Color.White);
                spriteBatch.Draw(Game1.pathfindingDebugLineX, new Vector2(pos.X, pos.Y + 24), Color.White);
                spriteBatch.Draw(Game1.pathfindingDebugLineY, new Vector2(pos.X + 24, pos.Y), Color.White);
                // Rita vägledningslinjer för varje position i vägen.
            }
        }

        public bool isNextTileAvailable(Dir dir, Vector2 tile)
        {
            if (tile.Equals(new Vector2(0, 14)) || tile.Equals(new Vector2(numberOfTilesX - 1, 14)))
            {
                if (dir == Dir.Right || dir == Dir.Left)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                // Om tile är vid vänstra eller högra kanten och riktningen är höger eller vänster, returnera true.
            }
            else
            {
                switch (dir)
                {
                    case Dir.Right:
                        if (tileArray[(int)tile.X + 1, (int)tile.Y].tileType == Tile.TileType.Wall || tileArray[(int)tile.X + 1, (int)tile.Y].tileType == Tile.TileType.GhostHouse)
                        {
                            return false;
                        }
                        break;
                    case Dir.Left:
                        if (tileArray[(int)tile.X - 1, (int)tile.Y].tileType == Tile.TileType.Wall || tileArray[(int)tile.X - 1, (int)tile.Y].tileType == Tile.TileType.GhostHouse)
                        {
                            return false;
                        }
                        break;
                    case Dir.Down:
                        if (tileArray[(int)tile.X, (int)tile.Y + 1].tileType == Tile.TileType.Wall || tileArray[(int)tile.X, (int)tile.Y + 1].tileType == Tile.TileType.GhostHouse)
                        {
                            return false;
                        }
                        break;
                    case Dir.Up:
                        if (tileArray[(int)tile.X, (int)tile.Y - 1].tileType == Tile.TileType.Wall || tileArray[(int)tile.X, (int)tile.Y - 1].tileType == Tile.TileType.GhostHouse)
                        {
                            return false;
                        }
                        break;
                }
                return true;
                // Kontrollera om nästa tile i given riktning är tillgänglig.
            }
        }

        public bool isNextTileAvailableGhosts(Dir dir, Vector2 tile)
        {
            // Kontrollera om den aktuella tile är vid kanten av spelplanen.
            if (tile.Equals(new Vector2(0, 14)) || tile.Equals(new Vector2(numberOfTilesX - 1, 14)))
            {
                // Om riktningen är höger eller vänster, tillåt rörelse.
                if (dir == Dir.Right || dir == Dir.Left)
                {
                    return true;
                }
                else
                {
                    // Om riktningen är upp eller ner, tillåt inte rörelse.
                    return false;
                }
            }
            else
            {
                // Kontrollera tile-typen beroende på riktningen.
                switch (dir)
                {
                    case Dir.Right:
                        // Om nästa tile till höger är en vägg, tillåt inte rörelse.
                        if (tileArray[(int)tile.X + 1, (int)tile.Y].tileType == Tile.TileType.Wall)
                        {
                            return false;
                        }
                        break;
                    case Dir.Left:
                        // Om nästa tile till vänster är en vägg, tillåt inte rörelse.
                        if (tileArray[(int)tile.X - 1, (int)tile.Y].tileType == Tile.TileType.Wall)
                        {
                            return false;
                        }
                        break;
                    case Dir.Down:
                        // Om nästa tile nedåt är en vägg, tillåt inte rörelse.
                        if (tileArray[(int)tile.X, (int)tile.Y + 1].tileType == Tile.TileType.Wall)
                        {
                            return false;
                        }
                        break;
                    case Dir.Up:
                        // Om nästa tile uppåt är en vägg, tillåt inte rörelse.
                        if (tileArray[(int)tile.X, (int)tile.Y - 1].tileType == Tile.TileType.Wall)
                        {
                            return false;
                        }
                        break;
                }
                // Om ingen av de ovanstående villkoren uppfylls, tillåt rörelse.
                return true;
            }
        }

        public static Dir returnOppositeDir(Dir dir)
        {
            // Returnera motsatt riktning baserat på den aktuella riktningen.
            switch (dir)
            {
                case Dir.Up:
                    return Dir.Down;
                case Dir.Down:
                    return Dir.Up;
                case Dir.Right:
                    return Dir.Left;
                case Dir.Left:
                    return Dir.Right;
            }
            // Om ingen matchning, returnera ingen riktning.
            return Dir.None;
        }

        public int findSnackListPosition(Vector2 snackGridPos)
        {
            int listPosition = -1;
            // Hitta positionen för ett snack i listan baserat på dess grid-position.
            foreach (Snack snack in snackList)
            {
                if (snack.Position == snackGridPos)
                {
                    listPosition = snackList.IndexOf(snack);
                }
            }
            // Returnera positionen för snacken i listan.
            return listPosition;
        }

        public bool checkTileType(Vector2 gridIndex, Tile.TileType tileType)
        {
            // Kontrollera om tile-typen vid den angivna grid-positionen matchar den givna typen.
            bool tile = false;
            if (tileArray[(int)gridIndex.X, (int)gridIndex.Y].tileType == tileType)
            {
                tile = true;
            }
            // Returnera resultatet av kontrollen.
            return tile;
        }
    }
}