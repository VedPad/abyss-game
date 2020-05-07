using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public struct boid{
    public float x;
    public float y;
    public float dirDegrees;
    public int id;
    public float lookDistance;
    public float debug1;
    public float debug2;
    public float realAngle;
}

public class BoidsCPUSide : MonoBehaviour
{
    private List<Boid> boids;
    private List<Transform> boidTransforms;
    private ComputeShader _boidsShader;
    private List<boid> boidNodes = new List<boid>();
    private ComputeBuffer boidBuffer;

    private int kiMainSimulation;
    // Start is called before the first frame update
    void Start()
    {
        boids = FindObjectsOfType<Boid>().ToList();
        print(boids.Count);
        boidTransforms = new List<Transform>();
        for (var i = 0; i < boids.Count; i++)
        {
            boid boid = new boid();
            boidTransforms.Add(boids[i].transform);
            boid.x = boidTransforms[i].position.x;
            boid.y = boidTransforms[i].position.y;
            boid.dirDegrees = boidTransforms[i].rotation.eulerAngles.z;
            boid.id = boids[i].id;
            boid.lookDistance = boids[i].lookDistance;
            boidNodes.Add(boid);
        }
        _boidsShader = Resources.Load<ComputeShader>("BoidsCompute");
        kiMainSimulation = _boidsShader.FindKernel("MainSimulation");
        SetupBuffers();
    }

    void SetupBuffers()
    {
        boidBuffer = new ComputeBuffer(boidNodes.Count, 4 * 8);
        boidBuffer.SetData(boidNodes);
        _boidsShader.SetBuffer(kiMainSimulation, "boidBuffer", boidBuffer);
        _boidsShader.SetInt("nBoids", boidBuffer.count);
    }

    void SetupBoidBuffer()
    {
        for (var i = 0; i < boids.Count; i++)
        {
            boid boid = boidNodes[i];
            boid.x = boidTransforms[i].position.x;
            boid.y = boidTransforms[i].position.y;
            boid.dirDegrees = boidTransforms[i].rotation.eulerAngles.z * Mathf.Deg2Rad;
            boid.id = boids[i].id;
            boid.lookDistance = boids[i].lookDistance;
            boid.debug1 = 0;
            boid.debug2 = 0;
            boid.realAngle = boidTransforms[i].rotation.eulerAngles.z * Mathf.Deg2Rad;
            boidNodes[i] = boid;
        }
        boidBuffer.SetData(boidNodes);
    }

    private void FixedUpdate()
    {
        for (var i = 0; i < boids.Count; i++)
        {
            boids[i].DoRayCasts();
            boids[i].rb.AddRelativeForce(new Vector2(boids[i].force, 0));
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (var i = 0; i < boids.Count; i++)
        {
            if (boids[i].rb.velocity.magnitude > boids[i].maxVel)
            {
                boids[i].rb.velocity = boids[i].rb.velocity.normalized * boids[i].maxVel;
            }
        }

        SetupBoidBuffer();
        _boidsShader.Dispatch(kiMainSimulation, Mathf.CeilToInt(boidNodes.Count/32f), 1,1);
        boid[] array = new boid[boidBuffer.count];
        boidBuffer.GetData(array);
        boidNodes = array.ToList();
        for (var i = 0; i < boidNodes.Count; i++)
        {
            float zAngle = boidTransforms[i].rotation.eulerAngles.z;
           // print(boidNodes[i].debug1 + " " + boidNodes[i].debug2 + " " + boidTransforms[i].gameObject.name);
            boidTransforms[i].rotation = Quaternion.Euler(0,0,Mathf.LerpAngle(zAngle, boidNodes[i].dirDegrees * Mathf.Rad2Deg, 0.02f));
        }
    }

    private void OnDisable()
    {
        boidBuffer.Release();
    }
}
