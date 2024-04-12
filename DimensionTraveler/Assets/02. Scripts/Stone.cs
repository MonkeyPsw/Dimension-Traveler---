using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    Vector3 stoneOriPos;
    bool isCenter = false;

    void Start()
    {
        stoneOriPos = transform.position;
    }

    void LateUpdate()
    {
        if (CameraMove.mainCam.orthographic)
        {
            if (!isCenter)
            {
                isCenter = true;
                stoneOriPos = transform.position;
                StartCoroutine(MoveToCenterWithDelay(2.0f));
            }
        }
        else
        {
            if (isCenter)
            {
                isCenter = false;
                transform.position = new Vector3(stoneOriPos.x, transform.position.y, transform.position.z); // x��ǥ ����
            }
        }
    }

    IEnumerator MoveToCenterWithDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); // ������ ��ŭ ���
        transform.position = new Vector3(0.0f, transform.position.y, transform.position.z); // �߾����� �̵�
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            Rigidbody rb = collision.rigidbody;

            Debug.Log("���浹");
            player.HitMonster(1.0f);
            GameManager.instance.AddCurHp(-3);

            Vector3 direction = collision.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();
            rb.AddForce(direction * 8.0f, ForceMode.VelocityChange);

            // if (CameraMove.mainCam.orthographic) ���ǵ� �־�����ҵ�? x�࿡ �� 0���� �ؼ�

            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }


}
