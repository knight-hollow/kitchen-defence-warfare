using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultUI : MonoBehaviour
{
    public TMP_Text resultText;

    private void Start()
    {
        if (resultText != null)
            resultText.text = GameResult.LastWin ? "SUCCESS!" : "FAILED!";
    }

    public void Restart()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
