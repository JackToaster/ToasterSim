using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneScript : MonoBehaviour {
    public string nextScene;
    
    void Start() {
        UnityEngine.XR.InputTracking.Recenter();
    }
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Reset")){
            print("Resetting current scene");
            Scene active = SceneManager.GetActiveScene();
            SceneManager.LoadScene(active.name);
        }else if(Input.GetButtonDown("Switch")){
            print("Switching scenes");
            SceneManager.LoadScene(nextScene);
        }
	}
}
