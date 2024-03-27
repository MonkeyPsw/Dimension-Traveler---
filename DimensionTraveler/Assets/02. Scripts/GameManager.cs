using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool inputEnabled = true;
    float inputDelay = 2.0f; // 입력 딜레이 시간

    public Collider[] colliders;

    void Start()
    {
        
    }

    void Update()
    {
        if (inputEnabled)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && PlayerMovement.isGrounded)
            {
                Debug.Log("차원 전환");
                SetDimension();
                //CheckCollisionAndSetDimension();
                //Time.timeScale = 0.3f;
            }
        }

    }

    public void SetDimension()
    {
        CameraMove.ChangeDimension();
        PlayerMovement.isChange = true;
        inputEnabled = false; // 입력 비활성화
        StartCoroutine(EnableInputAfterDelay(inputDelay)); // 입력 딜레이 후에 다시 활성화
    }

    //void CheckCollisionAndSetDimension()
    //{
    //    colliders =
    //        Physics.OverlapSphere(PlayerMovement.instance.transform.position, 1.0f);

    //    foreach (var collider in colliders)
    //    {
    //        if (collider.CompareTag("Wall"))
    //        {
    //            SetDimension();
    //            return;
    //        }
    //    }

    //    SetDimension();
    //}

    IEnumerator EnableInputAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); // 입력 딜레이 시간만큼 대기
        inputEnabled = true; // 입력 활성화
        //Time.timeScale = 1.0f;
    }

}
