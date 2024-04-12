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
        // 블록을 주어진 높이만큼 이동
        transform.position += Vector3.up * height;

        // 원래 위치로 자연스럽게 내리기
        float elapsedTime = 0f;
        float duration = 0.25f; // 내리는 데 걸리는 시간
        Vector3 startPos = transform.position;
        Vector3 targetPos = oriPos;

        while (elapsedTime < duration)
        {
            // 내릴 위치를 시간에 따라 보간하여 결정
            float t = elapsedTime / duration;
            transform.position = Vector3.Lerp(startPos, targetPos, t);

            // 경과 시간 업데이트
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 원래 위치로 정확히 맞추기
        transform.position = oriPos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // https://teraphonia.tistory.com/719 자식의 태그
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
                        // 코인 자동 삭제할지 고민, 자동삭제하면 업데이트문 주석처리
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
