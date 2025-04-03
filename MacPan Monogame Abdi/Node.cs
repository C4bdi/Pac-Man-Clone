using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Definiera en klass för noden i en nodbaserad sökträdstruktur, användbar för pathfinding algoritmer som A*.
namespace Pacman
{
    // Deklarera en klass som representerar en nod i ett nodbaserat sökträd.
    public class Node
    {
        // Deklara kostattribut för att beräkna totala kost (fCost) för en nod.
        public int gCost; // Kost från startnod till denna nod.
        public int hCost; // Heuristisk kost från denna nod till målnod.

        // Referens till föräldernod i sökträdet.
        public Node parent;

        // Direktion som ska ignoreras vid generering av grannar.
        public Dir ignoreDirection = Dir.None;

        // Metod för att sätta vilken direktion som ska ignoreras.
        public void setIgnoreDirection(Dir currentDir)
        {
            switch (currentDir)
            {
                case Dir.Right:
                    ignoreDirection = Dir.Left;
                    break;
                case Dir.Left:
                    ignoreDirection = Dir.Right;
                    break;
                case Dir.Down:
                    ignoreDirection = Dir.Up;
                    break;
                case Dir.Up:
                    ignoreDirection = Dir.Down;
                    break;
            }
        }

        // Metod för att sätta föräldernod.
        public void setParent(Node parent)
        {
            this.parent = parent;
        }

        // Egenskap för att beräkna den totala kosten (fCost) för en nod.
        public int fCost
        {
            get
            {
                return gCost + hCost;
            }
        }

        // Anger om noden är gångbar eller inte (t.ex., om den är blockerad av ett vägg).
        public bool isWalkable;

        // Positionen för noden på spelkartan.
        public Vector2 pos;

        // Referens till den underliggande Tile-objektet för noden.
        Tile tile;

        // Konstruktor för att skapa en ny nod med given position och referens till spelkartan.
        public Node(Vector2 pos, Tile[,] tileArray)
        {
            // Kontrollera om positionen är giltig inom kartgränserna.
            if (pos.X < 0 || pos.Y < 0 || pos.X >= Controller.numberOfTilesX || pos.Y >= Controller.numberOfTilesY)
            {
                this.pos = new Vector2(-100, -100); // Odefinierad position.
            }
            else
            {
                this.pos = pos;
                tile = tileArray[(int)pos.X, (int)pos.Y];
                // Ange om noden är gångbar baserat på typen av Tile.
                if (tile.tileType == Tile.TileType.Wall)
                {
                    isWalkable = false;
                }
                else
                {
                    isWalkable = true;
                }
            }
        }

        // Metod för att skapa en kopia av denna nod.
        public Node Copy(Tile[,] tileArray)
        {
            Node node = new Node(pos, tileArray);

            node.hCost = hCost;
            node.gCost = gCost;
            node.ignoreDirection = ignoreDirection;
            // Kopiera föräldernod om den finns.
            if (parent != null)
            {
                node.setParent(parent.Copy(tileArray));
            }

            return node;
        }

        // Metod för att hämta grannar till denna nod, med undantag för den angivna riktningen.
        public List<Node> getNeighbours(Tile[,] tileArray)
        {
            List<Node> neighbours = new List<Node>();

            // Generera grannar för varje möjlig riktning, utom den angivna riktningen.
            if (ignoreDirection != Dir.Left)
            {
                Node left = new Node(new Vector2(pos.X - 1, pos.Y), tileArray);
                if (left.pos != new Vector2(-100, -100)) neighbours.Add(left);
            }

            if (ignoreDirection != Dir.Right)
            {
                Node right = new Node(new Vector2(pos.X + 1, pos.Y), tileArray);
                if (right.pos != new Vector2(-100, -100)) neighbours.Add(right);
            }

            if (ignoreDirection != Dir.Up)
            {
                Node up = new Node(new Vector2(pos.X, pos.Y - 1), tileArray);
                if (up.pos != new Vector2(-100, -100)) neighbours.Add(up);
            }

            if (ignoreDirection != Dir.Down)
            {
                Node down = new Node(new Vector2(pos.X, pos.Y + 1), tileArray);
                if (down.pos != new Vector2(-100, -100)) neighbours.Add(down);
            }

            return neighbours;
        }
    }
}

