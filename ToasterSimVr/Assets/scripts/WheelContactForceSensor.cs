using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelContactForceSensor : MonoBehaviour
{
	Vector3 contactForce;
	Vector3 contactPosition;
	Vector3 contactNormal;
	bool contacting = false;
	
	void OnCollisionEnter(Collision collision){
		onCollision(collision);
	}

	void OnCollisionStay(Collision collision){
		onCollision(collision);
	}

	void onCollision(Collision collision){
		contacting = true;
		contactForce = collision.impulse / Time.fixedDeltaTime;
		contactPosition = collision.GetContact(0).point;
		contactNormal = collision.GetContact(0).normal;
	}

	void onContactExit(Collision collision){
		contacting = false;
	}

	public Vector3 getContactForce(){
		if(contacting) return contactForce;
		else return Vector3.zero;
	}

	public Vector3 getContactPosition(){
		if(contacting) return contactPosition;
		else return Vector3.zero;
	}
	
	public Vector3 getContactNormal(){
		if(contacting) return contactNormal;
		else return Vector3.up;
	}
}
