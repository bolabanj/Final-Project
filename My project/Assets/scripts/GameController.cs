using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    private List<string> dictionary = new List<string> ();
    private List<string> guessingWords = new List<string> ();
    public string correctWord; 
    public List<Transform> wordBoxes =  new List<Transform>();
    private int currentWordBox;
    private int currentRow;
    private int charactersPerRowCount = 5;
    private int amountOfRows = 5;
    private Color colorCorrect = new Color(0.3254902f, 0.5529412f, 0.3058824f);
    private Color colorIncorrectPlace = new Color(0.7098039f, 0.6235294f, 0.2313726f);
    private Color colorUnused = new Color(159f, 159f, 159f);
    public Sprite clearedWordBoxSprite;
    public PlayerController playerController;
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
            return; 
        }



        guess = guess.ToLower();
        CheckWord(guess);
        foreach (char c in guess) {
            playerController.GetKeyboardImage(c.ToString()); 
        }
        Debug.Log("Player guess: "+ guess);
        if (guess == correctWord)
        {
            Debug.Log("correct Word");


        }
        else {
            Debug.Log("Wrong guess!");
            currentWordBox = 0;
            currentRow++;
        }

        if (currentRow > amountOfRows) {
            Debug.Log("No more rows available");
            return; 
        }

        
    }

    void CheckWord(string guess)
    {
        for (int i = (currentRow * charactersPerRowCount); i < (currentRow * charactersPerRowCount) + currentWordBox + 1; i++)
        {
            if (guess[i - (currentRow * charactersPerRowCount)] == correctWord[i - (currentRow * charactersPerRowCount)])
            {
                wordBoxes[i].GetComponent<UnityEngine.UI.Image>().sprite = clearedWordBoxSprite;
                wordBoxes[i].GetComponent<UnityEngine.UI.Image>().color = colorCorrect;
                Debug.Log("This is supposed to work!");            }
        }
    }


}
