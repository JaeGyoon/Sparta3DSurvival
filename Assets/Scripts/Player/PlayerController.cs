using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;

    [Header("플레이어 이동")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector2 movementInputValue;

    [Header("플레이어 회전")]
    [SerializeField] private float minXLook;
    [SerializeField] private float maxXLook;
    [SerializeField] private float camCurXRot;
    [SerializeField] private float lookSensitivity;
    [SerializeField] private Vector2 mouseDelta;
    [SerializeField] private Transform cameraContainer;

    [Header("플레이어 점프")]
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayerMask;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // 게임에서 마우스 커서가 보이지 않도록 잠금.
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
        // 키를 계속 누르고 있으면서 방향을 변경할 수 있으므로 Started가 아닌 Performed 사용
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
        // Vector2 데이터 inputValue로 상하,좌우 이동이 아닌 앞뒤, 좌우 이동을 하기 위해 Vector3 데이터로 변경
        // transform.forward 및 transform.right 를 사용하는 이유 : Player 오브젝트의 회전값 * 입력값으로 이동하기 위해서 ( return rotation * Vector3.forward; )
        // Vector3.forward가 아니라 transform.forward라 트랜스폼의 회전값도 계산되어 있는 로컬 앞방향
        Vector3 movementDirection = transform.forward * movementInputValue.y + transform.right * movementInputValue.x;

        movementDirection *= moveSpeed;

        // Jump 등의 로직을 위해 기존 rigidbody의 y값을 유지
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
        // 마우스 위치y값을 카메라 로테이션x에 넣는다. 단 -85 ~ 85 사이 ( 고개가 90도를 넘어 뒤로 넘어가지 않게 ) 
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        // 마우스 위치 x 만큼 플레이어를 회전시킨다.
        this.transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);

    }
    #endregion
    #region InputSystem - Jump
    public void InputJump(InputAction.CallbackContext context)
    {
        // 점프키를 눌렀고, 바닥에 있다면
        if ( context.phase == InputActionPhase.Started && GroundedCheck())
        {
            rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }

    private bool GroundedCheck()
    {
        // 플레이어를 기준으로 앞뒤좌우에 아래 방향으로 탐지하는 레이를 생성
        Ray[] rays = new Ray[4]
        {
            new Ray ( transform.position + (transform.forward * 0.2f) + (transform.up * 0.1f), Vector3.down ),
            new Ray ( transform.position + (-transform.forward * 0.2f) + (transform.up * 0.1f), Vector3.down ),
            new Ray ( transform.position + (transform.right * 0.2f) + (transform.up * 0.1f), Vector3.down ),
            new Ray ( transform.position + (-transform.right * 0.2f) + (transform.up * 0.1f), Vector3.down ),
        };

        // 레이를 0.1 만큼 쏘아서 하나라도 바닥을 탐지하게 되면 true 
        for ( int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.5f , groundLayerMask))
            {
                Debug.Log("바닥임");
                return true;
            }
        }

        // 모든 레이에 바닥을 탐지하지 못하면 false
        Debug.Log("바닥임x");
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
