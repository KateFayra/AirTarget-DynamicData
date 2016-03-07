using UnityEngine;
using System.Collections;
using ReadWriteCsv;
using System;
using System.Collections.Generic;

public class PilotData : AircraftData {

	/*
	 * Configuration
	 */
	private float lineWidth = 10; // The thickness of the lines.

	private bool showLine1 = true; // Main green line
	private bool showLine2 = true; // Red line
	private bool showLine3 = true; // Second green line
	private bool showLine4 = true; // Pink line

	private float hockeyPuckOpacity = .0f;
	private float lineOpacity = 1f;

	/* FAA regulations on aircraft seperation under IFR (Instrument Flight Rules):
	 * http://aviation.stackexchange.com/questions/2806/how-much-is-the-minimum-safe-distance-between-two-planes-in-flight
	 * https://en.wikipedia.org/wiki/Separation_%28aeronautics%29
	 */

	private double separationRadius = Utilities.milesToMeters(5);
	private double separationHeight = Utilities.feetToMeters(1000);

	/*
	 * End Configuration
	 */

	/*
	 * Public variables
	 */

	public int score = 0;
	public bool airspaceViolated = false;

	public AircraftData oncomingAircraftData = null;
	public string oncomingAircraftType = null;
	public GameObject oncomingAircraft = null;

	public Vector3 ovrCameraRigRotation = Vector3.zero;
	public Vector3 closestPoint = Vector3.zero;

	public double bearingHorizontal = 0;
	public double bearingVertical = 0;
	public double aspectHorizontal = 0;
	public double aspectVertical = 0;

	/*
	 * Private internal script variables
	 */
	private GameObject centerEyeAnchor = null;
	private GameObject hockeyPuck = null;

	private LineRenderer line1 = null;
	private LineRenderer line2 = null;
	private LineRenderer line3 = null;
	private LineRenderer line4 = null;

	private Movement movement;

	private Vector3 oncomingAircraftVelocity;

	private StraightPath oncomingAircraftPath;

	DynamicsDataCollection savedDynamicsData = new DynamicsDataCollection();

	private string fileName;

	// Use this for initialization
	new void Start () {
		base.Start ();
		movement = (Movement)GetComponent ("Movement");
		movement.init ();
		oncomingAircraft = movement.spawn1.gameObject;
		oncomingAircraftPath = ((StraightPath)oncomingAircraft.GetComponent ("StraightPath"));
		oncomingAircraftPath.Start ();

		//oncomingAircraft = GameObject.Find(oncomingAircraftObjectName);
		oncomingAircraftVelocity = oncomingAircraftPath.plane1.velocity;
		oncomingAircraftData = (AircraftData)oncomingAircraft.GetComponent ("AircraftData");
		oncomingAircraftType = oncomingAircraftData.aircraftType;

		line1 = GameObject.Find ("Line1Object").AddComponent<LineRenderer> ();
		line2 = GameObject.Find ("Line2Object").AddComponent<LineRenderer> ();
		line3 = GameObject.Find ("Line3Object").AddComponent<LineRenderer> ();
		line4 = GameObject.Find ("Line4Object").AddComponent<LineRenderer> ();
		line1.SetVertexCount (2);
		line2.SetVertexCount (2);
		line3.SetVertexCount (2);
		line4.SetVertexCount (2);
		line1.SetWidth (lineWidth, lineWidth);
		line2.SetWidth (lineWidth, lineWidth);
		line3.SetWidth (lineWidth, lineWidth);
		line4.SetWidth (lineWidth, lineWidth);
		line1.material = new Material (Shader.Find ("Particles/Additive"));
		line2.material = new Material (Shader.Find ("Particles/Additive"));
		line3.material = new Material (Shader.Find ("Particles/Additive"));
		//line1.material = new Material (Shader.Find("Particles/Additive"));
		Color g = Color.green;
		g.a = lineOpacity;
		Color r = Color.red;
		r.a = lineOpacity;
		Color p = Color.magenta;
		p.a = lineOpacity;
		line1.SetColors (g, g);
		line2.SetColors (r, r);
		line3.SetColors (g, g);
		line4.SetColors (p, p);
		// line4 color does not need to be set, default is pink.

		hockeyPuck = GameObject.Find ("HockeyPuck");
		print (hockeyPuck);

		//hockeyPuck.transform.gameObject.tag = "HockeyPuck";
		hockeyPuck.transform.localScale = new Vector3 ((float)separationRadius, (float)separationHeight, (float)separationRadius);

		centerEyeAnchor = GameObject.Find ("CenterEyeAnchor");

		Color col = hockeyPuck.GetComponent<Renderer> ().material.color;
		col.a = hockeyPuckOpacity;
		hockeyPuck.GetComponent<Renderer> ().material.color = col;

		
		Physics.IgnoreCollision (hockeyPuck.GetComponent<MeshCollider> (), oncomingAircraft.GetComponent<BoxCollider> ());

		fileName = "Recorded Data " + DateTime.Now.ToString (@"MM\.dd\.yyyy h\.mm tt") + ".csv";

	}

