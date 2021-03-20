using System.Collections.Generic;

namespace Giant.Battle
{
    public partial class MapScene
    {
        private Dictionary<long, Robot> robotList = new Dictionary<long, Robot>();
        public Dictionary<long, Robot> RobotList => robotList;

        protected virtual void UpdateRobot(double dt)
        {
        }
    }
}
