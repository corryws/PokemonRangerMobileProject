using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokeTattics : MonoBehaviour
{
    //type dati dall'utente
    //effect e descrizioni sono date dalle varie funzioni per tipo
    public enum Effect {Null,Pause, Tired, Slow, Stop, Confused};

    [Header("TACTCIS VARIABLE")]
    public Effect CurrentEffect;
    public string choice_tactics_type;
    public float TacticsTime=0f;

    public bool isActive;

    void FixedUpdate(){TacticsCountDown();}

//Following method split effect when start battle
    void SplitTacticsEffect(string choice_tactics_type)
    {
        if(choice_tactics_type == "Null")                                         CurrentEffect = Effect.Null;

        if(choice_tactics_type == "Grass"    || choice_tactics_type == "Flying"   || choice_tactics_type == "Water"  ||
           choice_tactics_type == "Rock"     || choice_tactics_type == "Bug")     CurrentEffect = Effect.Slow;

        if(choice_tactics_type == "Electric" || choice_tactics_type == "Ice"      || choice_tactics_type == "Psychic")
                                                                                  CurrentEffect = Effect.Stop;

        if(choice_tactics_type == "Fire"     || choice_tactics_type == "Poison"   ||
           choice_tactics_type == "Ghost"    || choice_tactics_type == "Dragon")  CurrentEffect = Effect.Tired;

        if(choice_tactics_type == "Ground"   || choice_tactics_type == "Steel")   CurrentEffect = Effect.Pause;

        if(choice_tactics_type == "Normal"   || choice_tactics_type == "Fighting" || 
           choice_tactics_type == "Dark"     || choice_tactics_type == "Fairy")   CurrentEffect = Effect.Confused;
    }

//Following method split tactics effect function when pokemon is inside a circle
    public void SplitFunctionTacticsEffect()
    {
        if(CurrentEffect == Effect.Slow)          SlowEffect("slow"); 
        else if(CurrentEffect == Effect.Stop)     StopEffect("stop");          
        else if(CurrentEffect == Effect.Tired)    TiredEffect("tired");          
        else if(CurrentEffect == Effect.Pause)    PauseEffect("pause");            
        else if(CurrentEffect == Effect.Confused) ConfusedEffect("confused");   
    }

//Following method is SlowEffect
    public void SlowEffect(string choice_effect)//POKEMON IS MORE SLOW
    {
        if(TacticsTime > 0)
        {
            SetIconTactics(choice_effect,true);
            pokemon.GetComponent<PokemonSpawnSetting>().pkm_speed =
            (pokemon.GetComponent<PokemonSpawnSetting>().pkm_speed*25)/100;
        }
    }

//Following method is StopEffect
    public void StopEffect(string choice_effect)//POKEMON NOT MOVE AND NOT ATTACK
    {
        if(TacticsTime > 0)
        {
            SetIconTactics(choice_effect,true);
            pokemon.GetComponent<PokemonMovement>().setPauseState();
        }
    }

//Following method is TiredEffect
    public void TiredEffect(string choice_effect)//STOP ENEMY FRIEND HEALTH BAR(NON SCENDE)
    {
        if(TacticsTime > 0)
        {
            SetIconTactics(choice_effect,true);
            pokemon.GetComponent<PokemonSpawnSetting>().timecircle = 999;
        }
    }

//Following method is PauseEffect
    public void PauseEffect(string choice_effect)//POKEMON NOT ATTACK
    {
        if(TacticsTime > 0)
        {
            SetIconTactics(choice_effect,true);
            pokemon.GetComponent<PokemonMovement>().setPauseState();
        }
    }

//Following method is ConfusedEffect
    public void ConfusedEffect(string choice_effect)
    {
        if(TacticsTime > 0) SetIconTactics(choice_effect,true);
    }

//Folowing method enabled animation icon of tacticseffect
    public void SetIconTactics(string name,bool enable)
    {
        /*pokemon.transform.GetChild(0).gameObject.SetActive(enable);
        Animator anim = pokemon.transform.GetChild(0).GetComponent<Animator>();
        anim.SetBool(name,enable);*/
    }

    //Following method set Effect Time Duration
    public void SetTime()
    {
        TacticsTime = 8f;
        SplitTacticsEffect(GameManager.take_player_tactics);
    }

    //Following methos simplify the countdown code
    public void ZeroTime()
    {
        Animator tacticsanim = GameObject.Find("Tactics_graphic").GetComponent<Animator>();
        bool isOpen       = animator.GetBool("show");
        tacticsanim.SetBool("show",!isOpen);
        //SetIconTactics  ("slow",false);
    }

    //Following method is the effective countdown duration to Effect
    public void TacticsCountDown()
    {   
        Text timestring = GameObject.Find("timer_text").GetComponent<Text>();
        if(TacticsTime > 0)
        {
            if(timestring != null)
            {
                TacticsTime -= Time.deltaTime;
                timestring.text = "TIME LEFT "+(TacticsTime).ToString("0");

                if(TacticsTime <= 0) ZeroTime();
            }
        }
        
    }
}
