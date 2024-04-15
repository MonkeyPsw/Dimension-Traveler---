using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    void Update()
    {
        // "shift" Ű�� ������ ���� ���� �ε��մϴ�.
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            LoadNextScene();
        }
    }

    public void LoadNextScene()
    {
        // ���� ���� ���� �ε����� �����ɴϴ�.
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // ���� ���� ���� ���� ���� �ε����� ����մϴ�.
        int nextSceneIndex = currentSceneIndex + 1;

        // ���� ���� �� �̻� ���� ��� ù ��° ������ ���ư��ϴ�.
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
            GameManager.level = 2;
            //PlayerMovement.isDimension = false;
        }

        // ���� ���� �ε��մϴ�.
        SceneManager.LoadScene(nextSceneIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
