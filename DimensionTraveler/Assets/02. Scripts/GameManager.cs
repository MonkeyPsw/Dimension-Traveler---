using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool inputEnabled = true;
    public static int level = 2; // 0:오프닝, 1:메인메뉴, 2:맵1_1, ...
    float inputDelay = 2.0f; // 입력 딜레이 시간

    GameObject pausePanel;
    bool isPaused = false;

    int curHp = 10;
    int maxHp = 10;
    int score = 0;

    //public Collider[] colliders;

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        pausePanel = canvas.transform.Find("PausePanel").gameObject;
        pausePanel.SetActive(false);
    }

    void Update()
    {
        if (inputEnabled && PlayerMovement.isDimension)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && PlayerMovement.isGrounded)
            {
                Debug.Log("차원 전환");
                SetDimension();
                //CheckCollisionAndSetDimension();
                //Time.timeScale = 0.3f;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void SetDimension()
    {
        CameraMove.ChangeDimension();
        PlayerMovement.isChange = true;
        inputEnabled = false; // 입력 비활성화
        StartCoroutine(EnableInputAfterDelay(inputDelay)); // 입력 딜레이 후에 다시 활성화
    }

    //void CheckCollisionAndSetDimension()
    //{
    //    colliders =
    //        Physics.OverlapSphere(PlayerMovement.instance.transform.position, 1.0f);

    //    foreach (var collider in colliders)
    //    {
    //        if (collider.CompareTag("Wall"))
    //        {
    //            SetDimension();
    //            return;
    //        }
    //    }

    //    SetDimension();
    //}

    IEnumerator EnableInputAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); // 입력 딜레이 시간만큼 대기
        inputEnabled = true; // 입력 활성화
        //Time.timeScale = 1.0f;
    }

    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }

    void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }
    
    void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1.0f;
        pausePanel.SetActive(isPaused);
    }

    public void AddScore(int score)
    {
        this.score += score;
    }

    public int GetScore()
    {
        return score;
    }

    public void SetCurHp(int hp)
    {
        this.curHp = hp;
    }

    public void AddCurHp(int hp)
    {
        this.curHp += hp;
    }

    public int GetCurHp()
    {
        return curHp;
    }

    public void AddMaxHp(int hp)
    {
        this.maxHp += hp;
    }

    public int GetMaxHp()
    {
        return maxHp;
    }

    public static void LoadNextMap()
    {
        SceneManager.LoadScene(level);
    }

}
