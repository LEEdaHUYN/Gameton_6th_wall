
    public class HadSelectCharacterCondition : Condition
    {
        public override bool CheckCondition()
        {
            return Managers.Game.SelectCharacter != null;
        }
    }
