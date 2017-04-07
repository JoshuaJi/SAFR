using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class communication_failed : MonoBehaviour {

	public float dest_x;
	public float dest_z;

	public float vertical_speed;
	public float horizontal_speed;
	public GameObject parachute;
	Rigidbody rb;
	private float timeLeft;
	private bool fail_stop;

	// Use this for initialization
	void Start () {
		parachute.transform.localScale = new Vector3 (0, 0, 0);
		rb = GetComponent<Rigidbody> ();
		rb.isKinematic = true;
		timeLeft = 10;
		fail_stop = false;
	}
	
	// Update is called once per frame
	void Update () {

		Quaternion lookAt = Quaternion.LookRotation (new Vector3 (dest_x, 2f, dest_z) - new Vector3 (transform.position.x, 2f, transform.position.z));
		transform.rotation = Quaternion.Lerp (transform.rotation, lookAt, Time.smoothDeltaTime * horizontal_speed);

		transform.Translate (Vector3.forward * horizontal_speed * Time.deltaTime);

		Collider[] hitColliders = Physics.OverlapSphere(transform.position, 30);
		int i = 0;
		while (i < hitColliders.Length)
		{
			
			float distance = Vector3.Distance (transform.position, hitColliders [i].transform.position);
			if (distance < 15 && distance>0) {
				if (!fail_stop) {
					Debug.DrawLine (transform.position, hitColliders [i].transform.position, Color.yellow);
					print ("Communication lost, waiting for reconnection, fail stop tiggered in "+timeLeft);
				}
				horizontal_speed = 0;
				timeLeft -= Time.deltaTime;
				if (timeLeft <= 0 && fail_stop == false) {
					timeLeft = 0;
					fail_stop = true;
					rb.isKinematic = false;
					print("Fail stop triggered");
				}


			}else if (hitColliders [i].tag == "drone") {
				Debug.DrawLine (transform.position, hitColliders [i].transform.position, Color.green);
			}
			i++;
		}

		if (rb.velocity.magnitude > 30) {
			parachute.transform.localScale = new Vector3 (5, 5, 5);
			rb.drag = 2;
		}

	}
}
