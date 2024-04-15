using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool inputEnabled = true;
    public static int level = 2; // 0:오프닝, 1:메인메뉴, 2:맵1_1, ... // 기본 2
    float inputDelay = 2.0f; // 입력 딜레이 시간

    Canvas canvas;
    GameObject pausePanel;
    bool isPaused = false;
    public GameObject gameOverPanel;

    int curHp = 10;
    int maxHp = 10;
    int score = 0;

    int tmpCurHp;
    int tmpMaxHp;
    int tmpScore;
    bool isReload = false;
    bool isDim;

    public GameObject startEffectPrefab;
    public GameObject transitionEffectPrefab;
    public float transitionDuration = 2.0f;
    public AudioClip transitionSound;
    public AudioClip dimensionChangeSound;

    public static GameManager instance;
    AudioSource audioSource;
    public AudioClip openingBGM;
    public GameObject bgm;
    public AudioClip pauseSound;
    public AudioClip gameOverSound;

    private void Awake()
    {
        inputEnabled = false;
        foreach (GameObject obj in DontDestroy.dontDestroyListObj)
        {
            obj.SetActive(true);
        }

        if (instance == null)
        {
            instance = this;

            SceneManager.sceneLoaded += OnSceneLoaded;

            canvas = FindObjectOfType<Canvas>();
            if (canvas != null)
            {
                pausePanel = canvas.transform.Find("PausePanel")?.gameObject;
                if (pausePanel != null)
                    pausePanel.SetActive(false);
            }

        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (gameObject.activeSelf)
        {
            // 이 조건 되는건가 몰루
            if (SceneManager.GetActiveScene().buildIndex != SceneManager.sceneCountInBuildSettings)
            {
                StartCoroutine(EnableInputAfterDelay(inputDelay + 1.0f));

                GameObject loadingEffect = Instantiate(startEffectPrefab, new Vector3(0, -4.1f, 0), Quaternion.identity);
                if (transitionSound != null)
                    AudioSource.PlayClipAtPoint(transitionSound, transform.position);
                Destroy(loadingEffect, transitionDuration);
            }
        }
        else
            InitValue();

        // 씬이 로드될 때마다 현재 값들을 임시 변수에 저장
        tmpCurHp = curHp;
        tmpMaxHp = maxHp;
        tmpScore = score;
        isDim = PlayerMovement.isDimension;

    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isReload)
        {
            curHp = tmpCurHp;
            maxHp = tmpMaxHp;
            score = tmpScore;
            PlayerMovement.isDimension = isDim;
            isReload = false;
        }

        if (inputEnabled && !isPaused && PlayerMovement.isDimension)
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
        audioSource.PlayOneShot(dimensionChangeSound);
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
    
    public void TogglePause()
    {
        audioSource.PlayOneShot(pauseSound);
        isPaused = !isPaused;

        if (isPaused)
            bgm.GetComponent<AudioSource>().Pause();
        else
            bgm.GetComponent<AudioSource>().UnPause();

        Time.timeScale = isPaused ? 0 : 1.0f;
        pausePanel.SetActive(isPaused);
    }

    public void GameOver()
    {
        bgm.GetComponent<AudioSource>().Pause();
        audioSource.PlayOneShot(gameOverSound);
        isPaused = true;

        Time.timeScale = 0;
        gameOverPanel.SetActive(isPaused);
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

    public static void LoadNextMap(Vector3 Pos)
    {
        instance.PlayTransitionEffect(Pos);
    }

    public void PlayTransitionEffect(Vector3 Pos)
    {
        StartCoroutine(EnableInputAfterDelay(inputDelay));

        //if (!Camera.main.orthographic)
        //    SetDimension();

        GameObject transitionEffect = Instantiate(transitionEffectPrefab, Pos, Quaternion.identity);
        Destroy(transitionEffect, transitionDuration);

        StartCoroutine(LoadMapAfterDelay(transitionDuration));
    }

    IEnumerator LoadMapAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(level);
    }

    public void ReLoadCurrentMap()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        isReload = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas != null)
        {
            GameObject pausePanel = canvas.transform.Find("PausePanel")?.gameObject;
            if (pausePanel != null)
            {
                pausePanel.SetActive(false);
                TogglePause();
            }
            if (gameOverPanel != null)
            {
                isPaused = false;
                Time.timeScale = 1.0f;
                gameOverPanel.SetActive(isPaused);
            }
        }
    }

    public void InitValue()
    {
        curHp = 10;
        maxHp = 10;
        score = 0;
        level = 2;
        PlayerMovement.isDimension = false;

        tmpCurHp = curHp;
        tmpMaxHp = maxHp;
        tmpScore = score;
    }

    public void PanelHide()
    {
        TogglePause();
        gameOverPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void BackToMenu()
    {
        PanelHide();
        InitValue();
        SceneManager.LoadScene(1);
    }

    public void BackToTitle()
    {
        InitValue();
        SceneManager.LoadScene(0);
    }

    //private void OnDestroy()
    //{
    //    instance = null;
    //}
}
