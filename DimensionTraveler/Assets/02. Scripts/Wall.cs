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
                //Debug.Log(IsWall());
                if (IsWall())
                {
                    Debug.Log("벽겹친다");
                    //gameObject.GetComponent<MeshRenderer>().enabled = false;
                }
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
