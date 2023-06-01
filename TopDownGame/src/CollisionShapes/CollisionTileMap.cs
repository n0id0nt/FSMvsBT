using System;
using System.Collections.Generic;
using System.Text;
using Box2D;

namespace TopDownGame
{
    class CollisionTileMap : CollisionShape
    {
        List<CollisionRect> collisionRects;
        public CollisionTileMap(TileMap tileMap) : base(tileMap)
        {
            collisionRects = new List<CollisionRect>();
        }

        public void UpdateTileMap()
        {
            collisionRects = new List<CollisionRect>();

            TileMap tileMap = (TileMap)BaseObject;

            for (int y = 0; y < tileMap.tiles.GetLength(0); y++)
            {
                for (int x = 0; x < tileMap.tiles.GetLength(1); x++)
                {
                    if (tileMap.tiles[y, x] == 'w')
                        collisionRects.Add(new CollisionRect(new Vector2(tileMap.tileSize, tileMap.tileSize), new Vector2(-x * tileMap.tileSize, -y * tileMap.tileSize), tileMap, b2BodyType.b2_staticBody));
                }
            }
        }

        public override bool IsColliding(CollisionShape other)
        {
            foreach (CollisionRect collisionRect in collisionRects)
            {
                if (collisionRect.IsColliding(other))
                    return true;
            }
            return false;
        }

        public override bool IsColliding(Vector2 point)
        {
            foreach (CollisionRect collisionRect in collisionRects)
            {
                if (collisionRect.IsColliding(point))
                    return true;
            }
            return false;
        }

        public override void Draw()
        {
            foreach (CollisionRect collisionRect in collisionRects)
            {
                collisionRect.Draw();
            }
        }
    }
}
