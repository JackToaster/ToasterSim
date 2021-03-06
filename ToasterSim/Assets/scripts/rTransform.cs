using System;
using UnityEngine;
//rigid transform- contains position & rotation data
[System.Serializable]
public class rTransform
{
	public Vector3 position;
	public Vector3 rotation;
	public rTransform(){
		position = Vector3.zero;
		rotation = Vector3.zero;
	}

	//interpolate between this transform and another one
	public void lerp(double pAlpha, double rAlpha, rTransform t2){
		position = Vector3.Lerp (position, t2.position, (float) pAlpha);
		rotation = Quaternion.Lerp(Quaternion.Euler (rotation), Quaternion.Euler (t2.rotation), (float) rAlpha).eulerAngles;
	}
	/*//interpolate between two transforms
	public static rTransform lerp(double alpha, rTransform t1, rTransform t2){
		Vector3 pos = Vector3.Lerp (t1.position, t2.position, alpha);
		Vector3 rot = Quaternion.Lerp(Quaternion.Euler (t1.rotation), Quaternion.Euler (t2.rotation), (float) alpha).eulerAngles;
	}*/
}

