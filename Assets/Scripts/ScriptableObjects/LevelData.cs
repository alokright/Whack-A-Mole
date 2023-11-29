using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelData : ScriptableObject,IComparable<LevelData>
{
    public int Index;
    public int NumberOfHoles;
    public float MovementDuration;
    public float MoleLifeTime;
    public GameObject MolePrefab; 
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
