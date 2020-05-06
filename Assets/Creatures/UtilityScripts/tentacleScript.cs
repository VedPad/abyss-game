using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tentacleScript : MonoBehaviour
{
    public int length;

    public float pointDist;
    public float drawSize;

    private List<Vector2> partPositions = new List<Vector2>();

    private List<float> partRotations = new List<float>();
    public float rotationLerp;
    public float movementVelocity;
    public float rotThreshold;
    public float targetDist;
    public GameObject headSprite;
    public GameObject middleSprite;
    private List<GameObject> bodyParts = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        partPositions.Add(this.gameObject.transform.position);
        partRotations.Add(this.transform.eulerAngles.z);
        bodyParts.Add(middleSprite);
        for (var i = 1; i < length; i++)
        {
            if (i != 1 && i != length - 1)
            {
                bodyParts.Add(GameObject.Instantiate(middleSprite, this.transform));
            }
            partRotations.Add(this.transform.eulerAngles.z);
            partPositions.Add(new Vector2(Mathf.Cos(partRotations[i] * Mathf.Deg2Rad), Mathf.Sin(partRotations[i] * Mathf.Deg2Rad)) * pointDist + partPositions[i - 1]);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (var i = partPositions.Count - 1; i > 0; i--)
        {
            Vector2 v;
            Vector2 target;
            if (i == partPositions.Count - 1)
            {
                target = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
                
                
            }
            else
            {
                target = partPositions[i + 1];
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
            float oldPartRot = partRotations[i];
            partRotations[i] = Mathf.LerpAngle(partRotations[i], rot, rotationLerp);
            //partRotations[i] = rot;
            if (Mathf.Abs(partRotations[i] - partRotations[i - 1]) > rotThreshold)
            {
                partRotations[i] = oldPartRot;
            }
                    //partRotations[i] = Mathf.LerpAngle(partRotations[i], rot, rotationLerp);
            






        }
        for (var i = partPositions.Count - 1; i > 0; i--)
        {
            Vector2 dir = new Vector2(Mathf.Cos(partRotations[i] * Mathf.Deg2Rad),
                Mathf.Sin(partRotations[i] * Mathf.Deg2Rad));
            Vector2 dir1 = dir;
            if (i != partPositions.Count - 1)
            {
                dir1 = (partPositions[i + 1] - partPositions[i]).normalized;
            }
            float dist = drawSize * 2;
            /*Debug.DrawRay(partPositions[i] + (dir1 + (dir1 - dir)) * dist,
                    (dir1 + (dir1 - dir)) * dist * 2);
                Debug.DrawRay(partPositions[i] + dir * dist,
                    dir * dist * 2);*/
                if (!Physics2D.Raycast(partPositions[i] + (dir1 + (dir1 - dir)) * dist,
                        (dir1 + (dir1 - dir)),
                        dist * 2))
                {
                    Vector2 temp = dir * movementVelocity * Time.deltaTime;
                    // temp.y += -9.8f * Time.deltaTime;


                    partPositions[i] += temp;
                }


                if ( (partPositions[i - 1] - partPositions[i]).magnitude >= pointDist)
            {
                partPositions[i] = partPositions[i - 1] +
                                   (partPositions[i] - partPositions[i - 1]).normalized * pointDist;
            }
            if ((partPositions[i - 1] - partPositions[i]).magnitude < pointDist && i > 1)
            {
                partPositions[i - 1] = partPositions[i] +
                                       (partPositions[i - 1] - partPositions[i]).normalized * pointDist;
            }




            
        }

        for (var i = partPositions.Count - 1; i > 0; i--)
        {
            
            if (i == partPositions.Count - 1)
            {
                headSprite.transform.position = partPositions[partPositions.Count - 1];
                headSprite.transform.rotation = Quaternion.Euler(0,0,partRotations[partRotations.Count - 1]);
            }
            else
            {
                bodyParts[i - 1].transform.position = partPositions[i];
                Vector2 angleVec = partPositions[i + 1] - partPositions[i - 1];
                bodyParts[i - 1].transform.rotation = Quaternion.Euler(0,0,Mathf.Atan2(angleVec.y, angleVec.x) * Mathf.Rad2Deg + 90);
            }
        }

        
    }

    private void OnDrawGizmos()
    {
        for (var i = 0; i < partPositions.Count; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(partPositions[i], drawSize);
            if (i < partPositions.Count - 1)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(partPositions[i], partPositions[i + 1]);
            }
            
        }
    }
}
