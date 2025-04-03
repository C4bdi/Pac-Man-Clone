
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Reflection.Metadata;
using static System.Reflection.Metadata.BlobBuilder;
using Pacman;

namespace Pacman
{
    // GameOver-klassen hanterar spelets "Game Over"-skärm
    public static class GameOver
    {
        // Textur för outro-skärmen
        private static Texture2D outro;
        // Position och storlek för outro-skärmen
        private static Rectangle outroPos = new Rectangle(0, 0, 702, 774);
        // Font för textvisning på skärmen
        private static SpriteFont basicFont;

        // Property för att sätta fonten från andra delar av spelet
        public static SpriteFont setBasicFont
        {
            set { basicFont = value; }
        }

        // Property för att sätta outro-texturen från andra delar av spelet
        public static Texture2D setoutro
        {
            set { outro = value; }
        }

        // Update-metoden hanterar uppdateringar för Game Over-skärmen
        public static void Update(GameTime gameTime)
        {
            // Kollar tillståndet för tangentbordet
            KeyboardState kState = Keyboard.GetState();
            // Om mellanslagstangenten är nedtryckt, återställ spelet till normalt läge och spela startljudet
            if (kState.IsKeyDown(Keys.Space))
            {
                Game1.gameController.gameState = Controller.GameState.Normal;
                MySounds.game_start.Play();
            }
        }

        // Draw-metoden hanterar rendering av Game Over-skärmen
        public static void Draw(SpriteBatch spriteBatch)
        {
            // Ritar outro-texturen på skärmen
            spriteBatch.Draw(outro, outroPos, Color.White);
        }
    }
}