using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raptorScript : MonoBehaviour
{
    public float maxVel;
    private Rigidbody2D rb;

    public float acceleration;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x > transform.position.x)
        {
            if ((rb.velocity.x) <= maxVel)
            {
                rb.velocity = new Vector2(rb.velocity.x + (acceleration * Time.deltaTime), rb.velocity.y);
            }
        }
        if (mousePos.x <= transform.position.x)
        {
            if ((rb.velocity.x) >= -maxVel)
            {
                rb.velocity = new Vector2(rb.velocity.x - (acceleration * Time.deltaTime), rb.velocity.y);
            }
        }
    }
}
