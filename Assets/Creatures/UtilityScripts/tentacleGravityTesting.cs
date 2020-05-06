using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tentacleGravityTesting : MonoBehaviour
{
    public GameObject circleCol;
    public int length;

    public float pointDist;
    public float maxVelocity;

    [HideInInspector]
    public List<GameObject> parts = new List<GameObject>();

    [HideInInspector] public List<Rigidbody2D> partRigidbodies = new List<Rigidbody2D>();
    
    public float rotationLerp;
    public float movementVelocity;
    
    public Vector2 targetPoint = Vector2.positiveInfinity;
    public GameObject targetGameObject;
    

    [HideInInspector]
    public bool isGrabbed;

    private tentaclePartScript lastPart;

    public bool partShouldBeFirstLayer = true;
    // Start is called before the first frame update
    void Start()
    {
        parts.Add(circleCol);
        partRigidbodies.Add(circleCol.GetComponent<Rigidbody2D>());
        circleCol.transform.rotation = Quaternion.Euler(0,0,this.transform.eulerAngles.z);
        circleCol.transform.position = this.transform.position;
        Rigidbody2D body = partRigidbodies[0];
        for (var i = 1; i < length; i++)
        {
            GameObject circle = GameObject.Instantiate(circleCol, this.transform);
            if (!partShouldBeFirstLayer)
            {
                circle.layer = 0;
            }
            parts.Add(circle);
            circle.transform.rotation = Quaternion.Euler(0,0,this.transform.eulerAngles.z);
            partRigidbodies.Add(circle.GetComponent<Rigidbody2D>());
           /* circle.transform.position =
                new Vector2(Mathf.Cos(circle.transform.rotation.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(circle.transform.rotation.eulerAngles.z * Mathf.Deg2Rad)) *
                 pointDist + (Vector2)parts[i - 1].transform.position;*/
            DistanceJoint2D joint = circle.GetComponent<DistanceJoint2D>();
            joint.connectedBody = partRigidbodies[i - 1];
            joint.distance = pointDist;
           /* DistanceJoint2D test = circle.AddComponent<DistanceJoint2D>();
            test.connectedBody = body;
            test.autoConfigureDistance = false;
            test.maxDistanceOnly = true;
            test.distance = i * pointDist;*/

        }
        
        for (var i = 0; i < length - 1; i++)
        {
            GameObject circle = parts[i];
            DistanceJoint2D test = circle.AddComponent<DistanceJoint2D>();
            test.connectedBody = partRigidbodies[length - 1];
            test.autoConfigureDistance = false;
            test.maxDistanceOnly = true;
            test.distance = (length - 1 - i) * pointDist;
        }

        lastPart = parts[length - 1].GetComponent<tentaclePartScript>();
        circleCol.GetComponent<DistanceJoint2D>().enabled = false;
        body.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        isGrabbed = lastPart.isGrabbed;
        for (var i = parts.Count - 1; i > 0; i--)
        {
            Vector2 v;
            Vector2 target;
            if (i == parts.Count - 1)
            {
                if (targetGameObject == null)
                {
                    if (targetPoint.Equals(Vector2.positiveInfinity))
                    {
                        target = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    }else if (targetPoint.Equals(Vector2.negativeInfinity))
                    {
                        continue;
                    }
                    else
                    {
                        target = targetPoint;
                    }
                }
                else
                {
                    target = targetGameObject.transform.position;
                }
                



            }
            else
            {
                target = parts[i + 1].transform.position;
            }

            v = target - (Vector2)parts[i].transform.position;
            float rot = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
            /* if (rot < 0)
             {
                 partRotations[i] += -rotationVelocity * Time.deltaTime;
             }
             else
             {
                 partRotations[i] += rotationVelocity * Time.deltaTime;
             }*/
            float origRot = parts[i].transform.rotation.eulerAngles.z;
            parts[i].transform.rotation = Quaternion.Euler(0,0,Mathf.LerpAngle(parts[i].transform.eulerAngles.z, rot, rotationLerp));






        }

        for (var i = parts.Count - 1; i > parts.Count - 2; i--)
        {
            if (targetPoint.Equals(Vector2.negativeInfinity))
                {
                    continue;
                }

            //if (i == parts.Count - 1)
            //{
                Vector2 dir = new Vector2(Mathf.Cos(parts[i].transform.eulerAngles.z * Mathf.Deg2Rad),
                    Mathf.Sin(parts[i].transform.eulerAngles.z * Mathf.Deg2Rad));
                // temp.y += -9.8f * Time.deltaTime;

                //parts[i].transform.position += (Vector3)dir * movementVelocity * Time.deltaTime;
                //parts[i].GetComponent<Rigidbody2D>().velocity = dir * movementVelocity * Time.deltaTime;
                Rigidbody2D partRb = partRigidbodies[i];
                partRb.velocity = dir * movementVelocity;
                // partRb.AddForce(dir * movementVelocity * Time.deltaTime);
                if (partRb.velocity.magnitude > maxVelocity)
                {
                    partRb.velocity = maxVelocity * partRb.velocity.normalized;
                }
           // }

              
                //}

        }

        /*for (var i = 1; i < parts.Count; i++)
        {
            //parts[i].transform.position = new Vector2(parts[i].transform.position.x, parts[i].transform.position.y - 9.8f * Time.deltaTime);
            Vector2 pos = parts[i].transform.position;
            Vector2 pos1 = parts[i - 1].transform.position;
            if ( (pos1 - pos).magnitude != pointDist)
            {
                //parts[i].GetComponent<Rigidbody2D>().AddForce((pos - pos1).normalized * 0.02f * Time.deltaTime);

              //  parts[i].transform.position = pos1 + (pos - pos1).normalized * pointDist;
            }
        }*/
    }
}
