using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class LevelEditorWindow : EditorWindow
{
    private LevelData selectedLevel;
    private LevelData templateLevel;
    private List<LevelData> levels;

    private string[] options = new string[] { "Create New", "Create from Template & Select Template", "Edit & Select Level" };
    private int selectedOption = 0;

    private Color defaultColor;

    [MenuItem("Deca/Level Editor")]
    public static void ShowWindow()
    {
        GetWindow<LevelEditorWindow>("Level Editor");
    }

    private void OnEnable()
    {
        defaultColor = GUI.backgroundColor;
        LoadLevels();
    }
    private int lastSelectedOption = -1;
    private void OnGUI()
    {
        EditorGUILayout.LabelField("Level Editor", EditorStyles.boldLabel);

        EditorGUIUtility.labelWidth = 150f; // Set label width for better alignment

        selectedOption = EditorGUILayout.Popup("Select Option", selectedOption, options);

        GUILayout.Space(10);

        if (selectedOption == 0)
        {
            if (lastSelectedOption != selectedOption)
                selectedLevel = null;
            // Create New Level
            GUI.backgroundColor = Color.cyan;
            if (GUILayout.Button("Create New Level"))
            {
                CreateNewLevel();
            }
            GUI.backgroundColor = defaultColor;
        }
        else if (selectedOption == 1)
        {
            // Create from Template & Select Template
            templateLevel = EditorGUILayout.ObjectField("Template Level", templateLevel, typeof(LevelData), false) as LevelData;

            if (GUILayout.Button("Select Template"))
            {
                if (templateLevel != null)
                {
                    CreateLevelFromTemplate();
                }
                else
                {
                    ShowError("Select a template level.");
                }
            }
        }
        else if (selectedOption == 2)
        {
            // Edit & Select Level
            selectedLevel = EditorGUILayout.ObjectField("Level To Edit", selectedLevel, typeof(LevelData), false) as LevelData;

           
            if (GUILayout.Button("Select Level"))
            {
                if (selectedLevel != null)
                {
                    SelectLevelFromAssets();
                }
                else
                {
                    ShowError("Select a template level.");
                }
            }
        }
        lastSelectedOption = selectedOption;
        GUILayout.Space(10);

        if (selectedLevel != null)
        {
            DisplayLevelFields();

            GUILayout.Space(10);

            GUI.backgroundColor = Color.blue;
            if (GUILayout.Button("Save"))
            {
                SaveChanges();
                ResetEditor();
            }
            GUI.backgroundColor = defaultColor;
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
        selectedLevel.NumberOfHoles = EditorGUILayout.IntSlider("Number Of Holes", selectedLevel.NumberOfHoles, 1, 27);
        selectedLevel.MovementDuration = EditorGUILayout.Slider("Movement Duration", selectedLevel.MovementDuration, 0.7f, 2.0f);
        selectedLevel.MoleLifeTime = EditorGUILayout.Slider("Mole Life Time", selectedLevel.MoleLifeTime, 0.7f, 2.0f);

        selectedLevel.MolePrefab = EditorGUILayout.ObjectField("Mole Prefab", selectedLevel.MolePrefab, typeof(GameObject), false) as GameObject;
        selectedLevel.Score = EditorGUILayout.IntSlider("Score", selectedLevel.Score, 1, 10);
        selectedLevel.Damage = EditorGUILayout.IntSlider("Damage", selectedLevel.Damage, 1, 10);
        selectedLevel.AnimationClipId = EditorGUILayout.TextField("Animation Clip Id", selectedLevel.AnimationClipId);
        selectedLevel.MaxScore = EditorGUILayout.IntSlider("Max Score", selectedLevel.MaxScore, 1, 100);
        selectedLevel.MaxLives = EditorGUILayout.IntSlider("Max Lives", selectedLevel.MaxLives, 1, 100);
        selectedLevel.MaxAdsResumes = EditorGUILayout.IntSlider("Max Ads Resumes", selectedLevel.MaxAdsResumes, 1, 5);
    }

    private void SaveChanges()
    {
        if (selectedLevel != null)
        {
            EditorUtility.SetDirty(selectedLevel);
            AssetDatabase.SaveAssets();
            Debug.Log("Changes Saved");
        }
        else
        {
            Debug.LogError("No level selected to save.");
        }
    }

    private void CreateNewLevel()
    {
        selectedOption = 0;
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
        selectedOption = 1;
        selectedLevel = Instantiate(templateLevel);
    }

    private void SetDefaultValues()
    {
        // Set default values for the new or template level
        selectedLevel.NumberOfHoles = 3;
        selectedLevel.MovementDuration = 1.0f;
        selectedLevel.MoleLifeTime = 1.0f;
        // Set other default values as needed
    }

    private void SelectLevelFromAssets()
    {
        selectedOption = 2;
      //  selectedLevel = Instantiate(templateLevel);
        //GenericMenu menu = new GenericMenu();

        //foreach (var level in levels)
        //{
        //    menu.AddItem(new GUIContent(level.name), false, () => { selectedLevel = level; });
        //}

        //menu.ShowAsContext();
    }

    private void ResetEditor()
    {
        selectedOption = 0;
        selectedLevel = null;
        templateLevel = null;
    }

    private void ShowError(string message)
    {
        EditorUtility.DisplayDialog("Error", message, "OK");
    }
}
