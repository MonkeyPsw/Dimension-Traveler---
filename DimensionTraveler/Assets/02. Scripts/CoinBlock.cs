using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBlock : MonoBehaviour
{
    public Material emptyMaterial;
    //public Texture emptyTexture;
    public GameObject coinPrefab;

    GameObject coin;
    Vector3 coinPos;
    bool isCoin = false;
    Vector3 oriPos;

    void Start()
    {
        oriPos = transform.position;
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

    IEnumerator MoveBlock(float height)
    {
        // ����� �־��� ���̸�ŭ �̵�
        transform.position += Vector3.up * height;

        // ���� ��ġ�� �ڿ������� ������
        float elapsedTime = 0f;
        float duration = 0.25f; // ������ �� �ɸ��� �ð�
        Vector3 startPos = transform.position;
        Vector3 targetPos = oriPos;

        while (elapsedTime < duration)
        {
            // ���� ��ġ�� �ð��� ���� �����Ͽ� ����
            float t = elapsedTime / duration;
            transform.position = Vector3.Lerp(startPos, targetPos, t);

            // ��� �ð� ������Ʈ
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ���� ��ġ�� ��Ȯ�� ���߱�
        transform.position = oriPos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // https://teraphonia.tistory.com/719 �ڽ��� �±�
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                if (contact.normal.y > 0.9f)
                {
                    StartCoroutine(MoveBlock(0.1f));

                    if (!isCoin)
                    {
                        coinPos = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
                        coin = Instantiate(coinPrefab, coinPos, Quaternion.identity);
                        // ���� �ڵ� �������� ���, �ڵ������ϸ� ������Ʈ�� �ּ�ó��
                        isCoin = true;
                        gameObject.GetComponentInChildren<Renderer>().material = emptyMaterial;
                        break;
                    }
                }
            }
            
            //gameObject.GetComponentInParent<Renderer>().material.SetTexture("_MainTex", emptyTexture);

            //Debug.Log(collision.gameObject.name);
            //collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.down * 5.0f;
        }
    }
}
