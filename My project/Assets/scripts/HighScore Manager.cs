using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HighScoreManager : MonoBehaviour
{
    public static int currentStreak ;
    public int highestStreak;
    private static HighScoreManager instance;
    
    public GameObject HiStreak;
    public  GameObject currStreak;

    // Start is called before the first frame update
    private void Start()
    {
       

    }
    private void Awake()
    {
   
        int highestStreak = PlayerPrefs.GetInt("Highscore", 0);
        HiStreak.GetComponent<TextMeshProUGUI>().text = "Highest Streak: " + highestStreak;
        currStreak.GetComponent<TextMeshProUGUI>().text = "Current Streak: " + currentStreak;



    }

    public void UpdateHighScore(int score) {

        int currentHighScore = PlayerPrefs.GetInt("Highscore", 0);
        if (score > currentHighScore) {
            PlayerPrefs.SetInt("Highscore", score);
            PlayerPrefs.Save();
            Debug.Log("yes"+score);
            Debug.Log("kffjyjvjtkckhhnkfyurxghg");
            
            HiStreak.GetComponent<TextMeshProUGUI>().text = "Highest Streak: " + score;
            currStreak.GetComponent<TextMeshProUGUI>().text = "Current Streak: " + currentStreak; 
        }

    
    }
    public void ResetHighscore()
    {
        
        PlayerPrefs.SetInt("Highscore", 0);
        PlayerPrefs.Save();
        int highScore = PlayerPrefs.GetInt("Highscore", 0);
        currentStreak = 0; 
        HiStreak.GetComponent<TextMeshProUGUI>().text = "Highest Streak: "+ highScore;
        currStreak.GetComponent<TextMeshProUGUI>().text = "Current Streak: "+ currentStreak;
    }



   
    //not semantically correct or whatever but eh
    public void OpenURL()
    {
        Application.OpenURL("https://www.canva.com/policies/ai-product-terms/");
        Debug.Log("is this working?");
    }

}
