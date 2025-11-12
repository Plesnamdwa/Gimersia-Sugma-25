using UnityEngine;

public class TrashMinigameUI : MonoBehaviour
{
    public GameObject pake_ikan;
    public GameObject gapake_ikan;

    private int currentIndex;
    private bool fishTaken = false;

    public void OpenMinigame(int index) //ditentuin ada ikannya apa ngga tuh disini, btw disini juga udah ambil indeksnya
    {
        currentIndex = index;

        bool hasFish = TrashMinigameManager.instance.HasFishInTrash(index);
        fishTaken = !TrashMinigameManager.instance.HasFishInTrash(currentIndex);
        pake_ikan.SetActive(hasFish && !fishTaken);
        gapake_ikan.SetActive(!hasFish || fishTaken);

        

        if (pake_ikan.activeSelf)
        {
            Debug.Log("Minigame dibuka! Ikan ada di tong " + currentIndex);
        }
        else
        {
            Debug.Log("Minigame dibuka! Tidak ada ikan di tong " + currentIndex);
        }

        gameObject.SetActive(true);
    }

    public void TutupMinigame()
    {
        pake_ikan.SetActive(false);
        gapake_ikan.SetActive(false);
    }

    public void RemoveFish()
    {
        Debug.Log("Ikan di tong " + currentIndex + " diambil.");
        TrashMinigameManager.instance.RemoveFishFromTrash(currentIndex);
        TrashMinigameManager.instance.AddScore(1);
        pake_ikan.SetActive(false);
        gapake_ikan.SetActive(false);
        Time.timeScale = 1;

    }

    
}
        
