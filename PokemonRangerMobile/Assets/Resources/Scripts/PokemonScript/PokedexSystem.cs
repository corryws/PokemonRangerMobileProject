using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PokedexSystem : MonoBehaviour
{
    /*
    Un pokedex non è altro che un Database dei Pokemon,
    quindi caricherò i campi che sono Nome Index Tipo e Informazione

    File 1 - Informazioni Pokemon
    1,Bulbasaur,Grass,Poison,[Info]

    File 2 - Descrizione Pokemon
    Description

    File 3 - Registrato o meno
    0
    1
    0
    */
    
    public int index;
    public string[] _pkmnfield;
    public bool[] _registerfield;
    public Sprite[] sprites;

    public Text _indexText;
    public Text _nameText;
    public Text _infoText;
    public Image _type1,_type2,iconpkmn;
    public Slider index_slide;

    
    void Awake()
    {
        sprites = Resources.LoadAll<Sprite>("DatabasePokemon/PokemonIcon/sheet_icon");
        SetFieldString();
    }

    void Start(){ LoadFromString(); }

    [MenuItem("Tools/Read file")]
    public void SetFieldString()
    {
        string path = "Assets/Resources/DatabasePokemon/_fieldname.txt";
        int count=0,i=0;
        string line;

        StreamReader reader = new StreamReader(path);
        while(reader.ReadLine() != null)count++;
        reader.Close();

        StreamReader reader2 = new StreamReader(path);
         _pkmnfield     = new string[count];
         _registerfield = new bool  [count];
        while ((line = reader2.ReadLine()) != null) _pkmnfield[i++] = line;
        reader2.Close();

        path = "Assets/Resources/DatabasePokemon/_description.txt";
        i = 0;
        StreamReader reader3 = new StreamReader(path);
        while((line = reader3.ReadLine())  != null)_pkmnfield[i++] += line;

        path = "Assets/Resources/DatabasePokemon/_registered.txt";
        i = 0;
        StreamReader reader4 = new StreamReader(path);
        while((line = reader4.ReadLine())  != null)
        {
            if(line == "0")_registerfield[i++] = false;
            else if(line == "1") _registerfield[i++] = true;
        }
    }

    void LoadFromString()
    {
        int[] arrtmp = new int[5];
        int k=0;

        string tmpstr = _pkmnfield[index];
        for(int i=0;i<tmpstr.Length;i++) if(tmpstr[i] ==',') arrtmp[k++] = i;
        
        int id;
        int.TryParse(tmpstr.Substring(0,arrtmp[0]),out id);
        _indexText.text = "#"+ id.ToString("000");

        iconpkmn.sprite = sprites[id-1];

        _nameText.text  = tmpstr.Substring(arrtmp[0]+1,arrtmp[1]-arrtmp[0]-1);
        

        string type1 = tmpstr.Substring(arrtmp[1]+1,arrtmp[2]-arrtmp[1]-1);
        string type2 = tmpstr.Substring(arrtmp[2]+1,arrtmp[3]-arrtmp[2]-1);
        _type1.sprite = Resources.Load<Sprite>("Sprites&Images/Pokemon_type_icon/"+type1+"_type"); 
        if(type2 == " ") _type2.enabled = false; 
        else 
        {
            _type2.sprite = Resources.Load<Sprite>("Sprites&Images/Pokemon_type_icon/"+type2+"_type");
            _type2.enabled = true;           
        }
        _infoText.text  = tmpstr.Substring(arrtmp[3]+1,tmpstr.Length-arrtmp[3]-1);
        
         getRegistered();
        //Debug.Log(_indexText.text + "," + _nameText.text + "," + type1 + "," + type2 + "," + _infoText.text);
    }

    public void getRegistered()
    {
        for(int i=0;i<_registerfield.Length;i++)
        {
            if(!_registerfield[i])
            {
                iconpkmn.color = Color.black;
                _infoText.enabled = false;
                 _type1.enabled = false;
                 _type2.enabled = false; 
                //lock info and type
            }else
            {
                iconpkmn.color = Color.white;
                _infoText.enabled = true;
                _type1.enabled = true;
                _type2.enabled = true; 
                //unlock info and type
            }
        }
    }


    public void OnClickRight()
    {
        if(index >= _pkmnfield.Length-1) index = 0;
        else index +=1;

        index_slide.value = index;
        LoadFromString();
    }

    public void OnClickLeft()
    {
        if(index <= 0) index = _pkmnfield.Length-1;
        else index -=1;

        index_slide.value = index;
        LoadFromString();
    }

    public void OnValueChange()
    {
        index = (int)index_slide.value;
        LoadFromString();
    }
}
