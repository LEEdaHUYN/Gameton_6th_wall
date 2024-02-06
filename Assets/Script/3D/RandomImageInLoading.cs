using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomImageInLoading : MonoBehaviour
{
    public List<Sprite> bgimages = new List<Sprite>();
    void Start()
    {
        int RandomNumber = Random.Range(0, bgimages.Count);
        this.GetComponent<Image>().sprite = bgimages[RandomNumber];
    }

    // Update is called once per frame
    void Update()
    {
    
    }
}
