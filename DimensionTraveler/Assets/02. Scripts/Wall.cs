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
                //if (gameObject.GetComponent<MeshRenderer>())
                //    gameObject.GetComponent<MeshRenderer>().enabled = true;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2);

            // 충돌한 벽 중에서 하나만 선택합니다.
            GameObject selectedWall = null;
            foreach (var collider in colliders)
            {
                if (collider.gameObject != gameObject)
                {
                    selectedWall = collider.gameObject;
                    break;
                }
            }

            // 선택된 벽 이외의 모든 벽의 Mesh Renderer를 비활성화합니다.
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
