using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StylerTriggerScript : MonoBehaviour
{
    Player player;
    DrawStyler stylerline;

    void Awake()
    {
        player     = this.gameObject.transform.parent.transform.parent.gameObject.GetComponent<Player>();
        stylerline = this.gameObject.transform.parent.gameObject.GetComponent<DrawStyler>();
    }

    void OnCollisionEnter2D (Collision2D otherCollider)
    {
        if(otherCollider.gameObject.tag == "Pokemon")
        {
            stylerline.DestroyElement();
        }
    }

     void OnTriggerEnter2D (Collider2D otherCollider)
    { 
        if(otherCollider.gameObject.tag == "Bullet" 
        || otherCollider.gameObject.tag == "Obstacle")
        {   
            player.bonus_nohit = true;
            player.SelfDamage();
            stylerline.DestroyElement();
        }
    }
}
