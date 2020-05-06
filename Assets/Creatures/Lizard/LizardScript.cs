using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardScript : MonoBehaviour
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

    public List<LegLizard> legs;

    public LayerMask grabbable;

    private float initMovementVeloc;

    public GameObject headGO;
    // Start is called before the first frame update
    void Start()
    {
        initMovementVeloc = movementVelocity;
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
            /*DistanceJoint2D test = circle.AddComponent<DistanceJoint2D>();
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

        for (var i = 0; i < legs.Count; i++)
        {
            Vector2 outPos;
            if (legs[i].bodyIndex - legs[i].lookAheadAmount < 0)
            {
                float ang = parts[0].transform.rotation.eulerAngles.z;
                Vector2 pos = new Vector2(Mathf.Cos(ang), Mathf.Sin(ang)) * pointDist + (Vector2)parts[0].transform.position;
                outPos = pos;
            }
            else
            {
                outPos = parts[legs[i].bodyIndex - legs[i].lookAheadAmount].transform.position;
            }
            Vector2 dir = outPos - (Vector2)parts[legs[i].bodyIndex].transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;;
            Vector2 angle1 = new Vector2(Mathf.Cos((angle + legs[i].outAngle) * Mathf.Deg2Rad),
                Mathf.Sin((angle + legs[i].outAngle) * Mathf.Deg2Rad));
            Vector2 angle2 = new Vector2(Mathf.Cos((angle + 90f) * Mathf.Deg2Rad),
                Mathf.Sin((angle + 90f) * Mathf.Deg2Rad));
            RaycastHit2D hit = Physics2D.Raycast((Vector2) parts[legs[i].bodyIndex].transform.position, angle1,
                legs[i].length, grabbable);
            if (hit)
            {
                legs[i].target = hit.point;
            }
            else
            {
                legs[i].target = Vector2.positiveInfinity;
            }
            hit = Physics2D.Raycast((Vector2) parts[legs[i].bodyIndex].transform.position, angle2,
                legs[i].length, grabbable);
            if (hit)
            {
                legs[i].pos = hit.point;
            }
            else
            {
                legs[i].pos = (Vector2)parts[legs[i].bodyIndex].transform.position + (angle2 * legs[i].length);;
            }
        }
        circleCol.GetComponent<DistanceJoint2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        for (var i = 0; i < parts.Count; i++)
        {
            Vector2 v;
            Vector2 target;
            if (i == 0)
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
                target = parts[i - 1].transform.position;
            }

            v = target - (Vector2)parts[i].transform.position;
            float rot = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
            float origRot = parts[i].transform.rotation.eulerAngles.z;
            parts[i].transform.rotation = Quaternion.Euler(0,0,Mathf.LerpAngle(parts[i].transform.eulerAngles.z, rot, rotationLerp));
            
        }
        int grabCount = 0;
        for (var i = 0; i < legs.Count; i++)
        {
            Vector2 outPos;
            if (legs[i].bodyIndex - legs[i].lookAheadAmount < 0)
            {
                float ang = parts[0].transform.rotation.eulerAngles.z;
                Vector2 pos = new Vector2(Mathf.Cos(ang), Mathf.Sin(ang)) * pointDist + (Vector2)parts[0].transform.position;
                outPos = pos;
            }
            else
            {
                outPos = parts[legs[i].bodyIndex - legs[i].lookAheadAmount].transform.position;
            }
            Vector2 dir = outPos - (Vector2)parts[legs[i].bodyIndex].transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;;
            Vector2 angle1 = new Vector2(Mathf.Cos((angle + legs[i].outAngle) * Mathf.Deg2Rad),
                Mathf.Sin((angle + legs[i].outAngle) * Mathf.Deg2Rad));
            RaycastHit2D hit = Physics2D.Raycast((Vector2) parts[legs[i].bodyIndex].transform.position, angle1,
                legs[i].length, grabbable);
            if (hit)
            {
                legs[i].target = hit.point;
                grabCount += 1;
            }
            else
            {
                angle1 = new Vector2(Mathf.Cos((angle - legs[i].outAngle) * Mathf.Deg2Rad),
                    Mathf.Sin((angle - legs[i].outAngle) * Mathf.Deg2Rad));
                hit = Physics2D.Raycast((Vector2) parts[legs[i].bodyIndex].transform.position, angle1,
                    legs[i].length, grabbable);
                if (hit)
                {
                    legs[i].target = hit.point;
                    grabCount += 1;
                }
                else
                {
                    if (!legs[i].target.Equals(Vector2.positiveInfinity))
                    {
                        legs[i].target = Vector2.positiveInfinity;
                        legs[i].diff = legs[i].pos - (Vector2) parts[legs[i].bodyIndex].transform.position;
                    }
                    
                }
            }

            if (legs[i].target.Equals(Vector2.positiveInfinity))
            {
                legs[i].pos = (Vector2) parts[legs[i].bodyIndex].transform.position + legs[i].diff;
            }
            else
            {
                if (Vector2.Distance(legs[i].pos, legs[i].target) > legs[i].length)
                {
                    legs[i].pos = legs[i].target;
                }
            }
        }
        for (var i = 0; i < parts.Count; i++)
        {
            if (grabCount == 0)
            {
                partRigidbodies[i].gravityScale = 2.5f;
                //movementVelocity = 0;
            }
            else
            {
                partRigidbodies[i].gravityScale = 0;
                movementVelocity = initMovementVeloc;
            }
            if (i == 0)
            {
                if (targetPoint.Equals(Vector2.negativeInfinity))
                {
                    continue;
                }
            }

            Vector2 dir = new Vector2(Mathf.Cos(parts[i].transform.eulerAngles.z * Mathf.Deg2Rad),
                Mathf.Sin(parts[i].transform.eulerAngles.z * Mathf.Deg2Rad));
            Rigidbody2D partRb = partRigidbodies[i];
            //partRb.velocity = dir * movementVelocity;
            partRb.AddForce(dir * movementVelocity * Time.deltaTime);
            if (partRb.velocity.magnitude > maxVelocity)
            {
                partRb.velocity = maxVelocity * partRb.velocity.normalized;
            }

            if (i == 0)
            {
                headGO.transform.position = parts[i].transform.position;
                headGO.transform.rotation = parts[i].transform.rotation;
            }
        }

        
    }

    private void OnDrawGizmos()
    {
        if (parts.Count > 0)
        {
            for (var i = 0; i < legs.Count; i++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(legs[i].pos, 0.08f);
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(parts[legs[i].bodyIndex].transform.position, legs[i].pos);
            }
        }
        
    }
}
[System.Serializable]
public class LegLizard
{
    public int bodyIndex;
    public float length;
    public float outAngle;
    public int lookAheadAmount;
    [HideInInspector]
    public Vector2 pos;
    [HideInInspector]
    public Vector2 target;
    [HideInInspector]
    public Vector2 diff;
}
