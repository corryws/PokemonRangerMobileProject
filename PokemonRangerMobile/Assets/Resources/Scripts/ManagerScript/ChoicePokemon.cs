using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChoicePokemon : MonoBehaviour
{
    public DatabasePokemon dbpokemon;
    public Dropdown dropdown_tactis;
    public int randompokemon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DropDownTatticsUpdate();
    }

    public void DropDownTatticsUpdate()
    {
        int value;
        value = dropdown_tactis.value;
        GameManager.take_player_tactics = dropdown_tactis.options[value].text;
    }

    public void RandomEncounter()
    {
        randompokemon = Random.Range(0,dbpokemon.AllArraylength);
        GameManager.take_enemy_name        = dbpokemon.name           [randompokemon];
        GameManager.take_enemy_speed       = dbpokemon.pkm_speed      [randompokemon];
        GameManager.take_enemy_time_attack = dbpokemon.bullet_time_spd[randompokemon];
        GameManager.take_enemy_circlemax   = dbpokemon.circlemax      [randompokemon];
        GameManager.take_enemy_type        = dbpokemon.type           [randompokemon].ToString();
        GameManager.take_enemy_weak        = dbpokemon.weakness       [randompokemon].ToString();
    }

    public void OnCatchButtonClick()
    {
        RandomEncounter();
        Application.LoadLevel(1);
    }

   
}
