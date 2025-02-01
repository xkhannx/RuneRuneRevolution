using System.Collections;
using UnityEngine;

public class Note : MonoBehaviour
{
    public int noteInd;
    [SerializeField] GameObject blood;
    NoteSpawner spawner;
    PaddleController paddle;
    RuneController swiper;
    float xPos;
    void Start()
    {
        spawner = GetComponentInParent<NoteSpawner>();
        paddle = FindObjectOfType<PaddleController>();
        swiper = FindObjectOfType<RuneController>();
        xPos = transform.position.x;
    }

    float timer = 0;
    bool dead = false;
    void Update()
    {
        if (dead) return;

        timer += Time.deltaTime;

        Vector3 pos = spawner.start.position + (spawner.finish.position - spawner.start.position) * timer / (0.58f * spawner.scrollTime);
        pos.x = xPos;
        transform.position = pos;

        if (transform.position.y <= spawner.death.position.y)
        {
            spawner.HeatBase();
            Die(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Paddle"))
        {
            if (noteInd == swiper.curRuneInd)
            {
                paddle.ShakePaddle();
                Die(true);
            }
        }
    }

    void Die(bool win)
    {
        dead = true;
        Vector3 pos = transform.position;
        pos.z = -2.5f;
        if (!win)
        {
            Instantiate(blood, pos, Quaternion.identity);
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(NoteCaught());
        }
    }

    IEnumerator NoteCaught()
    {
        float tTotal = 0.1f;
        float t = tTotal;
        float curScale = transform.localScale.x;
        while (t > 0)
        {
            t -= Time.deltaTime;
            transform.localScale = Vector3.one * t / tTotal * curScale;
            yield return null;
        }
        Destroy(gameObject);
    }
}
