using UnityEngine;

// Diletakkan pada GameObject kosong seperti "GameManager"
public class ManajerMinigame : MonoBehaviour
{
    [Header("Objek Game Utama")]
    public GameObject player; // Referensi ke objek Player Anda
    
    [Header("UI Panels (di dalam satu Canvas)")]
    public GameObject panelGameUtama; // Panel/GameObject untuk HUD utama
    public GameObject panelMinigame;  // Panel/GameObject untuk minigame

    [Header("Referensi Skrip")]
    public GameWhackARat scriptGameWhackARat; // Script di panelMinigame

    // Ganti "PlayerMovement" dengan nama skrip gerakan Anda yang sebenarnya
    private string namaSkripGerakanPlayer = "PlayerMovement";

    void Start()
    {
        // Pastikan minigame disembunyikan saat mulai
        if (panelMinigame != null) panelMinigame.SetActive(false);
        if (panelGameUtama != null) panelGameUtama.SetActive(true);
    }

    public void MulaiMinigameWhackARat()
    {
        Debug.Log("Memulai Minigame...");
        
        // 1. Nonaktifkan kontrol player
        if (player != null)
        {
            // Cara yang lebih aman untuk menonaktifkan skrip berdasarkan nama
            MonoBehaviour scriptGerakan = player.GetComponent(namaSkripGerakanPlayer) as MonoBehaviour;
            if (scriptGerakan != null)
            {
                scriptGerakan.enabled = false;
            }
            else
            {
                Debug.LogWarning("Skrip gerakan '" + namaSkripGerakanPlayer + "' tidak ditemukan di Player!");
            }
        }

        // 2. Sembunyikan UI game utama
        if (panelGameUtama != null) panelGameUtama.SetActive(false);

        // 3. Tampilkan UI minigame
        if (panelMinigame != null) panelMinigame.SetActive(true);

        // 4. Mulai logika game di skrip minigame
        if (scriptGameWhackARat != null)
        {
            scriptGameWhackARat.MulaiGame();
        }
    }

    public void SelesaiMinigameWhackARat()
    {
        Debug.Log("Mengakhiri Minigame...");

        // 1. Sembunyikan UI minigame
        if (panelMinigame != null) panelMinigame.SetActive(false);

        // 2. Tampilkan UI game utama
        if (panelGameUtama != null) panelGameUtama.SetActive(true);

        // 3. Aktifkan kembali kontrol player
        if (player != null)
        {
            MonoBehaviour scriptGerakan = player.GetComponent(namaSkripGerakanPlayer) as MonoBehaviour;
            if (scriptGerakan != null)
            {
                scriptGerakan.enabled = true;
            }
        }
    }
}