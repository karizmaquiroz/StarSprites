using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    [Header("Enemies in Scene")]
    public List<GameObject> enemyList = new List<GameObject>();

    [Header("Scene to Load When All Enemies Are Inactive")]
    public int sceneBuildIndex;

    private bool sceneChanging = false;

    void Update()
    {
        if (sceneChanging) return;

        bool allEnemiesInactive = true;
        foreach (GameObject enemy in enemyList)
        {
            if (enemy != null && enemy.activeInHierarchy)
            {
                allEnemiesInactive = false;
                break;
            }
        }

        if (allEnemiesInactive)
        {
            sceneChanging = true;
            StartCoroutine(LoadSceneAfterDelay(1f));
        }
    }

    IEnumerator LoadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneBuildIndex);
    }
}