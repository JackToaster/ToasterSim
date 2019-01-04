using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WheelInterface : MonoBehaviour
{
    abstract public void setThrottles(float left, float right);
}
