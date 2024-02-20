
    using Sirenix.OdinInspector;
    using UnityEngine;

    public abstract class Ability : SerializedScriptableObject,IAbilityRun
    {
        [SerializeField] private string _itemId;
        public string GetItemId => _itemId;
        public abstract void RunAbility();
    }
