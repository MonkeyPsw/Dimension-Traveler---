using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player != null)
        {
            // �÷��̾ ���ϴ� ���� ���� ���
            Vector3 direction = (player.position - transform.position).normalized;

            // �÷��̾ ���ϴ� �������� ȸ��
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y, 0f); // Y�� ���⸸ ����Ͽ� ȸ��
        }
    }
}
