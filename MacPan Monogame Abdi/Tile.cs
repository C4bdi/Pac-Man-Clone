using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
    // Klass som representerar en tile i Pacman-spelet
    public class Tile
    {
        // Olika typer av tiles i spelet
        public enum TileType { None, Wall, Ghost, GhostHouse, Player, Snack };

        // Typen av tile, standardvärde är None
        public TileType tileType = TileType.None;
        // Indikerar om tile är tom (ingen spelare, spöke eller objekt)
        public bool isEmpty = true;

        // Positionen av tile i spelvärlden
        Vector2 position;

        // Egenskap för att få positionen av tile
        public Vector2 Position
        {
            get
            {
                return position;
            }
        }

        // Statisk metod för att beräkna avståndet mellan två tiles
        public static int getDistanceBetweenTiles(Vector2 pos1, Vector2 pos2)
        {
            // Beräknar avståndet mellan två punkter i 2D-rymden
            return (int)Math.Sqrt(Math.Pow(pos1.X - pos2.X, 2) + Math.Pow(pos1.Y - pos2.Y, 2));
        }

        // Konstruktor som sätter positionen av tile
        public Tile(Vector2 newPosition)
        {
            position = newPosition;
        }

        // Konstruktor som sätter både position och typ av tile
        public Tile(Vector2 newPosition, TileType newTileType)
        {
            position = newPosition;
            tileType = newTileType;
        }
    }
}