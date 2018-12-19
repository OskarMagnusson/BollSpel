using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*By Björn Andersson*/

public static class GameManager {
    
    public static void LoadNextLevel()
    {
        int nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextLevel < SceneManager.sceneCount)
            SceneManager.LoadScene(nextLevel);
    }

    public static IEnumerator RespawnWait()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
