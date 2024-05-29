using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;

    [Header("�÷��̾� �̵�")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector2 movementInputValue;

    [Header("�÷��̾� ȸ��")]
    [SerializeField] private float minXLook;
    [SerializeField] private float maxXLook;
    [SerializeField] private float camCurXRot;
    [SerializeField] private float lookSensitivity;
    [SerializeField] private Vector2 mouseDelta;
    [SerializeField] private Transform cameraContainer;

    [Header("�÷��̾� ����")]
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayerMask;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // ���ӿ��� ���콺 Ŀ���� ������ �ʵ��� ���.
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        CameraLook();
    }

    #region InputSystem - Move 
    public void SetMovementInputValue(InputAction.CallbackContext context)
    {
        // Ű�� ��� ������ �����鼭 ������ ������ �� �����Ƿ� Started�� �ƴ� Performed ���
        if (context.phase == InputActionPhase.Performed)
        {
            movementInputValue = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            movementInputValue = Vector2.zero;
        }
    }

    private void Move()
    {
        // Vector2 ������ inputValue�� ����,�¿� �̵��� �ƴ� �յ�, �¿� �̵��� �ϱ� ���� Vector3 �����ͷ� ����
        // transform.forward �� transform.right �� ����ϴ� ���� : Player ������Ʈ�� ȸ���� * �Է°����� �̵��ϱ� ���ؼ� ( return rotation * Vector3.forward; )
        // Vector3.forward�� �ƴ϶� transform.forward�� Ʈ�������� ȸ������ ���Ǿ� �ִ� ���� �չ���
        Vector3 movementDirection = transform.forward * movementInputValue.y + transform.right * movementInputValue.x;

        movementDirection *= moveSpeed;

        // Jump ���� ������ ���� ���� rigidbody�� y���� ����
        movementDirection.y = rigidbody.velocity.y;

        rigidbody.velocity = movementDirection;
        //rigidbody.MovePosition(rigidbody.position + movementDirection);

    }
    #endregion
    #region InputSystem - Look
    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();

    }

    private void CameraLook()
    {
        // ���콺 ��ġy���� ī�޶� �����̼�x�� �ִ´�. �� -85 ~ 85 ���� ( ���� 90���� �Ѿ� �ڷ� �Ѿ�� �ʰ� ) 
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        // ���콺 ��ġ x ��ŭ �÷��̾ ȸ����Ų��.
        this.transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);

    }
    #endregion
    #region InputSystem - Jump
    public void InputJump(InputAction.CallbackContext context)
    {
        // ����Ű�� ������, �ٴڿ� �ִٸ�
        if ( context.phase == InputActionPhase.Started && GroundedCheck())
        {
            rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }

    private bool GroundedCheck()
    {
        // �÷��̾ �������� �յ��¿쿡 �Ʒ� �������� Ž���ϴ� ���̸� ����
        Ray[] rays = new Ray[4]
        {
            new Ray ( transform.position + (transform.forward * 0.2f) + (transform.up * 0.1f), Vector3.down ),
            new Ray ( transform.position + (-transform.forward * 0.2f) + (transform.up * 0.1f), Vector3.down ),
            new Ray ( transform.position + (transform.right * 0.2f) + (transform.up * 0.1f), Vector3.down ),
            new Ray ( transform.position + (-transform.right * 0.2f) + (transform.up * 0.1f), Vector3.down ),
        };

        // ���̸� 0.1 ��ŭ ��Ƽ� �ϳ��� �ٴ��� Ž���ϰ� �Ǹ� true 
        for ( int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.5f , groundLayerMask))
            {
                Debug.Log("�ٴ���");
                return true;
            }
        }

        // ��� ���̿� �ٴ��� Ž������ ���ϸ� false
        Debug.Log("�ٴ���x");
        return false;
    }

    private void OnDrawGizmos()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray ( transform.position + (transform.forward * 0.2f) + (transform.up * 0.1f), Vector3.down),
            new Ray ( transform.position + (-transform.forward * 0.2f) + (transform.up * 0.1f), Vector3.down),
            new Ray ( transform.position + (transform.right * 0.2f) + (transform.up * 0.1f), Vector3.down),
            new Ray ( transform.position + (-transform.right * 0.2f) + (transform.up * 0.1f), Vector3.down),
        };

        Gizmos.color = Color.red;

        for (int i = 0; i < rays.Length; i++)
        {
            Gizmos.DrawRay(rays[i]);
        }        
    }

    #endregion


}
