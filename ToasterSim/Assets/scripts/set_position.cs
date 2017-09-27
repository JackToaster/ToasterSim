using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class set_position : MonoBehaviour {
	public Rigidbody rb;
	public Reciever_Interface reciever;
	rTransform newer;
	float newTime = 0;
	rTransform older;
	float oldTime;
	void Start(){
		newer = new rTransform ();
		older = new rTransform ();
		pushPosition ();
		pushPosition ();
		oldTime = 0;
	}
	// Update is called once per frame
	void FixedUpdate () {
		//print (reciever.isReady ());
		if (reciever.isReady ()) {
			print ("Ready");
			orientation pos = reciever.getOrientation ();
			rb.position = new Vector3 (pos.xPos, rb.position.y, pos.yPos);
			rb.rotation = Quaternion.Euler (0,pos.angle, 0);
			pushPosition ();
			rb.velocity = calcVelocity ();
		}
	}

	void pushPosition(){
		print ("Pushed position: " + rb.position);
		older.position = newer.position;
		older.rotation = newer.rotation;
		oldTime = newTime;
		newer.position = rb.position;
		newer.rotation = rb.rotation.eulerAngles;
		newTime = Time.time;
	}
	Vector3 calcVelocity(){
		Vector3 displacement = newer.position - older.position;
		print ("Old: " + older.position + ", New: " + newer.position);
		float deltaT = newTime - oldTime;
		print ("DT: " + deltaT);
		print ("calculated velocity: " + displacement / deltaT);
		return displacement / deltaT;
	}
}