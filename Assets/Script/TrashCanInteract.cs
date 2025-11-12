using UnityEngine;
using UnityEngine.UI; // untuk UI Text / TMP

public class TrashCanInteract : MonoBehaviour
{
    public static TrashCanInteract instance;

    [Header("UI Interaksi")]
    public GameObject interactPrompt; // teks "Tekan E"
    public TrashMinigameUI TrashMinigameUI; // panel minigame

    [Header("Indeks Tong")]
    public int trashCanIndex = 0;

    private bool isPlayerNearby = false;
    //private Movement playerMovement;

    private void Start()
    {
        // Pastikan tulisan "Tekan E" mati di awal
        if (interactPrompt != null)
            interactPrompt.SetActive(false);

        
    }

    private void Update()
    {
        // Kalau player di dekat tong, tampilkan tulisan
        if (isPlayerNearby)
        {
            if (interactPrompt != null && !interactPrompt.activeSelf)
                interactPrompt.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                OpenMinigame(trashCanIndex);
                
            }
        }
        else
        {
            if (interactPrompt != null && interactPrompt.activeSelf)
                interactPrompt.SetActive(false);
        }
    }    

    private void OpenMinigame(int index)
    {
        if (TrashMinigameUI != null)
        {
            Time.timeScale = 0;
            TrashMinigameUI.OpenMinigame(index);
            // Nonaktifkan movement player saat minigame aktif
            //if (playerMovement == null)
            //{
            //    GameObject player = GameObject.FindGameObjectWithTag("Player");
            //    if (player != null)
            //    {
            //        playerMovement = player.GetComponent<Movement>();
            //    }
            //}
            //if (playerMovement != null)
            //{
            //    playerMovement.enabled = false;
            //}
        }
    }

    public void CloseMinigame()
    {
        TrashMinigameUI.TutupMinigame();

        Time.timeScale = 1;
        //TrashPiece.Instance.moved = false;

        //if (playerMovement != null)
        //{
        //    playerMovement.enabled = true;
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Masuk trigger: " + collision.name);
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }
}