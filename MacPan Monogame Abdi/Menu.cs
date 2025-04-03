using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

// Definiera en statisk klass för menyn i spelet Pacman.
namespace Pacman
{
    // Deklarera en statisk klass som hanterar menyn i spelet.
    public static class Menu
    {
        // Deklara en privat statisk Texture2D för att lagra bilddata för menybilden.
        private static Texture2D blob;

        // Deklara en privat statisk rektangel för att positionera menybilden på skärmen.
        private static Rectangle blobPos = new Rectangle(0, 0, 702, 774);

        // Deklara en privat statisk SpriteFont för att hantera textrendering i menyn.
        private static SpriteFont basicFont;

        // Exponera en egenskap för att sätta den statiska SpriteFont-variabeln.
        public static SpriteFont setBasicFont
        {
            set { basicFont = value; }
        }

        // Exponera en egenskap för att sätta den statiska Texture2D-variabeln för menybilden.
        public static Texture2D setblob
        {
            set { blob = value; }
        }

        // Metod för att uppdatera menyn baserat på användartillfällen.
        public static void Update(GameTime gameTime)
        {
            // Hämta den aktuella tillståndet för tangentbordet.
            KeyboardState kState = Keyboard.GetState();

            // Kontrollera om mellanslagstangenten är nedtryckt.
            if (kState.IsKeyDown(Keys.Space))
            {
                // Återställ spelkontrollerns tillstånd till normalt.
                Game1.gameController.gameState = Controller.GameState.Normal;

                // Spela upp startljudet igen.
                MySounds.game_start.Play();
            }
        }

        // Metod för att rita menybilden på skärmen.
        public static void Draw(SpriteBatch spriteBatch)
        {
            // Ritra menybilden på dess definierade position med vit färg.
            spriteBatch.Draw(blob, blobPos, Color.White);
        }
    }
}
