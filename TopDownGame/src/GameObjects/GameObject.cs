using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;

namespace TopDownGame
{
    abstract class GameObject
    {
        public Image Sprite { get; protected set; }

        public Vector2 Pos { get; set; }

        public Vector2 Size { get; }

        public GameObject(Vector2 pos, Vector2 size)
        {
            Pos = pos;
            Size = size;
        }

        public abstract void Update();

        public abstract void Render();
    }
}
