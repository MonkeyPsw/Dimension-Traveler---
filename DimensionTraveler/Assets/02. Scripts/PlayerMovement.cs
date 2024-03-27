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
    Coroutine reduceHp;
    bool isReduce = false;
    public float dimensionGauge = 10.0f;
    public static bool isChange = false;

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

        if (hp <= 0)
        {
            hp = 0;
            Debug.Log("��������");
            return;
        }

        // �������� ���� ��ȯ�ϴ°� �� �̻��ѵ� ����
        if (CameraMove.mainCam.orthographic) // 2D�϶�, 3D���� 2D�� ����
        {
            Debug.Log("if�� 3D���� 2D��");

            if (isChange)
                StartCoroutine(DimensionGaugeChange(2.0f));

            isWallCenter = false;

            // 2�ʵڿ� x��ǥ�� 0���� �Űܼ� ��������� �̵��ϰԲ�
            if (is2D)
                StartCoroutine(MoveToCenterWithDelay(2.0f));

            // Orthographic 2D�� ���� �¿�Ű�� z�� �̵�
            movement = new Vector3(0f, 0f, moveHorizontal).normalized;
        }
        else // 3D�϶�, 2D���� 3D�� ����
        {
            Debug.Log("else�� 2D���� 3D��");

            if (isChange)
                StartCoroutine(DimensionGaugeChange(2.0f));

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
            Jump(jumpForce);
        }

        if (isReduce)
        {
            reduceHp = StartCoroutine(ReduceHp(1.0f));
        }

        if (CameraMove.mainCam.orthographic && reduceHp != null)
        {
            Debug.Log("HP���Ҹ���");
            StopCoroutine(reduceHp);
            reduceHp = null;
        }

    }

    void FixedUpdate()
    {

    }

    //IEnumerator DimensionGaugeChange(float delay)
    //{
    //    isChange = false;
    //    Debug.Log("���� ��ȯ ������ ��ȭ��");

    //    if (CameraMove.mainCam.orthographic)
    //    {
    //        yield return new WaitForSecondsRealtime(delay);
    //        if (dimensionGauge < 10.0f)
    //        {
    //            dimensionGauge += Time.fixedDeltaTime;
    //            //dimensionGauge = Mathf.Min(dimensionGauge, 10.0f);
    //        }
    //        if (dimensionGauge >= 10.0f)
    //            dimensionGauge = 10.0f;
    //    }
    //    else
    //    {
    //        yield return new WaitForSecondsRealtime(delay);
    //        if (dimensionGauge > 0)
    //        {
    //            dimensionGauge -= Time.fixedDeltaTime;
    //            //dimensionGauge = Mathf.Max(dimensionGauge, 0.0f);
    //        }
    //        if (dimensionGauge <= 0)
    //            dimensionGauge = 0;

    //    }
    //}

    IEnumerator DimensionGaugeChange(float delay)
    {
        isChange = false;
        Debug.Log("���� ��ȯ ������ ��ȭ��");

        yield return new WaitForSecondsRealtime(delay); // ������ ��ŭ ���

        if (CameraMove.mainCam.orthographic)
        {
            //if (reduceHp != null)
            //{
            //    StopCoroutine(reduceHp);
            //}

            while (true)
            {
                yield return null;
                dimensionGauge += Time.fixedDeltaTime;
                Debug.Log("���� ��ȯ ������ ������");

                if (!CameraMove.mainCam.orthographic)
                    break;

                if (dimensionGauge >= 10.0f)
                {
                    dimensionGauge = 10.0f;
                    break;
                }
            }
        }
        else
        {
            while (true)
            {
                yield return null;
                dimensionGauge -= Time.fixedDeltaTime;
                Debug.Log("���� ��ȯ ������ ������");

                if (CameraMove.mainCam.orthographic)
                    break;

                if (dimensionGauge <= 0)
                {
                    dimensionGauge = 0;
                    isReduce = true;
                    break;
                }
            }
        }

    }

    IEnumerator ReduceHp(float delay)
    {
        isReduce = false;
        while (true)
        {
            yield return new WaitForSecondsRealtime(delay);
            Debug.Log("HP ����");
            hp--;
        }
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

    public void Jump(float force)
    {
        //transform.Translate(Vector3.up * jumpForce * Time.deltaTime);
        //rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        rb.velocity = Vector3.up * force;
        //rb.velocity = new Vector3(0f, jumpForce, 0f);
        isGrounded = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall"))
        {
            isGrounded = true;
        }

        //if (collision.collider.CompareTag("MonsterHead"))
        //{
        //    Jump(jumpForce * 0.7f);
        //    Destroy(collision.transform.parent.gameObject);
        //}
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
        }

    }


}
