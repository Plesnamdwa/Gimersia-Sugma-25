using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TrashPiece : MonoBehaviour
{
    public static TrashPiece Instance;

    public Button button;
    [HideInInspector] public TrashCan_WithFish panelWithFish;
    [HideInInspector] public TrashCan_Empty panelEmpty;

    [Header("Konfigurasi Gerakan")]
    public float moveDistance = 5000f;
    public float moveDuration = 0.56f;

    public bool moved = false;
    private Vector3 initialPosition;

    public void Awake()
    {
        gameObject.SetActive(true);
        RectTransform rt = GetComponent<RectTransform>();
        initialPosition = rt.localPosition;
    }


    private void OnEnable()
    {
        this.gameObject.SetActive(true);
        RectTransform rt = GetComponent<RectTransform>();
        rt.localPosition = initialPosition;
        moved = false;
    }


    private void Start()
    {
        if (button != null)
            button.onClick.AddListener(OnClick);
        gameObject.SetActive(true);
    }

    public void OnClick()
    {
        if (moved) return;
        StartCoroutine(MoveAndHide());
        moved = false;
    }

    private IEnumerator MoveAndHide()
    {
        moved = true;
        RectTransform rt = GetComponent<RectTransform>();
        Vector3 start = rt.localPosition;

        Canvas canvas = rt.GetComponent<Canvas>();
        float scaleFactor = canvas ? canvas.scaleFactor : 1f;

        Vector3 dir = Vector3.zero;
        int rand = Random.Range(0, 4);
        switch (rand)
        {
            case 0: dir = Vector3.right; break;
            case 1: dir = Vector3.left; break;
            case 2: dir = Vector3.up; break;
            case 3: dir = Vector3.down; break;
        }

        Vector3 target = start + dir * moveDistance;
        float t = 0f;

        while (t < moveDuration)
        {
            t += Time.unscaledDeltaTime; // gunakan unscaled supaya UI animation tetap meski Time.timeScale berubah
            rt.anchoredPosition = Vector3.Lerp(start, target, t / moveDuration);
            yield return null;
        }

        if (panelWithFish != null)
            panelWithFish.OnPieceRemoved();
        else if (panelEmpty != null)
            panelEmpty.OnPieceRemoved();


        gameObject.SetActive(false);
    }
}
