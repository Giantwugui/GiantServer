using UnityEngine;

namespace Giant.Battle
{
    public partial class Unit
    {
        public Region Region{ get; private set; }
        public MoveHandlerComponent MoveHandler { get; private set; }
        public bool IsMoving => MoveHandler.IsMoving;

        private void InitMoverHandler()
        {
            MoveHandler = AddComponent<MoveHandlerComponent, Unit>(this);
        }

        public void Transmit(Vector2 dest)
        {
            MoveHandler.Transmit(dest);
        }

        public void SetDestination(Vector2 dest)
        {
            MoveHandler.SetDestination(dest);
        }

        /// <summary>
        /// 移动并检测是否到达终点
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>pathend</returns>
        public bool Move(float dt)
        {
            if (!CanMove())
            {
                return true;
            }

            MoveHandler.Move(dt);

            return CheckPathEnd();
        }

        public void MoveStart()
        {
            MoveHandler.MoveStart();
        }

        public void MoveStop()
        {
            MoveHandler.MoveStop();
        }

        public bool CheckDestination()
        {
            return MoveHandler.CheckDestination(); ;
        }

        public bool CheckPathEnd()
        {
            return MoveHandler.CheckPathEnd();
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


