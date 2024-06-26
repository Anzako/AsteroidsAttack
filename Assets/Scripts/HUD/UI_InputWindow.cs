using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InputWindow : MonoBehaviour
{
    private TMP_InputField inputField;
    [SerializeField] private HighscoreTable highscoreTable;
    [SerializeField] private GameObject submitButton;

    private string validCharacters = "abcdefghijklmnoprstuwyzxvqABCDEFGHIJKLMNOPRSTUWYZXVQ1234567890";
    private int characterLimit = 12;
    private bool highscoreAdded;

    // Sounds
    [SerializeField] private AudioClip buttonPressedSoundClip;

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
        inputField.text = "";
        highscoreAdded = false;
        OnButtonPressed();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnCancelButtonPressed()
    {
        OnButtonPressed();
        Hide();
    }

    public void OnOkButtonPressed()
    {
        if (highscoreAdded) { Hide(); return; }

        string text = inputField.text;
        
        if (text != null && text != "")
        {
            highscoreAdded = true;
            highscoreTable.AddHighscoreEntry(ScoreManager.Instance.GetEndGameScore(), text);
            submitButton.SetActive(false);
        }

        Hide();
    }

    private void OnGUI()
    {
        
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

    private void OnButtonPressed()
    {
        SoundFXManager.Instance.PlaySoundFXClip(buttonPressedSoundClip, transform, 1f);
    }
}
