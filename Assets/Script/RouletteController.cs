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
        Managers.Ad.LoadRewardedAd(() => { this.rotspeed = 40f; }); 
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.Rotate(0, 0, this.rotspeed);
        this.rotspeed *= 0.99f;
    }

}
