using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ReadWriteCsv;

public class DynamicsDataCollection{

	public List<SerializedDynamicsData> dataCollection = new List<SerializedDynamicsData>();
	public List<CsvRow> rows = new List<CsvRow>();
	public CsvRow header = new CsvRow();

	public DynamicsDataCollection(){
		header.Add("x-coordinate");
		header.Add("y-coordinate");
		header.Add("z-coordinate");
		header.Add("Heading");
		header.Add ("Bank");
		header.Add ("Pitch");
		header.Add ("Speed");
		header.Add ("Altitude");
		header.Add ("Aircraft Type");
		header.Add ("Score");
		header.Add ("CameraRig x-rotation");
		header.Add ("CameraRig y-rotation");
		header.Add ("CameraRig z-rotation");
		header.Add ("Closest Point x-coordinate");
		header.Add("Closest Point y-coordinate");
		header.Add("Closest Point z-coordinate");
		header.Add("Bearing Horizontal");
		header.Add("Bearing Vertical");
		header.Add("Aspect Horizontal");
		header.Add("Aspect Vertical");
		header.Add("Target Aircraft x-coordinate");
		header.Add("Target Aircraft y-coordinate");
		header.Add("Target Aircraft z-coordinate");
		header.Add("Target Aircraft Heading");
		header.Add("Target Aircraft Bank");
		header.Add("Target Aircraft Pitch");
		header.Add("Target Aircraft Speed");
		header.Add("Target Aircraft Altitude");
		header.Add ("Target Aircraft Type");
		header.Add ("Airspace Violated");

		rows.Add(header);



	}

	public int getSize(){
		return dataCollection.Count;
	}

	public void addDataRow(CsvRow row, SerializedDynamicsData data){
		rows.Add (row);
		dataCollection.Add (data);
	}

	public SerializedDynamicsData getData(int aircraftIndex){
		if (aircraftIndex < 0 || aircraftIndex >= dataCollection.Count)
			return null;
		return dataCollection[aircraftIndex];
	}

	public void writeToFile(string fileName){
		using (CsvFileWriter writer = new CsvFileWriter(fileName))
		{

			
			foreach (CsvRow row in rows)
			{
				writer.WriteRow(row);
			}
		}
	}

}
