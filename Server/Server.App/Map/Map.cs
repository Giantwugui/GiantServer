using Giant.Data;
using System;
using System.Collections.Generic;
using System.Text;
using EpPathFinding;

namespace Server.App
{
    public class Map
    {
        private List<GridPos> mapGridPosList = new List<GridPos>();

        public MapModel Model { get; private set; }
        public int MapId => Model.MapId;

        public MapMamanger MapMamanger { get; private set; }

        public Map(MapMamanger mamanger, MapModel model)
        {
            this.MapMamanger = mamanger;
            this.Model = model;
        }
    }
}
