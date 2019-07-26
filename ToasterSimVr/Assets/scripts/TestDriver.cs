using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDriver : MonoBehaviour
{
    private WheelInterface wheels;

    public float leftThrottle;
    public float rightThrottle;

    void Start(){
	    wheels = (WheelInterface) GetComponent<WheelInterface>();
	    print(wheels);
    } 

    // Update is called once per frame
    void FixedUpdate()
    {
        wheels.setThrottles(leftThrottle, rightThrottle);

    }
}
