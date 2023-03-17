using System.Collections;
using UnityEngine;

public abstract class Removal : MonoBehaviour
{
    private bool shown;

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
        //if (gameObject.activeSelf)
        //{
        //    shown = false;
        //    StartCoroutine(Remove());
        //}
    }

    private void OnBecameVisible() => shown = true;

    private IEnumerator Remove()
    {
        for (var i = 0f; i < 1f; i += Time.deltaTime)
        {
            yield return null;
            if (shown) yield break;
        }
        Destroy(gameObject);
    }
}