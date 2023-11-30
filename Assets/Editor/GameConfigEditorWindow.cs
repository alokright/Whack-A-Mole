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
        GUIStyle coloredLabelStyle = new GUIStyle(EditorStyles.boldLabel);
        coloredLabelStyle.normal.textColor = Color.red;
        
        GUIStyle coloredButtonStyle = new GUIStyle(GUI.skin.button);
        coloredButtonStyle.normal.textColor = Color.blue; 
        coloredButtonStyle.fontStyle = FontStyle.Bold;
        
        GUILayout.Label("Game Configuration Editor");

        
        if (GUILayout.Button("Search GameConfig in Assets"))
        {
            SearchGameConfig();
        }

        if (gameConfig != null)
        {
            EditorGUI.BeginChangeCheck();
            
            gameConfig.LifeGenerationDuration = EditorGUILayout.FloatField("Life Generation Duration", gameConfig.LifeGenerationDuration);
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Mole Movement Positions", coloredLabelStyle);
            gameConfig.MoleHidePosition = EditorGUILayout.Vector3Field("Hidden Position ", gameConfig.MoleHidePosition);
            gameConfig.MoleShowPosition = EditorGUILayout.Vector3Field("Shown Position ", gameConfig.MoleHidePosition);
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Mole Positions", coloredLabelStyle);
            for (int i = 0; i < gameConfig.MolePositions.Count; i++)
            {
                gameConfig.MolePositions[i] = EditorGUILayout.Vector3Field("Position " + (i + 1), gameConfig.MolePositions[i]);
            }
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(gameConfig);
            }

            // Save Button with custom style
            if (GUILayout.Button("Save", coloredButtonStyle))
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
