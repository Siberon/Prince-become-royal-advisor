using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public Scene scene;
    [SerializeField] AudioClip[] ACPrologue;
    [SerializeField] AudioClip SFX;
    int count = 1;
    AudioSource ASPrologue;
    AudioSource SFXPrologue;

    void Awake()
    {
        SFXPrologue = gameObject.AddComponent<AudioSource>();
        ASPrologue = gameObject.AddComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        scene = SceneManager.GetActiveScene();
        ASPrologue.clip = ACPrologue[0];
        SFXPrologue.volume = 0.5f;
        ASPrologue.Play();
        Resource.CurrentScene = "Welcome";
    }

    // Update is called once per frame
    void Update()
    {
        ContinueAudioCanSkipped();
    }

    // public void ContinueAudioCantSkipped()
    // {
    //     if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
    //     {

    //         if (ASPrologue.isPlaying == false){
    //             count++;
    //         }
    //         if (count < ACPrologue.Length)
    //         {
    //             if (ASPrologue.isPlaying == false)
    //             {
    //             ASPrologue.clip = ACPrologue[count];
    //             ASPrologue.Play();
    //             Debug.Log(ACPrologue[count] + "is played " + count);
    //             Debug.Log("Hahaha" + count);
    //             }
    //         }

    //         else if (count == ACPrologue.Length)
    //         {
    //             Debug.Log("Next Scene is Loaded");
    //             SceneManager.LoadScene("1Tutorial");
    //         } 

    //     }
    // }

    private void ContinueAudioCanSkipped()
    {
        foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    SFXPrologue.clip = SFX;
                    SFXPrologue.Play();
                }
            }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (count < ACPrologue.Length - 1)
            {
                ASPrologue.clip = ACPrologue[count];
                ASPrologue.Play();
                Debug.Log(ACPrologue[count] + "is played " + count);
                
            }

            else if (count < ACPrologue.Length)
            {
                StartCoroutine(WaitForAudio(ASPrologue));
            }

            else if (Resource.CurrentScene == "Tutorial")
            {
                Debug.Log("Next Scene is Loaded");
                StartCoroutine(WaitUntilAudioIsFinished());
            }
            count++;
            Debug.Log(Resource.CurrentScene);
        }
        IEnumerator WaitUntilAudioIsFinished(){
            yield return new WaitUntil (() => SFXPrologue.isPlaying == false);
            SceneManager.LoadScene(scene.buildIndex + 1);
        }

    }

    IEnumerator WaitForAudio(AudioSource audioSource)
    {
        ASPrologue.clip = ACPrologue[count];
        ASPrologue.Play();
        yield return new WaitUntil(() => ASPrologue.isPlaying == false);
        Resource.CurrentScene = "Tutorial";
    }


}