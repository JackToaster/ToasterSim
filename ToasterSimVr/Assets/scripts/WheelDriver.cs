using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelDriver : WheelInterface {
	public GameObject[] leftW;
	public GameObject[] rightW;
	private Rigidbody[] leftRB;
	private Rigidbody[] rightRB;
	private WheelContactForceSensor[] leftContact;
	private WheelContactForceSensor[] rightContact;

	public float maxVel;
	public float stationaryTorque;
	public float COF;

	private float l = 0;
	private float r = 0;

	//start is called once at the beginning of excecution
	void Start(){
		leftRB = new Rigidbody[leftW.Length];	
		rightRB = new Rigidbody[rightW.Length];
		leftContact = new WheelContactForceSensor[leftW.Length];
		rightContact = new WheelContactForceSensor[rightW.Length];

		for(int i = 0; i < leftW.Length; i++){
			leftRB[i] = leftW[i].GetComponent<Rigidbody>();
			leftRB[i].maxAngularVelocity = 10000;
			leftContact[i] = leftW[i].GetComponent<WheelContactForceSensor>();
		}

		for(int i = 0; i < rightW.Length; i++){
			rightRB[i] = rightW[i].GetComponent<Rigidbody>();
			rightRB[i].maxAngularVelocity = 10000;
			rightContact[i] = rightW[i].GetComponent<WheelContactForceSensor>();
		}
	}

	// FixedUpdate is called once per physics frame
	void FixedUpdate () {
		// Calculate the motor torque for each motor based on its speed.
		float leftMotorVel = (leftW[1].transform.InverseTransformDirection(leftRB[1].angularVelocity)).y;
		float rightMotorVel = (rightW[1].transform.InverseTransformDirection(rightRB[1].angularVelocity)).y;
		
		float lTorque = leftMotorVel / maxVel * stationaryTorque + l * stationaryTorque;	
		float rTorque = rightMotorVel / maxVel * stationaryTorque + r * stationaryTorque;

		// Calculate the total normal force to find the total output force
		float leftNorm = 0;
		foreach(WheelContactForceSensor contact in leftContact){
			leftNorm += contact.getContactForce().magnitude;
		}

		float rightNorm = 0;
		foreach(WheelContactForceSensor contact in rightContact){
			rightNorm += contact.getContactForce().magnitude;
		}
		
		// Calculate the amount of torque applied based on the friction and motor torque.
		float leftMaxTorque = leftNorm * COF;
		float leftAppliedTorque = Mathf.Clamp(lTorque, -leftMaxTorque, leftMaxTorque);
		
		float rightMaxTorque = rightNorm * COF;
		float rightAppliedTorque = Mathf.Clamp(rTorque, -rightMaxTorque, rightMaxTorque);
		
		// Apply the torque.
		for(int i = 0; i < leftW.Length; i++){
			// How much of the total force is applied
			float forcePercent = leftContact[i].getContactForce().magnitude / leftNorm;
			
			Rigidbody wheel = leftRB[i];
			GameObject wheelO = leftW[i];

			Vector3 forceDirectionVector = transform.forward;
			Vector3 contactNormal = leftContact[i].getContactNormal();
			Vector3.OrthoNormalize(ref contactNormal,ref forceDirectionVector);
		      	Vector3 forceVector = forceDirectionVector * leftAppliedTorque * forcePercent;
			
			Debug.DrawRay(wheelO.transform.position, forceVector);	
			wheel.AddForce(forceVector);	
		}
		for(int i = 0; i < rightW.Length; i++){
			// How much of the total force is applied
			float forcePercent = rightContact[i].getContactForce().magnitude / rightNorm;
			
			Rigidbody wheel = rightRB[i];
			GameObject wheelO = rightW[i];

			Vector3 forceDirectionVector = transform.forward;
			Vector3 contactNormal = rightContact[i].getContactNormal();
			Vector3.OrthoNormalize(ref contactNormal,ref forceDirectionVector);
		      	Vector3 forceVector = forceDirectionVector * rightAppliedTorque * forcePercent;
			
			Debug.DrawRay(wheelO.transform.position, forceVector);	
			wheel.AddForce(forceVector);	
		}
	}

	override public void setThrottles(float left, float right){
		l = 0.0f;
		r = 0.0f;
	}
}
