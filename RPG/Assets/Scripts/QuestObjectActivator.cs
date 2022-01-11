using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObjectActivator : MonoBehaviour
{
    public GameObject objectToActivate;

    public string questToCheck;

    //If the quest is complete, this determines if the object should be active or not.
    public bool activeIfComplete;

    private bool initialCheckDone;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Makes sure that if the QuestManager is not loaded yet, it gives it time to load before using the QuestManager.
        //Update is after all start functions which is why we do this so everything is instanciated.
        if(!initialCheckDone)
        {
            initialCheckDone = true;

            CheckCompletion();
        }
    }

    public void CheckCompletion()
    {
        if(QuestManager.instance.CheckIfComplete(questToCheck))
        {
            objectToActivate.SetActive(activeIfComplete);
        }
    }
}
