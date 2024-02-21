using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteController : MonoBehaviour
{
    float rotspeed = 0f;
    // Start is called before the first frame update
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
