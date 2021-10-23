using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManagerScript : MonoBehaviour
{
    public Dropdown dropdown_tactis;

    void Update(){DropDownTatticsUpdate();}

    public void DropDownTatticsUpdate()
    {
        int value;
        value = dropdown_tactis.value;
        GameManager.take_player_tactics = dropdown_tactis.options[value].text;
    }

    public void OnCatchButtonClick()
    {
        Application.LoadLevel(2);
    }

   
}
