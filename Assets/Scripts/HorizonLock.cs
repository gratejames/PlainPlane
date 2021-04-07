using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizonLock : MonoBehaviour
{
	//private float heading;
	//private float pitch;
	private float roll;
	private float heading;
	private float pitch;

	private Vector3 pos;

	public GameObject LeftHorizonIndicator;
	public GameObject LeftHorizonIndicatorArm;

	public GameObject RightHorizonIndicator;
	public GameObject RightHorizonIndicatorArm;

	public GameObject HeadingRoller;
	public GameObject PitchRoller;

	void OnGUI() {
		pos = ProjectPointOnPlane(Vector3.up, Vector3.zero, transform.right);
		roll = SignedAngle(transform.right, pos, transform.forward);
		heading = Mathf.Atan2(transform.forward.z, transform.forward.x) * Mathf.Rad2Deg;
		pitch = SignedAngle(transform.forward, pos, transform.right);

		HeadingRoller.transform.localRotation = Quaternion.Euler(0, this.transform.rotation.eulerAngles.y, 0);
		PitchRoller.transform.localRotation = Quaternion.Euler(0, -this.transform.rotation.eulerAngles.x, 0);

		RightHorizonIndicator.transform.rotation = Quaternion.Euler(0, 0, roll);
		RightHorizonIndicatorArm.transform.rotation = Quaternion.Euler(0, 0, roll);
		LeftHorizonIndicator.transform.rotation = Quaternion.Euler(0, 0, roll);
		LeftHorizonIndicatorArm.transform.rotation = Quaternion.Euler(0, 0, roll);
	}
	Vector3 ProjectPointOnPlane(Vector3 planeNormal, Vector3 planePoint, Vector3 point) {
		planeNormal.Normalize();
		var distance = -Vector3.Dot(planeNormal.normalized, (point - planePoint));
		return point + planeNormal * distance;
	}
	float SignedAngle(Vector3 v1,Vector3 v2, Vector3 normal) {
		var perp = Vector3.Cross(normal, v1);
		var angle = Vector3.Angle(v1, v2);
		angle *= Mathf.Sign(Vector3.Dot(perp, v2));
		return angle;
	}
}
