using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Button> keyboardcharacterButtons = new List<Button> ();
    private string characterNames = "QWERTYUIOPASDFGHJKLZXCVBNM";
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
            keyboardButton.GetComponent<Button>().onClick.AddListener(() => ClickCharacter(letter));

        }
    }
    void ClickCharacter(string letter) {

        Debug.Log(letter); 
    }

    private void Update()
    {
        
    }
}
