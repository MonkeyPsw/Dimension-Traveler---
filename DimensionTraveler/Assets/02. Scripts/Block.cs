using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Block : MonoBehaviour
{
    public GameObject destroyEffect;

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                // Player의 윗면과 Cube의 아랫면이 닿았는지 확인
                if (contact.normal.y > 0.9f) // 윗면의 법선 벡터는 (0, 1, 0)에 가깝습니다.
                {
                    // Cube를 없애거나 비활성화하거나 원하는 동작 수행
                    Destroy(gameObject);
                    break; // 충돌한 지점 중 하나가 확인되면 반복문을 종료합니다.
                    // 고맙다 CHATGPT
                }
            }

            //Destroy(transform.parent.gameObject);
        }
    }

}
