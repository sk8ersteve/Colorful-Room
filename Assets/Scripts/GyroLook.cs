using UnityEngine;
using System.Collections;

public class GyroLook : MonoBehaviour {

	public Camera m_camera;
	private bool looking = false;
	private Quaternion Old;
	private Quaternion New;
	private float rotateTime = 0.1f;
	private float startTime = 0f;
	public bool startedOnce = false;

	// Use this for initialization
	void Start () {
		Input.gyro.enabled = true;
		menuView ();
	}

	public void startLooking () {
		if (!startedOnce) {
			Quaternion angle = new Quaternion (Input.gyro.attitude.x, Input.gyro.attitude.y, -Input.gyro.attitude.z, -Input.gyro.attitude.w);
			Quaternion angle1 = Quaternion.Inverse (angle);
			Vector3 angle2 = angle1.eulerAngles;
			Vector3 angle3 = new Vector3 (angle2.x, angle2.y + 180, angle2.z);
			Old = transform.rotation;
			New = Quaternion.Euler (angle3);
			transform.rotation = Quaternion.Euler (angle3);
			looking = true;
			startedOnce = true;
		}
	}

	public void menuView () {
		looking = false;
		Quaternion angle = new Quaternion (Input.gyro.attitude.x, Input.gyro.attitude.y, -Input.gyro.attitude.z, -Input.gyro.attitude.w);
		Quaternion angle1 = Quaternion.Inverse (angle);
		Vector3 angle2 = angle1.eulerAngles;
		Vector3 angle3 = new Vector3 (angle2.x, angle2.y + 180, angle2.z);
		Old = transform.rotation;
		New = Quaternion.Euler (angle3);
		startTime = Time.time;

	}

	// Update is called once per frame
	void Update () {
		if (startedOnce){
			m_camera.transform.localRotation = new Quaternion (Input.gyro.attitude.x, Input.gyro.attitude.y, -Input.gyro.attitude.z, -Input.gyro.attitude.w);
			float fracComplete = (Time.time - startTime) / rotateTime;
			transform.rotation = Quaternion.Slerp (Old, New, fracComplete);
		}
	}
}
