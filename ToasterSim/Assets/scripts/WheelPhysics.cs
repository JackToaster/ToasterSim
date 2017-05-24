using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelPhysics : MonoBehaviour {
	//rigidbody to apply force to
	public Rigidbody robot;

	//offset in meters of the COM of the robot
	public Vector3 comOffset;

	//number of meters below the center of the robot to apply force
	public double driveBaseCenterOffset;

	//max linear speed, in m/s
	public double maxSpeed;

	//max turning speed, in rad/s
	public double maxTurn;

	//multiplier for motor torques
	public double linTorqueMultiplier;
	public double turnTorqueMultiplier;

	//takes motor input and converts throttle to speed
	public AnimationCurve ThrottleSpeed;

	//used for evaluating speed -> torque, should go from 1 to zero.
	public AnimationCurve TurnSpeedDropoff;
	public AnimationCurve LinSpeedDropoff;

	
	//used to determine amount of side slip based on lateral acceleration
	public AnimationCurve SideSlip;

	//the throttle of left & right motors - value from 0 to 1
	double leftThrottle = 0;
	double rightThrottle = 0;

	//start is called once at the beginning of excecution
	void Start(){
		//set the center of mass
		robot.centerOfMass = comOffset;
	}

	// FixedUpdate is called once per physics frame
	void FixedUpdate () {
		//check if the robot's wheels are on the ground
		//TODO: Check if the wheels are on the ground
		if (true) {

			/*
		 *Calculations for the robot's wheel friction below. 
		 * First works out sideways force on the wheels,
		 * then robot accel. based on that and applies it.
		 */

			//get the robot's linear and angular velocity
			Vector3 robotVel = robot.velocity;
			Vector3 robotAngular = robot.angularVelocity;

			//calculate the sideways force on the robot (in m/s^2)
			double sideForce = Mathf.Sin (Mathf.Deg2Rad * robotAngular.y) * Vector3.Project (robotVel, robot.transform.rotation * Vector3.forward).magnitude;

			//calculate the lateral velocity the robot should have (in m/s)
			double lateralVel;
			if (sideForce >= 0) {
				lateralVel = SideSlip.Evaluate ((float)sideForce) * Time.fixedDeltaTime;
			} else {
				lateralVel = -SideSlip.Evaluate ((float)-sideForce) * Time.fixedDeltaTime;
			}

			//calculate the new robot velocity after the wheel friction
			Vector3 newRobotVel = robotVel - Vector3.Project (robotVel, robot.transform.rotation * Vector3.right) + (float)lateralVel * (robot.transform.rotation * Vector3.right);

			//set the robot's velocity.
			robot.velocity = newRobotVel;

			/*
		 *Calculations for forward/backward acceleration:
		 * Uses inputs and current speed to calculate how
		 * much force to apply to the robot.
		 */

			//calculate the robot's current linear velocity as a fraction of the max speed
			double linVel = Vector3.Dot (robotVel, robot.transform.rotation * Vector3.forward);
			linVel /= maxSpeed;

			//calculate the target linear velocity based on the input

			double linTarget = (rightThrottle + leftThrottle) / 2.0;
			
			//calculate the difference in current vs target
			double linVelDifference = linTarget - linVel;

			//calculate the linear acceleration force
			double linForce;
			if (linVelDifference >= 0) {
				linForce = LinSpeedDropoff.Evaluate ((float)linVelDifference) * linTorqueMultiplier;
			} else {
				linForce = LinSpeedDropoff.Evaluate ((float)-linVelDifference) * linTorqueMultiplier * -1.0;
			}
			
			//apply the linear force
			robot.AddForceAtPosition ((robot.transform.rotation * Vector3.forward) * (float)linForce, robot.transform.position + new Vector3 (0, (float)driveBaseCenterOffset, 0));

			/*
		 *Calculations for angular acceleration:
		 * Uses inputs and current angular speed to calculate how
		 * much torque to apply to the robot.
		 */

			//get the robot's current angular velocity as a fraction of the max turning speed
			double robotTurnVel = robotAngular.y;
			robotTurnVel /= maxTurn;

			//calculate the amount of turning input
			double turnTarget = rightThrottle - leftThrottle;

			//calculate the difference in current vs target
			double turnVelDifference = turnTarget - robotTurnVel;

			//calculate the amount of torque to apply to the robot
			double turnForce;

			if (turnVelDifference >= 0) {
				turnForce = TurnSpeedDropoff.Evaluate ((float)turnVelDifference) * turnTorqueMultiplier;
			} else {
				turnForce = TurnSpeedDropoff.Evaluate ((float)-turnVelDifference) * turnTorqueMultiplier * -1.0;
			}

			//apply the torque to the robot
			robot.AddRelativeTorque (new Vector3 (0, (float)turnForce, 0));
		}
	}


	public void setThrottles(double left, double right){
		leftThrottle = left;
		rightThrottle = right;

		left = Mathf.Clamp ((float)left, -1, 1);
		right = Mathf.Clamp ((float)right, -1, 1);

		left = inputToSpeed (left);
		right = inputToSpeed (right);
	}

	double inputToSpeed(double input){
		if (input > 1) {
			return(ThrottleSpeed.Evaluate ((float)input));
		} else {
			return(-ThrottleSpeed.Evaluate ((float)-input));
		}
	}
}
