using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmeticLizardTest : MonoBehaviour
{
   private LizardScript tentacle;
    public Gradient colors;
    public AnimationCurve widthCurve;
    public Color startColor;
    public Color endColor;
    private GameObject line;
    private LineRenderer lr;

    public float startWidth;

    public float endWidth;
    // Start is called before the first frame update
    void Start()
    {
        tentacle = GetComponent<LizardScript>();
        line = new GameObject();
        lr = line.AddComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.colorGradient = colors;
        lr.widthCurve = widthCurve;
        lr.sortingOrder = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (tentacle != null)
        {
            Vector3[] points = new Vector3[tentacle.parts.Count];
            for (var i = 0; i < tentacle.parts.Count; i++)
            {
                points[i] = tentacle.parts[i].transform.position;
            }

            points = MakeSmoothCurve(points, 0.2f);
            line.transform.position = points[0];
            lr.positionCount = points.Length;
            for (var i = 0; i < points.Length; i++)
            {
                lr.SetPosition(i, points[i]);
            }
        }
    }
    
    //arrayToCurve is original Vector3 array, smoothness is the number of interpolations. 
    public static Vector3[] MakeSmoothCurve(Vector3[] arrayToCurve,float smoothness){
             List<Vector3> points;
             List<Vector3> curvedPoints;
             int pointsLength = 0;
             int curvedLength = 0;
             
             if(smoothness < 1.0f) smoothness = 1.0f;
             
             pointsLength = arrayToCurve.Length;
             
             curvedLength = (pointsLength*Mathf.RoundToInt(smoothness))-1;
             curvedPoints = new List<Vector3>(curvedLength);
             
             float t = 0.0f;
             for(int pointInTimeOnCurve = 0;pointInTimeOnCurve < curvedLength+1;pointInTimeOnCurve++){
                 t = Mathf.InverseLerp(0,curvedLength,pointInTimeOnCurve);
                 
                 points = new List<Vector3>(arrayToCurve);
                 
                 for(int j = pointsLength-1; j > 0; j--){
                     for (int i = 0; i < j; i++){
                         points[i] = (1-t)*points[i] + t*points[i+1];
                     }
                 }
                 
                 curvedPoints.Add(points[0]);
             }
             
             return(curvedPoints.ToArray());
         }
    
}
