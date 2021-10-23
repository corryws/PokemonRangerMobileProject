using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawStyler : MonoBehaviour
{
    //VARIABLE-------------------
    
    public Text    increment_text;
    bool           isMousePressed;
    int            index;
    string         styler_color; 

    public GameObject[]  pokemons;

    GameObject     DialogueOBJ,
                   currentStyler,
                   StartPoint,
                   particle_styler;
    
    List<Vector2>  pointsList;
    Vector3        mousePos, 
                   fingerpos_halfpoint, 
                   fingerpos_startpoint;
   
    LineRenderer   line;
    EdgeCollider2D edgecollider;
    BattleManager  battleManager;
    Material       line_material;
    AudioSource    audioSource;

    // Structure for line points
    struct myLine{ public Vector3 StartPoint; public Vector3 EndPoint; public Vector3 HalfPoint; };

    void Awake ()
    {
        battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        pokemons      = GameObject.FindGameObjectsWithTag("Pokemon");
        DialogueOBJ   = GameObject.Find("DialogAsset");

        isMousePressed = false;

        currentStyler      = this.gameObject.transform.GetChild(0).gameObject;
        StartPoint         = this.gameObject.transform.GetChild(1).gameObject;
        particle_styler    = this.gameObject.transform.GetChild(3).gameObject;
        audioSource        = this.GetComponent<AudioSource>();

        GenerateLineComponent();
        currentStyler  .SetActive(false);
        StartPoint     .SetActive(false);
        particle_styler.SetActive(false);
        edgecollider .gameObject.SetActive(false);
    }

    void GenerateLineComponent()
    {
        // Create line renderer component and set its property
        line          = gameObject.AddComponent<LineRenderer> ();
        edgecollider  = this.gameObject.transform.GetChild(2).gameObject.GetComponent<EdgeCollider2D>();
        styler_color  = "blue";
        line_material = Resources.Load<Material>("Materials/"+styler_color) as Material;

        line.material = line_material;
        line.SetVertexCount (0);
        line.SetWidth (0.15f,0.15f); 
        line.SetColors (Color.white, Color.white);
        line.useWorldSpace = true; 
        pointsList         = new List<Vector2>();
    }

    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        if(isMousePressed)currentStyler.transform.position = mousePos;

        // If mouse button down, remove old line 
        if (Input.GetMouseButtonDown (0) && !DialogueOBJ.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            isMousePressed = true;
            currentStyler.SetActive(true);
            StartPoint   .SetActive(true);
            isMousePressed = true;
            //audioSource.pitch = 1;
            //audioSource.Play();
        }

        //If mouse button not touch
        if (Input.GetMouseButtonUp (0)) 
        {
            isMousePressed = false;
            DestroyElement();
        }

        // Drawing line when mouse is moving(presses)
        if (isMousePressed && StylerCheck())
        {
            edgecollider .gameObject.SetActive(true);
            SetLine();

            if (pointsList.Count < 2) 
            {
                //Debug.Log("CHARGE");
                particle_styler.SetActive(true);
                particle_styler.transform.position = currentStyler.transform.position; 
            }
            else
            {
                particle_styler.SetActive(false);
                //Debug.Log("NOT CHARGE");
            } 

            LengthLineCheck();
            if (isLineCollide ()) PlacementCenter();
        }
        
    }

    bool StylerCheck(){return (currentStyler.activeInHierarchy && StartPoint.activeInHierarchy);}

    //  Following method Set the variable to draw line
     void SetLine()
    {
        if (!pointsList.Contains (mousePos))
        {
            pointsList.Add      (mousePos);
            line.SetVertexCount (pointsList.Count);
            line.SetPosition    (pointsList.Count - 1, (Vector3)pointsList [pointsList.Count - 1]);
            edgecollider.points = pointsList.ToArray();

            edgecollider.offset              = new Vector2(this.transform.position.x*-1,this.transform.position.y * -1);
            currentStyler.transform.position = new Vector2(line.GetPosition(pointsList.Count - 1).x,line.GetPosition(pointsList.Count - 1).y);
            StartPoint.transform.position    = new Vector2(line.GetPosition(0).x,line.GetPosition(0).y);
            
           //if(audioSource.pitch < 10) audioSource.pitch += 0.002f;
        }else{
           //if(audioSource.pitch > 1) audioSource.pitch -= 0.002f;
           edgecollider.Reset();
        }
    }

    //  Following method checks if the length line is over 100 point
    void LengthLineCheck()
    {
        if (pointsList.Count < 2) return;

        float dist = Vector3.Distance(line.GetPosition(1), line.GetPosition(line.positionCount - 1));

        if(dist >= 10)
        {
           pointsList.Remove(pointsList[pointsList.Count-1]);
           Vector3[] newPositions = new Vector3[line.positionCount - 1];

           for (int i = 0; i < newPositions.Length; i++) newPositions[i] = line.GetPosition(i + 1);
           pointsList.Clear();
           line.SetPositions(newPositions);
        }
        SetLine();
        return;
    }

    //  Following method checks is currentLine(line drawn by last two points) collided with line 
    bool isLineCollide ()
    {
        if (pointsList.Count < 2)
            return false;
        int TotalLines = pointsList.Count - 1; myLine[] lines = new myLine[TotalLines];

        if (TotalLines > 1)
        {
            for (int i=0; i<TotalLines; i++)
            {
                lines [i].StartPoint = (Vector3)pointsList [i];
                lines [i].EndPoint   = (Vector3)pointsList [i + 1];
            }
        }

        for (int i=0; i<TotalLines-1; i++)
        {
            myLine currentLine;
            currentLine.StartPoint = (Vector3)pointsList [pointsList.Count - 2];
            currentLine.EndPoint   = (Vector3)pointsList [pointsList.Count - 1];
            currentLine.HalfPoint  = (Vector3)pointsList [pointsList.Count / 2];
            if (isLinesIntersect (lines[i], currentLine))
            {
                pointsList.RemoveRange (0, pointsList.Count);
                index = i; 
                if(index <= 0) index = 1; 
                line.positionCount = index;
                fingerpos_startpoint = currentLine.StartPoint;
                fingerpos_halfpoint = currentLine.HalfPoint;
                SetLine();     
                return true;
            }  
        }
        return false;
    }

    //    Following method checks whether given two points are same or not  
    bool checkPoints (Vector3 pointA, Vector3 pointB){return (pointA.x == pointB.x && pointA.y == pointB.y);}
  
    //    Following method checks whether given two line intersect or notW
    bool isLinesIntersect (myLine L1, myLine L2)
    {
        if (checkPoints (L1.StartPoint, L2.StartPoint) || checkPoints (L1.StartPoint, L2.EndPoint) ||
            checkPoints (L1.EndPoint, L2.StartPoint)   || checkPoints (L1.EndPoint, L2.EndPoint)) 
        return false;
        
       return((Mathf.Max (L1.StartPoint.x, L1.EndPoint.x) >= Mathf.Min (L2.StartPoint.x, L2.EndPoint.x)) &&
              (Mathf.Max (L2.StartPoint.x, L2.EndPoint.x) >= Mathf.Min (L1.StartPoint.x, L1.EndPoint.x)) &&
              (Mathf.Max (L1.StartPoint.y, L1.EndPoint.y) >= Mathf.Min (L2.StartPoint.y, L2.EndPoint.y)) &&
              (Mathf.Max (L2.StartPoint.y, L2.EndPoint.y) >= Mathf.Min (L1.StartPoint.y, L1.EndPoint.y)));
    }

