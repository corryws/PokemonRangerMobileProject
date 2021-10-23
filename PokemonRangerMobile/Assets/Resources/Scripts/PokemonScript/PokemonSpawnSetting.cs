using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokemonSpawnSetting : MonoBehaviour
{
    Pokemon pokemon;
    public float circlemax,circle;
    public float pkm_speed, bullet_time_spd,timecircle,StartTimeCircle;
    public string name,type_one,type_two,setScript;

    void Awake()
    {
       int Randompkm = Random.Range(1,7);
       //int Randompkm = 1;
       SetupPokemonParameter(Randompkm);
    }

    void SetupPokemonParameter(int Randompkm)
    {
        pokemon         = Resources.Load<Pokemon>("ScriptableObjects/"+Randompkm);
        name            = pokemon.name;
        circlemax       = (pokemon.circlemax*10);
        pkm_speed       = pokemon.pkm_speed;
        bullet_time_spd = pokemon.bullet_time_spd;
        type_one        = pokemon.getTypeOne();
        type_two        = pokemon.getTypeTwo();
        StartTimeCircle = 10;
        timecircle      = StartTimeCircle;
        circle          = 0;
        setScript       = name + "Moveset";

        this.gameObject.AddComponent(System.Type.GetType(setScript));
        this.gameObject.GetComponent<Animator>().runtimeAnimatorController = pokemon.ThisPokemonAnimator;
    }

    public void FriendlyBarTimer()
    {
        if(timecircle <= 0 && circle > 0)
        {
            if(circle <= 0.9) 
            {
                circle=0;
                timecircle = StartTimeCircle;
            }else 
            {
                timecircle = 0;
                circle-=0.05f;
            }
            
        }else if(timecircle > 0 && timecircle < 999)timecircle -= Time.deltaTime;
    }

     //    Following method is called by DrawStyler when the pokemon is inside the circle
    public void InsideCircle()
    {
        if(circle > circlemax && !this.gameObject.GetComponent<PokemonMovement>().isCatch)
        {
            this.gameObject.GetComponent<PokemonMovement>().isCatch = true;
            GameObject.Find("BattleManager").GetComponent<BattleManager>().CheckCatchPokemon();

            this.gameObject.GetComponent<PokemonMovement>().isPause = true;
            this.gameObject.GetComponent<PokemonMovement>().setPauseState();
            this.gameObject.GetComponent<PokemonMovement>().PokemonCheckState();
        }else
        {
             Player _player = GameObject.FindWithTag("Player").GetComponent<Player>();
            circle    += _player.power;//Random.Range(1,2);
            timecircle = StartTimeCircle;

            GameObject.FindWithTag("Player").GetComponent<PokeTattics>().SplitFunctionTacticsEffect();
            GameObject.Find("HUDManager").GetComponent<HUDSCriptManager>().EnergyTacticsIncrement();
        }
    }
}
