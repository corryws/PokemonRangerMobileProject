using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawStyler : MonoBehaviour
{
    //VARIABLE-------------------
    public List<Vector2> pointsList;
    public Text increment_text;

    public GameObject StylerPrefab;
    public GameObject currentStyler;
    private GameObject EnemyOBJ;
    private GameObject DialogueOBJ;

    private LineRenderer line;
    private EdgeCollider2D edgecollider;
    private BattleManager battleManager;
    private Material line_material;
    private AudioSource audioSource;
    
    private Vector3 mousePos;
    private Vector3 fingerpos_halfpoint, fingerpos_startpoint;
    
    private bool isMousePressed;
    private int index;
    private string styler_color; 

    // Structure for line points
    struct myLine{ public Vector3 StartPoint; public Vector3 EndPoint; public Vector3 HalfPoint; };

    void Awake ()
    {
        // Create line renderer component and set its property
        line = gameObject.AddComponent<LineRenderer> ();
        edgecollider = this.GetComponent<EdgeCollider2D>();
        styler_color = "blue";
        line_material = Resources.Load<Material>("Materials/"+styler_color) as Material;
        Debug.Log(line_material);
        line.material = line_material;
        line.SetVertexCount (0);
        line.SetWidth (0.15f,0.15f); line.SetColors (Color.white, Color.white);
        line.useWorldSpace = true; isMousePressed = false;
        pointsList = new List<Vector2>();

        battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        EnemyOBJ = GameObject.FindWithTag("Pokemon");
        DialogueOBJ = GameObject.Find("DialogAsset");
        audioSource = this.GetComponent<AudioSource>();
    }

    void Update ()
    {
        mousePos.z = 0;
        // If mouse button down, remove old line 
        if (Input.GetMouseButtonDown (0) && DialogueOBJ.transform.GetChild(0).gameObject.activeInHierarchy == false)
        {
            isMousePressed = true;
            audioSource.pitch = 1;
            audioSource.Play();
            currentStyler = Instantiate(StylerPrefab,Vector3.zero,Quaternion.identity);//istanzio lo styler
        }

        //If mouse button not touch
        if (Input.GetMouseButtonUp (0)) 
        {
            DestroyElement();
        }

        // Drawing line when mouse is moving(presses)
        if (isMousePressed && StylerCheck())
        {
            edgecollider.enabled=true;
            SetLine();
            LengthLineCheck();
            if (isLineCollide ()) PlacementCenter();
        }
    }

    bool StylerCheck()
    {
        if(currentStyler) return true;
        else return false;
    }

    //  Following method Set the variable to draw line
    private void SetLine()
    {
        mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
        if (!pointsList.Contains (mousePos))
        {
            pointsList.Add (mousePos);
            line.SetVertexCount (pointsList.Count);
            line.SetPosition (pointsList.Count - 1, (Vector3)pointsList [pointsList.Count - 1]);
            edgecollider.points = pointsList.ToArray();
            edgecollider.offset = new Vector2(this.transform.position.x*-1,this.transform.position.y * -1);
                
            currentStyler.transform.position = new Vector2(line.GetPosition(pointsList.Count - 1).x,line.GetPosition(pointsList.Count - 1).y);
            this.transform.position = currentStyler.transform.position;
            if(audioSource.pitch < 10) audioSource.pitch += 0.002f;
        }else{
           if(audioSource.pitch > 1) audioSource.pitch -= 0.002f;
        }
    }

    //  Following method checks if the length line is over 100 point
    private void LengthLineCheck()
    {
        Debug.Log(pointsList.Count);
        if(pointsList.Count > 100)
        {
           pointsList.Remove(pointsList[pointsList.Count-1]);
           Vector3[] newPositions = new Vector3[line.positionCount - 1];
           for (int i = 0; i < newPositions.Length; i++) newPositions[i] = line.GetPosition(i + 1);
           line.SetPositions(newPositions);
           //DestroyElement();
        }else return;
    }

    //  Following method checks is currentLine(line drawn by last two points) collided with line 
    private bool isLineCollide ()
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
                index = i; if(index <= 0) index = 1; line.positionCount = index;
                fingerpos_startpoint = currentLine.StartPoint;
                fingerpos_halfpoint = currentLine.HalfPoint;
                SetLine();     
                return true;
            }  
        }
        return false;
    }

    //    Following method checks whether given two points are same or not  
    private bool checkPoints (Vector3 pointA, Vector3 pointB){return (pointA.x == pointB.x && pointA.y == pointB.y);}
  
    //    Following method checks whether given two line intersect or notW
    private bool isLinesIntersect (myLine L1, myLine L2)
    {
        if (checkPoints (L1.StartPoint, L2.StartPoint) || checkPoints (L1.StartPoint, L2.EndPoint) ||
            checkPoints (L1.EndPoint, L2.StartPoint)   || checkPoints (L1.EndPoint, L2.EndPoint)) 
        return false;
        
       return((Mathf.Max (L1.StartPoint.x, L1.EndPoint.x) >= Mathf.Min (L2.StartPoint.x, L2.EndPoint.x)) &&
              (Mathf.Max (L2.StartPoint.x, L2.EndPoint.x) >= Mathf.Min (L1.StartPoint.x, L1.EndPoint.x)) &&
              (Mathf.Max (L1.StartPoint.y, L1.EndPoint.y) >= Mathf.Min (L2.StartPoint.y, L2.EndPoint.y)) &&
              (Mathf.Max (L2.StartPoint.y, L2.EndPoint.y) >= Mathf.Min (L1.StartPoint.y, L1.EndPoint.y)));
    }

