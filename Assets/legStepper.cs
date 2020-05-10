using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class legStepper : MonoBehaviour
{
    // The position and rotation we want to stay in range of
    [SerializeField] Transform homeTransform;

    private Vector2 targetPos;
    // Stay within this distance of home
    [SerializeField] float wantStepAtDistance;

    public float legLength;
    // How long a step takes to complete
    [SerializeField] float moveDuration;
  
    // Is the leg moving?
    public bool Moving;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        targetPos = (Vector2) homeTransform.position +
                    (new Vector2(Mathf.Cos(homeTransform.parent.rotation.eulerAngles.z * Mathf.Deg2Rad),
                        Mathf.Cos(homeTransform.parent.rotation.eulerAngles.z * Mathf.Deg2Rad)) * legLength);
        
        if (Moving) return;

        float distFromHome = Vector3.Distance(transform.position, targetPos);

        // If we are too far off in position or rotation
        if (distFromHome > wantStepAtDistance)
        {
            // Start the step coroutine
            StartCoroutine(MoveToHome());
        }
    }
    
    IEnumerator MoveToHome()
    {
        // Indicate we're moving (used later)
        Moving = true;

        // Store the initial conditions
        Quaternion startRot = transform.rotation;
        Vector3 startPoint = transform.position;

        //Quaternion endRot = homeTransform.rotation;
        Vector3 endPoint = new Vector3(targetPos.x, targetPos.y, homeTransform.position.z);

        // Time since step started
        float timeElapsed = 0;

        // Here we use a do-while loop so the normalized time goes past 1.0 on the last iteration,
        // placing us at the end position before ending.
        do
        {
            // Add time since last frame to the time elapsed
            timeElapsed += Time.deltaTime;

            float normalizedTime = timeElapsed / moveDuration;

            // Interpolate position and rotation
            transform.position = Vector3.Lerp(startPoint, endPoint,normalizedTime);
            //transform.rotation = Quaternion.Slerp(startRot, endRot, normalizedTime);

            // Wait for one frame
            yield return null;
        }
        while (timeElapsed < moveDuration);

        // Done moving
        Moving = false;
    }
}
