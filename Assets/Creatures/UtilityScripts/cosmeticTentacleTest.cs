using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cosmeticTentacleTest : MonoBehaviour
{
    private tentacleGravityTesting tentacle;
    public Gradient colors;
    public AnimationCurve widthCurve;
    public Color startColor;
    public Color endColor;
    private GameObject line;
    private LineRenderer lr;
    public int endCapVerts = 0;

    public float startWidth;

    public float endWidth;

    public bool doSmoothing = true;

    public int sortingOrder = 1;

    public string sortingLayerName = "creature";
    // Start is called before the first frame update
    void Start()
    {
        tentacle = GetComponent<tentacleGravityTesting>();
        line = new GameObject();
        lr = line.AddComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Lightweight Render Pipeline/2D/Sprite-Lit-Default"));
        lr.colorGradient = colors;
        lr.widthCurve = widthCurve;
        lr.sortingOrder = sortingOrder;
        lr.sortingLayerName = sortingLayerName;
        lr.numCapVertices = endCapVerts;
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

            if (doSmoothing)
            {
                points = MakeSmoothCurve(points, 0.2f);
            }
           
            line.transform.position = points[0];
            lr.positionCount = points.Length;
            Vector3[] positions = new Vector3[lr.positionCount];
            for (var i = 0; i < points.Length; i++)
            {
                positions[i] = points[i];
            }
            lr.SetPositions(positions);
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

    private void OnDrawGizmos()
    {

    }
}
