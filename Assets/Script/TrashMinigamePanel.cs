using UnityEngine;

public class TrashMinigamePanel : MonoBehaviour
{
    [Header("Identitas Tong Sampah")]
    public int trashCanIndex = 0;

    [Header("Isi Panel")]
    public TrashPiece[] pieces;
    public GameObject fishObject;

    private int removedCount = 0;
    private bool hasFish = false;
    private bool fishAlreadyTaken = false;

    private TrashMinigameManager manager;

    //private void Start()
    //{
    //    manager = TrashMinigameManager.instance;

    //    if (fishObject != null)
    //        fishObject.SetActive(false);

    //    if (manager != null)
    //        hasFish = manager.HasFishInTrash(trashCanIndex);

    //    fishAlreadyTaken = PlayerPrefs.GetInt("FishTaken_Can_" + trashCanIndex, 0) == 1;

    //    if (fishAlreadyTaken)
    //        hasFish = false;

    //    // kasih referensi balik ke setiap piece
    //    foreach (TrashPiece piece in pieces)
    //    {
    //        if (piece != null)
    //            piece.panel = this;
    //    }
    //}

    private void Start()
    {
        manager = TrashMinigameManager.instance;

        if (fishObject != null)
            fishObject.SetActive(false);

        // ambil status ikan dari manager, bukan langsung PlayerPrefs
        hasFish = manager.HasFishInTrash(trashCanIndex);

        // cek apakah ikan sudah diambil sebelumnya
        fishAlreadyTaken = PlayerPrefs.GetInt("FishTaken_Can_" + trashCanIndex, 0) == 1;

        // kasih referensi balik ke setiap piece
        foreach (TrashPiece piece in pieces)
        {
            //if (piece != null)
            //    piece.panel = this;
        }
    }

    public void OnPieceRemoved()
    {
        removedCount++;

        // kalau semua sampah udah diklik
        if (removedCount >= pieces.Length)
        {
            if (hasFish && !fishAlreadyTaken)
            {
                ShowFish();
            }
            else
            {
                Debug.Log("Tidak ada ikan di tong ini!");
            }
        }
    }

    private void ShowFish()
    {
        if (fishObject != null)
            fishObject.SetActive(true);

        fishAlreadyTaken = true;
        PlayerPrefs.SetInt("FishTaken_Can_" + trashCanIndex, 1);
        PlayerPrefs.Save();

        if (manager != null)
        {
            manager.RemoveFishFromTrash(trashCanIndex);
            manager.AddScore(1);
        }

        Debug.Log($"Ikan ditemukan di tong #{trashCanIndex}!");
    }
}