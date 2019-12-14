using EpPathFinding;
using Giant.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Giant.Battle
{
    public class MapModel : IData<MapModel>
    {
        private DynamicGrid dynamicGrid;
        private JumpPointParam jumpPointParam;

        public int Id { get; private set; }
        public string MapName { get; private set; }
        public string BianryName { get; private set; }

        public void Bind(DataModel data)
        {
            Id = data.Id;
            MapName = data.GetString("MapName");
            BianryName = data.GetString("BianryName");

            dynamicGrid = MapGridPosComponent.Instance.GetGrid(BianryName);
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
