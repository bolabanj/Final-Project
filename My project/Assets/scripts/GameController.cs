using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices.ComTypes;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    private List<string> dictionary = new List<string> ();
    private List<string> guessingWords = new List<string> ();
    public string correctWord;
    public int new_score;
    public int testScore = 0; 
    public List<Transform> wordBoxes =  new List<Transform>();
    private int currentWordBox;
    private int currentRow;
    private int charactersPerRowCount = 5;
    private int amountOfRows = 5;
    private Color colorCorrect = new Color(0.3254902f, 0.5529412f, 0.3058824f);
    private Color colorIncorrectPlace = new Color(0.7098039f, 0.6235294f, 0.2313726f);
    private Color colorUnused = new Color(0.2039216f, 0.2039216f, 0.2f);
    public Sprite clearedWordBoxSprite;
    public PlayerController playerController;
    public  HighScoreManager highScoreManager;
    
    bool isWordValid;
   

    public GameObject popup;
    public GameObject panel; 
    private Coroutine popupRoutine;


    private void Awake()
    {
        
        highScoreManager.currStreak.GetComponent<TextMeshProUGUI>().text = "Current Streak: " + HighScoreManager.currentStreak;
    }
    void Start()
    {
        AddwordsToList("Assets/Resources/dictionary.txt", dictionary);
        AddwordsToList("Assets/Resources/wordlist.txt", guessingWords);
        correctWord = GetRandomWord();

    }
    void AddwordsToList(string path, List<string> listofWords)
    {
        StreamReader reader = new StreamReader(path);
        string text = reader.ReadToEnd();

        Debug.Log(text);

        char[] seperate = { ',' };
        string[] singleWords = text.Split(seperate);

        foreach (string newWord in singleWords)
        {
            listofWords.Add(newWord); 
        }
        reader.Close();

    }

    // Update is called once per frame
   string GetRandomWord()
    {
        string randomWord = guessingWords[Random.Range(0, guessingWords.Count)];
        Debug.Log(randomWord);
        return randomWord;
    }
    public void AddLetterToWordBox(string letter) {
        if (currentRow > amountOfRows) {
            Debug.Log("No more rows available");
            return; 
        }
        int currentlySelectedWordbox = (currentRow * charactersPerRowCount) + currentWordBox; 
        if (wordBoxes[currentlySelectedWordbox].GetChild(0).GetComponent<TextMeshProUGUI>().text == "")
        {
            wordBoxes[currentlySelectedWordbox].GetChild(0).GetComponent<TextMeshProUGUI>().text = letter;
        }
        
        if (currentlySelectedWordbox < (currentRow * charactersPerRowCount)+ 4)  {
            currentWordBox++; 
        }
        }
     

    public void RemoveLetterFromWordBox() {
        if (currentRow > amountOfRows) {
            Debug.Log("No more rows available");
            return; 
        }
        int currentlySelectedWordbox = (currentRow * charactersPerRowCount) + currentWordBox;
        if (wordBoxes[currentlySelectedWordbox].GetChild(0).GetComponent<TextMeshProUGUI>().text == "")
        {
            if (currentlySelectedWordbox > ((currentRow * charactersPerRowCount)))
            {
               
                currentWordBox--;
            }
            
            currentlySelectedWordbox = (currentRow * charactersPerRowCount) + currentWordBox;

            wordBoxes[currentlySelectedWordbox].GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
        }
        else
        {
            
            wordBoxes[currentlySelectedWordbox].GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
        }


    }


    public void SubmitWord() {
        

        string guess= "";
        for (int i = (currentRow * charactersPerRowCount); i < (currentRow *charactersPerRowCount)+  currentWordBox +1; i++) {
            guess += wordBoxes[i].GetChild(0).GetComponent<TextMeshProUGUI>().text; 
        }
    

        if (guess.Length != 5) {
            Debug.Log("Answer too short");
            feedBack("Answer too short, must be 5 letters yo! ", 2f, false);
            return; 
        }



        guess = guess.ToLower();
        isWordValid = false;

        if (dictionary.Contains(guess)) {
            isWordValid = true; 
        }
        if (!isWordValid) {
            if (guessingWords.Contains(guess)) {
                isWordValid= true;
                
            }
        }
        if(!isWordValid) {
            feedBack("Yo, at least enter an ACTUAL word! ", 2f, false);
            return; 
        }

        nCheckWord(guess);
        if (currentRow == amountOfRows)
        {
            Debug.Log("No more rows available");
            feedBack("LMAOOO, that's wrong buddy\nThe word was : " + correctWord, 0f, true);
            HighScoreManager.currentStreak = 0;
            return;
        }
        foreach (char c in guess) {
            playerController.GetKeyboardImage(c.ToString()); 
        }
        Debug.Log("Player guess: "+ guess);
        if (guess == correctWord)
        {
            
            
            feedBack("You win!", 0f, true);
            HighScoreManager.currentStreak +=1;
            highScoreManager.currStreak.GetComponent<TextMeshProUGUI>().text = "Current Streak: " + HighScoreManager.currentStreak;
            highScoreManager.UpdateHighScore(HighScoreManager.currentStreak); 
            Debug.Log("correct Word");
            WinGame(); 


        }
        else {
            Debug.Log("Wrong guess!");
            currentWordBox = 0;
            currentRow++;
        }

        

        
    }

    void CheckWord(string guess)
    {
        //This method might be modified for a harder difficulty, we give the player fewer feedbacks to work with 
        for (int i = (currentRow * charactersPerRowCount); i < (currentRow * charactersPerRowCount) + currentWordBox + 1; i++)
        {
            if (guess[i - (currentRow * charactersPerRowCount)] == correctWord[i - (currentRow * charactersPerRowCount)])
            {
                wordBoxes[i].GetComponent<UnityEngine.UI.Image>().sprite = clearedWordBoxSprite;
                wordBoxes[i].GetComponent<UnityEngine.UI.Image>().color = colorCorrect;
                Debug.Log("This is supposed to work!");            }
        }
        char[] playerGuessArray = guess.ToCharArray();
        string tempPlayerGuess = guess;
        char[] correctWordArray = correctWord.ToCharArray();
        string tempCorrectWord = correctWord;

        // Thought process similar to implementation by LootLocker, replace the correct leters in the correct index with 0's
        //Then check for if the other letters in the word if they are displaced and replace those with the digit 1
        // This way we are able go validate duplicate and displaced guesses 
        for (int i = 0; i < 5; i++)
        {
            if (playerGuessArray[i] == correctWordArray[i])
            {
               
                playerGuessArray[i] = '0';
                correctWordArray[i] = '0';
            }
        }

        tempPlayerGuess = "";
        tempCorrectWord = "";
        for (int i = 0; i < 5; i++)
        {
            tempPlayerGuess += playerGuessArray[i];
            tempCorrectWord += correctWordArray[i];
        }

        
        for (int i = 0; i < 5; i++)
        {
            if (tempCorrectWord.Contains(playerGuessArray[i].ToString()) && playerGuessArray[i] != '0')
            {
                char playerCharacter = playerGuessArray[i];
                playerGuessArray[i] = '1';
                tempPlayerGuess = "";
                for (int j = 0; j < 5; j++)
                {
                    tempPlayerGuess += playerGuessArray[j];
                }

               
                int index = tempCorrectWord.IndexOf(playerCharacter, 0);
                correctWordArray[index] = '.';
                tempCorrectWord = "";
                for (int j = 0; j < 5; j++)
                {
                    tempCorrectWord += correctWordArray[j];
                }
            }
        }
    }
    void nCheckWord(string guess)
    {
        char[] playerGuessArray = guess.ToCharArray();
        string tempPlayerGuess = guess;
        char[] correctWordArray = correctWord.ToCharArray();
        string tempCorrectWord = correctWord;

  
        for (int i = 0; i < 5; i++)
        {
            if (playerGuessArray[i] == correctWordArray[i])
            {
            
                playerGuessArray[i] = '0';
                correctWordArray[i] = '0';
            }
        }

        tempPlayerGuess = "";
        tempCorrectWord = "";
        for (int i = 0; i < 5; i++)
        {
            tempPlayerGuess += playerGuessArray[i];
            tempCorrectWord += correctWordArray[i];
        }

        for (int i = 0; i < 5; i++)
        {
            if (tempCorrectWord.Contains(playerGuessArray[i].ToString()) && playerGuessArray[i] != '0')
            {
                char playerCharacter = playerGuessArray[i];
                playerGuessArray[i] = '1';
                tempPlayerGuess = "";
                for (int j = 0; j < 5; j++)
                {
                    tempPlayerGuess += playerGuessArray[j];
                }

                int index = tempCorrectWord.IndexOf(playerCharacter, 0);
                correctWordArray[index] = '.';
                tempCorrectWord = "";
                for (int j = 0; j < 5; j++)
                {
                    tempCorrectWord += correctWordArray[j];
                }
            }
        }

    
        Color newColor = colorUnused;

        for (int i = 0; i < 5; i++)
        {

            if (tempPlayerGuess[i] == '0')
            {
           
                newColor = colorCorrect;
            }
            else if (tempPlayerGuess[i] == '1')
            {
                
                newColor = colorIncorrectPlace;
            }
            else
            {
                
                newColor = colorUnused;
            }

            UnityEngine.UI.Image currentWordboxImage = wordBoxes[i + (currentRow * charactersPerRowCount)].GetComponent<UnityEngine.UI.Image>();

            currentWordboxImage.sprite = clearedWordBoxSprite;

            
            UnityEngine.UI.Image keyboardImage = playerController.nGetKeyboardImage(guess[i].ToString());

           
            if (newColor == colorCorrect)
            {
                keyboardImage.color = newColor;
            }

            
            if (newColor == colorIncorrectPlace && keyboardImage.color != colorCorrect)
            {
                keyboardImage.color = newColor;
            }

            if (newColor == colorUnused && keyboardImage.color != colorCorrect && keyboardImage.color != colorIncorrectPlace)
            {
                keyboardImage.color = newColor;
            }
            currentWordboxImage.color = newColor;

        }
    }
    public void ReplayScene()
    {
       
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene("Stage");
        


    }

    public void MainMenu() {
        SceneManager.LoadSceneAsync(0);
    }
    void feedBack(string message, float duration, bool stay) {

        if (popupRoutine != null) {
            StopCoroutine(popupRoutine);
        }
        popupRoutine = StartCoroutine(ShowPopupRoutine(message, duration, stay));
    }
    IEnumerator ShowPopupRoutine(string message, float duration, bool stay = false) {

        popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = message; 
        popup.SetActive(true);
        if (stay) {
            while (true) {

                yield return new WaitForSeconds(2);
                popup.SetActive(false);
                yield return null;  
                panel.SetActive(true);
                //SceneManager.LoadSceneAsync(0);
                
            }
        }
        yield return new WaitForSeconds(duration);
        popup.SetActive(false);
    }
    void WinGame() { 
    
    }
  
}
