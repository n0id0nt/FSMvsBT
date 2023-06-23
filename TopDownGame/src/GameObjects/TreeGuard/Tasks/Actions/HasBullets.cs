using System;
using System.Collections.Generic;
using System.Text;

namespace TopDownGame
{
    class HasBullets : Task
    {
        public HasBullets(TreeGuard baseObject) : base(baseObject)
        {

        }



        public override TaskStatus DoAction()
        {
            if (baseObject.ammo > 0)
                return TaskStatus.Success;

            return TaskStatus.Failure;
        }

        public override void End()
        {
        }

        public override void Start()
        {
        }
    }
}
