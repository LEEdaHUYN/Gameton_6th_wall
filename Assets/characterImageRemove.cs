using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class CharacterImageRemove : MonoBehaviour
{
   private Character _myCharacter;
   public void SetCharacter(Character character)
   {
      _myCharacter = character;
      this.ObserveEveryValueChanged(_myCharacter => character.GetIsAlive)
         .Where(x => !x)
         .Subscribe(_ =>
         {
            Destroy(this.gameObject);
         });
   }
}
