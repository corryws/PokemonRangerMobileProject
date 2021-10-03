using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    //Styler Variable
    [HideInInspector]
    public int styler_lvl = 1, styler_exp,styler_maxexp,newexp, 
               maxlife,life,power,add_bonus,tactics_energy;
    [HideInInspector]
    public bool bonus_nohit;
    [HideInInspector]
    public double exp_factor;
    public string tattica_scelta;
    //maxexp = lvl + energy(o potenza) * 4
    // 4 = 4 * 1,25
    //example 
    //lvl 1 energy 10 -> 1 + 10 * 4 = 44
    // 4 = 4* 1,25 = 5,25
    //lvl 2 energy 11 -> 2+11 * 5,25 = 68
    //lvl 3 energy 12 -> 3+13 * 5,25 = 84

    //exp = 10*number of pokemon + eventuali bonus(uguali a 1)
    //level up stats(power+1 and energy+1 or 2)

    public Slider hpslider,tacticsslider;
    public Text visualhptext;

    // Start is called before the first frame update
    void Start()
    {
        SetPlayerStats();
        SetPlayerLife();
        SetPlayerEnergytactics();
    }

    // Update is called once per frame
    void Update()
    {
        hpslider.value = life;
        visualhptext.text = life + "/" + maxlife;
        CheckLifeStatus();
    }

    //Following method set initial set of Player
    public void SetPlayerStats()
    {
        maxlife = 10;
        power = 10;
        exp_factor = 4;
        tactics_energy = 5;
        SetExpVariable();
    }

    //Following Increment stats and level by 1 
    public void IncrementLVL()
    {
        int maxrange_increase = 1;

        if(styler_lvl == 1) exp_factor = exp_factor+1.25; 
        styler_lvl += 1;
        
        maxlife+=Random.Range(1,maxrange_increase); power+=Random.Range(1,maxrange_increase);
        SetExpVariable();
    }

    //Following Increment and calculate the newexp and maxexp
    public void IncrementMaxExp()
    {
        while(newexp > -1)
        {
        if(styler_exp >= styler_maxexp)
        {
            IncrementLVL();
            SetExpVariable();
            newexp = styler_exp - styler_maxexp;
            styler_exp = 0;
            styler_exp += Mathf.Abs(newexp);
        }
        else if(newexp < styler_maxexp)
        {
            newexp = styler_exp - styler_maxexp;
            if(newexp > 0) styler_exp += Mathf.Abs(newexp);
        }
      }
      newexp = 0;
    }

    public void IncrementExp()
    {
        if(!bonus_nohit) add_bonus +=1;
        styler_exp += 10+add_bonus;
        IncrementMaxExp();
    }

    //Following method color health bar in base to Damage
    public void CheckLifeStatus()
    {
        GameObject fill = GameObject.Find("FillPlayer");
        if(life > maxlife/2)         fill.GetComponent<Image>().color = new Color(0,255,0);
        if(life <= (maxlife*50)/100) fill.GetComponent<Image>().color = new Color(255,255,0);
        if(life <= (maxlife*20)/100) fill.GetComponent<Image>().color = new Color(255,0,0);
    }

    //Following Set MAXEXP variable
    public void SetExpVariable(){styler_maxexp = (int)((styler_lvl + maxlife) * exp_factor);}

    //Following method set initial value of life
    public void SetPlayerLife(){hpslider.maxValue = life = maxlife;}

    //Following method take damage 
    public void SelfDamage(){life -= 1;}

    //Following method set initial value of energytactics
    public void SetPlayerEnergytactics(){tacticsslider.maxValue = tactics_energy;}

    //Following method increment tactics value by one
    public void EnergyTacticsIncrement(){tacticsslider.value += 1;}

    //Following method reset tactics value 
    public void EnergyTacticsReset(){tacticsslider.value = 0;}

}