//THE METHOD SOTTO ARE FOR THE CATCH POKEMON SYSTEM

    //    Following method Detect if a pokemon is inside of a circle
    void DetectedPokemon(Vector3 circle_position,float raggio)
    {   
            //battleManager.InsideCircle();
            float dist = Mathf.Sqrt(((circle_position.x - EnemyOBJ.transform.position.x)*(circle_position.x - EnemyOBJ.transform.position.x))+
                                    ((circle_position.y - EnemyOBJ.transform.position.y)*(circle_position.y - EnemyOBJ.transform.position.y)));
            
            if(dist + EnemyOBJ.GetComponent<CircleCollider2D>().radius <= raggio) battleManager.InsideCircle();
    }

    //    Following method Calculate the Close Circle When the pokemon was detected
    void PlacementCenter()
    {
        //placement_vector = central position
        Vector3 placement_vector = new Vector3((fingerpos_startpoint.x + fingerpos_halfpoint.x)/2,(fingerpos_startpoint.y + fingerpos_halfpoint.y)/2,1);
        //calculate the diametro
        float diametro = Vector3.Distance(placement_vector,fingerpos_startpoint)*2; 
        DetectedPokemon(placement_vector,diametro/2);
    }
   
    //    Following method Detect if the line collide with pokemon or wall
    private void OnCollisionEnter2D (Collision2D  otherCollider){ if(otherCollider.gameObject.tag == "Pokemon" || otherCollider.gameObject.tag == "wall") DestroyElement();}

    //    Following method Detect if the line enter in trigger with wall
    private void OnTriggerEnter2D (Collider2D  otherCollider){if(otherCollider.gameObject.tag == "Bullet" || otherCollider.gameObject.tag == "Obstacle") DestroyElement();}

    //    Following method Detect if the line stay trigger with wall
    private void OnTriggerStay2D(Collider2D otherCollider){if(otherCollider.gameObject.tag == "wall")DestroyElement();}

   
    //    Following method destroy all Line's object 
    public void DestroyElement()
    {
         Destroy(currentStyler); pointsList.Clear();
         isMousePressed = false; line.SetVertexCount(0); 
         pointsList.RemoveRange (0, pointsList.Count);
         edgecollider.Reset();
         edgecollider.enabled=false;
         audioSource.pitch = 1;
         audioSource.Pause(); 
    }
}
