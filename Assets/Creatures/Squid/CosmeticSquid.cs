using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmeticSquid : MonoBehaviour
{
    private SquidController _controller;

    private LineRenderer lR;
    public AnimationCurve widthCurve;

    public bool doSmoothing;
    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<SquidController>();
        lR = GetComponent<LineRenderer>();
        lR.widthCurve = widthCurve;
    }

    // Update is called once per frame
    void Update()
    {
        lR.positionCount = _controller.parts.Count;
        Vector3[] positions = new Vector3[lR.positionCount];
        for (var i = 0; i < lR.positionCount; i++)
        {
            positions[i] = _controller.parts[i].transform.position;
        }

        if (doSmoothing)
        {
            positions = MakeSmoothCurve(positions, 0.2f);
        }
        lR.SetPositions(positions);
    }
    
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
