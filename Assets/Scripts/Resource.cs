using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class Resource
{
    public static event EventHandler onAmountChanged; 
    public static bool hasPeople;
    public static bool GameOver;
    public static string voiceName;
    
    public static string CurrentScene;    
    public enum ResourceType {
        Gold, Population, publicOpinion 
    }
    public static Dictionary<ResourceType, int> resourceAmountDictionary;

    public static void Init() {
        voiceName = "Matthew";
        resourceAmountDictionary = new Dictionary<ResourceType, int>();
        hasPeople = false;
        foreach (ResourceType resourceType in System.Enum.GetValues(typeof(ResourceType))){
            resourceAmountDictionary[resourceType] = 100; 
        }
        GameOver = false;
    }

    public static void AddResourceAmount (ResourceType resourceType, int amount) {
        resourceAmountDictionary[resourceType] += amount;
        if (onAmountChanged != null) onAmountChanged(null, EventArgs.Empty);
    }
    
    public static int GetGoldAmount(){
        return resourceAmountDictionary[ResourceType.Gold];
    }

    public static int GetPopulationAmount(){
        return resourceAmountDictionary[ResourceType.Population];
    }
    public static int GetPublicOpinionAmount(){
        return resourceAmountDictionary[ResourceType.publicOpinion];
    }
}

