using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneSpawner : MonoBehaviour
{
    public GameObject stonePrefab; // Stone 프리팹
    public Transform[] spawnPoints; // Stone을 생성할 위치 배열
    public float spawnInterval = 3.0f; // Stone 생성 간격

    void Start()
    {
        // 일정한 간격으로 Stone 생성 함수 호출
        InvokeRepeating("SpawnStone", 0.0f, spawnInterval);
    }

    void SpawnStone()
    {
        // 랜덤한 위치 인덱스 선택
        int randomIndex = Random.Range(0, spawnPoints.Length);
        // 선택된 위치에 Stone 생성
        Instantiate(stonePrefab, spawnPoints[randomIndex].position, Quaternion.identity);
    }
}
