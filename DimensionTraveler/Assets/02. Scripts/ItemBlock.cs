using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBlock : MonoBehaviour
{
    public Material emptyMaterial;
    public AudioClip ItemSoundClip;
    public AudioClip EmptySoundClip;
    public GameObject itemPrefab;
    public GameObject markPrefab;

    GameObject item;
    Vector3 itemPos;
    bool isItem = false;

    Vector3 oriPos;

    void Start()
    {
        oriPos = transform.position;
    }

    void Update()
    {
        if (item != null)
        {
            if (CameraMove.mainCam.orthographic)
            {
                item.transform.position = new Vector3(0, itemPos.y, itemPos.z);
            }
            else
            {
                item.transform.position = itemPos;
            }
        }
    }

    IEnumerator MoveBlock(float height)
    {
        // 블록을 주어진 높이만큼 이동
        transform.position += Vector3.up * height;

        // 원래 위치로 자연스럽게 내리기
        float elapsedTime = 0f;
        float duration = 0.25f; // 내리는 데 걸리는 시간
        Vector3 startPos = transform.position;
        Vector3 targetPos = oriPos;

        while (elapsedTime < duration)
        {
            // 내릴 위치를 시간에 따라 보간하여 결정
            float t = elapsedTime / duration;
            transform.position = Vector3.Lerp(startPos, targetPos, t);

            // 경과 시간 업데이트
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 원래 위치로 정확히 맞추기
        transform.position = oriPos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AudioSource source = collision.gameObject.GetComponent<AudioSource>();
            
            foreach (ContactPoint contact in collision.contacts)
            {
                if (contact.normal.y > 0.9f)
                {
                    StartCoroutine(MoveBlock(0.1f));

                    if (!isItem)
                    {
                        itemPos = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
                        item = Instantiate(itemPrefab, itemPos, Quaternion.identity);
                        if (ItemSoundClip != null)
                            source.PlayOneShot(ItemSoundClip);
                        isItem = true;

                        gameObject.GetComponentInChildren<Renderer>().material = emptyMaterial;
                        markPrefab.SetActive(false);
                        break;
                    }
                    else
                    {
                        if (EmptySoundClip != null)
                            source.PlayOneShot(EmptySoundClip);
                        break;
                    }
                }
            }
            
            //collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.down * 5.0f;
        }
    }
}
