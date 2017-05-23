using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class robotDriver : MonoBehaviour {
	public WheelPhysics wheels;
	// Update is called once per frame
	void FixedUpdate () {
		float vertical = Input.GetAxis ("Vertical");
		float horizontal = Input.GetAxis ("Horizontal");

		wheels.setThrottles (vertical - horizontal, vertical + horizontal);


	}
}
