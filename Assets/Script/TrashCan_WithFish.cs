using UnityEngine;
using UnityEngine.UI;

public class TrashCan_WithFish : MonoBehaviour
{
    [Header("Panel Manager")]
    public TrashMinigameUI TrashMinigameUI;

    [Header("Isi Panel")]
    public TrashPiece[] pieces;
    public GameObject fishObject;

    private Vector3[] initialPositions;
    private bool[] initialActiveStates;

    private void Awake()
    {
        initialPositions = new Vector3[pieces.Length];
        initialActiveStates = new bool[pieces.Length];

        for (int i = 0; i < pieces.Length; i++)
        {
            initialPositions[i] = pieces[i].transform.localPosition;
            initialActiveStates[i] = pieces[i].gameObject.activeSelf;
        }
    }
    private void OnDisable()
    {
        ResetPanel();
    }

    public void ResetPanel()
    {
        for (int i = 0; i < pieces.Length; i++)
        {
            pieces[i].transform.localPosition = initialPositions[i];
            pieces[i].gameObject.SetActive(initialActiveStates[i]);
        }

        if (fishObject != null)
            fishObject.SetActive(true);
    }

    public void OnFishClicked()
    {
        TrashMinigameUI.RemoveFish();
    }


    //private void Start()
    //{
    //    //foreach (var p in pieces)
    //    //    if (p != null)
    //    //        p.panelWithFish = this;

    //    for (int i = 0; i < pieces.Length; i++)
    //    {
    //        pieces[i].gameObject.SetActive(true);
    //    }

    //}

    public void OnPieceRemoved()
    {
        // belum diisi apa-apa -_-
    }

    

}
