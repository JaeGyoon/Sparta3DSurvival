using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    // 업데이트 주기
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


            // 스크린 x값과 y값의 반 => 화면 정중앙에 레이 
            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            // 레이에 상호작용 가능한 대상이 있으면 설명이 나오고 없으면 사라짐
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
        // 상호작용 키를 눌렀고, 에임에 상호작용 대상이 있다면
        if ( context.phase == InputActionPhase.Started && interactable != null)
        {
            // 습득
            interactable.OnInteract();

            lastInteractObject = null;
            interactable = null;
            descriptionText.gameObject.SetActive(false);
        }
    }

}