	void addDataRow(){
		if (oncomingAircraftData != null) {
			CsvRow row = new CsvRow ();
			SerializedDynamicsData data = new SerializedDynamicsData ();
			data.xCoordinate = coordinates.x;
			data.yCoordinate = coordinates.y;
			data.zCoordinate = coordinates.z;
			data.heading = heading;
			data.bank = bank;
			data.pitch = pitch;
			data.speed = speed;
			data.altitude = altitude;
			data.aircraftType = aircraftType;
			data.score = score;

			data.cameraRigXRotation = ovrCameraRigRotation.x;
			data.cameraRigYRotation = ovrCameraRigRotation.y;
			data.cameraRigZRotation = ovrCameraRigRotation.z;
			data.closestPointXCoordinate = closestPoint.x;
			data.closestPointYCoordinate = closestPoint.y;
			data.closestPointZCoordinate = closestPoint.z;
			data.bearingHorizontal = bearingHorizontal;
			data.bearingVertical = bearingVertical;
			data.aspectHorizontal = aspectHorizontal;
			data.aspectVertical = aspectVertical;

			data.targetAircraftXCoordinate = oncomingAircraftData.coordinates.x;
			data.targetAircraftYCoordinate = oncomingAircraftData.coordinates.y;
			data.targetAircraftZCoordinate = oncomingAircraftData.coordinates.z;
			data.targetAircraftHeading = oncomingAircraftData.heading;
			data.targetAircraftBank = oncomingAircraftData.bank;
			data.targetAircraftPitch = oncomingAircraftData.pitch;
			data.targetAircraftSpeed = oncomingAircraftData.speed;
			data.targetAircraftAltitude = oncomingAircraftData.altitude;
			data.targetAircraftType = oncomingAircraftData.aircraftType;

			data.airspaceViolated = airspaceViolated;

			row.Add (String.Format ("{0}", coordinates.x));
			row.Add (String.Format ("{0}", coordinates.y));
			row.Add (String.Format ("{0}", coordinates.z));
			row.Add (String.Format ("{0}", heading));
			row.Add (String.Format ("{0}", bank));
			row.Add (String.Format ("{0}", pitch));
			row.Add (String.Format ("{0}", speed));
			row.Add (String.Format ("{0}", altitude));
			row.Add (String.Format ("{0}", aircraftType));
			row.Add (String.Format ("{0}", score));

			row.Add (String.Format ("{0}", ovrCameraRigRotation.x));
			row.Add (String.Format ("{0}", ovrCameraRigRotation.y));
			row.Add (String.Format ("{0}", ovrCameraRigRotation.z));
			row.Add (String.Format ("{0}", closestPoint.x));
			row.Add (String.Format ("{0}", closestPoint.y));
			row.Add (String.Format ("{0}", closestPoint.z));
			row.Add (String.Format ("{0}", bearingHorizontal));
			row.Add (String.Format ("{0}", bearingVertical));
			row.Add (String.Format ("{0}", aspectHorizontal));
			row.Add (String.Format ("{0}", aspectVertical));

			row.Add (String.Format ("{0}", oncomingAircraftData.coordinates.x));
			row.Add (String.Format ("{0}", oncomingAircraftData.coordinates.y));
			row.Add (String.Format ("{0}", oncomingAircraftData.coordinates.z));
			row.Add (String.Format ("{0}", oncomingAircraftData.heading));
			row.Add (String.Format ("{0}", oncomingAircraftData.bank));
			row.Add (String.Format ("{0}", oncomingAircraftData.pitch));
			row.Add (String.Format ("{0}", oncomingAircraftData.speed));
			row.Add (String.Format ("{0}", oncomingAircraftData.altitude));
			row.Add (String.Format ("{0}", oncomingAircraftData.aircraftType));
			row.Add (String.Format ("{0}", airspaceViolated));
	
			savedDynamicsData.addDataRow (row, data);
		}
	}

