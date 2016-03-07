using UnityEngine;
using System.Collections;

public class AircraftData : MonoBehaviour {

	//public Movement movement;

	public Vector3 coordinates = Vector3.zero;

	public double heading = 0;

	//Note: pitch and bank are only functional within the range of (90, -90) degrees. Aircraft should not be inverted, so this should not be an issue.
	public double bank = 0; //roll
	public double pitch = 0;

	public double speed = 0; //Speed in knots.
	public double altitude = 0; //Altitude in feet
	public string aircraftType = "";
	public string units = null;


	// Use this for initialization
	public void Start () {
		units = Utilities.globalUnits.ToString ();
	}

	double calculatePitchAndBank(double value){
		if (value > 90)
			return 360 - value;
		else
			return -value;
	}
	
	// Update is called once per frame
	public void Update () {
		coordinates = transform.position;
		heading = transform.rotation.eulerAngles.y;
		speed = getSpeed();
		pitch = calculatePitchAndBank(transform.rotation.eulerAngles.x);
		bank = calculatePitchAndBank(transform.rotation.eulerAngles.z);
		altitude = transform.position.y;

		if (Utilities.useAviationUnits()) {
			altitude = Utilities.metersToFeet (altitude);
			speed = Utilities.metersPerSecondToKnots (speed);
		}
	}

	public virtual double getSpeed(){
		return  ((StraightPath)GetComponent("StraightPath")).speed;      
	}
}
