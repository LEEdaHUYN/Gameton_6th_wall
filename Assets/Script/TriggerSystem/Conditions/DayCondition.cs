using System;
using UnityEngine;

public class DayCondition : Condition
{

    public int startDay;
    public int endDay;
    public override bool CheckCondition()
    {
        return Utils.InRange(Managers.Game.CurrentDay, startDay, endDay);
    }
}
