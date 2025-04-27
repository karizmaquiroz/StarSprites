using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LorebookUI : MonoBehaviour
{
    public TextMeshProUGUI loreTextUI;
    public int currentPage = 1;
    public int maxPages = 10;
    private GameObject FurnitureUI;

    void Start()
    {
        ShowPage(currentPage);
        FurnitureUI = GameObject.Find("FurnitureUI");
        if (FurnitureUI != null)
        {
            Debug.Log("Found FurnitureUI GameObject");
            FurnitureUI.SetActive(false);
        }
    }

    public void ShowPage(int pageNumber)
    {
        currentPage = pageNumber;
        if (LorebookManager.Instance.IsPageUnlocked(pageNumber))
        {
            loreTextUI.text = LorebookManager.Instance.GetLoreText(pageNumber);
        }
        else
        {
            loreTextUI.text = ""; // Blank for missing pages, or show "Page Missing"
        }
    }

    public void NextPage()
    {
        if (currentPage < maxPages)
        {
            ShowPage(currentPage + 1);
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 1)
        {
            ShowPage(currentPage - 1);
        }
    }

    public void CloseLorebook()
    {
        gameObject.SetActive(false);
        if (FurnitureUI != null)
        {
            FurnitureUI.SetActive(true);
        }
    }
}
