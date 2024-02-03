using System;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Utils
{
    public static T ParseEnum<T>(string value, bool ignoreCase = true)
    {
        return (T)Enum.Parse(typeof(T), value, ignoreCase);
    }

    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }
    public static void BindEvent(this GameObject go, Action action = null, Action<BaseEventData> dragAction = null, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_Base.BindEvent(go, action, dragAction, type);
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false,
        bool includeInactive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            Transform transform = go.transform.Find(name);
            if (transform != null)
                return transform.GetComponent<T>();
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>(includeInactive))
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false,
        bool includeInactive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive, includeInactive);
        if (transform != null)
            return transform.gameObject;
        return null;
    }
   public static bool InRange(int value, int minValue, int maxValue)
    {
        return value >= minValue && value <= maxValue;
    }
    public static bool InRange(float value, float minValue, float maxValue)
    {
        return value >= minValue && value <= maxValue;
    }

    public static void CalculateCharacterStatusValue(Character character, Define.SetStatusAction statusAction,Define.CharacterStatus status, float value)
    {
        float calculateValue = character.GetStatusValue(status);
        switch (statusAction)
        {
            case Define.SetStatusAction.Add:
            calculateValue += value;
            break;
            case Define.SetStatusAction.Mod:
            calculateValue = value;
            break;
            case Define.SetStatusAction.Sub:
            calculateValue -= value;
            break;
        }
        character.SetStatusValue(status, calculateValue);
    }

}