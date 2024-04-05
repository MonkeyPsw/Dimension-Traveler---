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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2);

            // �浹�� �� �߿��� �ϳ��� �����մϴ�.
            GameObject selectedWall = null;
            foreach (var collider in colliders)
            {
                if (collider.gameObject != gameObject)
                {
                    selectedWall = collider.gameObject;
                    break;
                }
            }

            // ���õ� �� �̿��� ��� ���� Mesh Renderer�� ��Ȱ��ȭ�մϴ�.
            foreach (var collider in colliders)
            {
                if (collider.gameObject != selectedWall)
                {
                    MeshRenderer renderer = collider.gameObject.GetComponent<MeshRenderer>();
                    if (renderer != null)
                        renderer.enabled = false;
                }
            }
        }
    }


}
