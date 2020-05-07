using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public int id;

    public float lookDistance;

    public float force;
    public float hitAngles;
    public LayerMask detect;

    [HideInInspector]
    public Rigidbody2D rb;

    public float maxVel;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /*private void FixedUpdate()
    {
        DoRayCasts();
        rb.AddRelativeForce(new Vector2(force, 0));
        if (rb.velocity.magnitude > maxVel)
        {
            rb.velocity = rb.velocity.normalized * maxVel;
        }
    }*/

    public void DoRayCasts()
    {
        float step = Mathf.PI / hitAngles;
        float ang = transform.eulerAngles.z * Mathf.Deg2Rad;
        float negAng = transform.eulerAngles.z * Mathf.Deg2Rad;
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, new Vector2(Mathf.Cos(ang), Mathf.Sin(ang)),
            lookDistance, detect);
        if (hit2D)
        {
            bool HasBroken = false;
            for (var i = 1; i < 180f / hitAngles; i++)
            {
                 ang += step;
                 negAng -= step;
                 if (!Physics2D.Raycast(transform.position, new Vector2(Mathf.Cos(ang), Mathf.Sin(ang)), lookDistance,
                     detect))
                 {
                     transform.rotation = Quaternion.Euler(0,0,ang * Mathf.Rad2Deg);
                     HasBroken = true;
                     break;
                 }
                 if (!Physics2D.Raycast(transform.position, new Vector2(Mathf.Cos(negAng), Mathf.Sin(negAng)), lookDistance,
                                      detect))
                 {
                     transform.rotation = Quaternion.Euler(0,0,negAng * Mathf.Rad2Deg);
                     HasBroken = true;
                     break;
                 }
            }

            if (!HasBroken)
            {
                transform.rotation = Quaternion.Euler(0,0,negAng * Mathf.Rad2Deg);
            }
        }
        
    }
}
