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
                // Player�� ����� Cube�� �Ʒ����� ��Ҵ��� Ȯ��
                if (contact.normal.y > 0.9f) // ������ ���� ���ʹ� (0, 1, 0)�� �������ϴ�.
                {
                    // Cube�� ���ְų� ��Ȱ��ȭ�ϰų� ���ϴ� ���� ����
                    Destroy(gameObject);
                    break; // �浹�� ���� �� �ϳ��� Ȯ�εǸ� �ݺ����� �����մϴ�.
                    // ���� CHATGPT
                }
            }

            //Destroy(transform.parent.gameObject);
        }
    }

}
