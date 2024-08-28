using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolableType
{
	None = 0,
	Object = 1,
	Effect = 2,
	UI = 3,
	End
}

[System.Serializable]
public struct PoolDataStruct
{
	public PoolableType poolableType;
	public PoolableMono poolableMono;
	public string poolableName;
	public int Count;
}


[CreateAssetMenu(fileName = "New Pool List", menuName = "SO/Data/Pool List")]
public class PoolListSO : ScriptableObject
{
	public PoolDataStruct[] DataStruct;

	private void OnEnable()
	{
		for(int count = 0; count < DataStruct.Length; count++)
		{
			if (DataStruct[count].poolableMono == null)
			{
				Debug.LogWarning($"{this.name}'s {count} PoolableMono Value is Null");
				continue;
			}
			if (string.IsNullOrEmpty(DataStruct[count].poolableMono.PoolName))
			{
				Debug.LogWarning($"PoolableMono{DataStruct[count].poolableMono}'s PoolName is Null or Empty!");
				continue;
			}
			// PoolableName is Null In Struct or Not Same PoolableMono's Name
			if (string.IsNullOrEmpty(DataStruct[count].poolableName) == true || DataStruct[count].poolableName != DataStruct[count].poolableMono.PoolName)
			DataStruct[count].poolableName = DataStruct[count].poolableMono.PoolName;
		}
	}
}
