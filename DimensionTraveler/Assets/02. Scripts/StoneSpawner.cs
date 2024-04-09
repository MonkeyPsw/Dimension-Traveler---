using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneSpawner : MonoBehaviour
{
    public GameObject stonePrefab; // Stone ������
    public Transform[] spawnPoints; // Stone�� ������ ��ġ �迭
    public float spawnInterval = 3.0f; // Stone ���� ����

    void Start()
    {
        // ������ �������� Stone ���� �Լ� ȣ��
        InvokeRepeating("SpawnStone", 0.0f, spawnInterval);
    }

    void SpawnStone()
    {
        // ������ ��ġ �ε��� ����
        int randomIndex = Random.Range(0, spawnPoints.Length);
        // ���õ� ��ġ�� Stone ����
        Instantiate(stonePrefab, spawnPoints[randomIndex].position, Quaternion.identity);
    }
}
