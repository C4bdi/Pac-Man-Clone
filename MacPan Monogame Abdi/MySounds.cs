using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Definiera en statisk klass för ljudhantering i spelet Pacman.
namespace Pacman
{
    // Deklarera en statisk klass som hanterar alla ljudeffekter i spelet.
    public static class MySounds
    {
        // Deklara statiska instanser av SoundEffect för olika ljud i spelet.
        public static SoundEffect game_start;
        public static SoundEffect munch;
        public static SoundEffectInstance munchInstance;
        public static SoundEffect credit;
        public static SoundEffect death_1;
        public static SoundEffect death_2;
        public static SoundEffect eat_fruit;
        public static SoundEffect eat_ghost;
        public static SoundEffect power_pellet;
        public static SoundEffectInstance power_pellet_instance;
        public static SoundEffect extend;
        public static SoundEffect intermission;
        public static SoundEffect retreating;
        public static SoundEffectInstance retreatingInstance;
        public static SoundEffect siren_1;
        public static SoundEffectInstance siren_1_instance;
        public static SoundEffect siren_2;
        public static SoundEffect siren_3;
        public static SoundEffect siren_4;
        public static SoundEffect siren_5;

        // Varje SoundEffect och SoundEffectInstance representerar en unik ljudsekvens i spelet,
        // som används för olika händelser som spelstart, matning, död, ätande av frukt, ätande av en ghost, aktivering av power pellet, förlängning av liv, mellanspel, retirering, och sirener.

        // Dessa ljudeffekter används för att förbättra spelares erfarenhet genom att ge visuellt feedback och dynamik till spelets mekaniker och händelser.
    }
}

