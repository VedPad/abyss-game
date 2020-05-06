using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cosmeticGlowyLeviathan : MonoBehaviour
{
    private glowyLeviathanScript gLS;

    private LineRenderer lR;

    public Gradient colors;

    public AnimationCurve widthCurve;

    private List<GameObject> eyeLights;
    public GameObject bodyLight;
    private bool bodyLightsSpawned;
    private List<GameObject> bodyTentacles = new List<GameObject>();
    public GameObject bodyTentacle;

    private List<GameObject> bodyLights = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        lR = GetComponent<LineRenderer>();
        gLS = GetComponent<glowyLeviathanScript>();
        
        lR.widthCurve = widthCurve;
        lR.colorGradient = colors;
    }

    // Update is called once per frame
    void Update()
    {
        if (!bodyLightsSpawned)
        {
            /*for (var i = 0; i < gLS.parts.Count; i += 3)
            {
                bodyLights.Add(GameObject.Instantiate(bodyLight, gLS.parts[i].transform.position, Quaternion.identity));
            }

            bodyLightsSpawned = true;*/
            bodyTentacle.GetComponent<tentacleGravityTesting>().length = 1;
            for (var i = 0; i < gLS.parts.Count - 10; i += 6)
            {
                bodyTentacles.Add(GameObject.Instantiate(bodyTentacle, gLS.parts[i].transform.position, Quaternion.identity, gLS.parts[i].transform));
            }

            bodyLightsSpawned = true;
        }

        for (var i = 0; i < bodyTentacles.Count; i++)
        {
            bodyTentacles[i].transform.position = gLS.parts[i * 6].transform.position;
        }

       /* for (var i = 0; i < bodyLights.Count; i++)
        {
            bodyLights[i].transform.position = gLS.parts[i * 3].transform.position;
        }*/
        lR.positionCount = gLS.parts.Count;
        Vector3[] positions = new Vector3[lR.positionCount];
        for (var i = 0; i < positions.Length; i++)
        {
            positions[i] = gLS.parts[i].transform.position;
        }

        //positions = MakeSmoothCurve(positions, 0.1f);
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
