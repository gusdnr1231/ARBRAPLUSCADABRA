using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolableMono : MonoBehaviour
{
    [Header("PoolableMono Default Value")]
    [Tooltip("�� ������Ʈ�� Ǯ���� ���� ���Ǵ� �̸�")]
    public string PoolName;

    public abstract void ResetPoolableMono();
    public abstract void EnablePoolableMono();
}
