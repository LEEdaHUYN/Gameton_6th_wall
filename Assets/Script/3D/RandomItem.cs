using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;


namespace dahyeon
{

    public class RandomItem : MonoBehaviour
    {

        public Objectitem[] Spots;
        public NomalItem[] ItemsList;

        List<int> numberList = new List<int>();

        private void Awake()
        {

        }
        void Start()
        {
            Spots = this.GetComponentsInChildren<Objectitem>();

            CreateUnDuplicateRandom(0, ItemsList.Length);

            for (int i = 0; i < numberList.Count; i++)
            {
                Debug.Log("현재 랜덤숫자 : " + numberList[i]);
                Spots[i].iteminObjectitem = ItemsList[numberList[i]];
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        void CreateUnDuplicateRandom(int min, int max)
        {
            int currentNumber = Random.Range(min, max);

            for (int i = 0; i < max;)
            {
                if (numberList.Contains(currentNumber))
                {
                    currentNumber = Random.Range(min, max);
                }
                else
                {
                    numberList.Add(currentNumber);
                    i++;
                }
            }
        }
    }
}
