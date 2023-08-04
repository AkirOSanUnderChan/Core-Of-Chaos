using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMove : MonoBehaviour
{
    public Transform cameraPosition;

    // Update is called once per frame
    void Update()
    {
    }
    private void FixedUpdate()
    {
        transform.position = cameraPosition.position;

    }
}
