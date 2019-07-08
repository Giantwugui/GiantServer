using EpPathFinding;
using Giant.Data;
using System.Collections.Generic;

namespace Server.App
{
    public class Map
    {
        private DynamicGrid dynamicGrid;
        private JumpPointParam jumpPointParam;

        public MapModel Model { get; private set; }
        public int MapId => Model.MapId;

        public MapMamanger MapMamanger { get; private set; }

        public Map(MapMamanger mamanger, MapModel model)
        {
            this.MapMamanger = mamanger;
            this.Model = model;

            this.dynamicGrid = MapGridPosManager.GetGrid(model.BianryName);
            this.jumpPointParam = new JumpPointParam(this.dynamicGrid, EndNodeUnWalkableTreatment.ALLOW, DiagonalMovement.Always, HeuristicMode.EUCLIDEAN);
        }

        public List<GridPos> PathFind(GridPos startPos, GridPos endPos)
        {
            jumpPointParam.Reset(startPos, endPos);
            return JumpPointFinder.FindPath(jumpPointParam);
        }
    }
}
