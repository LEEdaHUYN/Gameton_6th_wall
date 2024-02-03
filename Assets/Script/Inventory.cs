using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dahyeon
{

    public class Inventory : MonoBehaviour
    {
        public List<Item> Items;

        [SerializeField]
        private Transform slotParent;

        public Slot[] Slots;


        private void Awake()
        {
            Slots = slotParent.GetComponentsInChildren<Slot>();
        }
        public void AddItem(Item _item)
        {
            if (Items.Count < Slots.Length)
            {
                Items.Add(_item);
                int selectItemIndex = Items.Count - 1;
                Slots[selectItemIndex].ShowItemSlot(Items[selectItemIndex]);
            }
            else
            {
                print("������ ���� �� �ֽ��ϴ�.");
            }
        }

        internal void ClearSlot()
        {
            Items.Clear();
            foreach (var slot in Slots)
            {
                slot.ShowHandSlot();
            }
        }
    }
}