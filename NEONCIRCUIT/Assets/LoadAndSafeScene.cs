using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadAndSafeScene : MonoBehaviour
{
    public bool LoadsScene = false;
    // Start is called before the first frame update
    void Awake()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        GameObject[] ob = GameObject.FindGameObjectsWithTag("Manager");

        if (ob.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene == SceneManager.GetSceneByBuildIndex(1))
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            if (LoadsScene)
            {
                //GameObject.FindGameObjectWithTag("Player").GetComponent<Playerstats>().SavePlayer();
                GameObject.FindGameObjectWithTag("Player").GetComponent<Playerstats>().LoadPlayer();
            }
            else
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<Playerstats>().SavePlayer();
            }
        }
    }
}
