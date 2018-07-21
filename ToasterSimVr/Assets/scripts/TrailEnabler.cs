using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailEnabler : MonoBehaviour {

    public string button;
    public TrailRenderer tr;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetButtonDown(button)){
            tr.enabled = !tr.enabled;
        }
	}
}
