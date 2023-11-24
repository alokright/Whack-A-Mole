using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelData : ScriptableObject,IComparable<LevelData>
{
    public int Index; // Index field as ID
    public int NumberOfHoles;
    public float MovementDuration;
    public float MoleLifeTime;
    public List<Vector3> MolePositions = new List<Vector3>() { new Vector3(-1.8f,0f,-1.5f),new Vector3(0f,0f,-1.5f),new Vector3(1.8f,0f,-1.5f),
        new Vector3(-1.8f,0f,1f),new Vector3(0f,0f,1f),new Vector3(1.8f,0f,1f),
        new Vector3(-1.8f,0f,3.5f),new Vector3(0f,0f,3.5f),new Vector3(1.8f,0f,3.5f)

    }; // List of Vector3 for mole positions
    public GameObject MolePrefab; // Reference to the Mole prefab
    public int Score;
    public int Damage;
    public string AnimationClipId; // Assuming AnimationClipId is a string, otherwise use the appropriate type
    public int MaxScore = 15;
    public int MaxLives = 3;
    public int MaxAdsResumes = 1;
    public int CompareTo(LevelData other)
    {
        return Index.CompareTo(other.Index);
    }
}
