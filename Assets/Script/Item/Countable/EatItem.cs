using UnityEngine;

[CreateAssetMenu(fileName = "NewEatItem", menuName = "Items/EatItem")]
    public class EatItem :Item
    {
    

        [SerializeField] private UseEffectSetStatus _useEffectSetStatus;

        public override void UseItem(Character character)
        {
            this.useEffect = _useEffectSetStatus;
            base.UseItem(character);
        }

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
