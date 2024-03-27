using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Material emptyMaterial;
    //public Texture emptyTexture;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        // https://teraphonia.tistory.com/719 자식의 태그
        if (collision.collider.CompareTag("Head"))
        {
            gameObject.GetComponentInParent<Renderer>().material = emptyMaterial;
            //gameObject.GetComponentInParent<Renderer>().material.SetTexture("_MainTex", emptyTexture);
        }
    }

}