//THE METHOD SOTTO ARE FOR THE CATCH  SYSTEM

    //    Following method Calculate the Close Circle When the  was detected
    void PlacementCenter()
    {
        Vector3 placement_vector = new Vector3((fingerpos_startpoint.x + fingerpos_halfpoint.x)/2,(fingerpos_startpoint.y + fingerpos_halfpoint.y)/2,1);
        //calculate the diametro
        float diametro = Vector3.Distance(placement_vector,fingerpos_startpoint)*2; 
        DetectedPokemon(placement_vector,diametro/2);
    }

    //    Following method Detect if is inside of a circle
    void DetectedPokemon(Vector3 circle_position,float raggio)
    {   
        foreach(GameObject pokemon in pokemons)
        {
            
            float dist = 
            Mathf.Sqrt(((circle_position.x - pokemon.transform.position.x)*(circle_position.x - pokemon.transform.position.x))+
                    ((circle_position.y   - pokemon.transform.position.y)*(circle_position.y - pokemon.transform.position.y)));
            
            if(dist + pokemon.GetComponent<CircleCollider2D>().radius <= raggio) 
                pokemon.GetComponent<PokemonSpawnSetting>().InsideCircle();
        }
    }
   
    //    Following method destroy all Line's object 
    public void DestroyElement()
    {
        //Debug.Log("destroy"); 
         currentStyler  .SetActive(false);
         StartPoint     .SetActive(false);
         particle_styler.SetActive(false);
         edgecollider .gameObject.SetActive(false);
         edgecollider .Reset();
         pointsList   .Clear();
         pointsList   .RemoveRange(0, pointsList.Count);
         line.SetVertexCount(0); 
         //audioSource.pitch = 1;
         //audioSource.Pause(); 
    }
}
