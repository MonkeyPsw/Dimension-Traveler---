using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    Vector3 wallOriPos;

    void Start()
    {
        wallOriPos = transform.position;
    }

    void LateUpdate()
    {

        if (CameraMove.mainCam.orthographic)
        {
            //Debug.Log("벽가운데로이동");
            StartCoroutine(MoveToCenterWithDelay(2.0f));
        }
        else
        {
            //Debug.Log("벽원래대로이동");
            transform.position = wallOriPos;
        }

    }

    IEnumerator MoveToCenterWithDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); // 딜레이 만큼 대기
        transform.position = new Vector3(0.0f, transform.position.y, transform.position.z); // 중앙으로 이동
    }

    public Vector3 GetWallOriPos()
    {
        return wallOriPos;
    }
}
