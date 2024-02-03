
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item : SerializedScriptableObject
{
  // 아이템에 공통적으로 들어갈 내용 정리 필요.
  // 우선 아이템에 대한 이름 및 Sprite 정보 그리고 3D Prefab 연결 까지 해주면 될 듯

  #region GetProperty

  [SerializeField]
  protected string _name;
  public string GetName => _name;

  [SerializeField]
  protected Sprite _sprite;
  public Sprite GetSprite => _sprite;

  [SerializeField]
  protected GameObject _prefab;
  public GameObject GetPrefab => _prefab;

  #endregion

  protected UseEffect useEffect;

  public virtual void UseItem(Character character)
  {
    useEffect.UseItem(character);
  }

  protected float _amount;
  public virtual float GetAmount()
  {
    return _amount;
  }

  public virtual float GetMaxAmount()
  {
    //TODO
    return 10;
  }

  public virtual void SetAmount(float amount)
  {
    _amount = amount;
  }


}

