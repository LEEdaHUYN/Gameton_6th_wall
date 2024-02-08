using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{

    [SerializeField]
    Image progressBar;

    [SerializeField] private TMP_Text tooltipText;
    
    [SerializeField]
    private List<string> _tooltipMessage;
    private void Start()
    {
        StartCoroutine(LoadScene());
        int randomToolTipIdx = Random.Range(0, _tooltipMessage.Count);
        tooltipText.text = $"Tip : {_tooltipMessage[randomToolTipIdx]}";
    }
    
    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(Managers.Scene.GetNextScene);
        op.allowSceneActivation = false;

        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;

            timer += Time.deltaTime; if (op.progress < 0.9f) { progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer); if (progressBar.fillAmount >= op.progress) { timer = 0f; } } else { progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer); if (progressBar.fillAmount == 1.0f) { op.allowSceneActivation = true; yield break; } }
        }
    }
}
