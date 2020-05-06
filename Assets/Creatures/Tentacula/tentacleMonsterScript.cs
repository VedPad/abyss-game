using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class tentacleMonsterScript : MonoBehaviour
{
    private Rigidbody2D rb;

    public float maxVelocity;

    public float movementForce;
    private float initMovementForce;

    public List<tentacleGravityTesting> tentacles;

    public float tentacleGrabAreaDegrees;

    public int tentacleRayCount;
    public LayerMask grabMask;

    private int grabbedCount;

    public float switchTime;

    private float switchTimer;

    private bool disabled;

    public GameObject cosmetics;
    // Start is called before the first frame update
    void Start()
    {
        initMovementForce = movementForce;
        rb = GetComponent<Rigidbody2D>();
        for (var i = 0; i < tentacles.Count; i++)
        {
            int rays = tentacleRayCount;
            float totalRadians = tentacleGrabAreaDegrees * Mathf.Deg2Rad;
            float radians = -totalRadians/2;
            float sectorArea = totalRadians / rays;
            float length = tentacles[i].length * tentacles[i].pointDist;
            for (var j = 0; j < rays; j++)
            {
                float angle = tentacles[i].gameObject.transform.rotation.eulerAngles.z * Mathf.Deg2Rad + radians;
                RaycastHit2D hit = Physics2D.Raycast(this.gameObject.transform.position, new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)), length, grabMask);
                if (hit)
                {
                    tentacles[i].targetPoint = hit.point;
                    break;
                }

                radians += sectorArea;
            }

            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!disabled)
        {
            for (var i = 0; i < tentacles.Count; i++)
            {
                BoxCollider2D[] colliders = tentacles[i].parts[0].GetComponents<BoxCollider2D>();
                for (var j = 0; j < colliders.Length; j++)
                {
                    colliders[j].enabled = false;
                }
            }

            disabled = true;
        }
        grabbedCount = 0;
        for (var i = 0; i < tentacles.Count; i++)
        {
            if (tentacles[i].isGrabbed)
            {
                grabbedCount += 1;
            }
        }

        switchTimer += Time.deltaTime;
        if (switchTimer >= switchTime)
        {

            switchTimer = 0;
            for (var j = 0; j < tentacles.Count; j++)
            {
                int i = j;
                    int ray = Random.Range(0, tentacleRayCount - 1 * (int)360/(int)tentacleGrabAreaDegrees);
                    float totalRadians = 360 * Mathf.Deg2Rad;
                    float sectorArea = totalRadians / (tentacleRayCount * (int)360/(int)tentacleGrabAreaDegrees);
                    float radians = (-totalRadians/2) + (sectorArea * ray);

                    float length = tentacles[i].length * tentacles[i].pointDist;
                    float angle = tentacles[i].gameObject.transform.rotation.eulerAngles.z * Mathf.Deg2Rad + radians;
                    RaycastHit2D hit = Physics2D.Raycast(this.gameObject.transform.position, new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)), length, grabMask);
                    if (hit)
                    {
                        tentacles[i].targetPoint = hit.point;
                    }
            }
        }
        

        rb.gravityScale = (tentacles.Count - (float)grabbedCount) / tentacles.Count;
        Vector2 v = Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position;
        movementForce = initMovementForce * (1 - rb.gravityScale);
        rb.AddForce(v.normalized * movementForce * Time.deltaTime);
        if (rb.velocity.magnitude >= maxVelocity)
        {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }
    }
    
    void OnBecameVisible()
    {
        cosmetics.SetActive(true);
    }

    private void OnBecameInvisible()
    {
        cosmetics.SetActive(false);
    }
}
