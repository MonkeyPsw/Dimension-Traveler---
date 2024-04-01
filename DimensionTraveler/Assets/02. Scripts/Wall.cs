using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    Vector3 wallOriPos;
    bool isCenter = false;

    void Start()
    {
        wallOriPos = transform.position;
    }

    void LateUpdate()
    {

        if (CameraMove.mainCam.orthographic)
        {
            if (!isCenter)
            {
                //Debug.Log("벽가운데로이동");
                isCenter = true;
                StartCoroutine(MoveToCenterWithDelay(2.0f));
            }
        }
        else
        {
            if (isCenter)
            {
                //Debug.Log("벽원래대로이동");
                isCenter = false;
                transform.position = wallOriPos;
            }
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
