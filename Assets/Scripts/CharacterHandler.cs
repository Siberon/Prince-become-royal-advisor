using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHandler : MonoBehaviour
{
    public People people;
    public int totalGold = 100;
    private bool offerAccepted;
    private Dictionary<Resource.ResourceType, int> inventoryAmountDictionary;
    private int goldInventoryAmount;
    private int populationInventoryAmount;
    private int publicOpinionInventoryAmount;
    
    void Awake(){
        inventoryAmountDictionary = new Dictionary<Resource.ResourceType, int>();
        foreach (Resource.ResourceType resourceType in System.Enum.GetValues(typeof(Resource.ResourceType))) {
            inventoryAmountDictionary[resourceType] = 0;
        }
    }

    private int GetTotalGoldAmount(){
        // foreach (Resource.ResourceType resourceType in System.Enum.GetValues(typeof(Resource.ResourceType))) {
        //     totalGold += inventoryAmountDictionary[resourceType];
        // }
        return totalGold;
    }

    public void AddGold(int amount){
        totalGold += amount;
    }
    

    // Update is called once per frame
    void Update()
    {

    }
}
