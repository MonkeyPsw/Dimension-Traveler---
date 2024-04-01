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
                //Debug.Log("��������̵�");
                isCenter = true;
                StartCoroutine(MoveToCenterWithDelay(2.0f));
            }
        }
        else
        {
            if (isCenter)
            {
                //Debug.Log("����������̵�");
                isCenter = false;
                transform.position = wallOriPos;
            }
        }

    }

    IEnumerator MoveToCenterWithDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); // ������ ��ŭ ���
        transform.position = new Vector3(0.0f, transform.position.y, transform.position.z); // �߾����� �̵�
    }

    public Vector3 GetWallOriPos()
    {
        return wallOriPos;
    }
}
