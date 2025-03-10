using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Stone : MonoBehaviour
{
    Vector3 stoneOriPos;
    bool isCenter = false;
    public AudioClip stoneSoundClip;
    public AudioClip stoneBreakSoundClip;
    bool isStone = true;
    AudioSource audioSource;

    void Start()
    {
        stoneOriPos = transform.position;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (transform.position.y < -7.5f)
        {
            Destroy(gameObject);
        }

        //if (!GameManager.inputEnabled)
        //    GetComponent<Rigidbody>().isKinematic = true;
        //else
        //    GetComponent<Rigidbody>().isKinematic = false;


        if (Time.timeScale == 0)
            audioSource.Pause();
        else
            audioSource.UnPause();
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
                transform.position = new Vector3(stoneOriPos.x, transform.position.y, transform.position.z); // x좌표 유지
            }
        }
    }

    IEnumerator MoveToCenterWithDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); // 딜레이 만큼 대기
        transform.position = new Vector3(0.0f, transform.position.y, transform.position.z); // 중앙으로 이동
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            AudioSource playerAudio = collision.gameObject.GetComponent<AudioSource>();
            Rigidbody rb = collision.rigidbody;

            Debug.Log("돌충돌");
            player.HitMonster(1.0f);
            GameManager.instance.AddCurHp(-3);

            Vector3 direction = collision.transform.position - transform.position;
            direction.y = 0;

            // 2D 조건도 넣어줘야할듯? x축에 힘 0으로 해서
            if (CameraMove.mainCam.orthographic)
                direction.x = 0;

            direction.Normalize();

            playerAudio.PlayOneShot(stoneBreakSoundClip);

            rb.AddForce(direction * 8.0f, ForceMode.VelocityChange);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            if (stoneSoundClip != null && isStone)
            {
                audioSource.PlayOneShot(stoneSoundClip);
                isStone = false;
            }
        }

        if (collision.gameObject.CompareTag("Stone"))
        {
            audioSource.PlayOneShot(stoneBreakSoundClip);
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }


}
