using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seaStranglerCosmetics : MonoBehaviour
{
    public List<tentacleGravityTesting> tentacles;

    public List<Transform> heads;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (var i = 0; i < tentacles.Count; i++)
        {
            Transform t = tentacles[i].parts[tentacles[i].length - 1].transform;
            heads[i].position = t.position;
            heads[i].rotation = Quaternion.Euler(0,0,t.rotation.eulerAngles.z + 90f);
        }
    }
}
