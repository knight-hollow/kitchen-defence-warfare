using UnityEngine;
using UnityEngine.InputSystem; // 必须引用新的输入系统

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu; // 拖入你的暂停 UI
    public InputActionProperty pauseAction; // 稍后绑定左手菜单键

    private bool isPaused = false;

    void Update()
    {
        // 检测按键是否在这一帧被按下
        if (pauseAction.action.WasPressedThisFrame())
        {
            TogglePause();
        }
    }

        void OnEnable()
    {
        // 确保按键动作被激活
        pauseAction.action.Enable();
    }

    void OnDisable()
    {
        pauseAction.action.Disable();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f; // 冻结时间
            // 如果需要，这里可以加入锁定射线追踪的代码
            // 让 UI 强制出现在相机前方 1.5 米处
            Transform camTransform = Camera.main.transform;
            pauseMenu.transform.position = camTransform.position + camTransform.forward * 1.5f;

            // 让 UI 面向玩家
            pauseMenu.transform.LookAt(camTransform.position);
            pauseMenu.transform.Rotate(0, 180, 0); // LookAt 默认是背面，这里转回正面
        }
        else
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f; // 恢复时间
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Exiting...");
    }
}