using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject player; // Reference to the player object  

    public Vector3 cameraOffset; 
    public Vector3 stillCamera; 
    public float smoothSpeed = 0.125f;

    bool followPlayer = true;

    // Update is called once per frame
    void Update()
    {
        if (followPlayer) // Makes camera move to player if followPlayer is true 
        {
            Vector3 desiredPostion = player.transform.position + cameraOffset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPostion, smoothSpeed);

            transform.position = smoothedPosition;
        }
        else if (!followPlayer) // Makes camera static if followPlayer is false 
        {
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, stillCamera, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }

    public void FollowCamera(bool follow) // Called to turn on or off player follow
    {
        followPlayer = follow;
    }
}
