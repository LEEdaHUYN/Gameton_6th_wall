
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
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
    public void SetStatusValue(Define.CharacterStatus status, float value) => _status[status] = value;
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
    }

    public string SetName(string value) => this._name = value;
    

    public string GetName() => _name;

}
