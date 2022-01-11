using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialsLoader : MonoBehaviour
{
    public GameObject UIScreen;
    public GameObject player;
    public GameObject gameManager;
    public GameObject audioManager;
    public GameObject battleManager;

    // Start is called before the first frame update
    void Awake()
    {
        if(UIFade.instance == null)
        {
            Instantiate(UIScreen);
        }

        if(PlayerController.instance == null)
        {
            Instantiate(player);
        }

        if(GameManager.instance == null)
        {
            Instantiate(gameManager);
        }

        if(AudioManager.instance == null)
        {
            Instantiate(audioManager);
        }

        if(BattleManager.instance == null)
        {
            Instantiate(battleManager);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
