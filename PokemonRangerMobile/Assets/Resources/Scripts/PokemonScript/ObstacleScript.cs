using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
   float timetodespawn=1.5f;

    void Awake()
    {
        if(!GameObject.FindWithTag("Styler"))Destroy(gameObject);
    }
    void FixedUpdate()
    {
        CounterDespawn();
    }

    void CounterDespawn()
    {
        if(timetodespawn > 0) timetodespawn -= Time.deltaTime;
        else Destroy(gameObject);
    }
}
