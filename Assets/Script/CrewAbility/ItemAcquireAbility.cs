
    using UnityEngine;
    [CreateAssetMenu(fileName = "StatusAbility", menuName = "Ability/ItemAdd")]
    public class ItemAcquireAbility : Ability
    {
        [SerializeField]
        private string itemName;

        [SerializeField] private float amount;
        [SerializeField]
        private float addPercent;
        public override void RunAbility()
        {
            float randomValue = Random.Range(0f, 101f);

              bool isAdd = randomValue <= addPercent;
              if (isAdd)
              {
                  Managers.Game.AddTextNote($"특성에 의해 {itemName}을 습득하였다!");
                  Managers.Game.AddItem(itemName,amount,false);
              }
        }
    }
