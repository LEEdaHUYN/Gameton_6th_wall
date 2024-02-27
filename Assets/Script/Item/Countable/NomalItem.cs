
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewEatItem", menuName = "Items/NomalItem")]
    public class NomalItem : Item
    {
        public override float GetAmount()
        {
            return _amount;
        }

        public  override float GetMaxAmount()
        {
            return 0;
        }

        public override void SetAmount(float amount)
        {
            _amount = amount;
        }
    }
