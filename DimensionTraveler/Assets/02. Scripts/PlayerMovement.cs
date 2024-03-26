using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 6.0f;
    Vector3 movement;
    Rigidbody rb;

    public float jumpForce = 15.0f; // ���� ��
    public static bool isGrounded; // ���� ��Ҵ��� ����
    
    public int hp = 10;

    bool is2D = false;
    public Vector3 wallPos;
    public Vector3 targetPos;
    bool isWallCenter = false;

    //public static PlayerMovement instance;

    //private void Awake()
    //{
    //    if (instance == null)
    //        instance = this;
    //    else
    //        Destroy(gameObject);
    //}

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        
        // �������� ���� ��ȯ�ϴ°� �� �̻��ѵ� ����
        if (CameraMove.mainCam.orthographic) // 3D���� 2D�� ����
        {
            Debug.Log("if�� 3D���� 2D��");

            isWallCenter = false;

            // 2�ʵڿ� x��ǥ�� 0���� �Űܼ� ��������� �̵��ϰԲ�
            if (is2D)
                StartCoroutine(MoveToCenterWithDelay(2.0f));

            // Orthographic 2D�� ���� �¿�Ű�� z�� �̵�
            movement = new Vector3(0f, 0f, moveHorizontal).normalized;
        }
        else // 2D���� 3D�� ����
        {
            Debug.Log("else�� 2D���� 3D��");

            is2D = true;

            if (!isWallCenter)
                MoveToWallCenter();

            // Perspective 3D�� ���� ����Ű�� z��, �¿�Ű�� x���� �̵�
            movement = new Vector3(moveHorizontal, 0f, moveVertical).normalized;
        }

        if (GameManager.inputEnabled)
        {
            transform.Translate(movement * moveSpeed * Time.deltaTime);
            //rb.velocity = movement * moveSpeed;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && GameManager.inputEnabled)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        


    }

    IEnumerator MoveToCenterWithDelay(float delay)
    {
        Debug.Log("3D���� 2D��");
        yield return new WaitForSecondsRealtime(delay); // ������ ��ŭ ���
        transform.position = new Vector3(0.0f, transform.position.y, transform.position.z); // �߾����� �̵�
        is2D = false;
    }

    void MoveToWallCenter()
    {
        Debug.Log("2D���� 3D��");
        Debug.Log("������ x��ǥ " + wallPos.x);
        
        targetPos = new Vector3(wallPos.x, transform.position.y, transform.position.z); // Wall �߾����� �̵�
        
        Debug.Log("Ÿ�� " + targetPos);

        transform.position = targetPos;

        isWallCenter = true;
    }

    public void Jump()
    {
        //transform.Translate(Vector3.up * jumpForce * Time.deltaTime);
        //rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        rb.velocity = Vector3.up * jumpForce;
        //rb.velocity = new Vector3(0f, jumpForce, 0f);
        isGrounded = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall"))
        {
            isGrounded = true;
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall"))
        {
            isGrounded = true;

            if (collision.gameObject.CompareTag("Wall"))
            {
                // 2�� �̻��� Wall�� ������ ������ �ֱ���
                wallPos = collision.gameObject.GetComponent<Wall>().GetWallOriPos();
            }
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall"))
        {
            isGrounded = false;
        }
    }


}
