using EpPathFinding;
using Giant.Core;
using Giant.Data;
using System.Collections.Generic;

namespace Server.App
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

            dynamicGrid = MapGridPosComponent.Instance.GetGrid(model.BianryName);
            jumpPointParam = new JumpPointParam(dynamicGrid, EndNodeUnWalkableTreatment.ALLOW, DiagonalMovement.Never, HeuristicMode.EUCLIDEAN);
        }

        public List<GridPos> PathFind(GridPos startPos, GridPos endPos)
        {
            jumpPointParam.Reset(startPos, endPos);
            return JumpPointFinder.FindPath(jumpPointParam);
        }
    }
}
