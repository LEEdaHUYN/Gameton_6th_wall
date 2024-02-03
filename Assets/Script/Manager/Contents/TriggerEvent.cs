    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Script.Data;
    using UnityEngine;

    public class TriggerEvent : MonoBehaviour
    {
        private List<TriggerData> _triggerDatas;
        public void StartTrigger(Action callback)
        {
            _triggerDatas ??= Managers.Data.TriggerDatas.Values.ToList();
            
            //일단 현재 상황에서 만족하는 트리거를 모두 가져옴.
            var selectedTriggerDatas = _triggerDatas.Where(triggerData =>
                triggerData.ConditionList.All(condition => condition.CheckCondition())
            ).Select(triggerData => triggerData).ToList();
            
            //해당 트리거를 하나씩 순차적으로 실행함, 다만, 앞선 트리거 의해 조회된 다른 트리거가 실행되면 안될 경우를 대비하여
            //다시 한번 조건을 체크 함.
            foreach (var trigger in selectedTriggerDatas)
            {
                // if (!trigger.ConditionList.All(condition => condition.CheckCondition())) 
                //     continue;
                foreach (var triggerAction in trigger.ActionList)
                {
                    triggerAction.RunAction();
                }
            }
            // 트리거 로드가 모두 끝났을 때 실행
            callback?.Invoke();
        }
    }
