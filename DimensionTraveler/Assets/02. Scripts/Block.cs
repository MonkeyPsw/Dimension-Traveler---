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
        // https://teraphonia.tistory.com/719 �ڽ��� �±�
        if (collision.collider.CompareTag("Head"))
        {
            if (!isCoin)
            {
                coinPos = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
                coin = Instantiate(coinPrefab, coinPos, Quaternion.identity);
                // ���� �ڵ� �������� ���, �ڵ������ϸ� ������Ʈ�� �ּ�ó��
                isCoin = true;
            }

            gameObject.GetComponentInParent<Renderer>().material = emptyMaterial;
            //gameObject.GetComponentInParent<Renderer>().material.SetTexture("_MainTex", emptyTexture);
        }
    }

}
