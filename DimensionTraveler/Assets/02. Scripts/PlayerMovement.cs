using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 6.0f;
    Vector3 movement;
    Rigidbody rb;

    public float jumpForce = 15.0f; // 점프 힘
    public static bool isGrounded; // 땅에 닿았는지 여부
    
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
        
        // 연속으로 차원 전환하는게 좀 이상한데 몰루
        if (CameraMove.mainCam.orthographic) // 3D에서 2D로 갈때
        {
            Debug.Log("if문 3D에서 2D로");

            isWallCenter = false;

            // 2초뒤에 x좌표를 0으로 옮겨서 가운데에서만 이동하게끔
            if (is2D)
                StartCoroutine(MoveToCenterWithDelay(2.0f));

            // Orthographic 2D일 때는 좌우키로 z축 이동
            movement = new Vector3(0f, 0f, moveHorizontal).normalized;
        }
        else // 2D에서 3D로 갈때
        {
            Debug.Log("else문 2D에서 3D로");

            is2D = true;

            if (!isWallCenter)
                MoveToWallCenter();

            // Perspective 3D일 때는 상하키로 z축, 좌우키로 x방향 이동
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
        Debug.Log("3D에서 2D로");
        yield return new WaitForSecondsRealtime(delay); // 딜레이 만큼 대기
        transform.position = new Vector3(0.0f, transform.position.y, transform.position.z); // 중앙으로 이동
        is2D = false;
    }

    void MoveToWallCenter()
    {
        Debug.Log("2D에서 3D로");
        Debug.Log("가야할 x좌표 " + wallPos.x);
        
        targetPos = new Vector3(wallPos.x, transform.position.y, transform.position.z); // Wall 중앙으로 이동
        
        Debug.Log("타겟 " + targetPos);

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
                // 2개 이상의 Wall에 닿을때 문제가 있긴함
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
