using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class RuneController : MonoBehaviour
{
    [SerializeField] Transform[] runes;
    [SerializeField] Transform[] curNoteSprites;
    [SerializeField] Color[] runeColors;
    [SerializeField] LayerMask trackPointLayer;
    [SerializeField] Text runeTimerText;

    PaddleController paddle;
    Camera cam;
    NoteSpawner note;
    private void Start()
    {
        cam = Camera.main;
        note = FindObjectOfType<NoteSpawner>();
        paddle = FindObjectOfType<PaddleController>();
        paddle.lightningColor = runeColors[0];

        curRuneInd = -1;
        canDraw = true;
        curRune = NewRune(runes[0]);
    }

    private Transform NewRune(Transform runePrefab)
    {
        curTrackPoint = 0;

        Transform rune = Instantiate(runePrefab, transform.position, transform.rotation, transform);

        rune.GetChild(0).localScale = Vector3.one * 4f;
        rune.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
        rune.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = Color.black;
        for (int i = 1; i < rune.childCount; i++)
        {
            rune.GetChild(i).GetComponent<SphereCollider>().enabled = false;
            rune.GetChild(i).GetComponent<SpriteRenderer>().color = Color.black;
            if (i < rune.childCount - 1) 
                rune.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>().color = Color.black;
        }

        return rune;
    }

    public Transform curRune;
    public int curRuneInd = -1;
    int curTrackPoint;
    public bool firstDrawn = false;
    bool canDraw = false;

    private void Update()
    {
        if (drawingRune) runeTimer += Time.deltaTime;
        if (note.noteChanged)
        {
            note.noteChanged = false;

            if (curRune != null) Destroy(curRune.gameObject);

            if (note.curNote != curRuneInd)
            {
                curRune = NewRune(runes[note.curNote]);
                
                canDraw = true;
            }
            else
            {
                canDraw = false;
            }
        }

        if (Input.GetMouseButton(0) && canDraw)
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            if (mousePos.y < -2.41f && mousePos.x < 0.71f)
            {
                RaycastHit hit;

                if (Physics.Raycast(mousePos, Vector3.forward, out hit, 20, trackPointLayer))
                {
                    if (!drawingRune)
                    {
                        drawingRune = true;
                        runeTimer = 0;
                    } 
                    Transform rune = hit.transform.parent;

                    hit.transform.localScale = Vector3.one * 3f;
                    hit.transform.GetComponent<SpriteRenderer>().color = runeColors[note.curNote];
                    hit.transform.GetComponent<SphereCollider>().enabled = false;

                    if (curTrackPoint < rune.childCount - 1)
                        hit.transform.GetChild(0).GetComponent<SpriteRenderer>().color = runeColors[note.curNote];

                    curTrackPoint++;

                    if (curTrackPoint == rune.childCount)
                    {
                        if (curRuneInd >= 0)
                            curNoteSprites[curRuneInd].gameObject.SetActive(false);

                        curRuneInd = note.curNote;
                        
                        curNoteSprites[curRuneInd].gameObject.SetActive(true);
                        paddle.lightningColor = runeColors[curRuneInd];

                        canDraw = false;
                        drawingRune = false;
                        runeTimerText.text = runeTimer.ToString();
                        if (!firstDrawn) firstDrawn = true;
                        return;
                    }

                    rune.GetChild(curTrackPoint).localScale = Vector3.one * 4f;
                    rune.GetChild(curTrackPoint).GetComponent<SpriteRenderer>().color = Color.red;
                    if (curTrackPoint < rune.childCount - 1)
                        rune.GetChild(curTrackPoint).GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
                    rune.GetChild(curTrackPoint).GetComponent<SphereCollider>().enabled = true;
                }
            }
        }
    }

    bool drawingRune;
    float runeTimer;
}
