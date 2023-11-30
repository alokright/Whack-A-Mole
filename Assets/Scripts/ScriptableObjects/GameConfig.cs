using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObject/GameConfig")]
public class GameConfig : ScriptableObject
{
    public float LifeGenerationDuration = 10f;
    public List<Vector3> MolePositions = new List<Vector3>() { new Vector3(-1.8f,0f,-1.5f),new Vector3(0f,0f,-1.5f),new Vector3(1.8f,0f,-1.5f),
        new Vector3(-1.8f,0f,1f),new Vector3(0f,0f,1f),new Vector3(1.8f,0f,1f),
        new Vector3(-1.8f,0f,3.5f),new Vector3(0f,0f,3.5f),new Vector3(1.8f,0f,3.5f)

    };
}
