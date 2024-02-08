using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEditor;
using UnityEngine;

namespace dahyeon
{

    public class Objectitem : MonoBehaviour
    {
        [Header("������")]
        public Item iteminObjectitem;
        Vector3 position;
        Clock clockscript;

        private void Start()
        {
            //outline ���� �� �ð� ������

            //position = this.transform.GetChild(0).gameObject.transform.position;
            //iteminObjectitem.itemPrefab.transform.parent = this.transform;
            this.UpdateAsObservable().Select(_ => iteminObjectitem != null).DistinctUntilChanged().Subscribe(_ => ItemSetting());
        }
        private void ItemSetting()
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
            Instantiate(iteminObjectitem.GetPrefab, this.transform.GetChild(0).gameObject.transform).transform.parent = this.transform;
            iteminObjectitem.GetPrefab.transform.position = Vector3.zero;
        }
    }
}