	private void clearDynamicsData(){
		bearingVertical = 0;
		bearingHorizontal = 0;
		aspectVertical = 0;
		aspectHorizontal = 0;
		closestPoint = Vector3.zero;
		airspaceViolated = false;
	}

	
	// Update is called once per frame
	new void Update () {
		base.Update ();
		hockeyPuck.transform.position = transform.position;
		
		line1.enabled = false;
		line2.enabled = false;
		line3.enabled = false;
		line4.enabled = false;

		if (movement.spawn1 == null) {
			clearDynamicsData();
			return;
		}

		oncomingAircraft = movement.spawn1.gameObject;
		oncomingAircraftPath = ((StraightPath)oncomingAircraft.GetComponent ("StraightPath"));
		if (oncomingAircraftPath == null || oncomingAircraftPath.plane1 == null) {
			clearDynamicsData();
			return;
		}
		
		oncomingAircraftVelocity = oncomingAircraftPath.plane1.velocity;
		oncomingAircraftData = (AircraftData)oncomingAircraft.GetComponent ("AircraftData");
		oncomingAircraftType = oncomingAircraftData.aircraftType;
		Physics.IgnoreCollision (hockeyPuck.GetComponent<MeshCollider> (), oncomingAircraft.GetComponent<BoxCollider> ());
		Physics.IgnoreCollision (oncomingAircraft.GetComponent<BoxCollider> (), GetComponent<BoxCollider> ());


		if (centerEyeAnchor != null)
			ovrCameraRigRotation = centerEyeAnchor.transform.localRotation.eulerAngles;

		airspaceViolated = false;

		Vector3 oncomingAircraftPosition = oncomingAircraft.transform.position;
		Vector3 targetDirection = oncomingAircraftVelocity;

		Vector3 endPosition = oncomingAircraftData.coordinates + (targetDirection * (float)Utilities.milesToMeters (20));

		line1.enabled = true;

		Vector3 line1End = endPosition;
		Vector3 line3End = endPosition;
		Vector3 line2Start = oncomingAircraftPosition;

		bool outsideHit = false;
		bool insideHit = false;

		/*
	 * bearing calculation
	 */
	
		Vector3 thisDirection = movement.plane1.velocity;
		Vector3 thisDirection2d = new Vector3 (thisDirection.x, 0, thisDirection.z);
		//Vector3 thisPosition2d = new Vector3 (transform.position.x, 0, transform.position.z);

		Vector3 targetDirection2d = new Vector3 (targetDirection.x, 0, targetDirection.z);
	
		Vector3 oncomingAircraftPosition2d = new Vector3 (oncomingAircraftPosition.x, 0, oncomingAircraftPosition.z);
		Vector3 transformPosition2d = new Vector3 (transform.position.x, 0, transform.position.z);
	
		bearingHorizontal = Utilities.signedAngleBetween (oncomingAircraftPosition2d - transformPosition2d, thisDirection2d, Vector3.down);

		float h = (oncomingAircraftPosition - transform.position).magnitude;
		float o = Mathf.Abs (oncomingAircraftPosition.y - transform.position.y);
		bearingVertical = Utilities.RadianToDegree (System.Math.Asin (o / h));
		if (transform.position.y > oncomingAircraftPosition.y)
			bearingVertical = -bearingVertical;

		bearingVertical -= pitch;

		/*
	 * aspect calculation
	 */
	
		aspectHorizontal = Utilities.signedAngleBetween (transformPosition2d - oncomingAircraftPosition2d, targetDirection2d, Vector3.down);

		h = (transform.position - oncomingAircraftPosition).magnitude;
		o = Mathf.Abs (transform.position.y - oncomingAircraftPosition.y);
		double targetBearingVertical = Utilities.RadianToDegree (System.Math.Asin (o / h));
		if (transform.position.y < oncomingAircraftPosition.y)
			targetBearingVertical = -targetBearingVertical;
	
		targetBearingVertical -= oncomingAircraftData.pitch;

		aspectVertical = -targetBearingVertical;

		/*
	 * HockeyPuck hit detection for drawing lines.
	 */

		RaycastHit hit;
		if (Physics.Raycast (oncomingAircraftPosition, endPosition - oncomingAircraftPosition, out hit)) {
			if (hit.transform.tag == "HockeyPuck") {
				outsideHit = true;
				line1End = hit.point;
				line2Start = hit.point;
			}
		}

	
		RaycastHit hit2;
		if (Physics.Raycast (endPosition, oncomingAircraftPosition - endPosition, out hit2)) {
			if (hit2.transform.tag == "HockeyPuck") {
				insideHit = true;
				line2.SetPosition (1, hit2.point);
				line2.enabled = true;
			
				line3.SetPosition (0, hit2.point);
				line3.enabled = true;
			}
		}
	
		line1.SetPosition (0, oncomingAircraftPosition);
		line1.SetPosition (1, line1End);
	
		line2.SetPosition (0, line2Start);
	
		line3.SetPosition (1, line3End);
	
		if (!outsideHit && insideHit) {
			airspaceViolated = true;
			line1.enabled = false;
		}

		if (insideHit || outsideHit) {
			closestPoint = Utilities.getClosestPointOnLine (line2Start, targetDirection, transform.position);
			line4.SetPosition (0, transform.position);
			line4.SetPosition (1, closestPoint);
			line4.enabled = true;
		}

		if (!showLine1)
			line1.enabled = false;
	
		if (!showLine2)
			line2.enabled = false;
	
		if (!showLine3)
			line3.enabled = false;
	
		if (!showLine4)
			line4.enabled = false;
	
		// Needs to be called at the end of update when user presses the button. Tab should be replaced with the button.
		if (Input.GetKeyDown (KeyCode.Tab)) {
			addDataRow();
			//writeToFile only needs to be called at the end of the experiment, not every time data is added.
			savedDynamicsData.writeToFile (fileName);
		}

	}

	
	public override double getSpeed(){
		return movement.speed;      
	}

}
