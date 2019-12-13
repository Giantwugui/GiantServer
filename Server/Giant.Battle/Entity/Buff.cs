using System;
using System.Collections.Generic;
using System.Text;
using Giant.Core;
using Giant.Data;

namespace Giant.Battle
{
    public class Buff : Entity, IInitSystem<BuffModel>
    {
        public BuffModel Model { get; private set; }

        public void Init(BuffModel model)
        {
            Model = model;
        }
    }
}
