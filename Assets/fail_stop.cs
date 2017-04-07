using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fail_stop : MonoBehaviour {

	public GameObject parachute;
	Rigidbody rb;

	// Use this for initialization
	void Start () {
		parachute.transform.localScale = new Vector3 (0, 0, 0);
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (rb.velocity.magnitude > 30) {
			parachute.transform.localScale = new Vector3 (5, 5, 5);
			rb.drag = 2;
		}
	}
}
