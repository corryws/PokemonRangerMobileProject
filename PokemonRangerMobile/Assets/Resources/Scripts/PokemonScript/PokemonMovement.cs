using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonMovement : MonoBehaviour
{
    public enum PokemonState{IDLE,FOLLOW,ATTACK,CATCH,PAUSE}

    [Header("VARIABILI GESTIONE IA")]
    public PokemonState state;
    public bool isPause,isCatch;
    Transform moveSpot;
    PokemonSpawnSetting pokemon;
    Animator _animpkmn;
    

   [Header("VARIABILI GESTIONE TIMER")]
    public float startWaitTime;
    public float StartTimeBtwAttack;
    public float waitTime,timeBtwAttack;
    public bool  facingRight = false;

    [Header("VARIABILI GESTIONE MOVES")]
    public GameObject bullet;
    public GameObject obstacle;

    GameObject  att_sign_icon;
    
    //POKEMON HAVE 3 ATTACK, 1 BULLET , 2 SPAWN OBASTACLE, 3 ???

    // Start is called before the first frame update
    public void Start()
    {
        pokemon          = this.GetComponent<PokemonSpawnSetting>();
        _animpkmn        = this.gameObject.GetComponent<Animator>();
        att_sign_icon    = this.gameObject.transform.GetChild(1).gameObject;
        moveSpot         = GameObject.Find("MoveSpot").transform;
        waitTime         = startWaitTime;
        timeBtwAttack    = StartTimeBtwAttack;

        PokemonCheckState();
    }

    public void FixedUpdate()
    {
        if(isCatch)StartCoroutine(Catch());
        if(isPause) state = PokemonState.PAUSE;
        else SetTopDownAnim();
    }

    public void setIdleState  (){state = PokemonState.IDLE  ;}
    public void setFollowState(){state = PokemonState.FOLLOW;}
    public void setAttackState(){state = PokemonState.ATTACK;}
    public void setCatchState (){state = PokemonState.CATCH ;}
    public void setPauseState (){state = PokemonState.PAUSE ;}

    public void PokemonCheckState()
    {
        switch(state)
        {
            case PokemonState.IDLE:
                att_sign_icon.SetActive(false);
                StartCoroutine(Idle());
            break;

            case PokemonState.FOLLOW:
                _animpkmn.SetBool("isFollow",true);
                Follow();
            break;

            case PokemonState.ATTACK:
                _animpkmn.SetBool("isAttack",true);
                Attack();
            break;

            case PokemonState.CATCH:
                Debug.Log("CATCH");
            break;

            case PokemonState.PAUSE:
                pause();
            break;
        }
    }

    IEnumerator Idle()
    {
        int change_state = Random.Range(0,100);
        _animpkmn.SetBool("isAttack",false);
        _animpkmn.SetBool("isFollow",false);
        
        yield return new WaitForSeconds(1f);
        if(change_state < 50) state = PokemonState.ATTACK;
        else state = PokemonState.FOLLOW;
        yield return new WaitForSeconds(1f);
        PokemonCheckState();
    }

    IEnumerator Catch()
    {
        _animpkmn.SetBool("isCatch" ,true);
        _animpkmn.SetBool("isAttack",false);
        _animpkmn.SetBool("isFollow",false);
        yield return new WaitForSeconds(1f);
        this.gameObject.SetActive(false);
    }

    void pause()
    {
        _animpkmn.SetBool("isAttack",false);
        _animpkmn.SetBool("isFollow",false);
    }

    void Attack()
    {
        int numberattack = Random.Range(0,3);
        att_sign_icon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites&Images/Pokemon_effect_icon/"+(numberattack+1)+"_!");
        att_sign_icon.SetActive(true);
        BroadcastMessage("getAttackFunction",(numberattack+1),SendMessageOptions.DontRequireReceiver);      
    }

    void Follow()
    {
        BroadcastMessage("getFollowFunction",SendMessageOptions.DontRequireReceiver);
    }

    //    Following method flip scale to left/right animation
    void FlipAnimation()
    {
        facingRight        = !facingRight;
        Vector2 Scale      = transform.localScale,
                Scalechild = transform.GetChild(4).localScale;
        Scale.x      *= -1;
        Scalechild.x *= -1;
        transform.localScale = Scale;
        Scalechild = transform.GetChild(4).localScale = Scalechild;
    }

    void SetTopDownAnim()
    {
        _animpkmn.SetFloat("Horizontal",Mathf.Round(moveSpot.position.x));
        _animpkmn.SetFloat("Vertical",moveSpot.position.y);

        float h = _animpkmn.GetFloat("Horizontal");

        if(facingRight){
            if(Mathf.Round(h) < 0) FlipAnimation(); 
        }else{
            if(Mathf.Round(h) >= 0) FlipAnimation(); 
        }
    }

}
