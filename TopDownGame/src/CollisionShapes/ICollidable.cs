using System;
using System.Collections.Generic;
using System.Text;

namespace TopDownGame
{
    interface ICollidable
    {
        Vector2 Pos { get; set; }

        CollisionShape CollisionShape { get; }

        bool IsColliding();
    }
}
