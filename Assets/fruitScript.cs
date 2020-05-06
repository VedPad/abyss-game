using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fruitScript : MonoBehaviour
{
    public SpriteRenderer plant;
    private Transform plantTransform;

    public int vertice;

    private Vector2[] verts;
    // Start is called before the first frame update
    void Start()
    {
        plantTransform = plant.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = plantTransform.TransformPoint(plant.sprite.vertices[vertice]);
    }
}
