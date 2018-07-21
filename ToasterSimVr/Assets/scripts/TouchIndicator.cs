using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchIndicator : MonoBehaviour {
    public GameObject ignore;
    public Renderer objectRenderer;
    public Material touchedMat;

	void OnCollisionEnter (Collision col)
    {
        if(col.gameObject != ignore)
        {
            objectRenderer.material = touchedMat;            
        }
    }
}
