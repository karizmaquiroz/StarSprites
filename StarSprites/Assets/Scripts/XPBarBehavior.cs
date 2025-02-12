using UnityEngine;
using UnityEngine.UI;

public class XPBarBehavior : MonoBehaviour
{
    public Slider xpSlider;
    public int currentXP = 0;
    public int maxXP = 100;
    public int level = 1;

    void Start()
    {
        if (xpSlider == null)
            xpSlider = GetComponent<Slider>();

        xpSlider.maxValue = maxXP;
        xpSlider.value = currentXP;
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
        if (currentXP > maxXP)
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
        Debug.Log("Level Up!");
    }
}