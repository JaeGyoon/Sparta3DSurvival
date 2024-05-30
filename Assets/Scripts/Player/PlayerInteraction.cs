using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    // ������Ʈ �ֱ�
    public float checkRate = 0.05f;

    private float lastCheckTime;

    public float maxCheckDistance;
    public LayerMask layerMask;

    public GameObject lastInteractObject;
    private IInteractable interactable;

    public TextMeshProUGUI descriptionText;
    private Camera camera;

    private void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        if ( Time.time - lastCheckTime > checkRate )
        {            
            lastCheckTime = Time.time;


            // ��ũ�� x���� y���� �� => ȭ�� ���߾ӿ� ���� 
            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            // ���̿� ��ȣ�ۿ� ������ ����� ������ ������ ������ ������ �����
            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != lastInteractObject)
                {
                    lastInteractObject = hit.collider.gameObject;
                    interactable = hit.collider.GetComponent<IInteractable>();
                    SetDescriptionText();
                }
            }
            else
            {                
                lastInteractObject = null;
                interactable = null;
                descriptionText.gameObject.SetActive(false);
            }
        }
    }

    private void SetDescriptionText()
    {
        descriptionText.gameObject.SetActive(true);
        descriptionText.text = interactable.GetInteractionDescription();
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        // ��ȣ�ۿ� Ű�� ������, ���ӿ� ��ȣ�ۿ� ����� �ִٸ�
        if ( context.phase == InputActionPhase.Started && interactable != null)
        {
            // ����
            interactable.OnInteract();

            lastInteractObject = null;
            interactable = null;
            descriptionText.gameObject.SetActive(false);
        }
    }

}
