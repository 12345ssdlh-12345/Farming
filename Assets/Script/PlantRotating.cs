using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantRotating : MonoBehaviour
{
    public float rotatingSpeed;
    void Start()
    {
        rotatingSpeed = 2f;
    }

    void Update()
    {
        transform.Rotate(Vector3.up, rotatingSpeed,Space.Self);
    }
}
