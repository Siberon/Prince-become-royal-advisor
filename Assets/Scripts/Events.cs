using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events : MonoBehaviour
{

    // Start is called before the first frame update
    public GameHandler gameHandler;
    void Start()
    {
        gameHandler.OnDialogueEnd += CheckGold;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void CheckGold(){
        if(Resource.GetGoldAmount() >= 100){
            Debug.Log("You Win");
            Resource.GameOver = true;
        }
    }

    private void GameOver(){
        if(Resource.GetPopulationAmount() == 0){
            Debug.Log("You lose");
            Application.Quit();
        }
    }

}
