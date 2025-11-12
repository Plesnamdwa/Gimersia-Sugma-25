using UnityEngine;

public class TrashCan_Empty : MonoBehaviour
{
    [Header("Isi Panel")]
    public TrashPiece[] pieces;

    private bool[] initialActiveStates;

    private void Awake()
    {
        initialActiveStates = new bool[pieces.Length];

        for (int i = 0; i < pieces.Length; i++)
        {
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
            pieces[i].gameObject.SetActive(initialActiveStates[i]);
        }

    }

    //private void Start()
    //{
    //    for (int i = 0; i < pieces.Length; i++)
    //    {
    //        pieces[i].gameObject.SetActive(true);
    //    }

    //}

    public void OnPieceRemoved()
    {
        //masih kosong guys
    }
}