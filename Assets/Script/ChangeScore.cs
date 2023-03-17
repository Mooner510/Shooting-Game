using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChangeScore : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        var text = gameObject.GetComponent<Text>();
        for(var i = 0f; i < 1; i += Time.deltaTime)
        {
            yield return null;
            transform.position += 0.4f * Time.deltaTime * Vector3.up;
            var color = text.color;
            color.a = (2 - i) / 2;
            text.color = color;
        }
        Destroy(gameObject);
    }
}
