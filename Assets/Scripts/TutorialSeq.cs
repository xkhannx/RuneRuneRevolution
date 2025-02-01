using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSeq : MonoBehaviour
{
    public GameObject startButton;
    public GameObject noteTutorialText;
    public GameObject runeTutorialText;
    public GameObject notesArea;
    public GameObject runeArea;

    PaddleController paddle;
    void Start()
    {
        noteTutorialText.SetActive(false);
        startButton.SetActive(false);
        runeTutorialText.SetActive(true);
        paddle = FindObjectOfType<PaddleController>();
        paddle.gameObject.SetActive(false);
    }

    bool runeTutOver = false;
    float blinking = 0, blinkDir = 1;
    void Update()
    {
        if (!runeTutOver)
        {
            blinking += Time.deltaTime * blinkDir;

            if (blinking >= 0.5f && blinkDir > 0)
            {
                blinking = 0.5f;
                blinkDir = -1;
            }
            else if (blinking <= 0 && blinkDir < 0)
            {
                blinking = 0;
                blinkDir = 1;
            }
            Color col = runeArea.GetComponent<SpriteRenderer>().color;
            col.a = 1 - blinking;
            runeArea.GetComponent<SpriteRenderer>().color = col;

            if (FindObjectOfType<RuneController>().firstDrawn)
            {
                col.a = 1;
                runeArea.GetComponent<SpriteRenderer>().color = col;
                runeTutorialText.SetActive(false);

                runeTutOver = true;
                noteTutorialText.SetActive(true);
                paddle.gameObject.SetActive(true);
            }
        } else
        {
            blinking += Time.deltaTime * blinkDir;

            if (blinking >= 0.5f && blinkDir > 0)
            {
                blinking = 0.5f;
                blinkDir = -1;
            }
            else if (blinking <= 0 && blinkDir < 0)
            {
                blinking = 0;
                blinkDir = 1;
            }
            Color col = notesArea.GetComponent<SpriteRenderer>().color;
            col.a = 1 - blinking;
            notesArea.GetComponent<SpriteRenderer>().color = col;

            if (FindObjectOfType<PaddleController>().touched)
            {
                col.a = 1;
                notesArea.GetComponent<SpriteRenderer>().color = col;
                noteTutorialText.SetActive(false);
                
                startButton.SetActive(true);
                Destroy(gameObject);
            }
        }
    }
}
