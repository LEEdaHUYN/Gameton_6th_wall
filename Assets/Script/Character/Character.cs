
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Character : SerializedMonoBehaviour
{
    private int _id;
    private string _name;
    private bool _isAlive;
    private Sprite _sprite;
    public Sprite GetCharacterSprite => _sprite;
    private Sprite _portrait;

    public Sprite GetCharacterPortrait => _portrait;
    public void SetPortrait(Sprite sprite) => _portrait = sprite;
    
    
    [SerializeField]
    private Dictionary<Define.CharacterStatus, float> _status = new Dictionary<Define.CharacterStatus, float>();

    public float GetStatusValue(Define.CharacterStatus status) => _status[status];

    public void SetStatusValue(Define.CharacterStatus status, float value)
    {
        _status[status] = value;
    }

    public void NextDayStatus()
    {
        foreach (Define.CharacterStatus status in Enum.GetValues(typeof(Define.CharacterStatus)))
        {
            if (status != Define.CharacterStatus.Hungry && status != Define.CharacterStatus.Thirsty)
            {
                if (_status[status] == 0)
                {
                    continue;
                }
            }
            _status[status]++;
            DeathCheck(status);

        }
      
    }
    private void DeathCheck(Define.CharacterStatus status)
    {
        switch (status)
        {
            case Define.CharacterStatus.Panic:
                return;
            case Define.CharacterStatus.Hungry or Define.CharacterStatus.Thirsty:
            {
                if (_status[status] >= 5)
                    _isAlive = false;
                break;
            }
            default:
            {
                if (_status[status] >= 7)
                    _isAlive = false;
                break;
            }
        }
    }
    public List<string> StatusText { get; set; } = new List<string>();
    public List<string> DisplayStatusText { get; set; }= new List<string>();
    public bool GetIsAlive => _isAlive;
    public void Awake()
    {
        _sprite = GetComponent<Sprite>();
        _isAlive = true;
        foreach (Define.CharacterStatus status in Enum.GetValues(typeof(Define.CharacterStatus)))
        {
            _status.Add(status,0);
        }

        this.ObserveEveryValueChanged(_ => _isAlive)
            .Where(x => x == false).Subscribe(_ =>
            {
                Managers.Game.AddTextNote($"{_name} 이가 사망하였습니다.");
                Managers.Game.DeathCharacter(this);    
            });
    }

    public string SetName(string value) => this._name = value;
    

    public string GetName() => _name;

}
