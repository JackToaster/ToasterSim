using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class robotDriver : MonoBehaviour
{
    public WheelPhysics wheels;
    // Update is called once per frame
    void FixedUpdate()
    {
        //Don't know how to add options yet, but if you use a controller instead of keyboard invert vertical
        float vertical = -Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        wheels.setThrottles(vertical - horizontal, vertical + horizontal);


    }
}
