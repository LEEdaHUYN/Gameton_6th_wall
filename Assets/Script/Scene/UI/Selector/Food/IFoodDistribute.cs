
    using Script.Scene.UI.Selector.Food;
    using UnityEngine.UI;

    public interface IFoodDistribute
    {
        public FoodType GetFoodType();
        public void CharacterFoodDistribute();
        public void CharacterFoodBackIn();
        public bool CheckFoodDistribute();

        public void AddCharacterInfo(Toggle toggle);
    }
