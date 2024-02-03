using System;

[Serializable]
public abstract class Condition
{
    public string Type { get; set; }
    public abstract bool CheckCondition();
}
