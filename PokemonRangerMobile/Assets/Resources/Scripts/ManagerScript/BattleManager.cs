using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public GameObject PrefabPokemon,player_obj;
    Player              _player;
    HUDSCriptManager    _hudmanager;
    PokeTattics         _plytattics;
    int RandomPkmnSpawn=0,countpokemon=0;

    void Awake()
    {
        countpokemon=RandomPkmnSpawn = Random.Range(1,5);
        //countpokemon=RandomPkmnSpawn = 1;
        for(int i=0;i<RandomPkmnSpawn;i++)
        {
            Vector3 pkmpos = new Vector3(Random.Range(-2f,2f),Random.Range(-2f,2f),1f);
            Instantiate(PrefabPokemon,pkmpos,Quaternion.identity);
        }
        
        _hudmanager = GameObject.Find("HUDManager").GetComponent<HUDSCriptManager>();
        _plytattics = player_obj.GetComponent<PokeTattics>();
        _player     = player_obj.GetComponent<Player>();
        player_obj.SetActive(true);
        _hudmanager.setEndBattleBorder(false);
    }

    public void EndBattle(bool win)
    {
        player_obj.transform.GetChild(0).gameObject.GetComponent<DrawStyler>().DestroyElement();

        Animator _tacticsanim   = GameObject.Find("Tactics_graphic").GetComponent<Animator>();
        _plytattics.TacticsTime = 0f;
        _tacticsanim.SetBool("show",false);

        _hudmanager.setEndBattleBorder(true);
        countpokemon = RandomPkmnSpawn;

        if(win)//VINTO
        {
            int valueexp = 10+_player.add_bonus+RandomPkmnSpawn;
            _player.IncrementExp();
            StartCoroutine(_hudmanager.IncrementExpSlide(valueexp));
            _hudmanager.setEndBattleBorderComponent("POKEMON CATTURATO",""+(valueexp));
        }else//GAME OVER
        {
            _hudmanager.setEndBattleBorderComponent("GAME OVER","");
        }
    }

    public void CheckCatchPokemon(){if(--countpokemon<=0)EndBattle(true);}
}
