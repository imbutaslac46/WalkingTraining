//This script allows you to toggle music to play and stop.
//Assign an AudioSource to a GameObject and attach an Audio Clip in the Audio Source. Attach this script to the GameObject.

using UnityEngine;

public class HeartSoundPlayer : MonoBehaviour
{
    AudioSource m_MyAudioSource;
    Animator m_animator;

    private float timer;

    private float interval;
    

    //Play the music
    public bool m_Play;


    void Start()
    {
        //Fetch the AudioSource from the GameObject
        m_MyAudioSource = GetComponent<AudioSource>();
        //Ensure the toggle is set to true for the music to play at start-up
        m_Play = false;
        interval = 1.0f; //default is 1/sec
        timer = Time.time;

        m_animator = GetComponent<Animator>();
    }

    void Update()
    {
        if( DataStore.Instance.smartex.storage[7] != 0)
        {
            //interval time depends on the heart-rate value
            interval = 60.0f / DataStore.Instance.smartex.storage[7];
        }
       
        //play only if play=true and interval time has passed
        if (m_Play==true && timer+interval<=Time.time)
        {
            timer = Time.time;

            //Play the audio you attach to the AudioSource component
            m_MyAudioSource.Play();
            
            // Scale speed of animation based on a reference heart rate (60)
            m_animator.speed = DataStore.Instance.smartex.storage[7] / 60.0f;
            m_animator.SetTrigger("Pump");
          
        }
        //Check if you just set the toggle to false
        if (m_Play == false)
        {
            //Stop the audio
            m_MyAudioSource.Stop();
        }
    }

}