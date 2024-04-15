using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    void Update()
    {
        // "shift" 키를 누르면 다음 씬을 로드합니다.
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            LoadNextScene();
        }
    }

    public void LoadNextScene()
    {
        // 현재 씬의 빌드 인덱스를 가져옵니다.
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // 현재 씬의 다음 씬의 빌드 인덱스를 계산합니다.
        int nextSceneIndex = currentSceneIndex + 1;

        // 다음 씬이 더 이상 없을 경우 첫 번째 씬으로 돌아갑니다.
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
            GameManager.level = 2;
            //PlayerMovement.isDimension = false;
        }

        // 다음 씬을 로드합니다.
        SceneManager.LoadScene(nextSceneIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
