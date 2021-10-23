using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    //Styler Variable
    [Header("VARIABILI GESTIONE STYLER")]
    public int range_grade = 0;
    public int styler_lvl = 1;
    public int styler_exp;
    public int styler_maxexp;
    public int newexp; 
    public int maxlife;
    public int life;
    public int power;
    public int add_bonus;
    public int tactics_energy;

    [Header("VARIABILI GESTIONE ESPERIENZA STYLER")]
    public bool bonus_nohit;
    public double exp_factor;
    HUDSCriptManager    _hudmanager;

    public Slider tacticsslider;

    // Start is called before the first frame update
    void Awake()
    {
         _hudmanager = GameObject.Find("HUDManager").GetComponent<HUDSCriptManager>();
        SetPlayerStats();
        SetExpVariable();
    }

    //Following method set initial set of Player
    public void SetPlayerStats()
    {
        life  = maxlife = 10;
        power = 10;
        exp_factor = 4;
        tactics_energy = 5;
    }

    //Following Increment stats and level by 1 
    public void IncrementLVL()
    {
        int maxrange_increase = 1;

        if(styler_lvl == 1) exp_factor = exp_factor+1.25; 
        styler_lvl += 1;
        
        maxlife+=Random.Range(1,maxrange_increase); 
        power  +=Random.Range(1,maxrange_increase);
        SetExpVariable();
    }

    public void SetIncrementME(int value)
    {
        if(value <= 0) return;

        styler_exp += 1;//increment by value

        if(styler_exp > styler_maxexp)///increment lvl and maxexp
        {
            int newvalue = styler_exp - styler_maxexp;
            Debug.Log(styler_exp + ">=" + styler_maxexp + "=>exp-maxexp " + (styler_exp-styler_maxexp));
            IncrementLVL();
            styler_exp = 0;
            SetIncrementME(newvalue);
        }

        SetIncrementME(value-1);
    }

    public void IncrementExp()
    {
        if(!bonus_nohit) add_bonus +=1;
        SetIncrementME(10+add_bonus);
    }

   public void LifeControl()
   {
       if(life <= 0)
        GameObject.Find("BattleManager").GetComponent<BattleManager>().EndBattle(false);
   }
    
    //Following Set MAXEXP variable
    public void SetExpVariable(){styler_maxexp = (int)((styler_lvl + maxlife) * exp_factor);}

    //Following method take damage 
    public void SelfDamage(){life -= 1;}



    //maxexp = lvl + energy(o potenza) * 4
    // 4 = 4 * 1,25
    //example 
    //lvl 1 energy 10 -> 1 + 10 * 4 = 44
    // 4 = 4* 1,25 = 5,25
    //lvl 2 energy 11 -> 2+11 * 5,25 = 68
    //lvl 3 energy 12 -> 3+13 * 5,25 = 84

    //exp = 10*number of pokemon + eventuali bonus(uguali a 1)
    //level up stats(power+1 and energy+1 or 2)

    //(exp)
    // 36 ------> 44(maxexp)
    //+11 ------> 44(maxexp) => 47(exp)
    //se 47 > 44 => 47-44 = 3
    //increment maxexp => 68
    //0--------->68
    //+3
    //3--------->68

    

}
