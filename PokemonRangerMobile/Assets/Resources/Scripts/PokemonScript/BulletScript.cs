using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    public float speed;

    Transform styler;
    Vector2 target;

    // Start is called before the first frame update
    void Awake()
    {
        if(GameObject.FindWithTag("Styler")!= null)
        {
            styler = GameObject.FindWithTag("Styler").transform;
            target = new Vector2(styler.position.x, styler.position.y); 
        } else Destroy(gameObject);
    }

    void FixedUpdate(){FollowPlayer(); }

    //    Following method set the position for follow player by bullet
    void FollowPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position,target, speed * Time.deltaTime);
        if(transform.position.x == target.x && transform.position.y == target.y) Destroy(gameObject);
    }

    //    Following method Detect if the BULLET collide with WALL
    void OnTriggerEnter2D(Collider2D otherCollider)
    {if(otherCollider.gameObject.tag == "wall")Destroy(gameObject);}
}
