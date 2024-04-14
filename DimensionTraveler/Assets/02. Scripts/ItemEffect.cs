using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffect : MonoBehaviour
{
    public ItemType itemType;
    public GameObject ItemEffectPrefab;
    public AudioClip ItemSoundClip;

    public enum ItemType
    {
        Coin,
        Treasure,
        SmallHealKit,
        BigHealKit,
        Potion,
        Orb,
        Portal,

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayEffect();
            ItemFunction();
        }
    }

    private void ItemFunction()
    {
        switch (itemType)
        {
            case ItemType.Coin:
                GameManager.instance.AddScore(100);
                break;

            case ItemType.Treasure:
                GameManager.instance.AddScore(1000);
                break;

            case ItemType.SmallHealKit:
                GameManager.instance.AddCurHp(1);
                break;

            case ItemType.BigHealKit:
                GameManager.instance.AddCurHp(5);
                break;

            case ItemType.Potion:
                GameManager.instance.AddMaxHp(1);
                GameManager.instance.AddCurHp(1);
                break;

            case ItemType.Orb:
                PlayerMovement.isDimension = true;
                break;

            case ItemType.Portal:
                GameManager.inputEnabled = false;
                GameManager.level++;
                GameManager.LoadNextMap(GetComponent<Transform>().position);
                break;

            default:
                Debug.LogWarning("아이템오류");
                break;
        }

        Destroy(gameObject);
    }

    private void PlayEffect()
    {
        if (ItemEffectPrefab != null)
        {
            GameObject effect = Instantiate(ItemEffectPrefab, transform.position, ItemEffectPrefab.transform.rotation);
            Destroy(effect, 1.0f);
        }

        if (ItemSoundClip != null)
        {
            AudioSource.PlayClipAtPoint(ItemSoundClip, transform.position);
        }
    }
}
