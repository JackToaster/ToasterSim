using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using LCM.LCM;

public class LCM_Recieve : MonoBehaviour{
	private bool ready = false;
	//lcm instance
	//LCM.LCM lcm;
	//position message class
	//exlcm.position_t position_msg;

	//orientation lastOrientation;

	// Use this for initialization
	void Start () {
		/*try{
			//create LCM object
			lcm = new LCM.;
			//lcm.SubscribeAll(new UnitySubscriber());
		}catch(Exception e){
			print (e.ToString);
		}*/
	}

	//returns whether or not data is ready to be read.
	bool isReady (){
		/*if (ready) {
			ready = false;
			return true;
		} else {
			return false;
		}*/
		return false;
	}

	//returns an orientation object containing the data recieved from LCM
	orientation getOrientation (){
		return new orientation ();
	}


	/*internal class UnitySubscriber : LCM.LCM.LCMSubscriber {
		public void MessageReceived(LCM.LCM lcm, string channel, LCM.LCM.LCMDataInputStream dins){
			if (channel == "EXAMPLE") {
				print ("Recieved LCM message on EXAMPLE channel");

				position_msg = new exlcm.position_t (dins);

				lastOrientation = new orientation(position_msg.angle, position_msg.xPosition, position_msg.yPosition);

				ready = true;
			}
		}
	}*/

}
