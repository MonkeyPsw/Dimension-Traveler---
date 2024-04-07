using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 6.0f;
    Vector3 movement;
    Rigidbody rb;

    public float jumpForce = 8.0f; // ���� ��
    public static bool isGrounded; // ���� ��Ҵ��� ����
    public float jumpMaxY = 3.0f;
    public float jumpY = 0;
    public LayerMask jumpLayer;
    bool canJump = true;
    RaycastHit hit;
    Collider[] cols;

    public TextMeshProUGUI curHpText;
    public TextMeshProUGUI maxHpText;
    Coroutine reduceHp;
    bool isReduce = false;
    bool isGod = false;

    public static bool isDimension = false; // 1_1 ���� ��ȯ ������ ���� ����
    float maxDimensionGauge = 10.0f;
    public float curDimensionGauge = 10.0f;
    public float preDimensionGauge = 10.0f;
    public static bool isChange = false;

    //public GameObject dimensionGaugeParent;
    //public GameObject dimensionGaugePrefab;
    public Slider dimensionGaugeSlider;

    public TextMeshProUGUI scoreText;

    bool is2D = false;
    public Vector3 wallPos;
    public Vector3 targetPos;
    bool isWallCenter = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //if (GameManager.level > 2)
            isDimension = true;

        //for (int i = 0; i < (int)maxDimensionGauge; i++)
        //{
        //    GameObject gauge = Instantiate(dimensionGaugePrefab);
        //    gauge.transform.parent = dimensionGaugeParent.transform;
        //}

        curHpText = GameObject.Find("CurHP").GetComponent<TextMeshProUGUI>();
        maxHpText = GameObject.Find("MaxHP").GetComponent<TextMeshProUGUI>();
        dimensionGaugeSlider = GameObject.Find("DimensionGaugeSlider").GetComponent<Slider>();
        scoreText = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        CheckIsGrounded();

        if (GameManager.instance.GetCurHp() <= 0)
        {
            GameManager.instance.SetCurHp(0);
            Debug.Log("��������");
            //return;
        }
        if (GameManager.instance.GetCurHp() > GameManager.instance.GetMaxHp())
            GameManager.instance.SetCurHp(GameManager.instance.GetMaxHp());

        if (isGod)
            gameObject.layer = 8;
        else
            gameObject.layer = 7;

        if (transform.position.y < -10)
        {
            GameManager.instance.AddCurHp(-3);
            transform.position = new Vector3(0, -4.1f, 0);
        }


        // �������� ���� ��ȯ�ϴ°� �� �̻��ѵ� ����
        if (CameraMove.mainCam.orthographic) // 2D�϶�, 3D���� 2D�� ����
        {
            Debug.Log("if�� 3D���� 2D��");

            //if (CheckOverlap())
            //{
            //    Debug.Log("��ġ��ʵ�");
            //    // ���� �Ǳ�� �ٽ� ��ȯ�ϴ� ���
            //}

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
            //transform.Translate(movement * moveSpeed * Time.deltaTime);
            //rb.velocity = movement * moveSpeed;
            // Translate�� ���� Ʈ���� �浹�� 2���� �ǰ� velocity�� ���� �̵��� ����� ������ �ȵǳ�
            // https://rito15.github.io/posts/unity-fixed-update-and-stuttering/
            // ������ ���� ��������?
            // �׳� ������ó�� ������ �޴°� ������?
            // ���� �׳� �̵��ϴ°� ������.

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                jumpY = transform.position.y;
                Jump(jumpForce);
            }

            //if (transform.position.y > jumpMaxY + jumpY)
            //    rb.velocity = new Vector3(rb.velocity.x, -jumpForce / 2, rb.velocity.z);
        }

        if (isReduce)
        {
            //reduceHp = StartCoroutine(ReduceHp(1.0f));
            ReduceHpAndResetDimensionGauge();
        }

        //if (CameraMove.mainCam.orthographic && reduceHp != null)
        //{
        //    Debug.Log("HP���Ҹ���");
        //    StopCoroutine(reduceHp);
        //    reduceHp = null;
        //}

        curHpText.text = GameManager.instance.GetCurHp().ToString();
        maxHpText.text = GameManager.instance.GetMaxHp().ToString();
        dimensionGaugeSlider.value = curDimensionGauge;
        scoreText.text = "SCORE : " + GameManager.instance.GetScore();
    }

    void FixedUpdate()
    {
        if (GameManager.inputEnabled)
        {
            //Vector3 moveDir = new Vector3(movement.x, 0, movement.z);
            //movement.y = 0;
            //rb.velocity = movement * moveSpeed * Time.fixedDeltaTime;

            //rb.velocity = new Vector3(movement.x * moveSpeed,
            //                          rb.velocity.y,
            //                          movement.z * moveSpeed);

            rb.MovePosition(rb.position + new Vector3(movement.x * moveSpeed * Time.deltaTime,
                            rb.velocity.y * Time.deltaTime,
                            movement.z * moveSpeed * Time.deltaTime));

            //rb.AddForce(movement, ForceMode.VelocityChange);
            

        }
    }

    public void Jump(float force)
    {
        //transform.Translate(Vector3.up * jumpForce * Time.deltaTime);
        //transform.Translate(0, jumpForce * Time.deltaTime, 0);
        //if (transform.position.y > jumpMaxY + jumpY)
        //    transform.Translate(0, -jumpForce * Time.deltaTime, 0);

        //rb.AddForce(Vector3.up * force, ForceMode.Impulse);
        //rb.velocity = new Vector3(0f, jumpForce, 0f);
        //rb.velocity = new Vector3(rb.velocity.x, force, rb.velocity.z);
        //rb.velocity = Vector3.up * force;

        //rb.MovePosition(rb.position + Vector3.up * force * Time.deltaTime);
        if (canJump)
        {
            rb.AddForce(new Vector3(0, force, 0f));
            isGrounded = false;
            canJump = false;
            StartCoroutine(JumpCool(0.5f));
        }

    } 

    IEnumerator JumpCool(float cool)
    {
        yield return new WaitForSecondsRealtime(cool);
        canJump = true;
    }

    //IEnumerator DimensionGaugeChange(float delay)
    //{
    //    isChange = false;
    //    Debug.Log("���� ��ȯ ������ ��ȭ��");

    //    if (CameraMove.mainCam.orthographic)
    //    {
    //        yield return new WaitForSecondsRealtime(delay);
    //        if (curDimensionGauge < 10.0f)
    //        {
    //            curDimensionGauge += Time.fixedDeltaTime;
    //            //curDimensionGauge = Mathf.Min(curDimensionGauge, 10.0f);
    //        }
    //        if (curDimensionGauge >= 10.0f)
    //            curDimensionGauge = 10.0f;
    //    }
    //    else
    //    {
    //        yield return new WaitForSecondsRealtime(delay);
    //        if (curDimensionGauge > 0)
    //        {
    //            curDimensionGauge -= Time.fixedDeltaTime;
    //            //curDimensionGauge = Mathf.Max(curDimensionGauge, 0.0f);
    //        }
    //        if (curDimensionGauge <= 0)
    //            curDimensionGauge = 0;

    //    }
    //}

    IEnumerator DimensionGaugeChange(float delay)
    {
        isChange = false;
        Debug.Log("���� ��ȯ ������ ��ȭ��");

        yield return new WaitForSecondsRealtime(delay); // ������ ��ŭ ���

        DimensionGaugeCount();

        if (CameraMove.mainCam.orthographic)
        {
            while (true)
            {
                yield return null;
                curDimensionGauge += Time.deltaTime * 2;
                Debug.Log("���� ��ȯ ������ ������");

                if (!CameraMove.mainCam.orthographic)
                    break;

                if (curDimensionGauge >= maxDimensionGauge)
                {
                    curDimensionGauge = maxDimensionGauge;
                    break;
                }
            }
        }
        else
        {
            while (true)
            {
                yield return null;
                curDimensionGauge -= Time.deltaTime;
                Debug.Log("���� ��ȯ ������ ������");

                if (CameraMove.mainCam.orthographic)
                    break;

                if (curDimensionGauge <= 0)
                {
                    curDimensionGauge = 0;
                    isReduce = true;
                    //break;
                }
            }
        }

    }
    
    public void DimensionGaugeCount()
    {
        //if (curDimensionGauge != preDimensionGauge)
        //{
        //    if (curDimensionGauge < preDimensionGauge)
        //    {
        //        int countToRemove = (int)preDimensionGauge - (int)curDimensionGauge;
        //        for (int i = 0; i < countToRemove; i++)
        //        {
        //            if (dimensionGaugeParent.transform.childCount > 0)
        //            {
        //                GameObject lastChild = dimensionGaugeParent.transform.GetChild(dimensionGaugeParent.transform.childCount - 1).gameObject;
        //                Destroy(lastChild);
        //            }
        //        }
        //    }
        //    else if (curDimensionGauge > preDimensionGauge)
        //    {
        //        int countToAdd = (int)curDimensionGauge - (int)preDimensionGauge;
        //        for (int i = 0; i < countToAdd; i++)
        //        {
        //            GameObject gauge = Instantiate(dimensionGaugePrefab);
        //            gauge.transform.parent = dimensionGaugeParent.transform;
        //        }
        //    }

        //    preDimensionGauge = curDimensionGauge;
        //}
    }

    IEnumerator ReduceHp(float delay)
    {
        isReduce = false;
        while (true)
        {
            yield return new WaitForSecondsRealtime(delay);
            Debug.Log("HP ����");
            GameManager.instance.AddCurHp(-1);
        }
    }

    public void ReduceHpAndResetDimensionGauge()
    {
        GameManager.instance.AddCurHp(-1);
        curDimensionGauge = maxDimensionGauge / 2;
        isReduce = false;
    }

    IEnumerator MoveToCenterWithDelay(float delay)
    {
        Debug.Log("3D���� 2D��");
        is2D = false;
        yield return new WaitForSecondsRealtime(delay); // ������ ��ŭ ���
        transform.position = new Vector3(0.0f, transform.position.y, transform.position.z); // �߾����� �̵�
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

    IEnumerator InputDelayAndToggleGod(float delay)
    {
        GameManager.inputEnabled = false;
        yield return new WaitForSecondsRealtime(delay); // �Է� ������ �ð���ŭ ���
        GameManager.inputEnabled = true; // �Է� Ȱ��ȭ

        //��������
        ToggleGod();
        yield return new WaitForSecondsRealtime(delay * 2.0f);
        //����������
        ToggleGod();

        //Time.timeScale = 1.0f;
    }

    public void ToggleGod()
    {
        isGod = !isGod;

        // ���� �����̴� ȿ��?
    }

    private void CheckIsGrounded()
    {
        BoxCollider playerCollider = GetComponent<BoxCollider>();

        Vector3[] raycastOrigins = new Vector3[9];
        float offsetX = 0.24f;
        float offsetZ = 0.24f;
        bool isRay = false;

        // 3D ����
        raycastOrigins[0] = transform.position + new Vector3(offsetX, 0.0f, offsetZ); // �� ������ ������
        raycastOrigins[1] = transform.position + new Vector3(0.0f, 0.0f, offsetZ); // �� ��� ������
        raycastOrigins[2] = transform.position + new Vector3(-offsetX, 0.0f, offsetZ); // �� ���� ������

        raycastOrigins[3] = transform.position + new Vector3(offsetX, 0.0f, 0.0f); // ��� ������ ������
        raycastOrigins[4] = transform.position; // ��� ������
        raycastOrigins[5] = transform.position + new Vector3(-offsetX, 0.0f, 0.0f); // ��� ���� ������

        raycastOrigins[6] = transform.position + new Vector3(offsetX, 0.0f, -offsetZ); // �� ������ ������
        raycastOrigins[7] = transform.position + new Vector3(0.0f, 0.0f, -offsetZ); // �� ��� ������
        raycastOrigins[8] = transform.position + new Vector3(-offsetX, 0.0f, -offsetZ); // �� ���� ������

        float raycastDistance = playerCollider.size.y / 2f;

        for (int i = 0; i < raycastOrigins.Length; i++)
            Debug.DrawRay(raycastOrigins[i], Vector3.down * raycastDistance, Color.black);

        foreach (Vector3 origin in raycastOrigins)
        {
            if (Physics.Raycast(origin, Vector3.down, raycastDistance, jumpLayer))
            {
                isGrounded = true;
                isRay = true;
                break;
            }
        }

        if (!isRay)
            isGrounded = false;

        //Debug.DrawRay(transform.position + offset, Vector3.down * raycastDistance, Color.black);
        //Debug.DrawRay(transform.position - offset, Vector3.down * raycastDistance, Color.black);

        // �����ϰ� �����Ҷ� �𼭸��� �����ϸ� false�� ������ �׳� �𼭸��� �ٷ� ���� true
        //if (Physics.Raycast(transform.position, Vector3.down, raycastDistance, jumpLayer))
        //    isGrounded = true;
        //else
        //    isGrounded = false;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            //isGrounded = true;
            isWallCenter = true;
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            //isGrounded = true;
            // 2�� �̻��� Wall�� ������ ������ �ֱ���
            wallPos = collision.gameObject.GetComponent<Wall>().GetWallOriPos();
        }

        if (collision.gameObject.CompareTag("Block"))
        {
            //isGrounded = true;
            isWallCenter = true;
        }

        if (collision.gameObject.CompareTag("Monster") && GameManager.inputEnabled)
        {
            if (rb != null)
            {
                foreach (ContactPoint contact in collision.contacts)
                {
                    // Player�� �Ʒ���� Monster�� ������ ��Ҵ��� Ȯ��
                    if (contact.normal.y > 0.9f)
                    {
                        Debug.Log("������");
                        rb.velocity = new Vector3(rb.velocity.x, jumpForce * 0.012f, rb.velocity.z);
                        Destroy(collision.gameObject);
                        GameManager.instance.AddScore(collision.gameObject.GetComponent<Monster>().score);
                        return;
                    }
                }

                Debug.Log("�����浹");
                StartCoroutine(InputDelayAndToggleGod(0.5f));
                GameManager.instance.AddCurHp(-collision.gameObject.GetComponent<Monster>().atk);

                Vector3 direction = transform.position - collision.transform.position;
                direction.y = 0;
                direction.Normalize();
                //rb.velocity = direction * 5.0f;
                rb.AddForce(direction * 5.0f, ForceMode.VelocityChange);
            }
        }

    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall"))
    //    {
    //        isGrounded = true;

    //        if (collision.gameObject.CompareTag("Wall"))
    //        {
    //            // 2�� �̻��� Wall�� ������ ������ �ֱ���
    //            wallPos = collision.gameObject.GetComponent<Wall>().GetWallOriPos();
    //        }
    //    }

    //}

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall"))
        {
            //isGrounded = false;
            wallPos.x = 0;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            GameManager.instance.AddScore(100);
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Treasure"))
        {
            GameManager.instance.AddScore(1000);
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("SmallHealKit"))
        {
            GameManager.instance.AddCurHp(1);
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("BigHealKit"))
        {
            GameManager.instance.AddCurHp(5);
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Potion"))
        {
            GameManager.instance.AddMaxHp(1);
            GameManager.instance.AddCurHp(1);
            Destroy(other.transform.gameObject);
        }

        //if (other.gameObject.CompareTag("MonsterHead"))
        //{
        //    //rb.AddForce(new Vector3(0, jumpForce * 0.7f, 0f));
        //    rb.velocity = new Vector3(rb.velocity.x, jumpForce * 0.012f, rb.velocity.z);
        //    Destroy(other.transform.parent.gameObject);
        //    AddScore(100);
        //}

        if (other.gameObject.CompareTag("Orb"))
        {
            isDimension = true;
            Destroy(other.transform.gameObject);
        }

        if (other.gameObject.CompareTag("Portal"))
        {
            GameManager.level++;
            GameManager.LoadNextMap();
        }
    }


}
