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
            // 플레이어를 향하는 방향 벡터 계산
            Vector3 direction = (player.position - transform.position).normalized;

            // 플레이어를 향하는 방향으로 회전
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y, 0f); // Y축 방향만 고려하여 회전
        }
    }
}
