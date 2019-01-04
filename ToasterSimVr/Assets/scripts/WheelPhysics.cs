using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelPhysics : WheelInterface {
	//rigidbody to apply force to
	public Rigidbody robot;
    public Vector3 comOffset;
	public float maxVel;
    public float stationaryTorque;
    public float freeBackEMF;
    public float fricCoeficient;
    public float lateralSlipMultiplier;
    public float drivebaseRadius;
    public float wheelContactPos;

    private float leftThrottle;
    private float rightThrottle;
    	
    private float maxLinForce;
    //start is called once at the beginning of excecution
	void Start(){
		//set the center of mass
		robot.centerOfMass = comOffset;
        maxLinForce = robot.mass * 9.8f * fricCoeficient;
	}

	// FixedUpdate is called once per physics frame
	void FixedUpdate () {
        //print("Command: Left: " + leftThrottle + ", Right: " + rightThrottle);
        //print("Lin vel: " + getFwdVel() + ", Lateral vel: " + getTransVel());
	    Vector3 leftDriveTorque =  transform.forward * getMotorTorque(-drivebaseRadius, leftThrottle);
	    Vector3 rightDriveTorque = transform.forward * getMotorTorque(drivebaseRadius, rightThrottle);

        Vector3 lateralForce = transform.right * Mathf.Clamp(getTransVel() * lateralSlipMultiplier, -maxLinForce, maxLinForce);
        
        Vector3 leftNetForce = leftDriveTorque + (lateralForce * 0.5f);
	    Vector3 rightNetForce = rightDriveTorque + (lateralForce * 0.5f);

        if(leftNetForce.magnitude > maxLinForce / 2.0){
    		//print("Left slipping");
	    	leftNetForce = leftNetForce.normalized * maxLinForce * 0.5f;
		}

    	if(rightNetForce.magnitude > maxLinForce / 2.0){
    		//print("Right slipping");
	    	rightNetForce = leftNetForce.normalized * maxLinForce * 0.5f;
        }

        leftNetForce = new Vector3(leftNetForce.x, 0, leftNetForce.z);
        rightNetForce = new Vector3(rightNetForce.x, 0, rightNetForce.z);

        robot.AddForceAtPosition(leftNetForce, transform.position + new Vector3(0,-wheelContactPos,0) + drivebaseRadius * transform.right);
        robot.AddForceAtPosition(rightNetForce, transform.position + new Vector3(0,-wheelContactPos,0) - drivebaseRadius * transform.right);
        //robot.AddForce(new Vector3(0,0,1));
    }



    float getFwdVel(){
        return Vector3.Dot(robot.velocity, transform.forward);
    }

    float getTransVel(){
        return Vector3.Dot(robot.velocity, transform.right);
    }

    float getWheelVel(float offset){
	    return getFwdVel() + robot.angularVelocity.y * offset;
    }

    private float getMotorTorque(float offset, float command){
        if(command == 0){
            return -getWheelVel(offset) * freeBackEMF;
        }else{
            return command * stationaryTorque - getWheelVel(offset) / maxVel * stationaryTorque;
        }
    }

	override public void setThrottles(float left, float right){

		leftThrottle = left;
		rightThrottle = right;
	}
}
