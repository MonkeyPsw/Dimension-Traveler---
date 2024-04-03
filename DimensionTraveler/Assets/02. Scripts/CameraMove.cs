using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CameraMove : MonoBehaviour
{
    public static Camera mainCam;
    public Transform player;
    public Vector3 offset;
    public Vector3 cameraNewPosition;
    public static Animator camAnim;

    private Vector3 toPosition;
    private Quaternion toRotation;
    public Transform cam2DPos;
    public Transform cam3DPos;
    float camChangeSpeed = 3.0f;
    //�ڵ� ������ �ФФ�

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        cam2DPos = player.GetChild(0);
        cam3DPos = player.GetChild(1);
        mainCam = Camera.main;
        transform.position = SetPosition();
        transform.rotation = SetRotation();
    }

    void Update()
    {
        
    }

    // ī�޶� �̵��� ���� ���߿� �ؼ� ����Ծ���
    void LateUpdate()
    {
        //if (mainCam.orthographic) // 2D�϶�
        //{
        //    //transform.position = new Vector3(10.0f, -1.0f, 0f);
        //    //transform.rotation = Quaternion.Euler(0f, -90.0f, 0f); 
        //    setCamera2DPos();

        //    // ���� ��ȯ ����Ʈ ����
        //    // ī�޶� 3D �������� �̵�
        //}
        //else // 3D�϶�
        //{
        //    //transform.position = new Vector3(0f, -1.0f, -4.0f);
        //    //transform.rotation = Quaternion.Euler(20.0f, 0f, 0f);
        //    setCamera3DPos();

        //    // ���� ��ȯ ����Ʈ ����
        //    // ī�޶� 2D �������� �̵�
        //}

        toPosition = SetPosition();
        toRotation = SetRotation();
        transform.position = Vector3.Lerp(transform.position, toPosition, camChangeSpeed * 3.0f * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, camChangeSpeed * Time.deltaTime);
    }

    public static void ChangeDimension()
    {
        mainCam.orthographic = !mainCam.orthographic; // ���
        
    }

    public void SetCamera2DPos()
    {
        offset = new Vector3(10.0f, 0f, 0f);

        cameraNewPosition = new Vector3(player.position.x + offset.x,
                                            -1.0f,
                                            player.position.z);
        transform.position = cameraNewPosition;
    }

    public void SetCamera3DPos()
    {
        offset = new Vector3(0f, 0f, -4.0f);

        cameraNewPosition = new Vector3(player.position.x,
                                            -1.0f,
                                            player.position.z + offset.z);

        transform.position = cameraNewPosition;
    }

    Vector3 SetPosition()
    {
        //return mainCam.orthographic ? cam2DPos.position : cam3DPos.position;
        Vector3 position = mainCam.orthographic ? cam2DPos.position : cam3DPos.position;
        if (mainCam.orthographic)
            position.y = -1.0f;
        return position;
    }

    Quaternion SetRotation()
    {
        return mainCam.orthographic ? cam2DPos.rotation : cam3DPos.rotation;
    }

}
