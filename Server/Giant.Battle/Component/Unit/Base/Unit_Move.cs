using UnityEngine;

namespace Giant.Battle
{
    public partial class Unit
    {
        public Region Region{ get; private set; }
        public MoveHandlerComponent MoveHandlerComponent { get; private set; }

        public bool Move(Vector2 vector)
        {
            if (!CanMove())
            {
                return false;
            }

            MoveHandlerComponent.SetPosition(vector);

            return true;
        }

        private void InitMoverHandler()
        {
            MoveHandlerComponent = AddComponent<MoveHandlerComponent, Unit>(this);
        }

        public void SetCurRegion(Region region)
        {
            Region = region;
        }

        public bool UseDynamicGrid()
        {
            return false;
        }

        public bool CanMove()
        {
            return true;
        }
    }
}


