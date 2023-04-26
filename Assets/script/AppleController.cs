using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleController : MonoBehaviour
{
    private Vector3 startPosition;

    public Vector3 minPosition;
    public Vector3 maxPosition;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.y < -1)
        {
            Debug.Log("Apple relocation");
            Vector3 randomPosition = new Vector3(
                Random.Range(minPosition.x, maxPosition.x),
                Random.Range(minPosition.y, maxPosition.y),
                Random.Range(minPosition.z, maxPosition.z)
            );
            this.transform.position = startPosition + randomPosition + Quaternion.Euler(Vector3.up * Random.Range(0f, 360f)) * Vector3.forward * 20f;
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
