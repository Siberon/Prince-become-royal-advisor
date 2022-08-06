using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class People : ScriptableObject
{
    public new string name;
    public int gold;
    public int population;
    public int publicOpinion;
    public string speak;
    public string description;
    public int goldNegative;
    public int populationNegative;
    public int publicOpinionNegative;
        public string voiceName;

}
