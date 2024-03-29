using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Block : MonoBehaviour
{
    public Material emptyMaterial;
    //public Texture emptyTexture;
    public GameObject coinPrefab;

    GameObject coin;
    Vector3 coinPos;
    bool isCoin = false;

    void Start()
    {

    }

    void Update()
    {
        if (coin != null)
        {
            if (CameraMove.mainCam.orthographic)
            {
                coin.transform.position = new Vector3(0, coinPos.y, coinPos.z);
            }
            else
            {
                coin.transform.position = coinPos;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // https://teraphonia.tistory.com/719 자식의 태그
        if (collision.collider.CompareTag("Head"))
        {
            if (!isCoin)
            {
                coinPos = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
                coin = Instantiate(coinPrefab, coinPos, Quaternion.identity);
                // 코인 자동 삭제할지 고민, 자동삭제하면 업데이트문 주석처리
                isCoin = true;
            }

            gameObject.GetComponentInParent<Renderer>().material = emptyMaterial;
            //gameObject.GetComponentInParent<Renderer>().material.SetTexture("_MainTex", emptyTexture);
        }
    }

}
