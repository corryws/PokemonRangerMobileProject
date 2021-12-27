using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leviosa : MonoBehaviour
{
    /*
        This script is use for levitate an object or HUD object
    */
    [SerializeField]bool reversego=false;
    [SerializeField]bool UD=false,LR=false;//UP DOWN - LEFT RIGHT
    [SerializeField]float speed;
    float nextmove;

    // Update is called once per frame
    void FixedUpdate()
    {   
        if(Time.time > nextmove)
        {
            if(UD && !LR)
              if(reversego)transform.position = transform.position + new Vector3(0,speed,0);
                else transform.position = transform.position - new Vector3(0,speed,0);
            else if(LR && !UD)
              if(reversego)transform.position = transform.position + new Vector3(speed,0,0);
                else transform.position = transform.position - new Vector3(speed,0,0);

                nextmove = Time.time + 0.5f;
        }else reversego = !reversego;
    }
}
