using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ReflectionCamScript : MonoBehaviour
{
    private Transform cam;

    private Vector2 offset;

    public GameObject reflection;

    private Camera _camera;

    public bool isCamera;

    private float offsetY;
    // Start is called before the first frame update
    void Start()
    {
        if (isCamera)
        {
            _camera = GetComponent<Camera>();
        }
        cam = Camera.main.transform;
        offset = this.transform.position - cam.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCamera)
        {
            if (Mathf.Abs(cam.position.y - reflection.transform.position.y - 3) > Camera.main.orthographicSize + _camera.orthographicSize * 2)
            {
                _camera.enabled = false;
               // reflection.SetActive(false);
            }
            else
            {
                _camera.enabled = true;
                //reflection.SetActive(true);
            }
        }
      
        this.transform.position = new Vector3(cam.position.x + offset.x, this.transform.position.y, this.transform.position.z);
    }
}
