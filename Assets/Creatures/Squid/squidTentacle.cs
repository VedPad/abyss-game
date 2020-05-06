using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class squidTentacle : MonoBehaviour
{
    public GameObject circleCol;
    public int length;

    public float pointDist;
    
    [HideInInspector]
    public List<GameObject> parts = new List<GameObject>();

    [HideInInspector] public List<Rigidbody2D> partRigidbodies = new List<Rigidbody2D>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
