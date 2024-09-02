using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolableMono : MonoBehaviour
{
    [Header("PoolableMono Default Value")]
    [Tooltip("이 오브젝트를 풀링할 때에 사용되는 이름")]
    public string PoolName;

    public abstract void ResetPoolableMono();
    public abstract void EnablePoolableMono();
}
