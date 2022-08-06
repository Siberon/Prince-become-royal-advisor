using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  

[RequireComponent(typeof(AudioSource))]
public class GameController : MonoBehaviour
{
    public Scene scene;
    [SerializeField] AudioClip[] ACPrologue;
    int count = 1;
    AudioSource ASPrologue;
    
    void Awake() {
        ASPrologue = gameObject.AddComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        ContinueAudioCanSkipped();
    }

    public void ContinueAudioCantSkipped()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            
            if (ASPrologue.isPlaying == false){
                count++;
            }
            if (count < ACPrologue.Length)
            {
                if (ASPrologue.isPlaying == false)
                {
                ASPrologue.clip = ACPrologue[count];
                ASPrologue.Play();
                Debug.Log(ACPrologue[count] + "is played " + count);
                Debug.Log("Hahaha" + count);
                }
            }

            else if (count == ACPrologue.Length)
            {
                Debug.Log("Next Scene is Loaded");
                SceneManager.LoadScene (scene.buildIndex +1);
            } 
            
        }
    }

    private void ContinueAudioCanSkipped()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (count < ACPrologue.Length)
            {
                ASPrologue.clip = ACPrologue[count];
                ASPrologue.Play();
                Debug.Log(ACPrologue[count] + "is played " + count);
                Debug.Log("Hahaha" + count);
            }

            else if (count == ACPrologue.Length)
            {
                Debug.Log("Next Scene is Loaded");
                SceneManager.LoadScene (scene.buildIndex +1);
            } 
            
            count++;
        }
    }
}