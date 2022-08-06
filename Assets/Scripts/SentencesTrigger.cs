using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentencesTrigger : MonoBehaviour
{
    public SentencesManager sentencesManager;
    [SerializeField]
    public Sentences[] sentences;
    public IEnumerator TriggerSentences(){
        StartCoroutine(sentencesManager.StartConversation(sentences));
        yield return null;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
