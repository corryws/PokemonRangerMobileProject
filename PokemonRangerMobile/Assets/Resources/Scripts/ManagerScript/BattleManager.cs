using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{

    public  Text increment_text;
    public  GameObject EndBattleBorder,TacticsButton;
    private GameObject OBJenemy;
    private GameObject OBJPlayer;
    private GameObject OBJstyler;
    Player player;
    PokeTattics poketattics;
    Enemy enemy;

    // Start is called before the first frame update
    void Start()
    {
        OBJenemy      = GameObject.FindWithTag("Pokemon");
        OBJPlayer     = GameObject.FindWithTag("Player");
        OBJstyler     = GameObject.FindWithTag("StylerController");
        TacticsButton = GameObject.Find("TacticsButton");
        enemy         = OBJenemy.GetComponent<Enemy>();
        player        = OBJPlayer.GetComponent<Player>();
        poketattics   = OBJPlayer.GetComponent<PokeTattics>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.tacticsslider.value == player.tactics_energy) TacticsButton.GetComponent<Button>().interactable = true;
        else TacticsButton.GetComponent<Button>().interactable = false;

        PlayerLifeControl();
    }

    //    Following method is called by DrawStyler when the pokemon is inside the circle
    public void InsideCircle()
    {
        enemy.circle+=Random.Range(1,2);
        StartCoroutine(IncrementText());
        poketattics.SplitFunctionTacticsEffect();
        if(poketattics.TacticsTime <= 0) player.EnergyTacticsIncrement();
        EndCatchBattle();
    }

    //    Following method enable or nor the element of battle
    private void BattleObjectControll(bool enable,bool wincondition)
    {
        string capture_text_0, capture_text_1;
        Animator animator = GameObject.Find("Tactics_graphic").GetComponent<Animator>(); 

        EndBattleBorder.SetActive(!enable);
        if(wincondition) 
        {
            player.IncrementExp();
            capture_text_0 = "POKEMON CATTURATO"; capture_text_1 = "Esperienza Ceduta:     +"+(10+player.add_bonus);
        }else {capture_text_0 = "GAME OVER"; capture_text_1 = "";}

        EndBattleBorder.transform.GetChild(0).GetComponent<Text>().text = capture_text_0;
        EndBattleBorder.transform.GetChild(1).GetComponent<Text>().text = capture_text_1;
        animator.SetBool("show",false);poketattics.TacticsTime = 0f;
        
        OBJstyler.SetActive(enable);
        OBJenemy.GetComponent<Enemy>().enabled = OBJenemy.GetComponent<EnemyMovement>().enabled = enable;  
    }

    //    Following method check if a Pokemon is catch or not
    private void EndCatchBattle(){if(enemy.circle == enemy.circlemax+1) BattleObjectControll(false,true);}

    //    Following method check life Player, defeat or not
    private void PlayerLifeControl(){if(player.life <= 0) BattleObjectControll(false,false);}

    //    Following method confrim the continue button 
    public void OnConfirmBtn()
    {
        BattleObjectControll(true,false);
        OBJstyler  .GetComponent<DrawStyler>().DestroyElement();
        player     .SetPlayerLife();
        player     .EnergyTacticsReset();
        enemy      .Set_enemy_parameters();
        enemy      .SetCircleLife();
        poketattics.SetIconTactics(poketattics.choice_effect,false);
    }

    //    Following method confrim the back button 
    public void OnAnnulBtn(){Application.LoadLevel(0);}

    //    Following method Run from Battle
    public void OnRunButton(){Application.LoadLevel(0);}

    //    Following method run all tactics function and showup tactics_popup
    public void OnPokeTacticsButton()
    {
        Animator animator = GameObject.Find("Tactics_graphic").GetComponent<Animator>();
            if(animator != null)
            {
                bool isOpen = animator.GetBool("show"); 
                animator.SetBool("show",!isOpen);
            }
        player.EnergyTacticsReset();
        poketattics.SetTime();
        TacticsButton.GetComponent<Button>().interactable = false;
    }

    //    Following method enable and disable increment text obj
    IEnumerator IncrementText()
    {
        int remaincircle = enemy.circlemax - enemy.circle;
        increment_text.gameObject.SetActive(true);

        if(remaincircle < 0) increment_text.color = Color.yellow;
        else increment_text.color = Color.blue;

        increment_text.text=(""+Mathf.Abs(remaincircle));
        yield return new WaitForSeconds(1f);
        increment_text.gameObject.SetActive(false);
    }
}
