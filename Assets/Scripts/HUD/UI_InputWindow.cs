using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InputWindow : MonoBehaviour
{
    private TMP_InputField inputField;
    [SerializeField] private HighscoreTable highscoreTable;

    private string validCharacters = "ABCDEFGHIJKLMNOPRSTUWYZXVQ1234567890";
    private int characterLimit = 8;

    private void Awake()
    {
        inputField = transform.Find("InputField").GetComponent<TMP_InputField>();
        
        inputField.characterLimit = characterLimit;
        inputField.onValidateInput = (string text, int charIndex, char addedChar) =>
        {
            return ValidateChar(validCharacters, addedChar);
        };

        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnCancelButtonPressed()
    {
        Hide();
    }

    public void OnOkButtonPressed()
    {
        string text = inputField.text;
        if (!string.IsNullOrEmpty(text))
        {
            highscoreTable.AddHighscoreEntry(ScoreManager.Instance.GetEndGameScore(), text);
        }
        
        Hide();
    }

    private char ValidateChar(string validCharacters, char addedChar)
    {
        if (validCharacters.IndexOf(addedChar) != -1) 
        { 
            return addedChar;
        } else
        {
            return '\0';
        }
    }
}
