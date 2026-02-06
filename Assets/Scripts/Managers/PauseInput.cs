using UnityEngine;
using UnityEngine.InputSystem;

public class PauseInput : MonoBehaviour
{
    public PauseManager pauseManager;

    [Tooltip("绑定菜单键/Start键 的 InputActionReference（Button类型）")]
    public InputActionReference pauseAction;

    private void OnEnable()
    {
        if (pauseAction != null && pauseAction.action != null)
            pauseAction.action.Enable();
    }

    private void OnDisable()
    {
        if (pauseAction != null && pauseAction.action != null)
            pauseAction.action.Disable();
    }

    private void Update()
    {
        if (pauseManager == null) return;
        if (pauseAction == null || pauseAction.action == null) return;

        if (pauseAction.action.WasPressedThisFrame())
        {
            pauseManager.TogglePause();
        }
    }
}
