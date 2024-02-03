using Sirenix.OdinInspector;
using UnityEngine;

    public abstract class Selector : SerializedMonoBehaviour
    {
        public abstract void ShowCurrentDay();
        public abstract void NextDay();
    }
