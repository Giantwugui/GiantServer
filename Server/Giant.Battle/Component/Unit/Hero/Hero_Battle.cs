﻿using Giant.Core;
using UnityEngine;

namespace Giant.Battle
{
    public partial class Hero
    {
        public override void StartFighting()
        {
            base.StartFighting();
        }

        public override void StopFighting()
        {
            base.StopFighting();
        }

        public override void OnDead()
        {
            base.OnDead();
        }

        public override void OnHit(Unit target, int damage)
        {
            base.OnHit(target, damage);
        }
    }
}