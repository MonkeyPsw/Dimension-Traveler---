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
    bool isWall = false;
    public float moveSpeed = 2.0f;
    float verticalRange = 3.5f;
    public int atk = 1;
    public int score = 100;
    bool isDead = false;
    public AudioClip monsterSoundClip;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        directionToPlayer = player.position - transform.position;
        monsterDirection = directionToPlayer.normalized;

        if (isWall)
            return;

        if (!isDead && directionToPlayer.magnitude < 5.0f && Mathf.Abs(directionToPlayer.y) <= verticalRange && GameManager.inputEnabled)
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
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            isWall = false;

            if (isCenter)
            {
                isCenter = false;
                //monsterOriPos = transform.position; // 이거 왜 썼었지
                transform.position = new Vector3(monsterOriPos.x, transform.position.y, transform.position.z); // x좌표 유지
            }
        }

    }

    IEnumerator MoveToCenterWithDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); // 딜레이 만큼 대기
        transform.position = new Vector3(0.0f, transform.position.y, transform.position.z); // 중앙으로 이동
    }

    //bool IsWall()
    //{
    //    Collider monsterCollider = GetComponent<BoxCollider>();

    //    // 몬스터의 콜라이더와 겹치는 모든 충돌체들 가져오기
    //    //Collider[] colliders = Physics.OverlapBox(monsterCollider.bounds.center, monsterCollider.bounds.size / 2, Quaternion.identity);

    //    Vector3 worldCenter = monsterCollider.transform.TransformPoint(monsterCollider.bounds.center);
    //    Vector3 worldHalfExtents = Vector3.Scale(monsterCollider.bounds.size, monsterCollider.transform.lossyScale) * 0.5f;
    //    Collider[] colliders = Physics.OverlapBox(worldCenter, worldHalfExtents, monsterCollider.transform.rotation);

    //    // 몬스터의 콜라이더가 벽과 겹치는지 확인
    //    foreach (Collider col in colliders)
    //    {
    //        if (col.CompareTag("Wall"))
    //        {
    //            Debug.Log("몬스터벽과겹침");
    //            return true;
    //        }
    //    }

    //    // 벽과 겹치지 않으면 false 반환
    //    return false;
    //}

    IEnumerator MonsterDead(float delay)
    {
        if (monsterSoundClip != null)
            AudioSource.PlayClipAtPoint(monsterSoundClip, transform.position);

        // 몬스터의 스케일을 줄입니다.
        float originalYScale = transform.GetChild(0).localScale.y;
        float targetYScale = originalYScale * 0.5f;
        float duration = 0.25f; // 변경에 걸리는 시간
        float elapsed = 0.0f;

        isDead = true;
        // 이게 맞아? 되긴되는데 몰루
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;

        while (elapsed < duration)
        {
            float newYScale = Mathf.Lerp(originalYScale, targetYScale, elapsed / duration);
            transform.GetChild(0).localScale =
                new Vector3(transform.GetChild(0).localScale.x, newYScale, transform.GetChild(0).localScale.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(delay);

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            isWall = true;
        }

        if (collision.gameObject.CompareTag("Player") && GameManager.inputEnabled)
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            Rigidbody rb = collision.rigidbody;

            if (collision.rigidbody != null)
            {
                foreach (ContactPoint contact in collision.contacts)
                {
                    // Player의 아랫면과 Monster의 윗면이 닿았는지 확인
                    if (contact.normal.y < -0.9f)
                    {
                        Debug.Log("몬스터컷");
                        rb.velocity = new Vector3(rb.velocity.x, player.jumpForce * 0.012f, rb.velocity.z);
                        StartCoroutine(MonsterDead(0.5f));
                        //Destroy(gameObject);
                        GameManager.instance.AddScore(score);
                        return;
                    }
                }

                Debug.Log("몬스터충돌");
                player.HitMonster(0.5f);
                //StartCoroutine(InputDelayAndToggleGod(0.5f));
                GameManager.instance.AddCurHp(-atk);

                Vector3 direction = collision.transform.position - transform.position;
                direction.y = 0;

                if (CameraMove.mainCam.orthographic)
                    direction.x = 0;

                direction.Normalize();

                //rb.velocity = direction * 5.0f;
                rb.AddForce(direction * 5.0f, ForceMode.VelocityChange);

            }
        }
    }


}
