using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giant.Battle
{
    partial class MapScene
    {
        public RegionManager RegionManager { get; private set; }

        public void InitRegionManager()
        {
            RegionManager.Init(this, MaxX - MinX, MaxY - MinY, MinX, MinY);
        }
    }
}
