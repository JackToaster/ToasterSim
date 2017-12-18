using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//
public class CameraScript : MonoBehaviour
{
	public enum CameraMode
	{
		Track = 0,
		//follow the robot, but don't rotate
		Chase = 1,
		//follow the robot and rotate
		BirdsEye = 2,
		//follow directly above the robot with ortho projection
		DriverStation = 3,
		//fixed camera in driverstation
		FullField = 4,
		//birdseye of the whole field
        VR = 5
        //Camera in driverstation for VR
	}

	//camera gameObject
	public Camera cam;

	public GameObject driverStation;

	//mode that the camera is in
	public CameraMode camMode;
	//Offset target (x, y) from robot or driver station
	//and angle the camera faces, in degrees.
	public rTransform cameraOffset;
	//the factor of smoothing for camera position
	//doesn't effect DriverStation mode.
	public double PositionSmoothing;
	//the factor of smoothing for camera orientation
	//only effects the chase mode
	public double AngleSmoothing;
	//the robot body or driver station to follow
	public GameObject cameraTarget;

	//speed at which the camera zooms
	public double zoomSpeed = 0;

	//current zoom
	double zoom = 1;

	//position and rotation of robot
	rTransform robotTransform;

	//position and rotation of camera
	rTransform cameraTransform;

	//get the robot position and rotation
	void getRobotPosition ()
	{
		robotTransform.position = cameraTarget.transform.position;
		robotTransform.rotation = cameraTarget.transform.rotation.eulerAngles;
	}

	//get the target position of the camera
	rTransform getCameraTarget ()
	{
		//variable for the target rTransform of the camera
		rTransform target = new rTransform();

		//calculate the position/rotation of the camera
		switch (camMode) {
		case CameraMode.Track:
			target.position = robotTransform.position + Quaternion.Euler(0f,270f,0f) * (cameraOffset.position * (float) zoom);
			target.rotation = cameraOffset.rotation + new Vector3(0f, 270f, 0f);
			break;
		case CameraMode.Chase:
			target.position = Quaternion.Euler(0f, robotTransform.rotation.y, 0f) * (cameraOffset.position * (float) zoom) + robotTransform.position;
			target.rotation = new Vector3(0f, robotTransform.rotation.y, 0f) + cameraOffset.rotation;
			break;
		case CameraMode.BirdsEye:
			target.rotation = new Vector3 (90f, 270f, 0f);
			target.position = robotTransform.position + new Vector3(0f, 100f, 0f);
			cam.orthographicSize =  cameraOffset.position.y * (float) zoom;
			break;
		case CameraMode.DriverStation:
			target.position = driverStation.transform.position + Quaternion.Euler(0f,270f,0f) * (cameraOffset.position * (float) zoom);
			target.rotation = cameraOffset.rotation + new Vector3(0f, 270f, 0f);
			break;
		case CameraMode.FullField:
			target.rotation = new Vector3 (90f, 270f, 0f);
			target.position = new Vector3 (0f, 100f, 0f);
			cam.orthographicSize = cameraOffset.position.y * (float) zoom;
			break;
        case CameraMode.VR:
            target.rotation = new Vector3 (0f, -90f, 0f);
            target.position = new Vector3(9.16f, 1.324f, 1.927f);
            cam.orthographicSize = cameraOffset.position.y * (float)zoom;
        break;
		default:
			//something messed up!
			print ("Failed to read camera mode. Setting default values.");
			//set pos & rotation to 0
			target.rotation = Vector3.zero;
			target.position = Vector3.zero;
			break;
		}
		return target;
	}

	//move the camera
	void setCameraTransform ()
	{
		transform.rotation = Quaternion.Euler(cameraTransform.rotation);
		transform.position = cameraTransform.position;
	}

	//initializes robot rotation & orientation variables,
	//as well as setting camera position.
	void Start ()
	{
		robotTransform = new rTransform ();
		cameraTransform = new rTransform ();
		getRobotPosition ();
		cameraTransform = getCameraTarget ();
		setCameraTransform ();

		//if the camera is in birdseye mode, go to orthographic projection
		if (camMode == CameraMode.BirdsEye) {
			cam.orthographic = true;
			cam.orthographicSize = cameraOffset.position.y;
		} else {
			cam.orthographic = false;
		}
	}

	//called when things are changed in the editor
	void OnValidate(){
		if (camMode == CameraMode.BirdsEye) {
			cam.orthographic = true;
			cam.orthographicSize = cameraOffset.position.y * (float) zoom;
		} else if (camMode == CameraMode.FullField) {
			cam.orthographic = true;
			cam.orthographicSize = cameraOffset.position.y * (float) zoom;
		}else{
			cam.orthographic = false;
		}
	}

	// Update is called once per frame
	void Update ()
	{
		//update zoom
		zoom +=  zoomSpeed * Input.GetAxis("Scroll");

		//get the robot's position
		getRobotPosition ();
		//smoothly interpolate between the position and the target
		cameraTransform.lerp(1.0 - PositionSmoothing, 1.0 - AngleSmoothing,  getCameraTarget ());
		//set the camera's transform to its position
		setCameraTransform ();


	}

	//go to next camera mode
	public void incrementCamMode(){
		camMode = (CameraMode)(((int)camMode + 1) % 6);
		OnValidate ();
	}
}
	
