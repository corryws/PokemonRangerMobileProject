using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    private float timetodespawn;
    // Start is called before the first frame update
    void Start()
    {
        timetodespawn = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        CounterDespawn();
    }

    private void CounterDespawn()
    {
        if(timetodespawn > 0)
        {
            timetodespawn -= Time.deltaTime;
            if(timetodespawn <= 0) Destroy(gameObject);
        }
    }
}
