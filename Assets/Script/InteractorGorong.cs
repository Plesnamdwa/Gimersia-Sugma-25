using UnityEngine;
using UnityEngine.UI; // Jika Anda ingin menampilkan prompt UI

// Diletakkan pada objek "Gorong-gorong"
public class InteractorGorong : MonoBehaviour
{
    // Referensi ke Manajer Minigame utama
    public ManajerMinigame manajerMinigame;

    // Opsional: UI untuk memberitahu player "Tekan [E] untuk Interaksi"
    public GameObject promptInteraksiUI;

    private bool bisaInteraksi = false;

    void Start()
    {
        // Sembunyikan prompt di awal
        if (promptInteraksiUI != null)
        {
            promptInteraksiUI.SetActive(false);
        }
    }

    // Dipanggil ketika sesuatu MASUK ke trigger collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Periksa apakah itu Player
        if (other.CompareTag("Player"))
        {
            bisaInteraksi = true;
            if (promptInteraksiUI != null)
            {
                promptInteraksiUI.SetActive(true);
            }
        }
    }

    // Dipanggil ketika sesuatu KELUAR dari trigger collider
    private void OnTriggerExit2D(Collider2D other)
    {
        // Periksa apakah itu Player
        if (other.CompareTag("Player"))
        {
            bisaInteraksi = false;
            if (promptInteraksiUI != null)
            {
                promptInteraksiUI.SetActive(false);
            }
        }
    }

    // Periksa input setiap frame
    void Update()
    {
        // Jika player bisa berinteraksi dan menekan tombol 'E'
        if (bisaInteraksi && Input.GetKeyDown(KeyCode.E))
        {
            // Beritahu manajer untuk memulai minigame
            if (manajerMinigame != null)
            {
                // Sembunyikan prompt agar tidak menempel
                if (promptInteraksiUI != null)
                {
                    promptInteraksiUI.SetActive(false);
                }
                manajerMinigame.MulaiMinigameWhackARat();
            }
            else
            {
                Debug.LogError("ManajerMinigame belum di-assign di InteractorGorong!");
            }
        }
    }
}