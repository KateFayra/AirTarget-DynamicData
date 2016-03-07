using UnityEngine;
using System.Collections;

/*
 * Simple placeholder script to represent data from step3 - Aircrafts paths
 * The path script will from step 3 will need to have a global variabl to share the speed.
 */

public class StraightPath : MonoBehaviour {

	//private Vector3 path = Vector3.forward; 

	public float speed = 0;//Speed in meters per second;//(float)Utilities.knotsToMetersPerSecond(170f); 


	public Rigidbody plane1;

	public void Start()
	{
		plane1 = GetComponent<Rigidbody> ();
		Vector3 vel = new Vector3 (speed / transform.localScale.x, 0, 0);
		plane1.velocity = transform.TransformVector (vel);
	}

}
