using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulbasaurMoveset : MonoBehaviour
{
    /*
    BULBASAUR MOVESET SCRIPT
    3 ATTACK:
    - ISTANTIATE BULLET
    - ISTANTIATE OBSTACLE
    - PECULIAR ATTACK
    FOLLOW METHOD
    */
    PokemonMovement _pkmnmovement;
    PokemonSpawnSetting _pkmnsett;

    //VARIABILI GESTIONE MOVESPOT
    Transform moveSpot;
    float minX=-3f,maxX=3f,minY=-3.5f,maxY=3.5f;
    int count=0;

    void Awake()
    {
        _pkmnmovement = this.gameObject.GetComponent<PokemonMovement>    ();
        _pkmnsett     = this.gameObject.GetComponent<PokemonSpawnSetting>();
        moveSpot      = GameObject.Find("MoveSpot").transform;

        moveSpot.position = new Vector2(Random.Range(minX,maxX),Random.Range(minY,maxY));
    }

    public void getFollowFunction(){StartCoroutine(Follow_Move());}
    public void getAttackFunction(int id)
    {
        if(id==1)StartCoroutine(Attack_one());
        else if(id == 2)StartCoroutine(Attack_two());
        else StartCoroutine(Attack_three());
    }

    IEnumerator Attack_one()
    {
        //Debug.Log("attack 1");
        Instantiate(_pkmnmovement.bullet, this.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(2f);
        _pkmnmovement.setIdleState();
        _pkmnmovement.PokemonCheckState();
    }

    IEnumerator Attack_two()
    {
        //Debug.Log("attack 2");
        Vector2 obstaclepos = new Vector2(Random.Range(-2.5f,2.5f),Random.Range(-3.5f,3.5f)),
        obstaclescale = new Vector2(Random.Range(3,5),6);
        Instantiate(_pkmnmovement.obstacle, obstaclepos, Quaternion.identity);
        _pkmnmovement.obstacle.transform.localScale = obstaclescale;
        yield return new WaitForSeconds(2f);
        _pkmnmovement.setIdleState();
        _pkmnmovement.PokemonCheckState();
    }

    IEnumerator Attack_three()
    {
        //Debug.Log("attack 3");
        yield return new WaitForSeconds(2f);
        _pkmnmovement.setIdleState();
        _pkmnmovement.PokemonCheckState();
    }

    IEnumerator Follow_Move()
    {
        Animator _animpkmn  = this.gameObject.GetComponent<Animator>();
        if(Vector2.Distance(transform.position, moveSpot.position) >= 0.2f)
        {
            float step = _pkmnsett.pkm_speed * Time.deltaTime;

            transform.position = Vector2.MoveTowards(transform.position, moveSpot.position, step);

            yield return new WaitForSeconds(0.007f);
            StartCoroutine(Follow_Move());
        }else
        {
            moveSpot.position = new Vector2(Random.Range(minX,maxX),Random.Range(minY,maxY));
            yield return new WaitForSeconds(0.05f);
            _pkmnmovement.setIdleState();
            _pkmnmovement.PokemonCheckState();
        }  
    }
}
