using UnityEngine;
using System.Collections;

public static class Utilities : object {

	public enum Units {SI, Aviation};

	// Configure Dynamics data to use aviation units or SI units:
	public static Units globalUnits = Units.SI;//Units.Aviation;

	public static bool useAviationUnits(){
		return globalUnits == Units.Aviation;
	}

	public static double knotsToMetersPerSecond(double knots){
		return knots * 0.51444444444;
	}

	public static double metersPerSecondToKnots(double mps){
		return mps * 1.9438444924574;
	}

	public static double milesToMeters(double val){
		return val * 1609.344;
	}

	public static double metersToMiles(double val){
		return val * 0.00062137119223733;
	}

	public static double metersToFeet(double val){
		return val * 3.2808398950131;
	}

	public static double feetToMeters(double val){
		return val * 0.3048;
	}

	public static float signedAngleBetween(Vector3 a, Vector3 b, Vector3 n){
		// angle in [0,180]
		float angle = Vector3.Angle(a,b);
		float sign = Mathf.Sign(Vector3.Dot(n,Vector3.Cross(a,b)));
		
		// angle in [-179,180]
		float signed_angle = angle * sign;
		
		// angle in [0,360] (not used but included here for completeness)
		//float angle360 =  (signed_angle + 180) % 360;
		
		return signed_angle;
	}

	public static bool closestPointExistsOnLine(Vector3 linePnt, Vector3 lineDir, Vector3 pnt)
	{
		lineDir.Normalize();//this needs to be a unit vector
		var v = pnt - linePnt;
		var dot = Vector3.Dot(v, lineDir);
		if (dot > 0) {
			return true;
		} else {
			return false;
		}
	}

	public static Vector3 getClosestPointOnLine(Vector3 linePnt, Vector3 lineDir, Vector3 pnt)
	{
		lineDir.Normalize();//this needs to be a unit vector
		var v = pnt - linePnt;
		var dot = Vector3.Dot(v, lineDir);
		if (dot > 0) {
			return linePnt + lineDir * dot;
		} else {
			return linePnt;
		}
		
	}

	public static Vector3 getClosestPointToLine(Vector3 linePnt, Vector3 lineDir, Vector3 pnt)
	{
		lineDir.Normalize();//this needs to be a unit vector
		var v = pnt - linePnt;
		var dot = Vector3.Dot(v, lineDir);
		return linePnt + lineDir * dot;
		
	}

	public static double DegreeToRadian(double angle)
	{
		return System.Math.PI * angle / 180.0;
	}

	public static double RadianToDegree(double angle)
	{
		return angle * (180.0 / System.Math.PI);
	}

}
