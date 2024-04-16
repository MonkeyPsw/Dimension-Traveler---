using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    //public BlockType blockType;
    public AudioClip BlockBreakSoundClip;
    public GameObject destroyEffect;

    // �̷��� ��ũ��Ʈ �ϳ����� ������� ��������
    //public enum BlockType
    //{
    //    NormalBlock,
    //    CoinBlock,
    //    ItemBlock,

    //}

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
            AudioSource source = collision.gameObject.GetComponent<AudioSource>();

            foreach (ContactPoint contact in collision.contacts)
            {
                // Player�� ����� Cube�� �Ʒ����� ��Ҵ��� Ȯ��
                if (contact.normal.y > 0.9f) // ������ ���� ���ʹ� (0, 1, 0)�� �������ϴ�.
                {
                    // Cube�� ���ְų� ��Ȱ��ȭ�ϰų� ���ϴ� ���� ����

                    NormalBlockBreak(source);

                    break; // �浹�� ���� �� �ϳ��� Ȯ�εǸ� �ݺ����� �����մϴ�.
                    // ���� CHATGPT
                }
            }

            //Destroy(transform.parent.gameObject);
        }
    }

    void NormalBlockBreak(AudioSource source)
    {
        Destroy(gameObject);

        if (BlockBreakSoundClip != null)
        {
            source.PlayOneShot(BlockBreakSoundClip);
        }

        GameObject effect = Instantiate(destroyEffect, transform.position, Quaternion.identity);
        Destroy(effect, 0.5f);

    }
}
