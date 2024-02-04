using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace dahyeon
{

    public class Slot : MonoBehaviour
    {
        [SerializeField] Image image;
        [SerializeField] Image hand;

        private Item _item;

        private void Start()
        {

            Invoke("ShowHandSlot", 3f);
            //ShowHandSlot();
        }



        public void ShowHandSlot()
        {
            _item = null;
            image.color = new Color(1, 1, 1, 0);
            hand.color = new Color(1, 1, 1, 1);
        }
        public void ShowItemSlot(NomalItem item)
        {
            image.sprite = item.GetSprite;
            image.color = new Color(1, 1, 1, 1);
            hand.color = new Color(1, 1, 1, 0);
        }
        public Item item
        {
            get { return _item; }
            set
            {
                _item = value;

            }
        }
    }
}
