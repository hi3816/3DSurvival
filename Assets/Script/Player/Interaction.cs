using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f; // 체크를 수행하는 간격(초 단위). 예: 0.05초마다 검사.
    private float lastCheckTime;    // 마지막으로 체크가 수행된 시간을 저장하는 변수.
    public float maxCheckDistance;  // 체크할 최대 거리. 예: 플레이어가 탐지할 수 있는 거리.
    public LayerMask layerMask;     // 체크할 레이어를 설정하는 변수. 예: 충돌 검사를 할 때 특정 오브젝트만 검사하도록 레이어를 지정.

    public GameObject curInteractGameObject;
    private IInteractable curInteractable;

    public TextMeshProUGUI promptText;
    private Camera camera;
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        { 
            lastCheckTime = Time.time;

            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    //프롬포트에 출력해줘라.
                    SetPromptText();
                }
            }
            else
            {
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }

        }
    }

    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractPrompt();
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}
