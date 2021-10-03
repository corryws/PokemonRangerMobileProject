using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static ChoicePokemon pkmnmanager;
    public static string take_enemy_name, 
                         take_enemy_type,
                         take_enemy_weak,
                         take_player_tactics;
    public static float  take_enemy_speed, take_enemy_time_attack;
    public static int    take_enemy_circlemax;

    void Awake()
    {
       SetInstance();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetInstance()
    {
        if(instance == null){
           instance = this;
           DontDestroyOnLoad(transform.root.gameObject);
           pkmnmanager = GameObject.Find("PokemonManager").GetComponent<ChoicePokemon>(); 
       }else{
           Destroy(transform.root.gameObject);
           return;
       }
    }
    
}
