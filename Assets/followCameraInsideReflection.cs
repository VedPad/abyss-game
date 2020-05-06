using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followCameraInsideReflection : MonoBehaviour
{
    private Transform cam;
    public float offsetX;

    public float clampY;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(cam.position.x + offsetX, Mathf.Min(clampY, cam.position.y), transform.position.z);
    }
}
