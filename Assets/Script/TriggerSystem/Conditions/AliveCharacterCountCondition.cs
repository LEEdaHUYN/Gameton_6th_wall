
    public class AliveCharacterCountCondition : Condition
    {
        public int aliveCount;
        public override bool CheckCondition()
        {
            return Managers.Game.Characters.Count >= aliveCount;
        }
    }
