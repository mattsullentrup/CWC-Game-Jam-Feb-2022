using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Score { get; set; }
    [SerializeField] TextMeshProUGUI scoreText;
    public float score = 0;
    public float scoreMultiplier;

    private void Awake()
    {
        
        if (Score != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Score = this;
        }
    }

    private void Update()
    {
        if (GameManager.Manager.isGameActive == true)
        {
            CountScore();
        }
    }

    void CountScore()
    {
        if (SphereController.Controller.isOnGround == false)
        {
            score += (Time.deltaTime * scoreMultiplier);
            scoreText.text = "Score: " + Mathf.Round(score);
        }
    }
}
