using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterScript : MonoBehaviour
{
    public int columnCount;

    public float depth;

    private MeshFilter mF;
    private PolygonCollider2D mC;
    
    private float[] velocities;
    private float[] accelerations;
    private float[] leftDeltas;
    private float[] rightDeltas;
    const float springconstant = 0.02f;
    const float damping = 0.04f;
    const float spread = 0.05f;
    private LineRenderer lR;
    private Transform cam;
    private Vector3 lastFrameCam;
    private Vector3[] verts;

    public float mass;

    private int camIndex;

    public int indexesAround;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        GetComponent<MeshRenderer>().sortingLayerName = "water";
        lR = this.GetComponent<LineRenderer>();
        mC = GetComponent<PolygonCollider2D>();
        velocities = new float[columnCount];
        accelerations = new float[columnCount];
        leftDeltas = new float[columnCount];
        rightDeltas = new float[columnCount];
        mF = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[columnCount * 2];
        int[] triangles = new int[(vertices.Length - 2) * 3];
        for (var i = 0; i < columnCount; i++)
        {
            float x = -(0.25f * columnCount / 2) + (i * 0.25f);
            vertices[i * 2] = new Vector3(x,0);
            vertices[i * 2 + 1] = new Vector3(x,-depth);
        }

        int index = 0;
        for (int i = 0; i < columnCount - 1; i++)
        {
            triangles[index] = i * 2 + 2;
            triangles[index + 1] = i * 2 + 1;
            triangles[index + 2] = i * 2;
            
            triangles[index + 3] = i * 2 + 3;
            triangles[index + 4] = i * 2 + 1;
            triangles[index + 5] = i * 2 + 2;
            index += 6;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        verts = vertices;
        
        float xpos = transform.TransformPoint(cam.position).x;
        xpos -= vertices[0].x;
        camIndex = Mathf.RoundToInt((columnCount-1)*(xpos / (vertices[vertices.Length-2].x - vertices[0].x)));
        lastFrameCam = cam.position;
        mF.mesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(cam.position.y - this.transform.position.y) > Camera.main.orthographicSize)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Splash(2f, 2f);
        }
        if (!cam.position.Equals(lastFrameCam))
        {
            float xpos = transform.TransformPoint(cam.position).x;
            xpos -= verts[0].x;
            camIndex = Mathf.RoundToInt((columnCount-1)*(xpos / (verts[verts.Length-2].x - verts[0].x)));
        }

        lastFrameCam = cam.position;
       
        for (int i = camIndex - indexesAround; i < camIndex + indexesAround; i++)
        {
            float force = springconstant * (verts[i * 2].y) + velocities[i]*damping ;
            accelerations[i] = -force/mass;
            if (velocities[i] > 1f)
            {
                velocities[i] = 1f;
            }
            verts[i * 2].y += velocities[i];
            velocities[i] += accelerations[i];
            
        }
   
        
        for (int j = 0; j < 8; j++)
        {
            for (int i =  camIndex - indexesAround; i <  camIndex + indexesAround; i++)
            {
                if (i > 0)
                {
                    leftDeltas[i] = spread * (verts[i * 2].y - verts[(i-1) * 2].y);
                    velocities[i - 1] += leftDeltas[i];
                }
                if (i < columnCount - 1)
                {
                    rightDeltas[i] = spread * (verts[i * 2].y - verts[(i+1) * 2].y);
                    velocities[i + 1] += rightDeltas[i];
                }
            }
        }
        
        float highestY = Mathf.NegativeInfinity;
        for (int i =  camIndex - indexesAround; i <  camIndex + indexesAround; i++)
        {
            if (i > 0) 
            {
                verts[(i-1) * 2].y += leftDeltas[i];
                if (verts[(i - 1) * 2].y > highestY)
                {
                    highestY = verts[(i - 1) * 2].y;
                }
            }
            if (i < columnCount - 1) 
            {
                verts[(i+1) * 2].y += rightDeltas[i];
                if (verts[(i + 1) * 2].y > highestY)
                {
                    highestY = verts[(i + 1) * 2].y;
                }
            }
        }
        lR.positionCount = (camIndex + indexesAround) - (camIndex - indexesAround);
        Vector3[] positions = new Vector3[lR.positionCount];
        for (var i =  camIndex - indexesAround; i <  camIndex + indexesAround; i++)
        {
            Vector3 pos = transform.InverseTransformPoint(verts[i * 2]);
            positions[i - (camIndex - indexesAround)] =
                new Vector3(pos.x, transform.position.y + verts[i * 2].y, pos.z);
        }
        lR.SetPositions(positions);
        mF.mesh.vertices = verts;
        List<Vector2> verts2D = new List<Vector2>();
        verts2D.Add(new Vector2(verts[0].x, highestY));
        verts2D.Add(new Vector2(verts[verts.Length - 2].x, highestY));
        verts2D.Add(new Vector2(verts[verts.Length - 2].x, -depth));
        verts2D.Add(new Vector2(verts[0].x, -depth));
        mC.SetPath(0, verts2D);
    }

    public void Splash(float xpos, float velocity)
    {
        if (xpos >= verts[0].x && xpos <= verts[verts.Length - 2].x)
        {
            xpos -= verts[0].x;
            int index = Mathf.RoundToInt((columnCount-1)*(xpos / (verts[verts.Length-2].x - verts[0].x)));
            if (index > camIndex - indexesAround && index < camIndex + indexesAround)
            {
                velocities[index] = velocity;
            }
           
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.attachedRigidbody != null)
        {
            float xPos = transform.TransformPoint(other.transform.position).x;
            Splash(xPos, other.attachedRigidbody.velocity.y*other.attachedRigidbody.mass/40f);
            if (other.gameObject.CompareTag("waterCreature"))
            {
                other.attachedRigidbody.gravityScale = 0f;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.attachedRigidbody != null)
        {
            float xPos = transform.TransformPoint(other.transform.position).x;
            Splash(xPos, (other.attachedRigidbody.velocity.y*other.attachedRigidbody.mass/40f));
            if (other.gameObject.CompareTag("waterCreature"))
            {
                other.attachedRigidbody.gravityScale = 2f;
            }
        }
    }
}
