using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchField : MonoBehaviour
{
    public FixedTouchField _FixedTouchField;
    public Charcontroll _CharControll;

    private void Start()
    {
        _CharControll = this.GetComponent<Charcontroll>();
    }
    private void Update()
    {
        _CharControll.LockAxis = _FixedTouchField.TouchDist;
    }
}
