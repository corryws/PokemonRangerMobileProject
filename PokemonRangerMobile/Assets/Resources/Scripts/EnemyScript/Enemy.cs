using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    public int circlemax;
    public int circle;
    public float timecircle;
    public string name;
    public string type;
    public string weak;

    public Slider hpslider;
    public Text txt_name;
    public Image type_image,weak_image;

    private float StartTimeCircle;
    private Animator anim;
    private EnemyMovement enemymovement;

    // Start is called before the first frame update
    void Start()
    {
        StartTimeCircle = 5;
        timecircle = StartTimeCircle;

        enemymovement = this.GetComponent<EnemyMovement>();

        if(GameManager.instance != null) Set_enemy_parameters();//GameManager.pkmnmanager.choiched_name;
        else name = "missgno";
        
        hpslider.maxValue = circlemax;
        txt_name.text = name.ToUpper();

        anim = this.GetComponent<Animator>();
        anim.SetBool(name,true);
    }

    // Update is called once per frame
    void Update()
    {
        hpslider.value = circle;
        if(hpslider.value >= 1) FriendlyBarTimer();
    }

    public void SetCircleLife()
    {
        circle = 0;
        hpslider.value = circle;
        hpslider.maxValue = circlemax;
    }

    public void Set_enemy_parameters()
    {
        name                             = GameManager.take_enemy_name;
        type                             = GameManager.take_enemy_type;
        weak                             = GameManager.take_enemy_weak;
        circlemax                        = GameManager.take_enemy_circlemax;
        enemymovement.speed              = GameManager.take_enemy_speed;
        enemymovement.StartTimeBtwAttack = GameManager.take_enemy_time_attack;
        timecircle                       = StartTimeCircle;
        
        type_image.sprite = Resources.Load<Sprite>("Sprites/Pokemon_type_icon/"+type+"_type"); 
        weak_image.sprite = Resources.Load<Sprite>("Sprites/Pokemon_type_icon/"+weak+"_type"); 
    }

    public void FriendlyBarTimer()
    {
        if(timecircle <= 0)
        {
            circle-=1;
            timecircle = StartTimeCircle;
        }else if(timecircle > 0 && timecircle < 999)timecircle -= Time.deltaTime;
    }
}
