using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
    // Klassen SpriteAnimation hanterar animeringar för sprites i spelet.
    public class SpriteAnimation
    {
        // Timer för att kontrollera när nästa frame ska visas.
        private float timer = 0;
        // Tröskelvärdet för att bestämma när nästa frame ska bytas.
        private float threshold;
        // Array med rektanglar som definierar varje frame i animeringen.
        private Rectangle[] sourceRectangles;
        // Index för att hålla reda på vilken frame som ska visas nu.
        private int animationIndex = 0;
        // Anger om animeringen ska loopa eller bara spelas en gång.
        private bool isLooped = true;
        // Anger om animeringen är igång eller pausad.
        private bool isPlaying;

        // Egenskap för att få indexet på den aktiva animeringen.
        public int AnimationIndex
        {
            get { return animationIndex; }
        }

        // Egenskap för att få om animeringen är igång.
        public bool IsPlaying
        {
            get { return isPlaying; }
            set { isPlaying = value; }
        }

        // Konstruktor för att skapa en ny animering med en tröskel och en array av rektanglar.
        public SpriteAnimation(float newThreshold, Rectangle[] newSourceRectangles)
        {
            threshold = newThreshold;
            sourceRectangles = newSourceRectangles;
            isPlaying = true;
        }

        // Alternativ konstruktor som också låter dig ange vilken frame som ska vara aktiv från början.
        public SpriteAnimation(float newThreshold, Rectangle[] newSourceRectangles, int startingAnimIndex)
        {
            threshold = newThreshold;
            sourceRectangles = newSourceRectangles;
            animationIndex = startingAnimIndex;
            isPlaying = true;
        }

        // Ytterligare alternativ konstruktor som även låter dig ange om animeringen ska loopa och om den ska vara igång från början.
        public SpriteAnimation(float newThreshold, Rectangle[] newSourceRectangles, int startingAnimIndex, bool newIsLooped, bool newIsPlaying)
        {
            threshold = newThreshold;
            sourceRectangles = newSourceRectangles;
            animationIndex = startingAnimIndex;
            isLooped = newIsLooped;
            isPlaying = newIsPlaying;
        }

        // Metod för att manuellt sätta vilken frame som ska vara aktiv.
        public void setAnimIndex(int newAnimIndex)
        {
            animationIndex = newAnimIndex;
        }

        // Metod för att byta ut de underliggande rektanglarna som definierar frames i animeringen.
        public void setSourceRects(Rectangle[] newSourceRects)
        {
            if (newSourceRects.Length != sourceRectangles.Length)
                animationIndex = 0; // Återställer animeringen om antalet frames har ändrats.
            sourceRectangles = newSourceRects;
        }

        // Startar animeringen.
        public void start()
        {
            isPlaying = true;
            animationIndex = 0;
        }

        // Egenskap för att få arrayen med rektangler som definierar frames i animeringen.
        public Rectangle[] SourceRectangles
        {
            get { return sourceRectangles; }
        }

        // Metod som uppdaterar animeringen baserat på tiden som gått sedan sist.
        public void Update(GameTime gameTime)
        {
            if (isLooped)
            {
                timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (timer > threshold)
                {
                    timer -= threshold;
                    if (animationIndex < sourceRectangles.Length - 1)
                    {
                        animationIndex++; // Går till nästa frame.
                    }
                    else
                    {
                        animationIndex = 0; // Återställer till början av animeringen.
                    }
                }
            }
            // Om animeringen inte ska loopa, spelas den bara en gång och stoppas sedan.
            else
            {
                if (isPlaying)
                {
                    timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (timer > threshold)
                    {
                        timer -= threshold;
                        if (animationIndex < sourceRectangles.Length - 1)
                        {
                            animationIndex++; // Går till nästa frame.
                        }
                        else
                        {
                            isPlaying = false; // Stoppar animeringen.
                        }
                    }
                }
            }
        }

        // Metod för att teckna animeringsramverket på skärmen.
        public void Draw(SpriteBatch spriteBatch, SpriteSheet spriteSheet, Vector2 position)
        {
            if (isPlaying)
                spriteSheet.drawSprite(spriteBatch, sourceRectangles[animationIndex], position); // Tecknar det aktuella ramen.
        }
    }
}
