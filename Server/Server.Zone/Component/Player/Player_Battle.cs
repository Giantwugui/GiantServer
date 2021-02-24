using Giant.Battle;
using UnityEngine;

namespace Server.Zone
{
    partial class Player
    {
        public void Transmit(Vector2 vector)
        {
            PlayerUnit.Transmit(vector);
        }

        public void SetDestination(Vector2 vector)
        {
            PlayerUnit.SetDestination(vector);
        }

        public void CastSkill(int skillId, int targetId, Vector2 pos, Vector2 direction)
        {
            PlayerUnit.CastSkill(skillId, targetId, pos, direction);
        }
    }
}
