using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabasePokemon : MonoBehaviour
{
    [Header("DATABASE VOICE")]
    [Header("NAME | CIRCLEMAX | TYPE | WEAKNESS")]
    public int AllArraylength;
    public string[] name;
    public int[] circlemax;
    public float[] pkm_speed, bullet_time_spd;

    public enum getType
    {
        normal,fire,fighting,
        water,flying,grass,
        poison,electric,ground,
        psychic,rock,ice,
        bug,dragon,ghost,
        dark,steel,fairy
    };
    public getType[] type;
    public getType[] weakness;

    void Awake()
    {
        AllArraylength = name.Length;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
