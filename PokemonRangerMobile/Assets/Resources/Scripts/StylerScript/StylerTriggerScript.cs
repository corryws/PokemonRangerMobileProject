using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StylerTriggerScript : MonoBehaviour
{
    public static GameObject this_instance;
    private GameObject OBJPlayer;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        if(this_instance == null)
        {
            this_instance = this.gameObject;
            OBJPlayer = GameObject.FindWithTag("Player");
            player = OBJPlayer.GetComponent<Player>();
        }else Destroy(this_instance);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    { 
        if(otherCollider.gameObject.tag == "Bullet" || otherCollider.gameObject.tag == "Obstacle")
        {   
            player.bonus_nohit = true;
            player.SelfDamage();
        }
    }

     
}
