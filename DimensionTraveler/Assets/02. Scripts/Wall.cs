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
            //Debug.Log("��������̵�");
            StartCoroutine(MoveToCenterWithDelay(2.0f));
        }
        else
        {
            //Debug.Log("����������̵�");
            transform.position = wallOriPos;
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
