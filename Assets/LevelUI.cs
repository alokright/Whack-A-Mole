
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [SerializeField] Image Background;
    [SerializeField] TextMeshProUGUI DetailsText;
    [SerializeField] TextMeshProUGUI Id;
    [SerializeField] Image Lock;

    private LevelData LevelData;
    private bool IsLocked = false;
    private const string DETAILS_FORMAT = "Hole : {0}, Target Score : {1}";
    private const string ID_FORMAT = "Level {0}";
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnLevelTapped);
    }

    public void SetDetails(LevelData level, bool isLocked)
    {
        LevelData = level;
        DetailsText.text = string.Format(DETAILS_FORMAT, level.NumberOfHoles, level.MaxScore);
        Id.text = string.Format(ID_FORMAT, level.Index);
        IsLocked = isLocked;
        Lock.gameObject.SetActive(IsLocked);
    }

    private void OnLevelTapped()
    {
        if (!IsLocked)
            GameEventManager.LevelSelected(LevelData);
        else
            Debug.Log("Current Level is Locked!");
    }
}
