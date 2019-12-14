using Giant.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Giant.Battle
{
    public class MapComponent : InitSystem<MapModel>
    {
        public MapModel Model { get; private set; }
        public int MapId => Model.Id;

        public override void Init(MapModel model)
        {
            Model = model;
        }

        public List<Vector2> PathFind(Vector2 start, Vector2 end)
        {
            return Model.PathFind(start, end);
        }
    }
}
