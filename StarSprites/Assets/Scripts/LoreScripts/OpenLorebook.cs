using UnityEngine;

public class OpenLorebook : MonoBehaviour
{
    private GameObject LoreBookUI;

    private void Start()
    {
        LoreBookUI = GameObject.Find("LorebookUI");
        if (LoreBookUI != null)
        {
            LoreBookUI.SetActive(false);
        }
        else
        {
            Debug.LogError("LoreBookUI not found in the scene.");
        }
    }
    private void OnMouseDown()
    {
        LoreBookUI.SetActive(true);
    }
}
