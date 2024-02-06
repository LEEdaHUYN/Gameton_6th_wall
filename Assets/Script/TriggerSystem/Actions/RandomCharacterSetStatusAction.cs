
    using System.Collections.Generic;
    using System.Linq;
    using Random = UnityEngine.Random;

    public class RandomCharacterSetStatusAction : TriggerAction
    {        
        public Define.SetStatusAction calculate;
        public Define.CharacterStatus status;
        public float value;
        public int count;
        public override void RunAction()
        {
            var charaters = Managers.Game.Characters.ToList();
            for (int i = 0; i < count || i < charaters.Count; i++)
            {
                var randIdx = Random.Range(0, charaters.Count);
                Managers.Game.CharacterStatusCalculate(charaters[randIdx],status,calculate,value);
                charaters.Remove(charaters[randIdx]);
            }

        }
    }
