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
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponentInParent<Renderer>().material = emptyMaterial;
            //gameObject.GetComponentInParent<Renderer>().material.SetTexture("_MainTex", emptyTexture);
        }
    }


}
