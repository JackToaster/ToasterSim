using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class set_position : MonoBehaviour {
	public UDP_Recieve reciever;

	// Update is called once per frame
	void Update () {
		if (reciever.isReady ()) {
			orientation pos = reciever.getOrientation ();
			transform.position = new Vector3 (pos.xPos, transform.position.y, pos.yPos);
			transform.rotation = Quaternion.Euler (0,pos.angle, 0);
		}
	}
}