using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 暂停管理：暂停/恢复/重新开始/回主界面
/// 适用于 VR/XR：暂停时 Time.timeScale = 0
/// </summary>
public class PauseManager : MonoBehaviour
{
    [Header("UI")]
    [Tooltip("暂停面板（Panel），默认应为隐藏 SetActive(false)")]
    public GameObject pausePanel;

    [Header("Optional: Pause Button (UI)")]
    [Tooltip("如果你有一个 UI 的 Pause 按钮，可以在 OnClick 里调用 TogglePause()")]
    public bool startPaused = false;

    private bool paused = false;

    private void Awake()
    {
        // 防呆：场景加载时确保时间正常
        Time.timeScale = 1f;
    }

    private void Start()
    {
        if (pausePanel != null)
            pausePanel.SetActive(startPaused);

        paused = startPaused;

        if (paused)
            Time.timeScale = 0f;
    }

    /// <summary>
    /// 切换暂停/恢复（推荐绑到 UI 按钮或手柄菜单键）
    /// </summary>
    public void TogglePause()
    {
        if (paused) Resume();
        else Pause();
    }

    /// <summary>
    /// 暂停
    /// </summary>
    public void Pause()
    {
        paused = true;
        Time.timeScale = 0f;

        if (pausePanel != null)
            pausePanel.SetActive(true);
    }

    /// <summary>
    /// 恢复
    /// </summary>
    public void Resume()
    {
        paused = false;
        Time.timeScale = 1f;

        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    /// <summary>
    /// 重新开始当前游戏（建议 GameScene 名字固定为 "GameScene"）
    /// </summary>
    public void RestartGame()
    {
        Time.timeScale = 1f;
        paused = false;

        SceneManager.LoadScene("GameScene");
    }

    /// <summary>
    /// 回到主界面（建议 MainMenu 名字固定为 "MainMenu"）
    /// </summary>
    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        paused = false;

        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// （可选）退出应用：仅在真机/打包时生效
    /// </summary>
    public void QuitApp()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }

    /// <summary>
    /// 供外部查询当前是否暂停
    /// </summary>
    public bool IsPaused()
    {
        return paused;
    }
}
