using System;
using UnityEngine;

[Serializable]
public struct PoolConfig
{
    public GameObject Prefab;
    public uint Count;
    public PoolObjectType Type;
}