using Giant.Core;
using Giant.EnumUtil;
using UnityEngine;

namespace Giant.Battle
{
    public class MoveHandlerComponent : Component, IInitSystem<Unit>
    {
        private int speed;
        private float moveFactor;
        private float moveFactorSpeed;
        private bool isMoving;
        public Unit Owner { get; private set; }
        MapScene currentMap => Owner.MapScene;
        Region curRegion => Owner.Region;

        private float duration;

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
        /// <summary>
        /// 需要寻路
        /// </summary>
        public bool NeedFindPath = true;

        public bool UseNewJps = true;
        /// <summary>
        /// 改变寻路终点
        /// </summary>
        bool NeedChangePathDestination = false;
        /// <summary>
        /// 寻路终点
        /// </summary>
        public Vector2 pathDestination = new Vector2();


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

            Owner.OnMove(vector);

            return true;
        }

        internal void SetPosition(float x, float y)
        {
            curPosition.x = x;
            curPosition.y = y;
        }

        private void UpdatePositionByClampFactor(Vector2 from, Vector2 to, float clampFactor)
        {
            float x = from.x * (1 - clampFactor) + to.x * clampFactor;
            float y = from.y * (1 - clampFactor) + to.y * clampFactor;
            SetPosition(x, y);
        }

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="deltaTime"></param>
        /// <returns>返回duration</returns>
        public void Move(float deltaTime)
        {
            if (currentMap != null)
            {
                isMoving = true;
                moveFactor += moveFactorSpeed * deltaTime;
                float clampFactor = Mathf.Clamp01(moveFactor);

                UpdatePositionByClampFactor(moveFromPosition, moveToPosition, clampFactor);

                // 全图同步，不需要对格子进行维护
                if (currentMap.MapModel.AOIType == AOIType.All)
                {
                    return;
                }

                Region destRegion = currentMap.RegionManager.GetRegion(curPosition);
                if (destRegion != null && curRegion != null)
                {
                    if (destRegion.index == curRegion.index)
                    {
                        return;
                    }
                    else
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            // 对于旧格子，清除相关数据
                            if (curRegion.NeighborList[i] != null && !destRegion.InMyRegions(curRegion.NeighborList[i]))
                            {
                                curRegion.NeighborList[i].NotifyCurRegionFieldObjectOut(Owner);
                            }
                        }
                        curRegion.LeaveRegion(Owner);

                        for (int i = 0; i < 8; i++)
                        {
                            // 对于新格子，添加相关数据 
                            if (destRegion.NeighborList[i] != null && !curRegion.InMyRegions(destRegion.NeighborList[i]))
                            {
                                destRegion.NeighborList[i].NotifyCurRegionFieldObjectIn(Owner);
                            }
                        }
                        destRegion.EnterRegion(Owner);

                        Owner.SetCurRegion(destRegion);
                    }
                }
            }
            return;
        }

        /// <summary>
        /// 传送
        /// </summary>
        /// <param name="dest">目的地点</param>
        /// <returns>duration</returns>
        public void Transmit(Vector2 dest)
        {
            if (currentMap != null)
            {
                SetPosition(dest.x, dest.y);
                if (currentMap.AOIType == AOIType.All)
                {
                    // 对于全图 同步，广播当前位置信息即可
                    Owner.BroadcastSimpleInfo();
                    return;
                }

                Region destRegion = currentMap.RegionManager.GetRegion(dest);
                if (destRegion != null && curRegion != null)
                {
                    if (!destRegion.InMyRegions(curRegion))
                    {
                        curRegion.NotifyCurRegionFieldObjectOut(Owner);
                    }
                    for (int i = 0; i < 8; i++)
                    {
                        // 对于旧格子，清除相关数据
                        if (curRegion.NeighborList[i] != null && !destRegion.InMyRegions(curRegion.NeighborList[i]))
                        {
                            curRegion.NeighborList[i].NotifyCurRegionFieldObjectOut(Owner);
                        }
                    }
                    curRegion.LeaveRegion(Owner);

                    if (!curRegion.InMyRegions(destRegion))
                    {
                        destRegion.NotifyCurRegionFieldObjectIn(Owner);
                    }
                    for (int i = 0; i < 8; i++)
                    {
                        // 对于新格子，添加相关数据 
                        if (destRegion.NeighborList[i] != null && !curRegion.InMyRegions(destRegion.NeighborList[i]))
                        {
                            destRegion.NeighborList[i].NotifyCurRegionFieldObjectIn(Owner);
                        }
                    }
                    destRegion.EnterRegion(Owner);

                    Owner.SetCurRegion(destRegion);
                }
            }
        }

        public void MoveStart(float speedScale = 1.0f)
        {
            isMoving = true;
            bool useDynamicGrid = Owner.UseDynamicGrid();
            if (useDynamicGrid)
            {
                // 生成动态阻挡信息
                currentMap.UpdateDynamicGrid();
                // 将自己设置为非阻挡后，再寻路
                currentMap.SetFieldObjectObstract(Owner, false);
            }
            InitPath(CurPosition, pathDestination, useDynamicGrid);
            InitValue();
        }

        public void InitPath(Vector2 from, Vector2 to, bool useDynamic)
        {
            //if (currentMap.IsHighPrecision)
            //{
            //    //高精度地图，约定精度为0.5，乘以2取整，结果路径需要除以2还原。
            //    from = from * 2.0f;
            //    to = to * 2.0f;
            //}

            if (NeedFindPath && Owner.UnitType == UnitType.Player)
            {
                if (UseNewJps)
                {
                    movePath = Owner.MapScene.GetPath_New(from, to, useDynamic);
                    if (movePath == null || movePath.Length == 0)
                    {
                        movePath = new Vector2[2];
                        movePath[0] = from;
                        movePath[1] = from;
                    }
                }
                else
                {
                    movePath = Owner.MapScene.GetPath(from, to);
                    if (NeedUseBigPath(movePath))
                    {
                        movePath = currentMap.GetPath(from, to, true);
                    }
                }
            }
            else
            {
                movePath = new Vector2[2];
                movePath[0] = from;
                movePath[1] = to;
            }

            //if (currentMap.IsHighPrecision)
            //{
            //    foreach (var item in movePath)
            //    {
            //        //路径除以2还原
            //        item.X = item.X / 2.0f;
            //        item.Y = item.Y / 2.0f;
            //    }
            //}
            moveFromPosition.Init(movePath[0]);
            moveToPosition.Init(movePath[1]);

            movePathIndex = 1;
            NeedFindPath = true;
            NeedChangePathDestination = false;
            pathDestination.Init(movePath[movePath.Length - 1]);
        }

        public float InitValue()
        {
            moveFactor = 0;

            duration = GetDuration(moveToPosition, moveFromPosition);
            if (duration > 0)
            {
                moveFactorSpeed = 1 / duration;
            }
            else
            {
                moveFactor = 1;
            }

            return duration;
        }

        public float GetDuration(Vector2 dest, Vector2 sour)
        {
            float distance = Vector2.Distance(sour, dest);
            float duration = distance / speed;
            return duration;
        }

        private bool NeedUseBigPath(Vector2[] path)
        {
            //if ((owner.CurFsmStateType == FsmStateType.MONSTER_SEARCH) || owner.FieldObjectType == TYPE.PC)
            {
                //TODO:这里是否必要。
                if (path.Length == 2 && path[0].x == path[1].x && path[0].y == path[1].y)
                {
                    return true;
                }
            }
            return false;
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


