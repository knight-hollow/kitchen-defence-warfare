using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text hpText;
    public TMP_Text waveText;
    public TMP_Text remainingText;

    public void UpdateHP(int hp)
    {
        if (hpText != null)
            hpText.text = $"HP: {hp}";
    }

    public void UpdateWave(int wave, int total)
    {
        if (waveText != null)
            waveText.text = $"Wave: {wave}/{total}";
    }

    public void UpdateRemaining(int remaining)
    {
        if (remainingText != null)
            remainingText.text = $"Remaining: {remaining}";
    }
}
