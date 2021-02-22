using Giant.Core;
using Giant.EnumUtil;
using UnityEngine;

namespace Giant.Battle
{
    public class MoveHandlerComponent : Component, IInitSystem<Unit>
    {
        private int speed;

        public Unit Owner { get; private set; }

        //当前位置
        private Vector2 curPositionCopy = new Vector2(Vector2.zero.x, Vector2.zero.y);
        private Vector2 curPosition = new Vector2(Vector2.zero.x, Vector2.zero.y);
        public Vector2 CurPosition => curPosition;

        /// <summary>
        /// 位移起点
        /// </summary>
        private Vector2 moveFromPosition= new Vector2();
        /// <summary>
        /// 位移终点
        /// </summary>
        private Vector2 moveToPosition = new Vector2();
        /// <summary>
        /// 移动路径
        /// </summary>
        private Vector2[] movePath;
        /// <summary>
        /// 位移路径标识
        /// </summary>
        private int movePathIndex;


        public void Init(Unit unit)
        {
            this.Owner = unit;
            InitMoveSpeed();
        }

        private void InitMoveSpeed()
        {
            speed = Owner.GetNatureValue(NatureType.Speed);
        }

        public bool Move(Vector2 vector)
        {
            SetPosition(vector);

            //TODO视野检测，地图可达性检测

            Owner.MsgSource.OnMove(Owner, vector);

            return true;
        }

        public void SetPosition(Vector2 pos)
        {
            curPosition.x = pos.x;
            curPosition.y = pos.y;
        }

        public void MoveTo(Vector2 destPos)
        {
            moveToPosition.x = destPos.x;
            moveToPosition.y = destPos.y;
        }
    }
}


