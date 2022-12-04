using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Doozy.Engine.Events;
using Doozy.Engine;
using WasdStudio.LeaderboardSystem;
using WasdStudio.GameFlowManager;
public class PlayerManager : MonoBehaviour {

    public int Score { get => Mathf.RoundToInt( currentScore ); }
    public GameFlowManager gameFlowManager;
    public static PlayerManager instance;
    public TextMeshProUGUI scoreText;
    public Image healtBar;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI highScoreText;
    public EntryValidator entryValidator;
    public List<Image> waveON;
    int currentWave = -1;
    float currentScore;
    float healtAmount;

    private void Awake()
    {
        instance = this;
        AddScore(0);

        healtAmount = healtBar.fillAmount;        
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(healtAmount - healtBar.fillAmount) > 0.05f)
        {
            healtBar.fillAmount = Mathf.Lerp(healtBar.fillAmount, healtAmount, 0.5f);
        }
        else
        {
            healtBar.fillAmount = healtAmount;
        }
    }

    public void ResetManager() {
        healtAmount = healtBar.fillAmount = 1;
        currentScore = 0;
        scoreText.text = currentScore.ToString();
    }

    public void HurtPlayer(float damage)
    {
        if (healtBar.fillAmount > 0)
        {
            healtAmount = Mathf.Max(healtAmount - damage, 0);

            if (healtAmount == 0)
                GameOver();
        }
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        scoreText.text = currentScore.ToString();
    }

    public int AddWave(int maxWaves)
    {
        currentWave = Mathf.Min(currentWave + 1, maxWaves - 1);

        if (currentWave < waveON.Count)
            waveON[currentWave].gameObject.SetActive(true);

        waveText.text = (currentWave + 1).ToString();
                
        return currentWave;
    }

    void GameOver()
    {
        finalScoreText.text = string.Format("{0} pts",Score.ToString());
        if(LeaderboardManager.lastRecord != null)
        highScoreText.text = string.Format("{0} pts", LeaderboardManager.lastRecord[0]);
        entryValidator.Points = Score;
        gameFlowManager.ChangePhase();
        GameEventMessage.SendEvent( "GameOver" );
        currentWave = -1;
    }
}
