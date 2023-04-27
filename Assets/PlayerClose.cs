using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClose : MonoBehaviour
{

    private Transform playerTransform;
    public float distanceThreshold = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Your existing Start() code

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null)
        {
            float distance = Vector3.Distance(transform.position, playerTransform.position);

            if (distance <= distanceThreshold)
            {
                FacePlayer();
            }
        }
    }

    void FacePlayer()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        direction.y = 0; // Prevent the character from rotating upwards or downwards
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);
    }

}
