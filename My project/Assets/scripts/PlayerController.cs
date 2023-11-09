using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

using TMPro;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public List<UnityEngine.UI.Button> keyboardcharacterButtons = new List<UnityEngine.UI.Button> ();
    private string characterNames = "QWERTYUIOPASDFGHJKLZXCVBNM";
    public GameController gameController;
    void Start()
    {
        SetupButtons ();
    }

    // Update is called once per frame
    void SetupButtons()
    {
        for (int i = 0; i < keyboardcharacterButtons.Count; i++) {
            keyboardcharacterButtons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = characterNames[i].ToString();
        }

        foreach (var keyboardButton in keyboardcharacterButtons) {
            string letter = keyboardButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
            keyboardButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ClickCharacter(letter));

        }
    }
    void ClickCharacter(string letter) {

        Debug.Log(letter); 
        gameController.AddLetterToWordBox(letter);
    }
    public void GetKeyboardImage(string letter)
    {
        letter = letter.ToUpper();
        foreach (var keyboardLetter in keyboardcharacterButtons)
        {
            if (keyboardLetter.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text == letter) {
                Debug.Log("This is supposed to work!"); 
                keyboardLetter.transform.GetComponent<UnityEngine.UI.Image>().color = new Color(0.3254902f, 0.5529412f, 0.3058824f);  
            }
        }
        Debug.Log("This letter does not exist on the current keyboard.");
        
    }
    private void Update()
    {
        
    }
}
