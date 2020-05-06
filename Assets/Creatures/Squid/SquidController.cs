using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SquidController : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject circleCol;
    public int length;

    public float pointDist;
    
    [HideInInspector]
    public List<GameObject> parts = new List<GameObject>();

    [HideInInspector] public List<Rigidbody2D> partRigidbodies = new List<Rigidbody2D>();

    private Camera cam;

    public float rotLerp; 
    private float initRotLerp;

    public float moveForce;
    public float maxVel;
    public int tentacleIndex;
    public List<Transform> tentacles;
    private List<tentacleGravityTesting> tentacleScripts = new List<tentacleGravityTesting>();
    
    

    public float tentacleMoveForce;
    private bool hasTentaclesAttached = false;
    // Start is called before the first frame update
    void Start()
    {
        for (var i = 0; i < tentacles.Count; i++)
        {
            tentacleScripts.Add(tentacles[i].GetComponent<tentacleGravityTesting>());
        }
        initRotLerp = rotLerp;
        cam = Camera.main;
        //rb = GetComponent<Rigidbody2D>();
        parts.Add(circleCol);
        partRigidbodies.Add(circleCol.GetComponent<Rigidbody2D>());
        circleCol.transform.rotation = Quaternion.Euler(0,0,this.transform.eulerAngles.z);
        circleCol.transform.position = this.transform.position;
        Rigidbody2D body = partRigidbodies[0];
        for (var i = 1; i < length; i++)
        {
            GameObject circle = GameObject.Instantiate(circleCol, this.transform);
            parts.Add(circle);
            circle.transform.rotation = Quaternion.Euler(0,0,this.transform.eulerAngles.z);
            partRigidbodies.Add(circle.GetComponent<Rigidbody2D>());
            /* circle.transform.position =
                 new Vector2(Mathf.Cos(circle.transform.rotation.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(circle.transform.rotation.eulerAngles.z * Mathf.Deg2Rad)) *
                  pointDist + (Vector2)parts[i - 1].transform.position;*/
            DistanceJoint2D joint = circle.GetComponent<DistanceJoint2D>();
            joint.connectedBody = partRigidbodies[i - 1];
            joint.distance = pointDist;
            DistanceJoint2D test = circle.AddComponent<DistanceJoint2D>();
            test.connectedBody = body;
            test.autoConfigureDistance = false;
            test.maxDistanceOnly = true;
            test.distance = i * pointDist;
            if (i == tentacleIndex)
            {
                for (var j = 0; j < tentacles.Count; j++)
                {
                    tentacles[j].parent = circle.transform;
                }
            }

            if (i > tentacleIndex)
            {
                circle.GetComponent<SpriteRenderer>().enabled = false;
                //circle.GetComponent<CircleCollider2D>().enabled = false;
            }

        }
        circleCol.GetComponent<DistanceJoint2D>().enabled = false;
        //body.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (!hasTentaclesAttached)
        {
            for (var i = 0; i < tentacles.Count; i++)
            {
                Rigidbody2D col = tentacleScripts[i].partRigidbodies[0];
                GameObject temp = parts[tentacleIndex];
                DistanceJoint2D test = col.gameObject.AddComponent<DistanceJoint2D>();
                test.connectedBody = partRigidbodies[tentacleIndex];
                test.autoConfigureDistance = false;
                test.distance = Vector2.Distance(col.transform.position, temp.transform.position);
                //tentacleScripts[i].targetGameObject = parts[parts.Count - 1];
            }

            hasTentaclesAttached = true;
        }*/
        for (var i = 0; i < parts.Count; i++)
            {
               // if (partRigidbodies[i].velocity.magnitude > 0)
                //{
                /*if (partRigidbodies[i].velocity.magnitude > maxVel)
                {
                    partRigidbodies[i].velocity = partRigidbodies[i].velocity.normalized * maxVel;
                }*/
                Vector2 mousePos;
                    Vector2 v;
                    if (i == 0)
                    {
                        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                        v = mousePos - (Vector2)parts[i].transform.position;
                    }
                    else
                    {
                        v = parts[i - 1].transform.position - parts[i].transform.position;
                    }
                    float mag = partRigidbodies[i].velocity.magnitude;
                   /* if (mag > maxVel)
                    {
                        partRigidbodies[i].velocity = partRigidbodies[i].velocity.normalized * maxVel;
                    }*/
                   // rotLerp = mag / maxVel * initRotLerp;
                   float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
                    parts[i].transform.rotation = Quaternion.Euler(0,0,Mathf.LerpAngle(parts[i].transform.rotation.eulerAngles.z, angle, rotLerp));
                
                    //}
            }
            /*float mag = rb.velocity.magnitude;
            if (mag > maxVel)
            {
                rb.velocity = rb.velocity.normalized * maxVel;
            }
            rotLerp = mag / maxVel * initRotLerp * (v.magnitude/10f);
            float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.Euler(0,0,Mathf.LerpAngle(transform.rotation.eulerAngles.z, angle, rotLerp));*/


            //if (Input.GetMouseButtonDown(0))
            //{
                for (var i = 0; i < parts.Count; i++)
                {
                    if (i != 0)
                    {
                        //continue;
                    }
                    
                    //if (partRigidbodies[i].velocity.magnitude < maxVel)
                    //{
                        partRigidbodies[i].AddRelativeForce(new Vector2(moveForce, 0));
                   // }
                   // partRigidbodies[i].velocity = maxVel * partRigidbodies[i].velocity.normalized;
                        //partRigidbodies[i].velocity = maxVel * partRigidbodies[i].velocity.normalized;
                   // partRigidbodies[i].AddRelativeForce(new Vector2(moveForce, 0));
                    if (partRigidbodies[i].velocity.magnitude > maxVel)
                    {
                        partRigidbodies[i].velocity = partRigidbodies[i].velocity.normalized * maxVel;
                    }
                    
                
                    
                }

                for (var i = 0; i < tentacles.Count; i++)
                {
                    for (var j = 0; j < tentacleScripts[i].partRigidbodies.Count; j++)
                    {
                        Rigidbody2D partRB = tentacleScripts[i].partRigidbodies[j];
                        partRB.AddRelativeForce(new Vector2(-tentacleMoveForce, 0));
                        if (partRB.velocity.magnitude > maxVel)
                        {
                            partRB.velocity = partRB.velocity.normalized * maxVel;
                        }
                    }
                }
        
           // }
            
            //rb.AddRelativeForce(new Vector2(moveForce, 0));
        
    }
}
