using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Canvas Canvas;
    [SerializeField] private Slider Slider;
    [SerializeField] private float speed;
    [SerializeField] private GameObject[] _monster;
    [SerializeField] private float[] _spawnDelay;

    private void Start()
    {
        for (int i = 0; i < _monster.Length; i++)
        {
            StartCoroutine(Spawn(_monster[i], _spawnDelay[i]));
        }
        StartCoroutine(FuelDecrement());
    }

    private IEnumerator FuelDecrement()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            DataManager.RemoveFuel(1);
        }
    }

    private IEnumerator Spawn(GameObject gameObject, float delay)
    {
        yield return new WaitForSeconds((delay + Random.Range(-delay * 0.4f, delay * 0.4f)) * speed);
        var camera = _camera.transform.position;
        Instantiate(gameObject, new Vector2(Random.Range(-16f, 16f) + camera.x, Random.Range(6f, 7f) + camera.y), Quaternion.identity);
        StartCoroutine(Spawn(gameObject, delay));
    }
}
