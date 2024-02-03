using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public abstract class UI_Base : MonoBehaviour
{
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();
    protected bool _init = false;

    // 일단 abstract에서 virtual로 변경 기능 동작하게 작성
    public virtual bool Init()
    {
        if (_init)
            return false;

        _init = true;
        return true;
    }

    private void Awake()
    {
        Init();
    }

    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        // Bind 문 구현 로직 자체가 Find를 빈번히 일으켜서 뺴고 싶긴 한데.
        // 아직 까진 다른 방법은 떠오르지 않음. 
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = Utils.FindChild(gameObject, names[i], true);
            else
                objects[i] = Utils.FindChild<T>(gameObject, names[i], true);

            if (objects[i] == null)
                Debug.Log($"Failed to bind({names[i]})");
        }
    }

    protected void BindObject(Type type) { Bind<GameObject>(type); }
    protected void BindImage(Type type) { Bind<Image>(type); }
    protected void BindText(Type type) { Bind<TMP_Text>(type); }
    protected void BindButton(Type type) { Bind<Button>(type); }
    protected void BindToggle(Type type) { Bind<Toggle>(type); }
    protected void BindSlider(Type type) { Bind<Slider>(type); }


    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[idx] as T;
    }

    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }
    protected TMP_Text GetText(int idx) { return Get<TMP_Text>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }
    protected Toggle GetToggle(int idx) { return Get<Toggle>(idx); }
    protected Slider GetSlider(int idx) { return Get<Slider>(idx); }


    public static void BindEvent(GameObject go, Action triggerAction = null, Action<BaseEventData> dragAction = null, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_EventHandler evt = Utils.GetOrAddComponent<UI_EventHandler>(go);
        // 이벤트가 더 추가 될 것 같지 않아서 그대로 써도 괜찮을 것 같음 다만, 계속해서 뭔가 더 추가 될 가능성이 보이면 switch 문 자체를 리팩토링 해야할 것 같긴 함.
        switch (type)
        {
            case Define.UIEvent.Click:
                evt.OnClickHandler -= triggerAction;
                evt.OnClickHandler += triggerAction;
                break;
            case Define.UIEvent.Preseed:
                evt.OnPressedHandler -= triggerAction;
                evt.OnPressedHandler += triggerAction;
                break;
            case Define.UIEvent.PointerDown:
                evt.OnPointerDownHandler -= triggerAction;
                evt.OnPointerDownHandler += triggerAction;
                break;
            case Define.UIEvent.PointerUp:
                evt.OnPointerUpHandler -= triggerAction;
                evt.OnPointerUpHandler += triggerAction;
                break;
            case Define.UIEvent.Drag:
                evt.OnDragHandler -= dragAction;
                evt.OnDragHandler += dragAction;
                break;
            case Define.UIEvent.BeginDrag:
                evt.OnBeginDragHandler -= dragAction;
                evt.OnBeginDragHandler += dragAction;
                break;
            case Define.UIEvent.EndDrag:
                evt.OnEndDragHandler -= dragAction;
                evt.OnEndDragHandler += dragAction;
                break;
        }
    }

    public void PopupOpenAnimation(GameObject contentObject) // 팝업 오픈 연출
    {
        contentObject.transform.localScale = new Vector3(0.8f, 0.8f, 1);
        contentObject.transform.DOScale(1f, 0.1f).SetEase(Ease.InOutBack).SetUpdate(true);
    }
}