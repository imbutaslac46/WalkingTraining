using UnityEngine;
using UnityEngine.UI;

public class HeartRateVisualization : AbstractVisualization, IHideable
{
    public Text heartRateText;
    private CanvasGroup m_canvasGroup;
    private AudioSource m_MyAudioSource;
    private Animator m_animator;

    private int m_heartRateValue;

    private float timer;
    private float interval;

    public bool m_Play;

    public bool Visible
    {
        get
        {
            return m_canvasGroup.alpha > 0.0f;
        }

        set
        {
            m_canvasGroup.alpha = value ? 1.0f : 0.0f;
        }
    }

    public override void UpdateProperty(string propertyName, object value)
    {
        if (propertyName.Equals("value"))
        {
            m_heartRateValue = System.Convert.ToInt32(value);
        }
    }

    private void Awake()
    {
        m_canvasGroup = GetComponent<CanvasGroup>();

        //Fetch the AudioSource from the GameObject
        m_MyAudioSource = GetComponent<AudioSource>();

        m_animator = GetComponent<Animator>();
    }

    private void Start()
    {
        m_heartRateValue = 0;
        heartRateText.text = m_heartRateValue.ToString();
        
        m_Play = false;
        interval = 1.0f; //default is 1/sec
        timer = Time.time;
    }

    private void Update()
    {
        m_heartRateValue = DataStore.Instance.smartex.storage[7];
        if (m_heartRateValue != 0)
        {
            heartRateText.text = m_heartRateValue.ToString();

            // Scale speed of animation based on a reference heart rate (60)
            m_animator.speed = DataStore.Instance.smartex.storage[7] / 60.0f;
            m_animator.SetTrigger("Pump");

            if (m_Play == true && timer + interval <= Time.time)
            {
                timer = Time.time;

                //interval time depends on the heart-rate value
                interval = 60.0f / m_heartRateValue;

                //Play the audio you attach to the AudioSource component
                m_MyAudioSource.Play();
            }
            //Check if you just set the toggle to false
            if (m_Play == false)
            {
                //Stop the audio
                m_MyAudioSource.Stop();
            }
        }
    }
}
