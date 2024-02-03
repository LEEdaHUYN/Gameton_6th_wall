using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dahyeon
{
    [CreateAssetMenu]
    public class Item : ScriptableObject
    {
        public string itemName;
        public Sprite itemImage;
        public GameObject itemPrefab;
    }
}

