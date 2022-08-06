using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Crosstales.RTVoice;
public class GestureController : MonoBehaviour
{

    public bool isHorizontalSlide;
    public bool isVerticalSlide;
    public Vector2 fingerDown;
    public Vector2 fingerUp;
    public bool detectSwipeOnlyAfterRelease = false;
    public float SWIPE_THRESHOLD = 20f;
    public static bool isTappingDone;



    public IEnumerator DoneSlidingHorizontal()
    {
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => isHorizontalSlide == true);
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => Speaker.Instance.isSpeaking == false);
        isHorizontalSlide = false;
    }
    public IEnumerator DoneSlidingVertical()
    {
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => isVerticalSlide == true);
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => Speaker.Instance.isSpeaking == false);
        isVerticalSlide = false;
    }
    public bool done = false;

    public IEnumerator WaitForTap()
    {
        yield return new WaitForSeconds(0.5f);
        // while (Speaker.Instance.isSpeaking == false && done == false)
        // {
        //     done = true;
        yield return new WaitUntil(() => done == true);
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => Speaker.Instance.isSpeaking == false);
        done = false;
        // }
        // // return;
        // while (!done)
        // {
        //     done = true;

        //     yield return null;
        // }
    }
    public void checkSwipe(People people)
    {
        Debug.Log("checkSwipe is on");
        //Check if Vertical swipe
        if (verticalMove() > SWIPE_THRESHOLD && verticalMove() > horizontalValMove())
        {
            Debug.Log("Vertical");
            if (fingerDown.y - fingerUp.y > 0)//up swipe
            {
                Debug.Log("North");
                OnSwipeUp();
            }
            else if (fingerDown.y - fingerUp.y < 0)//Down swipe
            {
                Debug.Log("South");
                OnSwipeDown();
            }
            fingerUp = fingerDown;
            Resource.hasPeople = false;

            DoneSlidingVertical();
        }

        //Check if Horizontal swipe
        else if (horizontalValMove() > SWIPE_THRESHOLD && horizontalValMove() > verticalMove())
        {
            // GameHandler.RandomizePeople = people[Random.Range(0, people.Length)];
            // Debug.Log(RandomizePeople.name);      
            // Debug.Log("0000000");
            Debug.Log("Horizontal");
            if (fingerDown.x - fingerUp.x > 0)//Right swipe
            {
                Speaker.Instance.Speak("You have accept the offer!", null, Speaker.Instance.VoiceForName(GameHandler.RandomizePeople.voiceName));
                // Debug.Log("11111111");
                OnSwipeRight(people);
            }
            else if (fingerDown.x - fingerUp.x < 0)//Left swipe
            {
                Speaker.Instance.Speak("You have decline the offer!", null, Speaker.Instance.VoiceForName(GameHandler.RandomizePeople.voiceName));
                // Debug.Log("22222222");
                OnSwipeLeft(people);
            }
            // Ini manggil getCurrentGold getCurrentBlabla (PAKE TEXT TO SPEECH PLUGIN KALAU BISA)

            Resource.hasPeople = false;
            fingerUp = fingerDown;
            DoneSlidingHorizontal();
        }

        else
        {
            Resource.hasPeople = false;
            if (Resource.CurrentScene == "gameArea")
            {
                StartCoroutine(SpeakDescription());
            }
            if (Resource.CurrentScene == "kingdom information")
            {
                StartCoroutine(SpeakResource());
            }
            // Debug.Log("NoSwipe");
            // WaitForTap();
            isTappingDone = true;
        }
    }
    public GameHandler gameHandler;
    public IEnumerator RandomNewPeople()
    {
        if (Resource.hasPeople == false && Resource.GameOver == false && GameHandler.SpeakFirstTime == false)
        {
            if(gameHandler == null){
                gameHandler = gameObject.GetComponent<GameHandler>();
            }
            yield return GameHandler.RandomizePeople = gameHandler.people[UnityEngine.Random.Range(0, gameHandler.people.Length)];
            // Speaker.Instance.Speak(RandomizePeople.speak, null, Speaker.Instance.VoiceForName(Resource.voiceName));
            Resource.hasPeople = true;
            yield return null;
        }
    }

    public IEnumerator SpeakResource()
    {
        if (GameHandler.SpeakFirstTime == false)
        {
            Debug.Log("Speak resource ongoing");
            yield return Speaker.Instance.Speak("You have :" + Resource.GetGoldAmount().ToString(), null, Speaker.Instance.VoiceForName(GameHandler.RandomizePeople.voiceName));
            yield return new WaitForSeconds(0.5f);
            yield return new WaitUntil(() => Speaker.Instance.isSpeaking == false);
            yield return Speaker.Instance.Speak("Gold. You have :" + Resource.GetPopulationAmount().ToString(), null, Speaker.Instance.VoiceForName(GameHandler.RandomizePeople.voiceName));
            yield return new WaitForSeconds(0.5f);
            yield return new WaitUntil(() => Speaker.Instance.isSpeaking == false);
            yield return Speaker.Instance.Speak("Population. You have :" + Resource.GetPublicOpinionAmount().ToString(), null, Speaker.Instance.VoiceForName(GameHandler.RandomizePeople.voiceName));
            yield return new WaitForSeconds(0.5f);
            yield return new WaitUntil(() => Speaker.Instance.isSpeaking == false);
            yield return Speaker.Instance.Speak("Public Opinion", null, Speaker.Instance.VoiceForName(GameHandler.RandomizePeople.voiceName));
        }
        yield return null;
    }
    public IEnumerator SpeakDescription()
    {
        yield return new WaitUntil(() => Speaker.Instance.isSpeaking == false);
        Debug.Log(GameHandler.RandomizePeople.description);
        yield return StartCoroutine(speakSpeak());
        yield return StartCoroutine(speakDescription());
    }

    public IEnumerator speakSpeak()
    {
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => Speaker.Instance.isSpeaking == false);
        yield return Speaker.Instance.Speak(GameHandler.RandomizePeople.speak, null, Speaker.Instance.VoiceForName(GameHandler.RandomizePeople.voiceName));
    }
    public IEnumerator speakDescription()
    {
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => Speaker.Instance.isSpeaking == false);
        yield return Speaker.Instance.Speak(GameHandler.RandomizePeople.description, null, Speaker.Instance.VoiceForName(GameHandler.RandomizePeople.voiceName));
    }



    void OnSwipeRight(People people)
    {
        // Accept Offer
        Debug.Log("Accept Offer");
        Resource.AddResourceAmount(Resource.ResourceType.Gold, people.gold);
        Resource.AddResourceAmount(Resource.ResourceType.Population, people.population);
        Resource.AddResourceAmount(Resource.ResourceType.publicOpinion, people.publicOpinion);
        StartCoroutine(RandomNewPeople());
        Debug.Log("Current Gold: " + Resource.GetGoldAmount());
    }

    void OnSwipeLeft(People people)
    {
        // Reject Offer
        Debug.Log("Reject Offer");
        Resource.AddResourceAmount(Resource.ResourceType.Gold, people.goldNegative);
        Resource.AddResourceAmount(Resource.ResourceType.Population, people.populationNegative);
        Resource.AddResourceAmount(Resource.ResourceType.publicOpinion, people.publicOpinionNegative);
        StartCoroutine(RandomNewPeople());
        Debug.Log("Current Gold: " + Resource.GetGoldAmount());
    }


    void CheckScene()
    {
        Debug.Log("Check Scene is On");
        StartCoroutine(thisIsGameArea());
        StartCoroutine(thisIsKingdomInformation());
    }

    public IEnumerator thisIsGameArea()
    {
        if (Resource.CurrentScene == ("kingdom information"))
        {
            yield return new WaitForSeconds(0.5f);
            yield return new WaitUntil (() => Speaker.Instance.isSpeaking == false);
            yield return Speaker.Instance.Speak("This is the game area!", null, Speaker.Instance.VoiceForName(GameHandler.RandomizePeople.voiceName));
            Debug.Log("Change scene to game area");
            Resource.CurrentScene = "gameArea";
        }
        else
        {
            yield return null;
        }
    }
    public IEnumerator thisIsKingdomInformation()
    {
        if (Resource.CurrentScene == ("gameArea"))
        {
            yield return new WaitForSeconds(0.5f);
            yield return new WaitUntil (() => Speaker.Instance.isSpeaking == false);
            yield return Speaker.Instance.Speak("This is the kingdom information!", null, Speaker.Instance.VoiceForName(GameHandler.RandomizePeople.voiceName));
            Debug.Log("Change scene to kingdom information");
            yield return StartCoroutine(RandomNewPeople());
            Resource.CurrentScene = "kingdom information";
        }
        else
        {
            yield return null;
        }
    }

    public void OnSwipeUp()
    {
        CheckScene();
        Debug.Log("Swipe up");

    }

    public void OnSwipeDown()
    {
        CheckScene();
        Debug.Log("Swipe down");
    }

    public float verticalMove()
    {
        return Mathf.Abs(fingerDown.y - fingerUp.y);
    }

    public float horizontalValMove()
    {
        return Mathf.Abs(fingerDown.x - fingerUp.x);
    }


}

