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
