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

            // 2D ���ǵ� �־�����ҵ�? x�࿡ �� 0���� �ؼ�
            if (CameraMove.mainCam.orthographic)
                direction.x = 0;

            direction.Normalize();

            //AudioSource.PlayClipAtPoint(stoneBreakSoundClip, transform.position);
            audioSource.clip = stoneBreakSoundClip;
            audioSource.volume = 1.0f;
            audioSource.Play();

            rb.AddForce(direction * 8.0f, ForceMode.VelocityChange);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            if (stoneSoundClip != null && isStone)
            {
                //AudioSource.PlayClipAtPoint(stoneSoundClip, transform.position);
                audioSource.clip = stoneSoundClip;
                audioSource.volume = 0.5f;
                audioSource.Play();
                if (Time.timeScale == 0)
                    audioSource.Stop();
                isStone = false;
            }
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }


}
