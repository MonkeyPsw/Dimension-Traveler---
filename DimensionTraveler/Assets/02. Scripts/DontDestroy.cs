using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{
    // https://velog.io/@mayostar0514/DontDestroyOnLoad%EC%99%80-%EC%A4%91%EB%B3%B5-%EA%B0%9D%EC%B2%B4
    // °í¸¿½À´Ï´Ù
    public static List<string> dontDestroyList = new List<string>();
    public static List<GameObject> dontDestroyListObj = new List<GameObject>();

    private void Awake()
    {
        if (dontDestroyList.Contains(gameObject.name))
        {
            Destroy(gameObject);
            return;
        }

        dontDestroyList.Add(gameObject.name);
        dontDestroyListObj.Add(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            foreach (GameObject obj in dontDestroyListObj)
            {
                obj.SetActive(false);
            }
        }

        if (SceneManager.GetActiveScene().buildIndex == 6)
        {
            foreach (GameObject obj in dontDestroyListObj)
            {
                obj.SetActive(false);
            }

        }
    }
}
