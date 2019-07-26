using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class robotDriver : MonoBehaviour
{
    private WheelInterface wheels;
    public float inputRampRate;

    private float leftThrottle;
    private float rightThrottle;

    void Start(){
	    wheels = (WheelInterface) GetComponent<WheelInterface>();
	    print(wheels);
    } 

    // Update is called once per frame
    void FixedUpdate()
    {
        //Don't know how to add options yet, but if you use a controller instead of keyboard invert vertical
        float vertical = -Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        

        float power;
        float rotation;
        if(vertical > 0) power = Mathf.Pow(vertical, 1.5f);
        else power = -Mathf.Pow(-vertical, 1.5f);
        
        if(horizontal > 0) rotation = Mathf.Pow(horizontal, 1.5f);
        else rotation = -Mathf.Pow(-horizontal,1.5f);
        
        const float gain = 1f;
		const float limit = 0.25f;
		const float subLimitWeight = 0.8f;

	    float arcadeRotation = rotation * 0.8f;
		float cheesyRotation = rotation * gain * Mathf.Abs(power);
		float arcadeWeight = (1 - Mathf.Abs(vertical) / limit / subLimitWeight);
		float cheesyWeight = (Mathf.Abs(vertical) / limit * subLimitWeight);

		float outputRotation = cheesyRotation;
		if (Mathf.Abs(vertical) <= limit)
            outputRotation = cheesyWeight * cheesyRotation + arcadeWeight * arcadeRotation;

        float idealL = clamp(power + outputRotation,-1,1);
	    float idealR = clamp(power - outputRotation,-1,1);
        
        float maxChange = Time.fixedDeltaTime * inputRampRate;

        if(Mathf.Abs(idealL - leftThrottle) < maxChange) leftThrottle = idealL;
        else if(idealL > leftThrottle) leftThrottle += maxChange;
        else leftThrottle -= maxChange;

        if(Mathf.Abs(idealR - rightThrottle) < maxChange) rightThrottle = idealR;
        else if(idealR > rightThrottle) rightThrottle += maxChange;
        else rightThrottle -= maxChange;

        wheels.setThrottles(leftThrottle, rightThrottle);

    }

    private float clamp(float val, float min, float max){
        if(val < min) return min;
        if(val > max) return max;
        return val;
    }
}
