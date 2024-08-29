using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct PoolListStruct
{
	public string PoolingListName;
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

	private PoolListSO CurrentPoolingData;
	public event Action OnPoolingComplete;

	private Dictionary<string, PoolListSO> PoolListData = new Dictionary<string, PoolListSO>();
	private Dictionary<string, Pool<PoolableMono>> CompletePoolableMonos = new Dictionary<string, Pool<PoolableMono>>();
	private List<string> PoolingListNames = new();

	public override void Awake()
	{
		SetDataListInDictionary();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			DestroyObjectsInPool("ScrollCard");
		}
	}

	public void SetDataListInDictionary()
	{
		PoolingListNames.Clear();
		foreach (PoolListStruct structData in DataStruct)
		{
			if(structData.PoolData != null && string.IsNullOrEmpty(structData.PoolingListName) == false)
			{
				PoolListData.Add(structData.PoolingListName, structData.PoolData);
				PoolingListNames.Add(structData.PoolingListName);
				SetDataOnStruct(structData.PoolingListName);
			}

		}
	}

	public void SetDataOnStruct(string floorName, bool isReset = false)
	{
		if(PoolingListNames.Contains(floorName) == false)
		{
			Debug.LogWarning("Init Name is Null! Please Check PoolingList's Name");
			return;
		}
		if (isReset == true) ClearPreviousData();

		CurrentPoolingData = PoolListData[floorName];

		StartPooling();
	}

	#region Destroy Methods

	public void DestroyObjectsInPool(string poolableName)
	{
		if (CompletePoolableMonos.ContainsKey(poolableName))
		{
			CompletePoolableMonos.TryGetValue(poolableName, out Pool<PoolableMono> value);
			value.DestroyAll();
			CompletePoolableMonos.Remove(poolableName);
		}
		else
		{
			Debug.LogWarning($"No pool found for {poolableName}");
		}
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

	public void DestroyAllActiveObjects() // 현재 모든 생성된 오브젝트들 제거
	{
		foreach (var pool in CompletePoolableMonos.Values)
		{
			pool.DestroyAll(); // Destroy All Object In Pool
		}
	}

	#endregion

	private void StartPooling()
	{
		Pool<PoolableMono> poolTemp = new Pool<PoolableMono>(null, null, 0);
		foreach (PoolDataStruct pds in CurrentPoolingData.DataStruct)
		{
			if (pds.poolableMono == null) Debug.LogError($" PoolableMono is Null.");
			if (pds.Count <= 0) Debug.LogError($" Count is Wrong Value");
			
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

	public void Push(PoolableMono item)
	{
		if (CompletePoolableMonos[item.PoolName] == null)
		{
			Debug.LogError($"Named {item.PoolName} Object is Null");
			return;
		}
		CompletePoolableMonos[item.PoolName].Push(item);
	}
}
