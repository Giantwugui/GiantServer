using UnityEngine;

namespace Giant.Battle
{
    public partial class Unit
    {
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

        public bool CanMove()
        {
            return true;
        }
    }
}


