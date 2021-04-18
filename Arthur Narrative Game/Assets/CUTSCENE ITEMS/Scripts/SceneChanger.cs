using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public float timeTillNextScene;
    public string NextLevel = "Cave Inside";

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadLevelAfterDelay(timeTillNextScene));
    }
    
    IEnumerator LoadLevelAfterDelay(float timeTillNextScene)
    {
        yield return new WaitForSeconds(timeTillNextScene);
        SceneManager.LoadScene(NextLevel);
    }
}
