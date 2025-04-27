using UnityEngine;
using UnityEngine.UI;

public class LorebookUI : MonoBehaviour
{
    public Text loreTextUI;
    public int currentPage = 1;
    public int maxPages = 10;

    void Start()
    {
        ShowPage(currentPage);
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
}
