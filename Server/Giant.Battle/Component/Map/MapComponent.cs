using EpPathFinding;
using Giant.Core;
using Giant.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Giant.Battle
{
    public class MapComponent : InitSystem<MapModel>
    {
        private DynamicGrid dynamicGrid;
        private JumpPointParam jumpPointParam;

        public MapModel Model { get; private set; }
        public int MapId => Model.Id;

        public override void Init(MapModel model)
        {
            Model = model;

            dynamicGrid = MapGridPosComponent.Instance.GetGrid(Model.BianryName);
            jumpPointParam = new JumpPointParam(dynamicGrid, EndNodeUnWalkableTreatment.ALLOW, DiagonalMovement.Never, HeuristicMode.EUCLIDEAN);
        }

        public List<Vector2> PathFind(Vector2 start, Vector2 end)
        {
            GridPos startPos = new GridPos((int)Math.Round(start.x), (int)Math.Round(start.y));
            GridPos endPos = new GridPos((int)Math.Round(end.x), (int)Math.Round(end.y));

            jumpPointParam.Reset(startPos, endPos);

            List<GridPos> path = JumpPointFinder.FindPath(jumpPointParam);
            return path.ConvertAll(x => new Vector2(x.x, x.y));
        }
    }
}
