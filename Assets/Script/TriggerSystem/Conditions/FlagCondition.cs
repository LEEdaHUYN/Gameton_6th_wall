
using UnityEngine;
    
public class FlagCondition : Condition
{
        public Flag flag;
        public override bool CheckCondition()
        {
            return Managers.Game.CheckFlag(flag);
        }
}
