using System;
using System.Collections.Generic;
using System.Text;

namespace TopDownGame
{
    class ChangeSpriteIndex : Task
    {
        private int index;

        public ChangeSpriteIndex(int index, TreeGuard baseObject) : base(baseObject)
        {
            this.index = index;
        }

        public override void Start()
        {
            
        }

        public override TaskStatus DoAction()
        {
            baseObject.textureRect.Left = (int)baseObject.Size.X * index;
            return TaskStatus.Success;
        }

        public override void End()
        {

        }
    }
}
