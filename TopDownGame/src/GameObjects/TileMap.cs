using System;
using System.Collections.Generic;
using System.Text;
using SFML.System;
using SFML.Graphics;

namespace TopDownGame
{
    class TileMap : GameObject, ICollidable
    {
        // The grid of tile
        public char[,] tiles;
        // The size of each tile
        public int tileSize;

        // [] operatrion overload
        public char this[int y, int x]
        {
            get
            {
                if (x < 0 || y < 0 || x >= tiles.GetLength(1) || y >= tiles.GetLength(0))
                    return 'w';
                else
                    return tiles[y, x];
            }
            set
            {
                tiles[y, x] = value;
            }
        }

        public char this[Vector2 vector]
        {
            get
            {
                return tiles[(int)Math.Floor(vector.Y), (int)Math.Floor(vector.X)];
            }
            set
            {
                tiles[(int)Math.Floor(vector.Y), (int)Math.Floor(vector.X)] = value;
            }
        }

        // Tilemap collision shape
        public CollisionShape CollisionShape { get; }

        public TileMap(int width, int height, int tileSize) : base (new Vector2(), new Vector2())
        {
            tiles = new char[height, width];
            this.tileSize = tileSize;
            CollisionShape = new CollisionTileMap(this);
        }

        public override void Update()
        {

        }

        /// <summary>
        /// Draws each tile
        /// </summary>
        public override void Render()
        {
            for (int y = 0; y < tiles.GetLength(0); y++)
            {
                for (int x = 0; x < tiles.GetLength(1); x++)
                {
                    switch(tiles[y,x])
                    {
                        case 'e':
                            {
                                RectangleShape rect = new RectangleShape(new Vector2f(tileSize, tileSize));
                                rect.FillColor = new Color(235, 235, 235);
                                rect.Position = new Vector2f(x * tileSize, y * tileSize);
                                Window.Draw(rect);
                                break;
                            }
                        case 'w':
                            {
                                RectangleShape rect = new RectangleShape(new Vector2f(tileSize, tileSize));
                                rect.FillColor = new Color(55, 55, 55);
                                rect.Position = new Vector2f(x * tileSize, y * tileSize);
                                Window.Draw(rect);
                                break;
                            }
                    }
                }
            }
            if (Game.Debug)
                CollisionShape.Draw();
        }

        public bool IsColliding()
        {
            return false;
        }
    }
}
