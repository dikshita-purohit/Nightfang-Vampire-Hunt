using UnityEngine;
using TMPro;

public class CollectiblesManager : MonoBehaviour
{
    public static CollectiblesManager Instance { get; private set; }
    public TextMeshProUGUI gemCounterText;
    private int gemCount = 0;    

    private void OnEnable()
    {
        Gem.OnGemCollected += OnGemCollected;
    }

    private void OnDisable()
    {
        Gem.OnGemCollected -= OnGemCollected;
    }

    private void OnGemCollected()
    {
        gemCount++;
        UpdateGemUI();
    }

    private void UpdateGemUI()
    {
        if (gemCounterText != null)
        {
            gemCounterText.text = "Gems: " + gemCount;
        }
    }

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    public int GemCount => gemCount;

     public void ResetGemCount()
    {
        gemCount = 0;
        UpdateGemUI();
    }
}
