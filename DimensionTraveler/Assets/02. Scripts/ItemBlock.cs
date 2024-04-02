using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBlock : MonoBehaviour
{
    public Material emptyMaterial;
    public GameObject itemPrefab;
    
    GameObject item;
    Vector3 itemPos;
    bool isItem = false;

    void Start()
    {
        
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Head"))
        {
            if (!isItem)
            {
                itemPos = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
                item = Instantiate(itemPrefab, itemPos, Quaternion.identity);
                isItem = true;
            }

            gameObject.GetComponentInParent<Renderer>().material = emptyMaterial;
            collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.down * 5.0f;
        }
    }
}
