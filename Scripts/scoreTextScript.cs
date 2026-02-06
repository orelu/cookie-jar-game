using UnityEngine;
using TMPro;

public class scoreTextScript : MonoBehaviour
{

    private TextMeshProUGUI text; 
    public bool showHighScore = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();

        if (showHighScore) {
            int score = PlayerPrefs.GetInt("highscore", 0);

            text.text = "Best: " + score;
        } else {
            int score = PlayerPrefs.GetInt("score", 0);

            text.text = "Score: " + score;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
