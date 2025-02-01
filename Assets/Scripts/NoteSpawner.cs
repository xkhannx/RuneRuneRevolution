using System.Collections;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [SerializeField] Note[] notePrefabs;
    public Transform start;
    public Transform finish;
    public Transform death;
    public float scrollTime = 1;

    public int curNote = 0;
    public bool noteChanged;
    public void SpawnNote(int count)
    {
        if ((count + 1) % 8 == 0)
        {
            curNote += Random.Range(0, 2) * 2 - 1;
            if (curNote < 0) curNote += 3;
            else if (curNote > 2) curNote -= 3;
            noteChanged = true;
        }
        if ((count + 1) % 8 < 3)
        {
            return;
        }

        Note newNote = Instantiate(notePrefabs[curNote], start.position, Quaternion.identity, transform);

        int track = Random.Range(0, 3);
        newNote.transform.position = new Vector3(1.4f * (track - 1), start.position.y, 0);
    }

    [SerializeField] Color red;
    [SerializeField] SpriteRenderer deathZone;
    public void HeatBase()
    {
        StopAllCoroutines();
        FindObjectOfType<MusicFX>().MuteSong(true);
        StartCoroutine(FlashRed());
    }

    IEnumerator FlashRed()
    {
        Color initColor = deathZone.color;
        float t = 0, flashDur = 0.125f;
        while (t < flashDur)
        {
            t += Time.deltaTime;
            deathZone.color = initColor + (red - initColor) * t / flashDur;
            yield return null;
        }
        t = 0;
        while (t < flashDur)
        {
            t += Time.deltaTime;
            deathZone.color = red + (initColor - red) * t / flashDur;
            yield return null;
        }
        deathZone.color = initColor;
    }
}
