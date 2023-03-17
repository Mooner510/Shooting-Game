using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private Text scoreChangeText;
    [SerializeField] private Text healthText;
    [SerializeField] private Text fuelText;
    [SerializeField] private Text stageText;
    [SerializeField] private Text nextStageText;
    [SerializeField] private Slider scoreBar;
    [SerializeField] private int[] scoreAnchor;
    
    private void FixedUpdate()
    {
        fuelText.text = $"Fuel: {Mathf.Max(DataManager.Fuel, 0):0}";
    }

    public void UpdateHealthText()
    {
        healthText.text = $"HP: {Mathf.Max(DataManager.Health, 0):0}";
    }

    public void UpdateScoreText(int increment)
    {
        if(DataManager.Score <= 0)
        {
            scoreBar.
        }
        scoreBar.value = DataManager.Score / scoreAnchor[DataManager.Stage - 1];
        scoreText.text = $"{DataManager.Score:0}";
        if (DataManager.Stage != 2) nextStageText.text = $"Next Stage: {scoreAnchor[DataManager.Stage - 1]} Score";
        else if(nextStageText.enabled) nextStageText.enabled = false;
        if(increment != 0) Instantiate(scoreChangeText, GameObject.Find("Canvas").transform).GetComponent<Text>().text = $"+ {increment}";
    }

    public void UpdateStageText()
    {
        stageText.text = $"Stage {DataManager.Stage:0}";
        UpdateScoreText(0);
        StartCoroutine(StageText());
    }

    private IEnumerator StageText()
    {
        stageText.rectTransform.position = Vector3.zero;
        stageText.fontSize = 100;
        var color = new Color(1, 1, 1, 0);
        for(var i = 0f; i < 2; i += Time.deltaTime)
        {
            stageText.color = color;
            color.a = i / 2;
            yield return null;
        }
        color.a = 1;
        stageText.color = color;
        yield return new WaitForSeconds(1);
        for (var i = 2f; i >= 0; i -= Time.deltaTime)
        {
            stageText.color = color;
            color.a = i / 2;
            yield return null;
        }
        color.a = 0;
        stageText.color = color;
        stageText.fontSize = 50;
        yield return new WaitForSeconds(0.5f);
        var pos = new Vector3(0, 500);
        stageText.rectTransform.localPosition = new Vector3(0, 600);
        color.a = 1;
        stageText.color = color;
        for (var i = 0f; i < 2; i += Time.deltaTime)
        {
            stageText.rectTransform.localPosition = Vector3.Lerp(stageText.rectTransform.localPosition, pos, i / 2);
            yield return null;
        }
    }
}
