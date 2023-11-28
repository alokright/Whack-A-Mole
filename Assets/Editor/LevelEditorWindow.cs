using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class LevelEditorWindow : EditorWindow
{
    private LevelData selectedLevel;
    private LevelData templateLevel;
    private List<LevelData> levels;

    private string[] holeOptions = new string[] { "3 Holes", "6 Holes", "9 Holes" };
    private int selectedHoleOption = 0;

    private float moleLifeTimeMin = 0.7f;
    private float moleLifeTimeMax = 2.0f;
    private float movementDurationMin = 0.7f;
    private float movementDurationMax = 2.0f;

    [MenuItem("Window/Level Editor")]
    public static void ShowWindow()
    {
        GetWindow<LevelEditorWindow>("Level Editor");
    }

    private void OnEnable()
    {
        LoadLevels();
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Level Editor", EditorStyles.boldLabel);

        selectedLevel = EditorGUILayout.ObjectField("Selected Level", selectedLevel, typeof(LevelData), false) as LevelData;
        templateLevel = EditorGUILayout.ObjectField("Template Level", templateLevel, typeof(LevelData), false) as LevelData;

        if (GUILayout.Button("Select Level from Assets/Levels"))
        {
            SelectLevelFromAssets();
        }

        if (selectedLevel != null)
        {
            DisplayLevelFields();

            GUILayout.Space(10);

            EditorGUILayout.LabelField("Actions", EditorStyles.boldLabel);

            if (GUILayout.Button("Save Changes"))
            {
                SaveChanges();
            }

            if (GUILayout.Button("Create New Level"))
            {
                CreateNewLevel();
            }

            if (GUILayout.Button("Create Level from Template"))
            {
                CreateLevelFromTemplate();
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Select a LevelData object to edit.", MessageType.Info);
        }
    }

    private void LoadLevels()
    {
        levels = new List<LevelData>();
        string[] guids = AssetDatabase.FindAssets("t:LevelData", new[] { "Assets/Levels" });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            LevelData level = AssetDatabase.LoadAssetAtPath<LevelData>(path);
            levels.Add(level);
        }
    }

    private void DisplayLevelFields()
    {
        // Display LevelData values for editing
        selectedHoleOption = EditorGUILayout.Popup("Number Of Holes", selectedHoleOption, holeOptions);
        selectedLevel.NumberOfHoles = (selectedHoleOption + 1) * 3;

        selectedLevel.MovementDuration = EditorGUILayout.Slider("Movement Duration", selectedLevel.MovementDuration, movementDurationMin, movementDurationMax);
        selectedLevel.MoleLifeTime = EditorGUILayout.Slider("Mole Life Time", selectedLevel.MoleLifeTime, moleLifeTimeMin, moleLifeTimeMax);

        selectedLevel.MolePrefab = EditorGUILayout.ObjectField("Mole Prefab", selectedLevel.MolePrefab, typeof(GameObject), false) as GameObject;
        selectedLevel.Score = EditorGUILayout.IntSlider("Score", selectedLevel.Score, 1, int.MaxValue);
        selectedLevel.Damage = EditorGUILayout.IntSlider("Damage", selectedLevel.Damage, 1, int.MaxValue);
        selectedLevel.AnimationClipId = EditorGUILayout.TextField("Animation Clip Id", selectedLevel.AnimationClipId);
        selectedLevel.MaxScore = EditorGUILayout.IntSlider("Max Score", selectedLevel.MaxScore, 1, int.MaxValue);
        selectedLevel.MaxLives = EditorGUILayout.IntSlider("Max Lives", selectedLevel.MaxLives, 1, int.MaxValue);
        selectedLevel.MaxAdsResumes = EditorGUILayout.IntSlider("Max Ads Resumes", selectedLevel.MaxAdsResumes, 1, int.MaxValue);
    }

    private void SaveChanges()
    {
        EditorUtility.SetDirty(selectedLevel);
        AssetDatabase.SaveAssets();
        Debug.Log("Changes Saved");
    }

    private void CreateNewLevel()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save New Level", "NewLevel", "asset", "Enter a name for the new level");

        if (!string.IsNullOrEmpty(path))
        {
            LevelData newLevel = ScriptableObject.CreateInstance<LevelData>();
            AssetDatabase.CreateAsset(newLevel, path);
            levels.Add(newLevel);
            selectedLevel = newLevel;
            SaveChanges();
            LoadLevels();
        }
    }

    private void CreateLevelFromTemplate()
    {
        if (templateLevel != null)
        {
            string path = EditorUtility.SaveFilePanelInProject("Save New Level from Template", "NewLevelFromTemplate", "asset", "Enter a name for the new level");

            if (!string.IsNullOrEmpty(path))
            {
                LevelData newLevel = Instantiate(templateLevel);
                AssetDatabase.CreateAsset(newLevel, path);
                levels.Add(newLevel);
                selectedLevel = newLevel;
                SaveChanges();
                LoadLevels();
            }
        }
        else
        {
            Debug.LogError("Template Level is not selected.");
        }
    }

    private void SelectLevelFromAssets()
    {
        GenericMenu menu = new GenericMenu();

        foreach (var level in levels)
        {
            menu.AddItem(new GUIContent(level.name), false, () => { selectedLevel = level; });
        }

        menu.ShowAsContext();
    }
}
