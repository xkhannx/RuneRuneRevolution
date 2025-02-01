using UnityEngine;

public class PaddleController : MonoBehaviour
{
    [SerializeField] Animator paddle;
    [SerializeField] Transform line1;
    [SerializeField] Transform line2;
    [SerializeField] Sprite[] handSprites;
    [SerializeField] GameObject[] curLightningSprites;
    GameObject curLightning;
    [SerializeField] SpriteRenderer hand;
    Vector3 zeroPos;
    void Start()
    {
        cam = Camera.main;
        zeroPos = paddle.transform.position;
        zeroPos.x = 0;
    }

    Camera cam;
    private void OnMouseOver()
    {
        if (!Input.GetMouseButton(0))
        {
            if (touchStart) touched = true;
            paddle.transform.position += Vector3.left * 20;
            if (curLightning != null)
                curLightning.SetActive(false);
            return;
        }
        
        touchStart = true;

        Vector3 pos = zeroPos;
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (mousePos.x <= line1.position.x)
        {
            pos.x = -1.35f;
            hand.sprite = handSprites[0];
            if (curLightning != null)
                curLightning.SetActive(false);
            curLightning = curLightningSprites[0];
            curLightning.SetActive(true);
            curLightning.GetComponent<SpriteRenderer>().color = lightningColor;
        }
        else if (mousePos.x >= line2.position.x)
        {
            pos.x = 1.35f;
            hand.sprite = handSprites[2];
            if (curLightning != null)
                curLightning.SetActive(false);
            curLightning = curLightningSprites[2];
            curLightning.SetActive(true);
            curLightning.GetComponent<SpriteRenderer>().color = lightningColor;
        }
        else
        {
            pos = zeroPos;
            hand.sprite = handSprites[1];
            if (curLightning != null)
                curLightning.SetActive(false);
            curLightning = curLightningSprites[1];
            curLightning.SetActive(true);
            curLightning.GetComponent<SpriteRenderer>().color = lightningColor;
        }
        paddle.transform.position = pos;
    }
    public Color lightningColor;
    public bool touched = false;
    bool touchStart = false;

    private void OnMouseExit()
    {
        if (curLightning != null)
            curLightning.SetActive(false);
        paddle.transform.position += Vector3.left * 20;
        if (touchStart) touched = true;
    }

    public void ShakePaddle()
    {
        paddle.SetTrigger("Shake");
        FindObjectOfType<MusicFX>().MuteSong(false);
    }
}
