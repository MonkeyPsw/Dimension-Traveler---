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
                //Debug.Log(IsWall());
                if (IsWall())
                {
                    Debug.Log("����ģ��");
                    //gameObject.GetComponent<MeshRenderer>().enabled = false;
                }
            }
        }
        else
        {
            if (isCenter)
            {
                //Debug.Log("����������̵�");
                isCenter = false;
                transform.position = wallOriPos;
                //if (gameObject.GetComponent<MeshRenderer>())
                //    gameObject.GetComponent<MeshRenderer>().enabled = true;
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

    bool IsWall()
    {
        Collider wallCollider = GetComponent<BoxCollider>();

        Vector3 worldCenter = wallCollider.transform.TransformPoint(wallCollider.bounds.center);
        Vector3 worldHalfExtents = Vector3.Scale(wallCollider.bounds.size, wallCollider.transform.lossyScale) * 0.5f;
        Collider[] colliders = Physics.OverlapBox(worldCenter, worldHalfExtents, wallCollider.transform.rotation);
        //Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Wall"))
            {
                return true;
            }
        }

        return false;
    }

}
