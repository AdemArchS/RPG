using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public string[] questMarkerNames;
    public bool[] questMarkersComplete;

    public static QuestManager instance;
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        questMarkersComplete = new bool[questMarkerNames.Length];
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            MarkQuestComplete("quest test");
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            SaveQuestData();
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            LoadQuestData();
        }
    }

    //Searches for quest based on string and returns its number, returns -1 if it doesn't exist.
    public int GetQuestNumber(string questToFind)
    {
        for(int i = 0; i < questMarkerNames.Length; i++)
        {
            if(questMarkerNames[i] == questToFind)
            {
                return i;
            }
        }

        Debug.LogError("Quest " + questToFind + " does not exist");
        return -1;
    }

    //Checks if a quest is complete(true or false) based on its string. Invalid quest returns false.
    public bool CheckIfComplete(string questToCheck)
    {
        int questValue = GetQuestNumber(questToCheck);
        if(questValue != -1)
        {
            return questMarkersComplete[questValue];
        }
        
        return false;
    }

    //Marks a quest as complete based on given string.
    public void MarkQuestComplete(string questToMark)
    {
        int questValue = GetQuestNumber(questToMark);
        if(questValue != -1)
        {
            questMarkersComplete[questValue] = true;
        }

        UpdateLocalQuestObjects();
    }

    //Marks a quest as incomplete based on given string.
    public void MarkQuestIncomplete(string questToMark)
    {
        int questValue = GetQuestNumber(questToMark);
        if(questValue != -1)
        {
            questMarkersComplete[questValue] = false;
        }

        UpdateLocalQuestObjects();
    }

    //Updates all quest objects in scene to check their quest's completion.
    public void UpdateLocalQuestObjects()
    {
        QuestObjectActivator[] questObjects = FindObjectsOfType<QuestObjectActivator>();

        if(questObjects.Length > 0)
        {
            for(int i = 0; i < questObjects.Length; i++)
            {
                questObjects[i].CheckCompletion();
            }
        }
    }

    public void SaveQuestData()
    {
        for(int i = 0; i < questMarkerNames.Length; i++)
        {
            if(questMarkersComplete[i])
            {
                PlayerPrefs.SetInt("QuestMarker_" + questMarkerNames[i], 1);
            }
            else
            {
                PlayerPrefs.SetInt("QuestMarker_" + questMarkerNames[i], 0);
            }
        }
    }

    public void LoadQuestData()
    {
        for(int i = 0; i < questMarkerNames.Length; i++)
        {
            int valueToSet = 0;
            if(PlayerPrefs.HasKey("QuestMarker_" + questMarkerNames[i]))
            {
                valueToSet = PlayerPrefs.GetInt("QuestMarker_" + questMarkerNames[i]);
            }

            if(valueToSet == 0)
            {
                questMarkersComplete[i] = false;
            }
            else
            {
                questMarkersComplete[i] = true;
            }
        }
    }
}
