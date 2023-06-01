using System;
using System.Collections.Generic;
using System.Text;

namespace TopDownGame
{
    class Reload : Task
    {
        public Reload(TreeGuard baseObject) : base(baseObject)
        {

        }

        public override void Start()
        {
            Started = true;
            baseObject.Vel = new Vector2();
        }

        public override TaskStatus DoAction()
        {
            baseObject.ammo = TreeGuard.maxAmmo;
            return TaskStatus.Success;
        }

        public override void End()
        {
            Started = false;
        }
    }
}
