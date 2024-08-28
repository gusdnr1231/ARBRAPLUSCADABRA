using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolableMono : MonoBehaviour
{
    [Header("PoolableMono Default Value")]
    public string PoolName;

    public abstract void ResetPoolableMono();
    public abstract void EnablePoolableMono();
}
