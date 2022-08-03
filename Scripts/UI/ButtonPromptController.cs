using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonPromptController : MonoBehaviour
{
    [Header("Wait Fill Controls")]
    [SerializeField] float holdIncrement = 0.1f;
    [SerializeField] GameObject waitFillObjectRoot;
    [SerializeField] Image waitFillImage;

    [Header("Prompt Controls")]
    [SerializeField] GameObject promptTextRoot;
    [SerializeField] GameObject promptImageRoot;

    private float timeSinceLastFillAddition;
    public float targetFillAmount;
    private float collectionSpeed;
    private Tween fillTween;
    private TextMeshProUGUI promptText;
    private Image promptImage;

    private void Start()
    {
        // Necessary for initilizing TMPro.
        promptText = promptTextRoot.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        promptImage = promptImageRoot.transform.GetChild(0).GetComponent<Image>();

        SetPromptActive(true);
        SetPromptActive(false);

        SetWaitFillActive(true);
        SetWaitFillActive(false);

        ResetFill();

        collectionSpeed = GameManager.Instance.CollectionSpeed * holdIncrement;
    }

    public void SetPromptText(string text)
    {
        promptText.SetText(text);

        promptTextRoot.SetActive(true);
        promptImageRoot.SetActive(false);
    }

    public void SetPromptSprite(Sprite sprite)
    {
        promptImage.sprite = sprite;

        promptTextRoot.SetActive(false);
        promptImageRoot.SetActive(true);
    }

    public bool IncrementFill()
    {
        if (Time.time - timeSinceLastFillAddition >= collectionSpeed)
        {
            Debug.Log("Incremented Fill.");
            fillTween = waitFillImage.DOFillAmount(waitFillImage.fillAmount + holdIncrement, collectionSpeed).SetEase(Ease.Linear);

            if (waitFillImage.fillAmount >= 1f)
            {
                ResetFill();
                return true;
            }

            timeSinceLastFillAddition = Time.time;
        }

        return false;
    }

    public void ResetFill()
    {
        targetFillAmount = 0;
        waitFillImage.fillAmount = 0;

        fillTween.Kill();

        timeSinceLastFillAddition = Time.time - collectionSpeed;
    }

    public void SetPromptActive(bool state)
    {
        promptImageRoot.SetActive(state);
        promptTextRoot.SetActive(state);
    }

    public void SetWaitFillActive(bool state)
    {
        waitFillObjectRoot.SetActive(state);
    }
}
