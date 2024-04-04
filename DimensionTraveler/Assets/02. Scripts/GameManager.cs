using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool inputEnabled = true;
    public static int level = 2; // 0:������, 1:���θ޴�, 2:��1_1, ...
    float inputDelay = 2.0f; // �Է� ������ �ð�

    public Collider[] colliders;

    void Start()
    {
        
    }

    void Update()
    {
        if (inputEnabled && PlayerMovement.isDimension)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && PlayerMovement.isGrounded)
            {
                Debug.Log("���� ��ȯ");
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
        inputEnabled = false; // �Է� ��Ȱ��ȭ
        StartCoroutine(EnableInputAfterDelay(inputDelay)); // �Է� ������ �Ŀ� �ٽ� Ȱ��ȭ
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
        yield return new WaitForSecondsRealtime(delay); // �Է� ������ �ð���ŭ ���
        inputEnabled = true; // �Է� Ȱ��ȭ
        //Time.timeScale = 1.0f;
    }

    public static void LoadNextMap()
    {
        SceneManager.LoadScene(level);
    }
}
