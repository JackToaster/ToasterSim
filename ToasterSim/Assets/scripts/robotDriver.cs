using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class robotDriver : MonoBehaviour {
	public List<WheelCollider> LeftWheels;
	public List<WheelCollider> RightWheels;
	public float motorTorque;
	public AnimationCurve speed;
	public float turnTorque;
	public AnimationCurve turn;
	public float movingTurnTorque;
	public float brakeTorque;
	// Update is called once per frame
	void FixedUpdate () {
		float vertical = Input.GetAxis ("Vertical");
		float horizontal = Input.GetAxis ("Horizontal");

		vertical *= speed.Evaluate (Mathf.Abs(vertical));
		horizontal *= turn.Evaluate (Mathf.Abs(horizontal));

		float left, right;
		if (Mathf.Abs(vertical) < 0.05) {
			left = vertical * motorTorque + horizontal * turnTorque;
			right = vertical * motorTorque - horizontal * turnTorque;
		} else {
			left = vertical * motorTorque + horizontal * movingTurnTorque;
			right = vertical * motorTorque - horizontal * movingTurnTorque;
		}
		foreach (WheelCollider wheel in LeftWheels) {
			if (Mathf.Abs(left) < 1) {
				wheel.brakeTorque = brakeTorque;
			} else {
				wheel.brakeTorque = 0;
			}
			wheel.motorTorque = left;
		}
		foreach (WheelCollider wheel in RightWheels) {
			if (Mathf.Abs(right) < 1) {
				wheel.brakeTorque = brakeTorque;
			} else {
				wheel.brakeTorque = 0;
			}
			wheel.motorTorque = right;
		}
	}
}
