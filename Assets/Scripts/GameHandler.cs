using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Crosstales.RTVoice;
using Crosstales.RTVoice.Model;
public class GameHandler : MonoBehaviour
{
    public event System.Action OnDialogueEnd;
    public static GameHandler instance;
    public static People RandomizePeople;
    public GestureController gestureController;
    public People[] people;
    private AudioSource audioSource;
    public static bool SpeakFirstTime;
    [SerializeField] AudioClip SFX;

    public Queue<string> speakTexts;

    AudioSource SFXPrologue;
    void Awake()
    {
        StartCoroutine(DelayABit(5));
        if (instance is null)
        {
            Resource.Init();
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        SFXPrologue = gameObject.AddComponent<AudioSource>();
        // GetTextToSpeech();
    }
    void Start()
    {
        SFXPrologue.volume = 0.5f;
        Resource.CurrentScene = "gameArea";
        speakTexts = new Queue<string>();
        gestureController = gameObject.AddComponent<GestureController>();
        GestureController.isTappingDone = false;
        audioSource = gameObject.AddComponent<AudioSource>();
        SpeakFirstTime = true;
        Debug.Log(people.Length);
        RandomizePeople = people[UnityEngine.Random.Range(0, people.Length)];
        Debug.Log(RandomizePeople);
    }

    void Update()
    {

        if (!Speaker.Instance.isSpeaking)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    gestureController.fingerUp = touch.position;
                    gestureController.fingerDown = touch.position;
                }

                // //Detects Swipe while finger is still moving
                // if (touch.phase == TouchPhase.Moved)
                // {
                //     if (!detectSwipeOnlyAfterRelease)
                //     {
                //         gestureController.fingerDown = touch.position;
                //         gestureController.checkSwipe(RandomizePeople);
                //     }
                // }

                //Detects swipe after finger is released
                if (touch.phase == TouchPhase.Ended)
                {
                    SFXPrologue.clip = SFX;
                    SFXPrologue.Play();
                    gestureController.done = true;
                    gestureController.isVerticalSlide = true;
                    gestureController.isHorizontalSlide = true;
                    gestureController.fingerDown = touch.position;
                    gestureController.checkSwipe(RandomizePeople);
                    Debug.Log(GestureController.isTappingDone);
                    Debug.Log(SpeakFirstTime);


                    // StartCoroutine(checkArea());

                }

            }
        }
    }

    // public IEnumerator checkArea()
    // {
    //     if (GestureController.isTappingDone == true && SpeakFirstTime == false)
    //     {
    //         Debug.Log("Tapping detected");
    //         Debug.Log(Resource.CurrentScene);

    //         if (Resource.CurrentScene == "kingdom information")
    //         {
    //             yield return new WaitForSeconds(0.5f);
    //             yield return new WaitUntil(() => Speaker.Instance.isSpeaking == false);
    //             if (Speaker.Instance.isSpeaking == false)
    //             {
    //                 StartCoroutine(SpeakResource());
    //             }
    //         }
    //         Debug.Log("NoSwipe");
    //         gestureController.WaitForTap();
    //     }
    //     yield return Speaker.Instance.Speak(RandomizePeople.speak, null, Speaker.Instance.VoiceForName(RandomizePeople.voiceName));
    //     GestureController.isTappingDone = true;
    //     yield return null;
    // }
    // public IEnumerator speakDescription()
    // {
    //     yield return new WaitForSeconds(0.5f);
    //     yield return new WaitUntil(() => Speaker.Instance.isSpeaking == false);
    //     Debug.Log(GameHandler.RandomizePeople.description);
    //     Speaker.Instance.Speak(GameHandler.RandomizePeople.description, null, Speaker.Instance.VoiceForName(GameHandler.RandomizePeople.voiceName));
    //     yield return null;
    // }

    // public IEnumerator speakArea()
    // {
    //     yield return new WaitForSeconds(0.5f);
    //     yield return new WaitUntil(() => Speaker.Instance.isSpeaking == false);
    //     if (Resource.CurrentScene == ("kingdom information"))
    //     {
    //         Speaker.Instance.Speak("This is the game area!", null, Speaker.Instance.VoiceForName(GameHandler.RandomizePeople.voiceName));
    //         Debug.Log("Change scene to game area");
    //         Resource.CurrentScene = "gameArea";
    //     }

    //     else if (Resource.CurrentScene == ("gameArea"))
    //     {
    //         Speaker.Instance.Speak("This is the kingdom information!", null, Speaker.Instance.VoiceForName(GameHandler.RandomizePeople.voiceName));
    //         Debug.Log("Change scene to kingdom information");
    //         Resource.CurrentScene = "kingdom information";
    //     }
    // }

    IEnumerator DelayABit(int wait)
    {
        yield return new WaitForSeconds(wait);
    }



    // async Task Voicess1(){
    //     while (speakTexts.Count > 0){
    //         speakTexts.Enqueue(Speaker.Instance.Speak("This is the game area. Here, people will come to offer you. You can choose to Accept or Reject. Here comes the first citizen.", audioSource, Speaker.Instance.VoiceForName("Matthew")));
    //         await Task gestureController.WaitForTap();
    //     }
    // }
    public SentencesTrigger sentencesTrigger;
    IEnumerator Voicess()
    {
        for (int i = 0; speakTexts.Count >= i; i++)
        {
            yield return sentencesTrigger.TriggerSentences();
            if (i == (speakTexts.Count - 1))
            {
                SpeakFirstTime = false;
            }
        }
        Debug.Log(Speaker.Instance.isSpeaking);
    }


    public void voicesReady()
    {
        StartCoroutine(Voicess());
        StopAllCoroutines();
        Debug.Log("voicesReady: ");
    }

    public void OnEnable()
    {
        // Subscribe event listeners
        Speaker.Instance.OnVoicesReady += voicesReady;
        Speaker.Instance.OnSpeakStart += speakStart;
        Speaker.Instance.OnSpeakComplete += speakComplete;
    }

    public void OnDisable()
    {
        // Unsubscribe event listeners
        Speaker.Instance.OnVoicesReady -= voicesReady;
        Speaker.Instance.OnSpeakStart -= speakStart;
        Speaker.Instance.OnSpeakComplete -= speakComplete;
    }
    private void speakStart(Wrapper wrapper)
    {
        Debug.Log("speakStart: " + wrapper);
        gestureController.done = false;
        gestureController.isVerticalSlide = false;
        Debug.Log(gestureController.done);
    }
    private void speakComplete(Wrapper wrapper)
    {
        Debug.LogWarning("speakComplete: " + wrapper);
    }

}