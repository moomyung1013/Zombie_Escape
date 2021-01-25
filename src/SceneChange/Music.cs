using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Music : MonoBehaviour
{
    AudioSource m_MyAudioSource;
    bool music = true;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
        m_MyAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if ((Application.loadedLevel ==5) || (Application.loadedLevel == 6)|| (Application.loadedLevel == 7))
        {
            //Destroy(transform.gameObject);
            m_MyAudioSource.Stop();
            music = false;


        }
        if (music == false)
        {
            if ((Application.loadedLevel == 0) || (Application.loadedLevel == 1) || (Application.loadedLevel == 2) || (Application.loadedLevel == 3) || (Application.loadedLevel == 4))
            {
                m_MyAudioSource.Play();
                music = true ;
            }
        }
    }

}
