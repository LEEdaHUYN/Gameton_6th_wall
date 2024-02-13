
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using Random = UnityEngine.Random;
    [CreateAssetMenu(fileName = "StatusAbility", menuName = "Ability/Status")]
    public class StatusAbility : Ability
    {
        [SerializeField] private List<Define.CharacterStatus> statusList;
        [SerializeField]
        private float addPercent;
        public override void RunAbility()
        {
            var charaters = Managers.Game.Characters;
            var randIdx = Random.Range(0, charaters.Count);
            foreach (var status in statusList)
            {
                Managers.Game.CharacterStatusCalculate(charaters[randIdx],status,Define.SetStatusAction.Mod,0);
            }
            Managers.Game.AddTextNote($"{charaters[randIdx].GetName()}이(가) 동료 특성에 의해 상태가 회복되었습니다.");
        }
    }
