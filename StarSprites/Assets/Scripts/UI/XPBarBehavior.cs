using UnityEngine;
using UnityEngine.UI;

public class XPBarBehavior : MonoBehaviour
{
    public static XPBarBehavior Instance;

    public Slider xpSlider;
    public int currentXP = 0;
    public int maxXP = 100;
    public int level = 1;

    [Header("Ability Icons")]
    public GameObject[] abilityIcons; // Assign 3 icons in inspector

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps it alive across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (xpSlider == null)
            xpSlider = GetComponent<Slider>();

        xpSlider.maxValue = maxXP;
        xpSlider.value = currentXP;

        UpdateAbilityIcons(); // Set correct icons on start
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Light"))
        {
            GainXP(25);
        }
    }

    public void GainXP(int amount)
    {
        currentXP += amount;
        if (currentXP >= maxXP)
        {
            LevelUp();
        }
        xpSlider.value = currentXP;
    }

    void LevelUp()
    {
        level++;
        currentXP = 0;
        maxXP += 50; // Increase XP cap per level
        xpSlider.maxValue = maxXP;

        Debug.Log("Level Up! Now level: " + level);
        UpdateAbilityIcons();
    }

    void UpdateAbilityIcons()
    {
        for (int i = 0; i < abilityIcons.Length; i++)
        {
            abilityIcons[i].SetActive(level > i);
        }
    }
}
