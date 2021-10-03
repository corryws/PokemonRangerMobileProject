using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokeTattics : MonoBehaviour
{

    //type dati dall'utente
    //effect e descrizioni sono date dalle varie funzioni per tipo

    public string choice_tactics_type,choice_effect;
    public float TacticsTime;
    public enum getEffect {Null,Pause, Tired, Slow, Stop, Confused};
    public getEffect CurrentEffect;
    public Image type_tactics_image;

    private GameObject OBJenemy;
    private Enemy enemy;
    private EnemyMovement enemymov;
   
    // Start is called before the first frame update
    void Start()
    {   
        OBJenemy                  = GameObject.FindWithTag("Pokemon");
        enemy                     = OBJenemy.GetComponent<Enemy>();
        enemymov                  = OBJenemy.GetComponent<EnemyMovement>();
        choice_tactics_type       = GameManager.take_player_tactics;
        choice_effect             = "";
        type_tactics_image.sprite = Resources.Load<Sprite>("Sprites/Pokemon_type_icon/"+choice_tactics_type.ToLower()+"_type"); 

        SplitTacticsEffect();
    }

    // Update is called once per frame
    void Update()
    {
        TacticsCountDown();
    }

    //Following method split effect when start battle
    void SplitTacticsEffect()
    {
        if(choice_tactics_type == "Null")                                         CurrentEffect = getEffect.Null;

        if(choice_tactics_type == "Grass"    || choice_tactics_type == "Flying"   || choice_tactics_type == "Water"  ||
           choice_tactics_type == "Rock"     || choice_tactics_type == "Bug")     CurrentEffect = getEffect.Slow;

        if(choice_tactics_type == "Electric" || choice_tactics_type == "Ice"      || choice_tactics_type == "Psychic")
                                                                                  CurrentEffect = getEffect.Stop;

        if(choice_tactics_type == "Fire"     || choice_tactics_type == "Poison"   ||
           choice_tactics_type == "Ghost"    || choice_tactics_type == "Dragon")  CurrentEffect = getEffect.Tired;

        if(choice_tactics_type == "Ground"   || choice_tactics_type == "Steel")   CurrentEffect = getEffect.Pause;

        if(choice_tactics_type == "Normal"   || choice_tactics_type == "Fighting" || 
           choice_tactics_type == "Dark"     || choice_tactics_type == "Fairy")   CurrentEffect = getEffect.Confused;
    }

    //Following method split tactics effect function when pokemon is inside a circle
    public void SplitFunctionTacticsEffect()
    {
        if(CurrentEffect == getEffect.Slow)     SlowEffect(); 
        if(CurrentEffect == getEffect.Stop)     StopEffect();          
        if(CurrentEffect == getEffect.Tired)    TiredEffect();          
        if(CurrentEffect == getEffect.Pause)    PauseEffect();            
        if(CurrentEffect == getEffect.Confused) ConfusedEffect();   
    }

    //Following method is SlowEffect
    public void SlowEffect()//POKEMON IS MORE SLOW
    {
        choice_effect = "slow";
        if(TacticsTime > 0)
        {
            SetIconTactics(choice_effect,true);
            enemymov.speed = (GameManager.take_enemy_speed*25)/100;
        }
    }

    //Following method is StopEffect
    public void StopEffect()//POKEMON NOT MOVE AND NOT ATTACK
    {
        choice_effect = "stop";
        if(TacticsTime > 0)
        {
            SetIconTactics(choice_effect,true);
            enemymov.speed=0; enemymov.StartTimeBtwAttack = 999;
        }
    }

    //Following method is TiredEffect
    public void TiredEffect()//STOP ENEMY FRIEND HEALTH BAR(NON SCENDE)
    {
        choice_effect = "tired";
        if(TacticsTime > 0)
        {
            SetIconTactics(choice_effect,true);
            enemy.timecircle = 999;
        }
    }

    //Following method is PauseEffect
    public void PauseEffect()//POKEMON NOT ATTACK
    {
        choice_effect = "pause";
        if(TacticsTime > 0)
        {
            SetIconTactics(choice_effect,true);
            enemymov.StartTimeBtwAttack = 999;
        }
    }

    //Following method is ConfusedEffect
    public void ConfusedEffect()
    {
        choice_effect = "confused";
        if(TacticsTime > 0)
        {
            SetIconTactics(choice_effect,true);
        }
    }

    //Folowing method enabled animation icon of tacticseffect
    public void SetIconTactics(string name,bool enable)
    {
        OBJenemy.transform.GetChild(0).gameObject.SetActive(enable);
        Animator anim = OBJenemy.transform.GetChild(0).GetComponent<Animator>();
        anim.SetBool(name,enable);
    }

    //Following method set Effect Time Duration
    public void SetTime(){TacticsTime = 8f;}

    //Following methos simplify the countdown code
    public void ZeroTime()
    {
        Animator animator = GameObject.Find("Tactics_graphic").GetComponent<Animator>();
        bool isOpen       = animator.GetBool("show");
        SetIconTactics  (choice_effect,false);
        animator.SetBool("show",!isOpen);
        enemy.Set_enemy_parameters();
        enemymov.Start();
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
