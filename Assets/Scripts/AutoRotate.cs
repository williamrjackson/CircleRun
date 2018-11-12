using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour {
    public float degreesPerSecond = 45;
    public float accruingAngle = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float rotateAddition = degreesPerSecond * Time.deltaTime;
        transform.localEulerAngles += Vector3.forward * rotateAddition;
        accruingAngle += rotateAddition;
    }
}
