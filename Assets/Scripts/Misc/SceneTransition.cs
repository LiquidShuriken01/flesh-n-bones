using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string NextScene;
    public string tagEnemy = "Enemy";
    public GameObject loader;
    public bool enemiesPresent;

    void Start()
    {
        loader.SetActive(false);
        int count = CountObjectsWithTag(tagEnemy);
        Debug.Log("Total enemies " + count);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered");
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(NextScene);
        }
    }

    int CountObjectsWithTag(string tag)
    {
        GameObject[] totalEnemies = GameObject.FindGameObjectsWithTag(tagEnemy);
        if (totalEnemies.Length == 0)
        {
            enemiesPresent = false;
        }
        else
        {
            enemiesPresent = true;
        }
        return totalEnemies.Length;
    }

    void Update()
    {
        int countUpdate = CountObjectsWithTag(tagEnemy);
        Debug.Log(countUpdate);
        if (!enemiesPresent)
        {
            loader.SetActive(true);
        }
        else
        {
            loader.SetActive(false);
        }
    }
}