using UnityEngine;
using UnityEditor;
using System.Linq;

public class GameConfigEditorWindow : EditorWindow
{
    private GameConfig gameConfig;
    private string searchPath = "Assets"; // Default search path

    [MenuItem("Deca/GameConfig")]
    public static void ShowWindow()
    {
        GetWindow<GameConfigEditorWindow>("Game Config Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Game Configuration Editor", EditorStyles.boldLabel);

        if (GUILayout.Button("Search GameConfig in Assets"))
        {
            SearchGameConfig();
        }

        if (gameConfig != null)
        {
            EditorGUI.BeginChangeCheck();

            // Display and edit fields
            gameConfig.LifeGenerationDuration = EditorGUILayout.FloatField("Life Generation Duration", gameConfig.LifeGenerationDuration);

            EditorGUILayout.LabelField("Mole Positions");
            for (int i = 0; i < gameConfig.MolePositions.Count; i++)
            {
                gameConfig.MolePositions[i] = EditorGUILayout.Vector3Field("Position " + (i + 1), gameConfig.MolePositions[i]);
            }

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(gameConfig);
            }

            // Save Button
            if (GUILayout.Button("Save"))
            {
                SaveGameConfig();
            }
        }
    }

    private void SearchGameConfig()
    {
        var guids = AssetDatabase.FindAssets("t:GameConfig", new[] { searchPath });
        if (guids.Length > 0)
        {
            var path = AssetDatabase.GUIDToAssetPath(guids.First());
            gameConfig = AssetDatabase.LoadAssetAtPath<GameConfig>(path);
        }
        else
        {
            EditorUtility.DisplayDialog("GameConfig Not Found", "No GameConfig object found in " + searchPath, "OK");
        }
    }

    private void SaveGameConfig()
    {
        if (gameConfig != null)
        {
            EditorUtility.SetDirty(gameConfig);
            AssetDatabase.SaveAssets();
            Debug.Log("GameConfig saved.");
        }
    }
}
