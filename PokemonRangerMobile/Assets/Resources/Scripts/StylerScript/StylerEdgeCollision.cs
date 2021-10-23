using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StylerEdgeCollision : MonoBehaviour
{
    DrawStyler stylerline;

    void Awake()
    {
        stylerline = this.gameObject.transform.parent.gameObject.GetComponent<DrawStyler>();
    }

    void OnTriggerEnter2D (Collider2D otherCollider)
    {
        if(otherCollider.gameObject.tag == "Pokemon" 
        || otherCollider.gameObject.tag == "Bullet" 
        || otherCollider.gameObject.tag == "Obstacle") 
        {
            //Debug.Log(otherCollider.gameObject.name);
            stylerline.DestroyElement();
        }   
    }
}
