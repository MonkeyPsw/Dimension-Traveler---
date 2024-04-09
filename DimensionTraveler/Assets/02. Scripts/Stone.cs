using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    Vector3 stoneOriPos;
    bool isCenter = false;

    void Start()
    {
        stoneOriPos = transform.position;
    }

    void LateUpdate()
    {
        if (CameraMove.mainCam.orthographic)
        {
            if (!isCenter)
            {
                isCenter = true;
                stoneOriPos = transform.position;
                StartCoroutine(MoveToCenterWithDelay(2.0f));
            }
        }
        else
        {
            if (isCenter)
            {
                isCenter = false;
                transform.position = new Vector3(stoneOriPos.x, transform.position.y, transform.position.z); // x좌표 유지
            }
        }
    }

    IEnumerator MoveToCenterWithDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); // 딜레이 만큼 대기
        transform.position = new Vector3(0.0f, transform.position.y, transform.position.z); // 중앙으로 이동
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.AddCurHp(-3);
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }


}
