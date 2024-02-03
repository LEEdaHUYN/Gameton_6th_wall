using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Script.Data;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Script.Manager.Core
{
    public interface ILoader<Key, Value>
    {
        Dictionary<Key, Value> MakeDict();
    }

    public class DataManager
    {
        public Dictionary<int, TriggerData> TriggerDatas = new Dictionary<int, TriggerData>();
        public Dictionary<int, CharacterStatusData> CharacterStatusDatas = new Dictionary<int, CharacterStatusData>();

        public void Init()
        {
            TriggerDatas = LoadJson<TriggerDataLoader, int, TriggerData>("triggerData").MakeDict();
            CharacterStatusDatas = LoadJson<CharacterStatusDataLoader, int, CharacterStatusData>("characterStatusData")
                .MakeDict();

        }
        Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
        {
            TextAsset textAsset = Managers.Resource.Load<TextAsset>($"{path}");
            return JsonConvert.DeserializeObject<Loader>(textAsset.text);
        }
        
        // TODO : 차후 NetWork Manager에서 관리 download 는 뭔가 백단 서버가 필요할 것 같음. 차후 알아보겠음 playFab에서도 제공할 것 같긴 함.
        private const string downloadLink =
            "https://drive.google.com/file/d/17wQAVNeVjC6ys3gCzoHHDgkkDWjc7gVN/view?usp=drive_link";
        async UniTaskVoid GetTextList()
        {
            var txt = (await UnityWebRequest.Get(downloadLink).SendWebRequest()).downloadHandler.text;
            Debug.Log(txt);
        }
    }
     
}