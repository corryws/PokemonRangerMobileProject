using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDSCriptManager : MonoBehaviour
{
    [Header("HUD VARIABLE PLAYER")]
    Player player;
    public Slider player_hp_sld;
    public Slider player_tactics_sld;
    public Slider player_exp_sld;
    public Text   player_hp_txt;
    Text player_exp_txt;

    [Header("HUD VARIABLE POKEMON")]

    public Text  pokemon_name_txt;
    public Image pokemon_type_one_img,pokemon_type_two_img;
    
    public GameObject[]  pokemons;

    [Header("GENERAL HUD VARIABLE")]
    public GameObject EndBattleBorder;
    public Button TacticsButton;
    public Image  type_tactics_image;

    int newvalue =0;

    void Start()
    {
        player  = GameObject.FindWithTag("Player") .GetComponent<Player>();
        pokemons= GameObject.FindGameObjectsWithTag("Pokemon");

        player_exp_txt = EndBattleBorder.transform.GetChild(1).GetComponent<Text>();
        player_hp_sld     .maxValue      = player.life = player.maxlife;
        player_tactics_sld.maxValue      = player.tactics_energy;
        player_exp_sld    .maxValue      = player.styler_maxexp;

        string typetactics;
        if(GameManager.take_player_tactics != null) 
        typetactics = GameManager.take_player_tactics.ToLower();
        else typetactics = "normal";

        type_tactics_image.sprite   = Resources.Load<Sprite>("Sprites&Images/Pokemon_type_icon/"+typetactics+"_type");

        if(pokemons.Length > 1)
        {
            pokemon_type_one_img.gameObject.SetActive(false);
            pokemon_type_two_img.gameObject.SetActive(false);
            pokemon_name_txt.text = "GRUPPO POKEMON";
        }else
        {
            PokemonSpawnSetting pokemonspwn = pokemons[0].GetComponent<PokemonSpawnSetting>();
            pokemon_type_one_img.sprite     = Resources.Load<Sprite>("Sprites&Images/Pokemon_type_icon/"+pokemonspwn.type_one+"_type"); 
            pokemon_type_two_img.sprite     = Resources.Load<Sprite>("Sprites&Images/Pokemon_type_icon/"+pokemonspwn.type_two+"_type");
            pokemon_name_txt.text           = pokemonspwn.name.ToUpper();
        }    
    }

    void FixedUpdate()
    {
        player_hp_sld.value = player.life;
        player_hp_txt.text  = player.life + "/"+ player.maxlife;

        foreach(GameObject pokemon in pokemons)
        {
            PokemonSpawnSetting pokemonspwn = pokemon.GetComponent<PokemonSpawnSetting>();
            Image pokemon_hp_sld = pokemon.gameObject.transform.GetChild(4).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Image>();
            pokemon_hp_sld.fillAmount    = pokemonspwn.circle/pokemonspwn.circlemax;
            pokemonspwn.FriendlyBarTimer();
        }
    
        CheckTacticsStatus();
        CheckLifeStatus();
    }

    public void CheckTacticsStatus()
    {
        if(player_tactics_sld.value == player.tactics_energy)
            TacticsButton.interactable = true;
        else TacticsButton.interactable = false;
    }

    public void setEndBattleBorder(bool activate){EndBattleBorder.SetActive(activate);}
    public void setEndBattleBorderComponent(string first,string second)
    {
        EndBattleBorder.transform.GetChild(0).
                        transform.GetChild(0).
                        GetComponent<Text>().text = first;
    }

    public IEnumerator IncrementExpSlide(int value)
    {
        GameObject conatinerbutton = EndBattleBorder.transform.GetChild(3).gameObject;
        if(value > 0)
        {
            conatinerbutton.SetActive(false);
            player_exp_txt.text = ""+(value-1);
            player_exp_sld.value+=1;
            yield return new WaitForSeconds(0.09f);

            if(player_exp_sld.value+1 > player_exp_sld.maxValue)
            {
                int newvalue = (int)player_exp_sld.value - (int)player_exp_sld.maxValue;
                player_exp_txt.text = ""+(newvalue);
                player_exp_sld.maxValue = player.styler_maxexp;
                player_exp_sld.value = 0;
                yield return new WaitForSeconds(0.5f);
                StartCoroutine(IncrementExpSlide(newvalue));
            }
            StartCoroutine(IncrementExpSlide(value-1));
        }else conatinerbutton.SetActive(true);
        
    }

    //Following method increment tactics value by one
    public void EnergyTacticsIncrement(){player_tactics_sld.value += 1;}
    //Following method reset tactics value 
    public void EnergyTacticsReset    (){player_tactics_sld.value  = 0;}

     //Following method color health bar in base to Damage
    public void CheckLifeStatus()
    {
        int lf   = player.life;
        int mxlf = player.maxlife;

        if(lf > mxlf/2) 
         player_hp_sld.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color 
         = new Color(0,255,0);

        if(lf <= (mxlf*50)/100) 
         player_hp_sld.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color 
         = new Color(255,255,0);

        if(lf <= (mxlf*20)/100)
         player_hp_sld.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color 
         = new Color(255,0,0);
    }

    

    //Following method Run from Battle
    public void OnRunButton(){Application.LoadLevel(1);}
    //Following method confrim the back button 
    public void OnAnnulBtn (){Application.LoadLevel(1);}

    //Following method confrim the continue button 
    public void OnConfirmBtn()
    {
        setEndBattleBorder(false);
        EnergyTacticsReset();

        foreach(GameObject pokemon in pokemons)
        {
            PokemonSpawnSetting pokemonspwn = pokemon.GetComponent<PokemonSpawnSetting>();
            pokemonspwn.circle = 0;

            pokemon.GetComponent<PokemonMovement>().isCatch = false;
            pokemon.GetComponent<PokemonMovement>().isPause = false;
            pokemon.GetComponent<PokemonMovement>().setIdleState();
            pokemon.SetActive(true);
            pokemon.GetComponent<PokemonMovement>().PokemonCheckState();
        } 
        //_plytattics.SetIconTactics(_plytattics.choice_effect,false);
    }

    //Following method run all tactics function and showup tactics_popup
    public void OnPokeTacticsButton()
    {
        TacticsButton.GetComponent<Button>().interactable = false;
        Animator tacticsanim = GameObject.Find("Tactics_graphic").GetComponent<Animator>();
        tacticsanim.SetBool("show",true);
        GameObject.FindWithTag("Player").GetComponent<PokeTattics>().SetTime();
        EnergyTacticsReset();
    }
}
