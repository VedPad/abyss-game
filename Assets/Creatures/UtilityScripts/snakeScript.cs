using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class snakeScript : MonoBehaviour
{
    private List<Vector2> partPositions = new List<Vector2>(); //turn to transforms later
    private List<float> partRotations = new List<float>();
    public int length;

    public float pointDist;

    public float rotationVelocity;

    public float movementVelocity;

    public List<Leg> legs;

    private float initMovementVel;

    private float initRotationVel;

    private int planted;
    // Start is called before the first frame update
    void Start()
    {
        initMovementVel = movementVelocity;
        initRotationVel = rotationVelocity;
        partPositions.Add(this.gameObject.transform.position);
        partRotations.Add(0);
        for (var i = 1; i < length; i++)
        {
            partPositions.Add(new Vector2(partPositions[i - 1].x - pointDist, partPositions[i - 1].y));
            partRotations.Add(0);
        }
        initLegsTemp();
        for (var i = 0; i < legs.Count; i++)
        {
            try
            {
                Vector2 temp1 = partPositions[legs[i].bodyIndex - legs[i].lookAheadAmount] - partPositions[legs[i].bodyIndex];
                float angle = Mathf.Atan2(temp1.y, temp1.x) * Mathf.Rad2Deg;;
                Vector2 angle1 = new Vector2(Mathf.Cos((angle + legs[i].outAngle) * Mathf.Deg2Rad),
                    Mathf.Sin((angle + legs[i].outAngle) * Mathf.Deg2Rad));
                Vector2 angle2 = new Vector2(Mathf.Cos((angle - legs[i].outAngle) * Mathf.Deg2Rad),
                    Mathf.Sin((angle - legs[i].outAngle) * Mathf.Deg2Rad));
                legs[i].leftFootPos = partPositions[legs[i].bodyIndex] + (angle2 * legs[i].length);
                legs[i].rightFootPos = partPositions[legs[i].bodyIndex] + (angle1 * legs[i].length);
            }
            catch
            {
                
            }
        }

        //planted = legs.Count * 2;
    }

    void initLegsTemp()
    {
        for (var i = 1; i < length/2; i += 4)
        {
            Leg leg = new Leg();
            leg.bodyIndex = i;
            leg.length = 0.5f;
            leg.lookAheadAmount =2;
            leg.outAngle = 25f;
            legs.Add(leg);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //movementVelocity = initMovementVel * planted/(legs.Count);
       /* if (Input.GetMouseButtonDown(0))
        {
            movementVelocity = initMovementVel * 2;
            rotationVelocity = initRotationVel * 2;
        }

        if (Input.GetMouseButtonUp(0))
        {
            movementVelocity = initMovementVel/3;
            rotationVelocity = initRotationVel/3;
        }*/
        for (var i = 0; i < partPositions.Count; i++)
        {
            Vector2 v;
            Vector2 target;
            if (i == 0)
            {
                target = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
                
                
            }
            else
            {
                target = partPositions[i - 1];
                v = partPositions[i - 1] - partPositions[i];
            }
            v =  target - partPositions[i];
            float rot = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
           /* if (rot < 0)
            {
                partRotations[i] += -rotationVelocity * Time.deltaTime;
            }
            else
            {
                partRotations[i] += rotationVelocity * Time.deltaTime;
            }*/
           if (i == 0)
           {
               if (v.magnitude > 0.5)
               {
                   partRotations[i] = Mathf.LerpAngle(partRotations[i], rot, rotationVelocity);
               }
           }
           else
           {
               partRotations[i] = Mathf.LerpAngle(partRotations[i], rot, rotationVelocity);
           }
          
           

         
        }

        for (var i = 0; i < partPositions.Count; i++)
        {
            if (i == 0)
            {
                partPositions[i] += new Vector2(Mathf.Cos(partRotations[i] * Mathf.Deg2Rad), Mathf.Sin(partRotations[i] * Mathf.Deg2Rad)) * movementVelocity * Time.deltaTime;
                this.transform.position += (Vector3)new Vector2(Mathf.Cos(partRotations[i] * Mathf.Deg2Rad), Mathf.Sin(partRotations[i] * Mathf.Deg2Rad)) * movementVelocity * Time.deltaTime;
                this.transform.rotation = Quaternion.Euler(0,0,partRotations[i]);
            }
            else
            {
                Vector2 temp = new Vector2(Mathf.Cos(partRotations[i] * Mathf.Deg2Rad), Mathf.Sin(partRotations[i] * Mathf.Deg2Rad)) * movementVelocity * Time.deltaTime;
               // temp.y += -9.8f * Time.deltaTime;


                    partPositions[i] += temp;
                if ((partPositions[i - 1] - partPositions[i]).magnitude != pointDist)
                {
                    partPositions[i] = partPositions[i - 1] + (partPositions[i] - partPositions[i - 1]).normalized * pointDist;
                }
            }
        }

        for (var i = 0; i < legs.Count; i++)
        {
            if (legs[i].bodyIndex >= length)
            {
                continue;
            }
            if (legs[i].rightTarget.Equals(Vector2.positiveInfinity) &&
                legs[i].leftTarget.Equals(Vector2.positiveInfinity))
            {
                Vector2 temp1;
                if (legs[i].bodyIndex - legs[i].lookAheadAmount < 0)
                {
                    int diff = legs[i].lookAheadAmount - legs[i].bodyIndex;
                    float ang = partRotations[0] * Mathf.Deg2Rad;
                    Vector2 pos = new Vector2(Mathf.Cos(ang), Mathf.Sin(ang)) * pointDist + partPositions[0];
                    temp1 = pos - partPositions[legs[i].bodyIndex];
                }
                else
                {
                    temp1 = partPositions[legs[i].bodyIndex - legs[i].lookAheadAmount] - partPositions[legs[i].bodyIndex];
                }
                float angle = Mathf.Atan2(temp1.y, temp1.x) * Mathf.Rad2Deg;;
                Vector2 angle1 = new Vector2(Mathf.Cos((angle + legs[i].outAngle) * Mathf.Deg2Rad),
                    Mathf.Sin((angle + legs[i].outAngle) * Mathf.Deg2Rad));
                Vector2 angle2 = new Vector2(Mathf.Cos((angle - legs[i].outAngle) * Mathf.Deg2Rad),
                    Mathf.Sin((angle - legs[i].outAngle) * Mathf.Deg2Rad));
                Vector2 rightFootPos = partPositions[legs[i].bodyIndex] + (angle1 * legs[i].length);
                Vector2 leftFootPos = partPositions[legs[i].bodyIndex] + (angle2 * legs[i].length);
                if (Vector2.Distance(legs[i].leftFootPos, partPositions[legs[i].bodyIndex]) >= legs[i].length)
                {
                    legs[i].leftTarget = leftFootPos;
                    planted -= 1;
                }
                if (Vector2.Distance(legs[i].rightFootPos, partPositions[legs[i].bodyIndex]) >= legs[i].length)
                {
                    legs[i].rightTarget = rightFootPos;
                    planted -= 1;
                }
            }
            else
            {
                if (!legs[i].rightTarget.Equals(Vector2.positiveInfinity))
                {
                    if (Vector2.Distance(legs[i].rightTarget, legs[i].rightFootPos) > legs[i].length * 0.1f)
                    {
                        legs[i].rightFootPos = Vector2.Lerp(legs[i].rightFootPos, legs[i].rightTarget, 0.5f);
                    }
                    else
                    {
                        legs[i].rightFootPos = legs[i].rightTarget;
                        legs[i].rightTarget = Vector2.positiveInfinity;
                        planted += 1;
                    }
                } 
                if (!legs[i].leftTarget.Equals(Vector2.positiveInfinity))
                {
                    if (Vector2.Distance(legs[i].leftFootPos, legs[i].leftTarget) > legs[i].length * 0.1f)
                    {
                        legs[i].leftFootPos = Vector2.Lerp(legs[i].leftFootPos, legs[i].leftTarget, 0.5f);
                    }
                    else
                    {
                        legs[i].leftFootPos = legs[i].leftTarget;
                        legs[i].leftTarget = Vector2.positiveInfinity;
                        planted += 1;
                    }
                }
            }
           
        }
    }

    private void OnDrawGizmos()
    {
        for (var i = 0; i < partPositions.Count; i++)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(partPositions[i], pointDist/2 * ((length - (float)i)/length));
        }
        for (var i = 0; i < legs.Count; i++)
        {
            try
            {
                Gizmos.color = Color.red;
                /*Vector2 temp1;
                if (legs[i].bodyIndex - legs[i].lookAheadAmount < 0)
                {
                    int diff = legs[i].lookAheadAmount - legs[i].bodyIndex;
                    float ang = partRotations[0] * Mathf.Deg2Rad;
                    Vector2 pos = new Vector2(Mathf.Cos(ang), Mathf.Sin(ang)) * pointDist + partPositions[0];
                    temp1 = pos - partPositions[legs[i].bodyIndex];
                }
                else
                {
                    temp1 = partPositions[legs[i].bodyIndex - legs[i].lookAheadAmount] - partPositions[legs[i].bodyIndex];
                }
                float angle = Mathf.Atan2(temp1.y, temp1.x) * Mathf.Rad2Deg;;
                Vector2 angle1 = new Vector2(Mathf.Cos((angle + legs[i].outAngle) * Mathf.Deg2Rad),
                    Mathf.Sin((angle + legs[i].outAngle) * Mathf.Deg2Rad));
                Vector2 angle2 = new Vector2(Mathf.Cos((angle - legs[i].outAngle) * Mathf.Deg2Rad),
                    Mathf.Sin((angle - legs[i].outAngle) * Mathf.Deg2Rad));*/
               // Gizmos.DrawSphere(partPositions[legs[i].bodyIndex] + (angle1 * legs[i].length), 0.2f);
               // Gizmos.DrawSphere(partPositions[legs[i].bodyIndex] + (angle2 * legs[i].length), 0.2f);
                //Gizmos.DrawLine(partPositions[legs[i].bodyIndex], partPositions[legs[i].bodyIndex] + (angle2 * legs[i].length));
               // Gizmos.DrawLine(partPositions[legs[i].bodyIndex], partPositions[legs[i].bodyIndex] + (angle1 * legs[i].length));
               Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(legs[i].leftFootPos, 0.2f);
                Gizmos.DrawSphere(legs[i].rightFootPos, 0.2f);
                Gizmos.DrawLine(partPositions[legs[i].bodyIndex], legs[i].leftFootPos);
                Gizmos.DrawLine(partPositions[legs[i].bodyIndex], legs[i].rightFootPos);
            }
            catch
            {
                
            }
            
        }
    }
}

[System.Serializable]
public class Leg
{
    public int bodyIndex;
    public float length;
    public float outAngle;
    public int lookAheadAmount;
    public Vector2 leftFootPos;
    public Vector2 leftTarget;
    [HideInInspector]
    public Vector2 rightFootPos = Vector2.positiveInfinity;
    [HideInInspector]
    public Vector2 rightTarget = Vector2.positiveInfinity;
}
