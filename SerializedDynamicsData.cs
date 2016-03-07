using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


public class SerializedDynamicsData {
	public float xCoordinate {get; set;}
	public float yCoordinate {get; set;}
	public float zCoordinate {get; set;}
	public double heading {get; set;}
	public double bank {get; set;}
	public double pitch {get; set;}
	public double speed {get; set;}
	public double altitude {get; set;}
	public string aircraftType {get; set;}
	public int score {get; set;}
	
	public float cameraRigXRotation {get; set;}
	public float cameraRigYRotation {get; set;}
	public float cameraRigZRotation {get; set;}
	public float closestPointXCoordinate {get; set;}
	public float closestPointYCoordinate {get; set;}
	public float closestPointZCoordinate {get; set;}
	public double bearingHorizontal {get; set;}
	public double bearingVertical {get; set;}
	public double aspectHorizontal {get; set;}
	public double aspectVertical {get; set;}
	
	public float targetAircraftXCoordinate {get; set;}
	public float targetAircraftYCoordinate {get; set;}
	public float targetAircraftZCoordinate {get; set;}
	public double targetAircraftHeading {get; set;}
	public double targetAircraftBank {get; set;}
	public double targetAircraftPitch {get; set;}
	public double targetAircraftSpeed {get; set;}
	public double targetAircraftAltitude {get; set;}
	public string targetAircraftType {get; set;}

	public bool airspaceViolated {get; set;}
}