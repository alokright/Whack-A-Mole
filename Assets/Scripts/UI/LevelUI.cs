
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [SerializeField] Image Background;
    [SerializeField] TextMeshProUGUI DetailsText;
    [SerializeField] TextMeshProUGUI Id;
    [SerializeField] Image Lock;
    [SerializeField] Sprite LockSprite;
    [SerializeField] Sprite CompletedSprite;

    private LevelData LevelData;
    private bool IsLocked = false;
    private bool IsCompleted = false;
    private const string DETAILS_FORMAT = "Hole : {0}, Target Score : {1}";
    private const string ID_FORMAT = "Level {0}";
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnLevelTapped);
        GameEventManager.OnLevelFinished += OnLevelFinished;
    }

    private void OnLevelFinished(int levelId)
    {
        if(levelId == LevelData.Index)
        {
            Lock.sprite = CompletedSprite;
            Lock.gameObject.SetActive(true);
        }
    }

    public void SetDetails(LevelData level, bool isLocked, bool isCompleted)
    {
        LevelData = level;
        DetailsText.text = string.Format(DETAILS_FORMAT, level.NumberOfHoles, level.MaxScore);
        Id.text = string.Format(ID_FORMAT, level.Index);
        IsLocked = isLocked;
        
        if (isLocked)
        {
            Lock.sprite = LockSprite;
        }else if (isCompleted)
        {
            Lock.sprite = CompletedSprite;
        }else
            Lock.gameObject.SetActive(false);
    }

    private void OnLevelTapped()
    {
        if (!IsLocked)
            GameEventManager.LevelSelected(LevelData);
        else
            Debug.Log("Current Level is Locked!");
    }
}
