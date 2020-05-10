using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grassMaterialBlock : MonoBehaviour
{
    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;

    public float strength;

    public float lerpVal;
    // Start is called before the first frame update
    void Start()
    {
        _propBlock = new MaterialPropertyBlock();
        _renderer = GetComponent<Renderer>();
        _renderer.GetPropertyBlock(_propBlock);
        _propBlock.SetFloat("_WindDensity", 0.06f);
        _renderer.SetPropertyBlock(_propBlock);
        Vector2 temp = Vector2.zero;
        temp.x = 5;
    }

    // Update is called once per frame
    void Update()
    {
       // _renderer.SetPropertyBlock(_propBlock);
        //_propBlock.SetFloat("_WindStrength", strength);
        if (Mathf.Abs(strength) > 0.05f)
        {
            strength *= lerpVal;
            _propBlock.SetFloat("_WindStrength", Mathf.Clamp(strength, -3, 3));
            _renderer.SetPropertyBlock(_propBlock);
            
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Mathf.Abs(strength) <= 2f)
        {
            Rigidbody2D rB = other.attachedRigidbody;
            if (rB != null)
            {
                strength +=Mathf.Clamp(Mathf.Sign(-rB.velocity.x), -1, 1);
            }
        }
       
    }

   
}
