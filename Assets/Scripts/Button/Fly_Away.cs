using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.SceneManagement;
public class UIEffect_FlyOut : MonoBehaviour, IPointerClickHandler
{
    [Header("飞出参数")]
    public float duration = 0.5f;      // 飞行时间
    public float distance = 1500f;     // 向右飞出的距离（像素）
    public bool disableAfterFly = true; // 飞完后禁用按钮

    [Header("飞完后加载场景")]
    public string targetSceneName = ""; 

    private RectTransform rectTransform;
    private Vector2 startPos;

    public string sceneToLoad; // 直接加载场景的名称
    public AudioClip flySound; // 飞行音效


    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 防止重复点击
        if (IsInvoking()) return;
        StartCoroutine(FlyAndLoad());
        if (flySound != null)        {
            AudioSource.PlayClipAtPoint(flySound, Camera.main.transform.position,0.3f);
        }
    }

    IEnumerator FlyAndLoad()
    {
        float elapsed = 0f;
        Vector2 targetPos = startPos + new Vector2(distance, 0);
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            yield return null;
        }
        rectTransform.anchoredPosition = targetPos;

        if (disableAfterFly)
            gameObject.SetActive(false);
        if (!string.IsNullOrEmpty(sceneToLoad))
            SceneManager.LoadScene(sceneToLoad);
    }
}