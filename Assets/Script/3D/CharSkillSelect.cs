
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Script.Data;
using UnityEngine;

public class CharSkillSelect : MonoBehaviour
{
    [SerializeField] private GameObject _skillItemObject;


    private List<SkilItem> _selectSkill = new();
    
    private const int MAX_COUNT = 3;

    public void SelectSkill(SkilItem skill)
    {
        if (_selectSkill.Count >= MAX_COUNT)
        {
            var removeSkill = _selectSkill[MAX_COUNT - 1];
            RemoveSkill(removeSkill);
            return;
        }
        skill.SetChangeSelect(true);
        _selectSkill.Add(skill);
    }

    private void RemoveSkill(SkilItem item)
    {
        item.SetChangeSelect(false);
        _selectSkill.Remove(item);
    }
    
    private void Start()
    {
        Managers.Back.GetUserInventory(() =>
        {
            ShowStore().Forget();
        });
        
    }

    private async UniTaskVoid ShowStore()
    {
        var items = await  Managers.Back.GetStoreItems("SkillShop");
        foreach (var item in items.Store)
        {
          var itemObject = Instantiate(_skillItemObject,this.transform).GetComponent<SkilItem>();
          var itemInstance = Managers.Back.GetItem(item.ItemId);
          
          var customData = item.CustomData;
          var itemData = JsonConvert.DeserializeObject<SkillItemData>(customData.ToString());
          itemObject.Init(item.ItemId,itemData.text,item.VirtualCurrencyPrices[Define.Diamond],itemInstance,this);
        }
        
    }
}
