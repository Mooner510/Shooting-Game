using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Entity : Removal
{
    [SerializeField] private GameObject Arrow;
    [SerializeField] private GameObject Item;
    [SerializeField] private Slider Slider;

    private Slider slider;
    private SpriteRenderer spriteRenderer;
    private float downSpeed;

    private float health;
    private float maxHealth;
    private float damage;
    private Color defaultColor;

    public void RegisterSlider(Slider slider) => this.slider = slider;

    public void RemoveHealth(float hp)
    {
        health -= hp;
        if(slider == null)
        {
            slider = Instantiate(Slider, GameObject.Find("Canvas").transform);
        }
        slider.value = Mathf.Max(health, 0) / maxHealth;
        if (health <= 0)
        {
            DataManager.AddScore(Mathf.FloorToInt(maxHealth / 40 * Mathf.Max(damage / 5, 3)));
            StartCoroutine(Death());
        }
    }

    private IEnumerator Death()
    {
        Destroy(GetComponent<CircleCollider2D>());
        Destroy(slider.gameObject);
        if (Random.value < (0.8 - 0.15 * DataManager.Weapon)) Instantiate(Item, transform.position, Quaternion.identity);
        var spriteRenderer = GetComponent<SpriteRenderer>();
        for (var i = 0f; i < 1; i += Time.deltaTime)
        {
            var color = spriteRenderer.color;
            color.a = -i / 2;
            spriteRenderer.color = color;
            yield return null;
        }
        Destroy(gameObject);
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        var ran = Random.value;
        if(ran < 0.6)
        {
            downSpeed = Random.Range(1.5f, 5f);
            maxHealth = Random.Range(50f, 300f);
            damage = Random.Range(8f, 25f);
            defaultColor = Color.white;
        }
        else if(ran < 0.85)
        {
            downSpeed = Random.Range(1f, 3f);
            maxHealth = Random.Range(300f, 600f);
            damage = Random.Range(4f, 12.5f);
            defaultColor = new Color(0.65f, 0.65f, 1f);
        }
        else
        {
            downSpeed = Random.Range(0.6f, 2f);
            maxHealth = Random.Range(600f, 1000f);
            damage = Random.Range(2f, 8f);
            defaultColor = new Color(0.65f, 1f, 0.65f);
        }
        spriteRenderer.color = defaultColor;
        health = maxHealth;
        if(Arrow != null) StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        yield return new WaitForSeconds(Random.Range(1.5f, 5f));
        if (health <= 0) yield break;
        var arrow = Instantiate(Arrow, transform.localPosition, Quaternion.identity).GetComponent<Arrow>();
        arrow.isEnemyShooted = true;
        arrow.damage = damage / 2;
        arrow.speed = downSpeed * 2.5f;
        StartCoroutine(Run());
    }

    private void OnDestroy()
    {
        if(slider != null) Destroy(slider.gameObject);
    }

    private static readonly Color damaged = new(1, 0.8f, 0.8f, 1);

    public void Hit()
    {
        if(glowed || health <= 0) return;
        StartCoroutine(Glowing());
    }

    private bool glowed;

    private IEnumerator Glowing()
    {
        glowed = true;
        spriteRenderer.color = damaged;
        for(var i = 0f; i <= 0.5; i += Time.deltaTime)
        {
            yield return null;
            spriteRenderer.color += i * 2 * (defaultColor - spriteRenderer.color);
        }
        glowed = false;
    }

    public void Damage()
    {
        DataManager.Damage(damage);
        if(DataManager.IsDeath())
        {
            SceneManager.LoadScene("End");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DataManager.Damage(damage);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if(health <= 0) return;
        transform.Translate(downSpeed * Time.deltaTime * Vector3.down);
        if(slider) slider.transform.position = transform.position + new Vector3(0, transform.localScale.x);
    }
}