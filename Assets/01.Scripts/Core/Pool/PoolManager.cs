using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public struct PoolListStruct
{
	public string FloorName;
	public PoolListSO PoolData;
}

public class PoolManager : MonoSingleton<PoolManager>
{
	[Header("Pool Parents")]
	[SerializeField] private Transform PoolParent_Object;
	[SerializeField] private Transform PoolParent_Effect;
	[SerializeField] private Transform PoolParent_UI;

	[Header("Pool Values")]
	[SerializeField] private PoolListStruct[] DataStruct;

	private PoolListSO CurrentFloorPoolData;
	public event Action OnPoolingComplete;

	private Dictionary<string, PoolListSO> PoolListData = new Dictionary<string, PoolListSO>();
	private Dictionary<string, Pool<PoolableMono>> CompletePoolableMonos = new Dictionary<string, Pool<PoolableMono>>();
	private List<string> StructNamesList = new();

	public override void Awake()
	{
		DontDestroyOnLoad(PoolParent_Object);
		DontDestroyOnLoad(PoolParent_Effect);
		DontDestroyOnLoad(PoolParent_UI);

		SetDataListInDictionary();
	}

	public void SetDataListInDictionary()
	{
		StructNamesList.Clear();
		foreach (PoolListStruct structData in DataStruct)
		{
			if(structData.PoolData != null && string.IsNullOrEmpty(structData.FloorName) == false)
			{
				PoolListData.Add(structData.FloorName, structData.PoolData);
				StructNamesList.Add(structData.FloorName);
			}
		}
	}

	public void SetDataOnStruct(string floorName, bool isReset = false)
	{
		if(StructNamesList.Contains(floorName) == false)
		{
			Debug.LogWarning("Init Name is Null! Please Check PoolingList's Name");
			return;
		}
		if (isReset == true) ClearPreviousData();

		CurrentFloorPoolData = PoolListData[floorName];

		StartPooling();
	}

	public void ClearPreviousData()
	{
		foreach (var pool in CompletePoolableMonos.Values)
		{
			pool.DestroyAll();
		}

		DestroyInChild(PoolParent_Object);
		DestroyInChild(PoolParent_Effect);
		DestroyInChild(PoolParent_UI);

		CompletePoolableMonos.Clear();
	}

	private void DestroyInChild(Transform InitTrm)
	{
		if(InitTrm == null ||  InitTrm.childCount <= 0) return;

		for(int count = 0; count < InitTrm.childCount; count++)
		{
			Destroy(InitTrm.GetChild(count).gameObject);
		}
	}

	private void StartPooling()
	{
		foreach (PoolDataStruct pds in CurrentFloorPoolData.DataStruct)
		{
			if (pds.poolableType == PoolableType.None ||
				pds.poolableType == PoolableType.End) Debug.LogError($" PoolableType is Null.");
			if (pds.poolableMono == null) Debug.LogError($" PoolableMono is Null.");
			if (pds.Count <= 0) Debug.LogError($" Count is Wrong Value");

			Pool<PoolableMono> poolTemp = new Pool<PoolableMono>(null, null, 0);
			
			switch (pds.poolableType)
			{
				case PoolableType.Object:
					{
						poolTemp = new Pool<PoolableMono>(pds.poolableMono, PoolParent_Object, pds.Count);
						break;
					}
				case PoolableType.Effect:
					{
						poolTemp = new Pool<PoolableMono>(pds.poolableMono, PoolParent_Effect, pds.Count);
						break;
					}
				case PoolableType.UI:
					{
						poolTemp = new Pool<PoolableMono>(pds.poolableMono, PoolParent_UI, pds.Count);
						break;
					}
				case PoolableType.None:
				case PoolableType.End:
				default:
					Debug.LogWarning("PoolManager;s Value is Wrong");
					break;
			}

			CompletePoolableMonos.TryAdd(pds.poolableName, poolTemp);

		}
		
		OnPoolingComplete?.Invoke();
	}

	public PoolableMono Pop(string PoolableName)
	{
		if (CompletePoolableMonos[PoolableName] == null)
		{
			Debug.LogError($"Named {PoolableName} Object is Null");
			return null;
		}
		PoolableMono item = CompletePoolableMonos[PoolableName].Pop();
		return item;
	}

	public PoolableMono Pop(string PoolableName, Transform SpawnTrm)
	{
		if (CompletePoolableMonos[PoolableName] == null)
		{
			Debug.LogError($"Named {PoolableName} Object is Null");
			return null;
		}
		PoolableMono item = CompletePoolableMonos[PoolableName].Pop();
		item.transform.position = SpawnTrm.position;
		return item;
	}

	public PoolableMono Pop(string PoolableName, Vector3 SpawnPos)
	{
		if (CompletePoolableMonos[PoolableName] == null)
		{
			Debug.LogError($"Named {PoolableName} Object is Null");
			return null;
		}
		PoolableMono item = CompletePoolableMonos[PoolableName].Pop();
		item.transform.position = SpawnPos;
		return item;
	}

	public void Push(PoolableMono item, string PoolableName)
	{
		if (CompletePoolableMonos[PoolableName] == null)
		{
			Debug.LogError($"Named {PoolableName} Object is Null");
			return;
		}
		CompletePoolableMonos[PoolableName].Push(item);
	}
	
	public void DestroyAllActiveObjects()
	{
		foreach (var pool in CompletePoolableMonos.Values)
		{
			pool.DestroyAll(); // Destroy All Object In Pool
		}
	}
}
