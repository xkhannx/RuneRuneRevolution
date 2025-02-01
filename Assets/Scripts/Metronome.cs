using UnityEngine;

public class Metronome : MonoBehaviour
{
    [SerializeField] float bpm = 120;
    AudioSource ticker;
    [SerializeField] AudioSource music;

    NoteSpawner noteSpawner;
    void Start()
    {
        noteSpawner = FindObjectOfType<NoteSpawner>();
        ticker = GetComponent<AudioSource>();
        period = 60 / bpm;
    }

    float curTime = 0;
    float period;

    int count = 0;
    bool started = false;
    private void FixedUpdate()
    {
        if (!started) return;

        if (count == 0)
        {
            count++;
            music.Play();
        }
        curTime += Time.fixedDeltaTime;
        if (curTime >= period)
        {
            count++;
            noteSpawner.SpawnNote(count);
            
            ticker.Stop();
            ticker.Play();
            curTime -= period;
        }
    }

    public void StartSong()
    {
        started = true;
        curTime = period - 0.08f;
    }
}
