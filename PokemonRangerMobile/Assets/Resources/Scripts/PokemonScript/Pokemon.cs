using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pokemon", menuName="Pokemon")]
public class Pokemon : ScriptableObject
{
    public AnimatorOverrideController ThisPokemonAnimator;
    public int circlemax;
    public float pkm_speed, bullet_time_spd;
    public bool islegendary;
    public string name;

    public enum Type
    {
        normal,fire,fighting,
        water,flying,grass,
        poison,electric,ground,
        psychic,rock,ice,
        bug,dragon,ghost,
        dark,steel,fairy
    };

    public Type type_one;
    public Type type_two;

    public string getTypeOne(){return type_one.ToString();}
    public string getTypeTwo(){return type_two.ToString();}
}
