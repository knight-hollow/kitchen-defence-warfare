using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadMainMenu() => LoadByName("MainMenu");
    public void LoadGame()     => LoadByName("GameScene");
    public void LoadResult()   => LoadByName("ResultScene");

    public void QuitApp()
    {
        // 编辑器里不会退出，打包到 Quest 才会退出
        Application.Quit();
    }

    private void LoadByName(string sceneName)
    {
        Time.timeScale = 1f; // 防止从暂停切场景后时间卡住
        SceneManager.LoadScene(sceneName);
    }
}
