using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouletteController : MonoBehaviour
{
    //1 sodlfcoin, 2 dia, 3 key
    float rotspeed = 0f;
    // Start is called before the first frame update
    public Image[] CheckPanel;
    void Start()
    {
        
    }

    public void TrunRoulette()
    {
        Managers.Ad.RunRewardedAd(() => { this.rotspeed = 40f; },Admob.rulletId); 
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.Rotate(0, 0, this.rotspeed);
        this.rotspeed *= 0.99f;
    }

}
