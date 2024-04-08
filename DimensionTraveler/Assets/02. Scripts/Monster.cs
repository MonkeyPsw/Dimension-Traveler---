using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Monster : MonoBehaviour
{
    Transform player;
    Vector3 monsterOriPos;
    Vector3 directionToPlayer;
    Vector3 monsterDirection;
    bool isCenter = false;
    public float moveSpeed = 2.0f;
    float verticalRange = 3.5f;
    public int atk = 1;
    public int score = 100;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        directionToPlayer = player.position - transform.position;
        monsterDirection = directionToPlayer.normalized;

        if (IsWall())
            return;

        if (directionToPlayer.magnitude < 5.0f && Mathf.Abs(directionToPlayer.y) <= verticalRange && GameManager.inputEnabled)
        {
            monsterDirection.y = 0;
            transform.Translate(moveSpeed * Time.deltaTime * monsterDirection);
        }


    }

    void LateUpdate()
    {

        if (CameraMove.mainCam.orthographic)
        {
            if (!isCenter)
            {
                isCenter = true;
                monsterOriPos = transform.position;
                StartCoroutine(MoveToCenterWithDelay(2.0f));
            }
        }
        else
        {
            if (isCenter)
            {
                isCenter = false;
                //monsterOriPos = transform.position; // �̰� �� �����
                transform.position = new Vector3(monsterOriPos.x, transform.position.y, transform.position.z); // x��ǥ ����
            }
        }

    }

    IEnumerator MoveToCenterWithDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); // ������ ��ŭ ���
        transform.position = new Vector3(0.0f, transform.position.y, transform.position.z); // �߾����� �̵�
    }

    bool IsWall()
    {
        Collider monsterCollider = GetComponent<BoxCollider>();

        // ������ �ݶ��̴��� ��ġ�� ��� �浹ü�� ��������
        //Collider[] colliders = Physics.OverlapBox(monsterCollider.bounds.center, monsterCollider.bounds.size / 2, Quaternion.identity);

        Vector3 worldCenter = monsterCollider.transform.TransformPoint(monsterCollider.bounds.center);
        Vector3 worldHalfExtents = Vector3.Scale(monsterCollider.bounds.size, monsterCollider.transform.lossyScale) * 0.5f;
        Collider[] colliders = Physics.OverlapBox(worldCenter, worldHalfExtents, monsterCollider.transform.rotation);

        // ������ �ݶ��̴��� ���� ��ġ���� Ȯ��
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Wall"))
            {
                return true;
            }
        }

        // ���� ��ġ�� ������ false ��ȯ
        return false;
    }
}
