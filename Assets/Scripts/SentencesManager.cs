using System.Collections;
using UnityEngine;
using Crosstales.RTVoice;
public class SentencesManager : MonoBehaviour
{
    public GestureController gestureController;
    bool isReadyForNextConversation = false;

    public void Start()
    {
        gestureController = GameHandler.instance.gestureController;
        // gameHandler = gameObject.AddComponent<GameHandler>();
    }
    public IEnumerator StartConversation(Sentences[] sentences)
    {
        for (int i = 0; i < sentences.Length; i++)
        {
            isReadyForNextConversation = false;
            StartCoroutine(DisplayNextSentence(sentences[i].sentence, sentences[i].sentencesType, sentences[i].voiceName));
            yield return new WaitUntil(() => isReadyForNextConversation == true);
            if (i == (sentences.Length - 1))
            {
                GameHandler.SpeakFirstTime = false;
                GestureController.isTappingDone = true;
                Debug.Log("Sentences SpeakFirsttime into" + GameHandler.SpeakFirstTime);
            }   
        }
    }
    public IEnumerator DisplayNextSentence(string sentences, SentencesType type, VoiceName namespoken)
    {
        Debug.Log("first" + sentences);
        Debug.Log(Resource.CurrentScene);
        if (sentences.Contains("[RandomizeName]"))
        {
            sentences = sentences.Replace("[RandomizeName]", GameHandler.RandomizePeople.speak);
            Debug.Log(sentences);
        }
        else if (sentences.Contains("[CurrentScene]"))
        {
            sentences = sentences.Replace("[CurrentScene]", Resource.CurrentScene);
        }

        Speaker.Instance.Speak(sentences, null, Speaker.Instance.VoiceForName(namespoken.ToString()));
        Debug.Log(namespoken);
        if (type == SentencesType.WaitForInput)
        {
            yield return gestureController.WaitForTap();
        }
        else if (type == SentencesType.WaitForVoice)
        {
            Debug.Log("wait started");
            yield return new WaitForSeconds(2f);
            yield return null;
            yield return new WaitUntil(() => Speaker.Instance.isSpeaking == false);
            Debug.Log("wait finish");
        }
        else if (type == SentencesType.WaitForVerticalSlide)
        {
            yield return gestureController.DoneSlidingVertical();
        }
        else if (type == SentencesType.WaitForHorizontalSlide)
        {
            yield return gestureController.DoneSlidingHorizontal();
        }
        isReadyForNextConversation = true;
    }
}
