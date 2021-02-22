using Giant.Battle;
using UnityEngine;

namespace Server.Zone
{
    partial class Player
    {
        public void Move(Vector2 vector)
        {
            PlayerUnit.Move(vector);
        }

        public void CastSkill(int skillId, int targetId, Vector2 direction)
        {
            PlayerUnit.CastSkill(skillId, targetId, direction);
        }
    }
}
