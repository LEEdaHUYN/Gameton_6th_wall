using System;
using UnityEngine;
[Serializable]
public class UseEffectSetStatus : UseEffect
{
        [SerializeField]
     private float _value;
     [SerializeField]
     private Define.SetStatusAction _setStatusAction;
     [SerializeField]
     private Define.CharacterStatus _characterStatus;
     
     public override void UseItem(Character useCharacter)
     {
         Utils.CalculateCharacterStatusValue(useCharacter, _setStatusAction, _characterStatus, _value);
     }
     
     
}