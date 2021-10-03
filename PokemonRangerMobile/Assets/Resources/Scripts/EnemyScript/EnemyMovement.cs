using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed;
    public float startWaitTime;
    private float waitTime;
    private bool facingRight = false;
    
    public Transform moveSpot;
    public float minX,maxX;
    public float minY,maxY;

    public GameObject bullet,obstacle;
    public float timeBtwAttack;//time between attack to another
    public float StartTimeBtwAttack;
    

    //POKEMON HAVE 3 ATTACK, 1 BULLET , 2 SPAWN OBASTACLE, 3 ???

    // Start is called before the first frame update
    public void Start()
    {
        waitTime = startWaitTime;
        timeBtwAttack = StartTimeBtwAttack;
        moveSpot.position = new Vector2(Random.Range(minX,maxX),Random.Range(minY,maxY));
    }

    void Update()
    {
        EnemyFollowSpotToMove();
        SwitchAttack();
    }

    //    Following swtich and choice attack of a pokemon(option 1 2 3 4-noattack)
    public void SwitchAttack()
    {
        int percentofattack = Random.Range(0,100),numberattack;
        if(timeBtwAttack <= 0)
            {
                speed            = GameManager.take_enemy_speed;
                if(percentofattack < 50) numberattack = Random.Range(0,4);
                else numberattack = 3;
                    switch(numberattack)
                    {
                        case 0:
                            Debug.Log("attack 1");
                            FirstAttack();
                        break;
                        case 1:
                            Debug.Log("attack 2");
                            SecondAttack();
                        break;
                        case 2:
                            Debug.Log("attack 3");
                            speed = 0;
                        break;
                        case 3:
                            Debug.Log("NO ATTACK");
                        break;
                        default: return;
                    }
                timeBtwAttack = StartTimeBtwAttack;
            }else if(timeBtwAttack > 0 && timeBtwAttack < 999) timeBtwAttack -= Time.deltaTime;        
    }

    //    Following method spawn a object around map and pokemon follow him 
    public void EnemyFollowSpotToMove()
    {
        Animator animator = this.gameObject.GetComponent<Animator>();
        transform.position = Vector2.MoveTowards(transform.position, moveSpot.position, speed * Time.deltaTime);

        animator.SetFloat("Horizontal",Mathf.Round(moveSpot.position.x));
        animator.SetFloat("Vertical", moveSpot.position.y);

        float h = animator.GetFloat("Horizontal");

        if(Mathf.Round(h) >= 0 && !facingRight) FlipAnimation(); else if(Mathf.Round(h) < 0 &&  facingRight) FlipAnimation();  

        if(Vector2.Distance(transform.position, moveSpot.position) < 0.2f)
        {
            if(waitTime <= 0)
            {
                moveSpot.position = new Vector2(Random.Range(minX,maxX),Random.Range(minY,maxY)); waitTime = startWaitTime;
            }else waitTime -= Time.deltaTime;
        }   
    }

    //    Following method flip scale to left/right animation
    public void FlipAnimation()
    {
        facingRight = !facingRight;
        Vector2 Scale = transform.localScale;
        Scale.x *= -1;
        transform.localScale = Scale;
    }

    //    Following method is for the first Attack/bullet
    public void FirstAttack(){Instantiate(bullet, transform.position, Quaternion.identity);speed = 0;}

    //    Following method is for the second Attack/obstacle
    public void SecondAttack()
    {
        Vector2 obstaclepos = new Vector2(Random.Range(-2.5f,2.5f),Random.Range(-3.5f,3.5f));
        Vector2 obstaclescale = new Vector2(Random.Range(3,5),6);
        Instantiate(obstacle, obstaclepos, Quaternion.identity);obstacle.transform.localScale = obstaclescale;
        speed = 0;
    }
}
