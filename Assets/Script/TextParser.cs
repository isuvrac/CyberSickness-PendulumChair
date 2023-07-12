using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TextParser : MonoBehaviour {
	public TextAsset text;
	public float[] Time1, Time2, jerkPX0, accelerationPX0, speedPX0, jerkPY0, accelerationPY0, speedPY0, jerkPZ0, accelerationPZ0, speedPZ0;
	// Use this for initialization
	void Awake () {
		ParseCSV();
	}
	
	void ParseCSV()
	{
		string[] allText = text.text.Split(',', '\n');
		string[] data = new string[allText.Length - 11];
		Array.Copy(allText, 11, data, 0, allText.Length - 11);

		Time1 = new float[data.Length / 11];
		Time2 = new float[data.Length / 11];
		jerkPX0 = new float[data.Length / 11];
		accelerationPX0 = new float[data.Length / 11];
		speedPX0 = new float[data.Length / 11];
		jerkPY0 = new float[data.Length / 11];
		accelerationPY0 = new float[data.Length / 11];
		speedPY0 = new float[data.Length / 11];
		jerkPZ0 = new float[data.Length / 11];
		accelerationPZ0 = new float[data.Length / 11];
		speedPZ0 = new float[data.Length / 11];

		int iterator = 0;

		for (int i = 0; i < data.Length - 1; i += 11)
		{
			Time1[iterator] = float.Parse(data[i]);
			Time2[iterator] = float.Parse(data[i + 1]);
			jerkPX0[iterator] = float.Parse(data[i + 2]);
			accelerationPX0[iterator] = float.Parse(data[i+3]);
			speedPX0[iterator] = float.Parse(data[i+4]);
			jerkPY0[iterator] = float.Parse(data[i+5]);
			accelerationPY0[iterator] = float.Parse(data[i+6]);
			speedPY0[iterator] = float.Parse(data[i+7]);
			jerkPZ0[iterator] = float.Parse(data[i+8]);
			accelerationPZ0[iterator] = float.Parse(data[i+9]);
			speedPZ0[iterator] = float.Parse(data[i+10]);

			iterator++;
		}
	}
}
