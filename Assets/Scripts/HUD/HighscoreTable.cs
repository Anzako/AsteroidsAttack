using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> highscoreEntryTransformList;


    private void Awake()
    {
        entryContainer = transform.Find("HighscoreEntryContainer");
        entryTemplate = entryContainer.Find("HighscoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        UpdateTable();
    }

    private void UpdateTable()
    {
        highscoreEntryTransformList = new List<Transform>();
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        if (highscores == null) return;

        // Sort entry list by Score
        for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highscores.highscoreEntryList.Count; j++)
            {
                if (highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score)
                {
                    // Swap
                    HighscoreEntry tmp = highscores.highscoreEntryList[i];
                    highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                    highscores.highscoreEntryList[j] = tmp;
                }
            }
        }

        int maxHighscoreEntries = 12;
        for (int i = 0; i < maxHighscoreEntries; i++)
        {
            CreateHighscoreEntryTransform(highscores.highscoreEntryList[i], entryContainer, highscoreEntryTransformList);
        }

        /*foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList)
        {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }*/
    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 35f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        entryTransform.Find("PositionText").GetComponent<TMP_Text>().text = rank.ToString();
        entryTransform.Find("ScoreText").GetComponent<TMP_Text>().text = highscoreEntry.score.ToString();
        entryTransform.Find("NameText").GetComponent<TMP_Text>().text = highscoreEntry.name;

        transformList.Add(entryTransform);
    }

    public void AddHighscoreEntry(int score, string name)
    {
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = name };

        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        if (highscores == null)
        {
            highscores = new Highscores();
        }

        highscores.highscoreEntryList.Add(highscoreEntry);

        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();

        DestroyTransforms();
        UpdateTable();
    }

    private void DestroyTransforms()
    {
        for (int i = highscoreEntryTransformList.Count - 1; i >= 0; i--)
        {
            Destroy(highscoreEntryTransformList[i].gameObject);
            highscoreEntryTransformList.RemoveAt(i);
        }
        
    }

    private class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList;

        public Highscores()
        {
            highscoreEntryList = new List<HighscoreEntry>();
        }
    }

    [System.Serializable]
    private class HighscoreEntry
    {
        public int score;
        public string name;
    }
}
