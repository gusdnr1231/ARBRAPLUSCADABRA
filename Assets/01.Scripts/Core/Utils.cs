using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PRS
{
	public Vector3 Position;
	public Quaternion Rotation;
	public Vector3 Scale;

	public PRS(Vector3 pos,  Quaternion rot, Vector3 scale)
	{
		this.Position = pos;
		this.Rotation = rot;
		this.Scale = scale;
	}
}

public class Utils
{
	
}
