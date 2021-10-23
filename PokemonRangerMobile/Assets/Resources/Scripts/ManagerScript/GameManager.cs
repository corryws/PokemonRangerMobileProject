using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static string take_player_tactics;

    void Awake()
    {
       if(instance == null){
           instance = this;
           DontDestroyOnLoad(transform.root.gameObject);
       }else{
           Destroy(transform.root.gameObject);
           return;
       }
    }
/*
    Savesystem && LoadSystem
    You can save game file with JSON methos, json can read int,string,float
    so transform all date in a json file that you can read and load
*/

//Example of Save && Load Function Object
/*
    public static void PlayerDataSave(GameObject _player)
    {
        ScriptPlayer player = _player.GetComponent<ScriptPlayer>();
        PlayerPrefs.SetInt("NomeDiComeMemorizzareLaVariabile",nomevariabiledasalvare);
        PlayerPrefs.SetFloat("NomeDiComeMemorizzareLaVariabile",nomevariabiledasalvare);

        string infoplayer  = JsonUtility.ToJson(_player.GetComponent<Character>()); 
        PlayerPrefs.SetString("NomeDiComeMemorizzareLaVariabile",nomevariabiledasalvare);
    }


    public static GameObject LoadPlayerData(GameObject _player)
    {
        JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("NomeDiComeMemorizzareE'SalvataLaVariabile"),_player.GetComponent<Character>());

        _player.life = PlayerPrefs.GetInt("NomeDiComeMemorizzareE'SalvataLaVariabile");

        return _player;
    }
*/
    
}
