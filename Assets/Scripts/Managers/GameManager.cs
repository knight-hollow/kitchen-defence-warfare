using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int maxHP = 10;
    public int hp;

    [Header("Refs")]
    public UIManager uiManager;
    public WaveManager waveManager;

    private bool gameEnded = false;

    private void Start()
    {
        hp = maxHP;
        gameEnded = false;

        if (uiManager != null)
            uiManager.UpdateHP(hp);

        if (waveManager != null)
            waveManager.StartWaves(); // 开始刷怪
    }

    public void TakeDamage(int amount)
    {
        if (gameEnded) return;

        hp -= amount;
        if (hp < 0) hp = 0;

        if (uiManager != null)
            uiManager.UpdateHP(hp);

        if (hp <= 0)
        {
            EndGame(false);
        }
    }

    public void EndGame(bool win)
    {
        if (gameEnded) return;
        gameEnded = true;

        GameResult.LastWin = win;
        Time.timeScale = 1f; // 防止从暂停进入结算后时间还为0
        SceneManager.LoadScene("ResultScene");
    }
}
