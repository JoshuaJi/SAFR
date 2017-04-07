using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flight_script : MonoBehaviour
{

	public float dest_x;
	public float dest_z;

	public float vertical_speed;
	public float horizontal_speed;

	public float max_height = 5;
	private Vector3 tempPosition;
	private Vector3 turn_dir = Vector3.right;
	private Collider the_other_drone;

	// Update is called once per frame
	void FixedUpdate ()
	{

		RaycastHit hitFront;
		Ray frontRay = new Ray (transform.position, transform.forward);
		if (Physics.Raycast (frontRay, out hitFront, 30.0f)) {
			if (hitFront.transform.gameObject.tag == "building") {
				Debug.DrawRay (transform.position, transform.forward * 30, Color.white);
			}
		} else {
			hitFront.distance = 0;
		}

		RaycastHit hitLeft;
		Ray leftRay = new Ray (transform.position, -transform.right);
		if (Physics.Raycast (leftRay, out hitLeft, 15.0f)) {
			if (hitLeft.transform.gameObject.tag == "building") {
				Debug.DrawRay (transform.position, -transform.right * 15, Color.gray);
			}
		} else {
			hitLeft.distance = 0;
		}

		RaycastHit hitRight;
		Ray rightRay = new Ray (transform.position, transform.right);
		if (Physics.Raycast (rightRay, out hitRight, 15.0f)) {
			if (hitRight.transform.gameObject.tag == "building") {
				Debug.DrawRay (transform.position, transform.right * 15, Color.gray);
			}
		} else {
			hitRight.distance = 0;
		}
			
		tempPosition = transform.position;
		float current_x = tempPosition.x;
		float current_y = tempPosition.y;
		float current_z = tempPosition.z;
		float dest_angle = Mathf.Atan ((dest_x - current_x) / (dest_z - current_z));
		float dist = Mathf.Sqrt (Mathf.Pow ((dest_x - current_x), 2) + Mathf.Pow ((dest_z - current_z), 2));


		if (dist < 1) {
			tempPosition.y -= vertical_speed * Time.deltaTime;
		} else if (current_y < max_height) {
			tempPosition.y += vertical_speed * Time.deltaTime;
		}else if (current_y > max_height) {
			tempPosition.y -= vertical_speed * Time.deltaTime;
		}
		if (tempPosition.y < 2)
			tempPosition.y = 2;

		transform.position = tempPosition;
		Quaternion lookAt = Quaternion.LookRotation (new Vector3 (dest_x, 2f, dest_z) - new Vector3 (transform.position.x, 2f, transform.position.z));
		transform.rotation = Quaternion.Lerp (transform.rotation, lookAt, Time.smoothDeltaTime * horizontal_speed);

		if (((new Vector3 (dest_x, 2f, dest_z) - new Vector3 (transform.position.x, 2f, transform.position.z)).magnitude > 0.5f) && (hitFront.distance > 30 || hitFront.distance == 0)) {
			transform.Translate (Vector3.forward * horizontal_speed * Time.deltaTime);
		}

		if (hitFront.distance <= 30 && hitFront.distance > 0) {

			if (hitLeft.distance == 0 && hitRight.distance == 0) {
				transform.Translate (turn_dir * horizontal_speed * Time.deltaTime);
			}
		} 
		if (hitLeft.distance <= 15 && hitLeft.distance > 0) {
			turn_dir = Vector3.right;
			transform.Translate (turn_dir * horizontal_speed * Time.deltaTime);
		}

		if (hitRight.distance <= 15 && hitRight.distance > 0) {
			turn_dir = -Vector3.right;
			transform.Translate (turn_dir * horizontal_speed * Time.deltaTime);
		}

		Collider[] hitColliders = Physics.OverlapSphere(transform.position, 30);
		int i = 0;
		while (i < hitColliders.Length)
		{
			if (hitColliders [i].tag == "drone") {
				Debug.DrawLine (transform.position, hitColliders [i].transform.position, Color.green);
			}
			float distance = Vector3.Distance (transform.position, hitColliders [i].transform.position);
			if (distance < 30 && distance>0) {
				print (Vector3.Distance (transform.position, hitColliders [i].transform.position));
				max_height = Random.Range (40f, 100f);
			}
			i++;
		}

		
	}
}
