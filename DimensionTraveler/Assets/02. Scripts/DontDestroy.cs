using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    // https://velog.io/@mayostar0514/DontDestroyOnLoad%EC%99%80-%EC%A4%91%EB%B3%B5-%EA%B0%9D%EC%B2%B4
    // °í¸¿½À´Ï´Ù
    static List<string> dontDestroyList = new List<string>();

    private void Awake()
    {
        if (dontDestroyList.Contains(gameObject.name))
        {
            Destroy(gameObject);
            return;
        }

        dontDestroyList.Add(gameObject.name);
        DontDestroyOnLoad(gameObject);
    }
}
