//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//public class TrashMinigameManager : MonoBehaviour
//{
//    public static TrashMinigameManager instance;

//    [Header("Konfigurasi)")]
//    public int totalTrashCan = 5;
//    private bool[] hasFish;

//    [Header("Gameplay")]
//    public int totalFish = 3;
//    private int score = 0;

//    private void Awake()
//    {
//        if (instance == null) instance = this;
//        else { Destroy(gameObject); return; }

//        hasFish = new bool[totalTrashCan];

//        // baca PlayerPrefs, default false kalau belum ada
//        for (int i = 0; i < totalTrashCan; i++)
//        {
//            hasFish[i] = PlayerPrefs.GetInt("HasFish_Can_" + i, 0) == 1;
//        }

//        // pastikan randomize tetap dijalankan kalau totalFish belum tercapai
//        int currentFish = hasFish.Count(x => x);
//        if (currentFish < totalFish)
//            RandomizeFish();
//    }

//    //private void RandomizeFish()
//    //{
//    //    // hitung berapa ikan sudah ada
//    //    int currentFish = 0;
//    //    for (int i = 0; i < totalTrashCan; i++)
//    //        if (hasFish[i]) currentFish++;

//    //    // jumlah ikan yang perlu ditambahkan
//    //    int fishToPlace = totalFish - currentFish;

//    //    while (fishToPlace > 0)
//    //    {
//    //        int rand = Random.Range(0, totalTrashCan);
//    //        if (!hasFish[rand])
//    //        {
//    //            hasFish[rand] = true;
//    //            PlayerPrefs.SetInt("HasFish_Can_" + rand, 1);
//    //            fishToPlace--;
//    //        }
//    //    }

//    //    PlayerPrefs.Save();
//    //}

//    private void RandomizeFish()
//    {
//        // buat list index tong yang kosong
//        List<int> emptyIndices = new List<int>();
//        for (int i = 0; i < totalTrashCan; i++)
//        {
//            if (!hasFish[i])
//                emptyIndices.Add(i);
//        }

//        // shuffle list
//        for (int i = 0; i < emptyIndices.Count; i++)
//        {
//            int rand = Random.Range(i, emptyIndices.Count);
//            int temp = emptyIndices[i];
//            emptyIndices[i] = emptyIndices[rand];
//            emptyIndices[rand] = temp;
//        }

//        // hitung ikan yang perlu ditambahkan
//        int currentFish = hasFish.Count(x => x);
//        int fishToPlace = totalFish - currentFish;

//        // ambil index pertama sesuai jumlah ikan yang kurang
//        for (int i = 0; i < fishToPlace && i < emptyIndices.Count; i++)
//        {
//            hasFish[emptyIndices[i]] = true;
//            PlayerPrefs.SetInt("HasFish_Can_" + emptyIndices[i], 1);
//        }

//        PlayerPrefs.Save();

//        // debug log
//        string fishPos = "";
//        for (int i = 0; i < hasFish.Length; i++)
//            if (hasFish[i]) fishPos += i + " ";
//        Debug.Log("Ikan di tong: " + fishPos);
//    }

//    public bool HasFishInTrash(int index)
//    {
//        if (index < 0 || index >= hasFish.Length) return false;
//        return hasFish[index];
//    }

//    public void RemoveFishFromTrash(int index)
//    {
//        if (index < 0 || index >= hasFish.Length) return;
//        hasFish[index] = false;
//        PlayerPrefs.SetInt("HasFish_Can_" + index, 0);
//        PlayerPrefs.Save();
//    }

//    public void AddScore(int amount)
//    {
//        score += amount;
//        Debug.Log("Score bertambah! Total: " + score);
//    }

//}

using System.Linq;
using UnityEngine;

public class TrashMinigameManager : MonoBehaviour
{
    public static TrashMinigameManager instance;

    [Header("Konfigurasi")]
    public int totalTrashCan = 5; // jumlah total tong
    public int totalFish = 3;     // jumlah ikan yang muncul di antara tong-tong

    private bool[] hasFish;
    private int score = 0;

    private void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }

        hasFish = new bool[totalTrashCan];

        //  RESET semua data PlayerPrefs lama
        for (int i = 0; i < totalTrashCan; i++)
        {
            PlayerPrefs.DeleteKey("HasFish_Can_" + i);
        }

        PlayerPrefs.Save();

        //  Langsung randomize ikan baru
        RandomizeFish();
    }

    private void RandomizeFish()
    {
        int placed = 0;

        // Pastikan array kosong dulu
        for (int i = 0; i < totalTrashCan; i++)
            hasFish[i] = false;

        // Randomkan lokasi ikan sebanyak totalFish
        while (placed < totalFish)
        {
            int rand = Random.Range(0, totalTrashCan);
            if (!hasFish[rand])
            {
                hasFish[rand] = true;
                PlayerPrefs.SetInt("HasFish_Can_" + rand, 1);
                placed++;
            }
        }

        PlayerPrefs.Save();
        Debug.Log($"{totalFish} ikan diacak dari {totalTrashCan} tong setiap startup.");
        Debug.Log("Ikan tersedia di tong: " + string.Join(", ", Enumerable.Range(0, totalTrashCan).Where(i => hasFish[i])));
    }

    public bool HasFishInTrash(int index)
    {
        if (index < 0 || index >= hasFish.Length) return false;
        return PlayerPrefs.GetInt("HasFish_Can_" + index, 0) == 1;
    }

    public void RemoveFishFromTrash(int index)
    {
        if (index < 0 || index >= hasFish.Length) return;
        hasFish[index] = false;
        PlayerPrefs.SetInt("HasFish_Can_" + index, 0);
        PlayerPrefs.Save();
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log($"Score sekarang: {score}");
    }
}