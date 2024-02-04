
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewEatItem", menuName = "Items/NomalItem")]
    public class NomalItem : Item
    {
        public virtual float GetAmount()
        {
            return _amount;
        }

        public  virtual float GetMaxAmount()
        {
            return 0;
        }

        public virtual void SetAmount(float amount)
        {
            _amount = amount;
        }
    }
