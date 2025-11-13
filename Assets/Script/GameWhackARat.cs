using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Diletakkan pada PanelMinigame
public class GameWhackARat : MonoBehaviour
{
    [Header("Referensi Manajer")]
    public ManajerMinigame manajerMinigame; // Untuk memberitahu saat game selesai
    public Button tombolKeluar;

    [Header("Logika Game")]
    public Transform[] lubangTikus;  // Array berisi transform dari "lubang"
    public GameObject tikusPrefab;   // Prefab dari tikus yang akan muncul
    public float durasiMuncul = 1.2f; // Berapa lama tikus diam sebelum hilang
    public float spawnInterval = 0.7f; // Seberapa sering tikus baru muncul
    public Text scoreText; // UI Text untuk menampilkan skor
    public int batasPukulan = 3; // Batas tikus yang harus dipukul

    private int score = 0;
    private int tikusTepukul = 0;
    private bool gameAktif = false;
    private Coroutine spawnCoroutine;

    void Start()
    {
        // Hubungkan tombol keluar ke fungsi KeluarMinigame
        if (tombolKeluar != null)
        {
            tombolKeluar.onClick.AddListener(KeluarMinigame);
        }
    }

    // Ini dipanggil oleh ManajerMinigame
    public void MulaiGame()
    {
        // --- DIAGNOSTIC DEBUG ---
        // Baris ini akan memberitahu kita OBJEK MANA yang menjalankan skrip ini
        Debug.LogWarning("--- MulaiGame() dipanggil pada GameObject: " + gameObject.name + " ---");
        // --- END DIAGNOSTIC ---

        Debug.Log("GameWhackARat: Memulai game...");

        // --- TAMBAHAN PENGECEKAN DEBUG ---
        if (lubangTikus == null || lubangTikus.Length == 0)
        {
            Debug.LogError("FATAL ERROR: Array 'lubangTikus' KOSONG (Size is 0). Tidak bisa spawn. Cek Inspector PanelMinigame.");
            gameAktif = false;
            return; // Hentikan game
        }
        if (tikusPrefab == null)
        {
            Debug.LogError("FATAL ERROR: 'tikusPrefab' adalah NULL. Tidak bisa spawn. Cek Inspector PanelMinigame.");
            gameAktif = false;
            return; // Hentikan game
        }
        // --- AKHIR PENGECEKAN DEBUG ---

        gameAktif = true;
        score = 0;
        tikusTepukul = 0;
        UpdateScoreUI();

        // Hentikan coroutine lama jika ada (untuk mencegah duplikasi)
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }

        // Mulai loop spawn yang baru
        spawnCoroutine = StartCoroutine(SpawnLoop());
    }

    // Ini dipanggil oleh tombolKeluar
    void KeluarMinigame()
    {
        // Hentikan logika game (timer, spawn, dll)
        StopGame();

        Debug.Log("Player menekan tombol keluar.");
        // Beritahu manajer utama bahwa minigame selesai
        if (manajerMinigame != null)
        {
            manajerMinigame.SelesaiMinigameWhackARat();
        }
    }

    // Fungsi helper untuk menghentikan game
    void StopGame()
    {
        gameAktif = false;
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
        HapusSemuaTikus();
    }

    // Ini adalah loop utama untuk memunculkan tikus
    IEnumerator SpawnLoop()
    {
        Debug.Log("SpawnLoop: Coroutine DIMULAI."); // <-- DEBUG BARU

        // Terus berjalan selama game aktif
        while (gameAktif)
        {
            Debug.Log("SpawnLoop: Menunggu " + spawnInterval + " detik..."); // <-- DEBUG BARU

            // Tunggu selama interval spawn
            // Gunakan Realtime agar tidak terpengaruh Time.timeScale = 0
            yield return new WaitForSecondsRealtime(spawnInterval); // <-- UBAH INI

            // Periksa lagi jika game dinonaktifkan saat menunggu
            if (!gameAktif)
            {
                Debug.Log("SpawnLoop: Game tidak aktif, keluar dari loop."); // <-- DEBUG BARU
                break;
            }

            Debug.Log("SpawnLoop: Mulai proses spawn..."); // <-- DEBUG BARU

            // Pilih lubang acak dari array
            int indexLubang = Random.Range(0, lubangTikus.Length);
            Transform lubang = lubangTikus[indexLubang];

            // --- TAMBAHAN PENGECEKAN DEBUG ---
            if (lubang == null)
            {
                Debug.LogError("FATAL ERROR: 'lubangTikus' di index " + indexLubang + " adalah NULL (Kosong). Cek Inspector PanelMinigame.");
                continue; // Coba loop berikutnya, jangan hancurkan coroutine
            }
            // --- AKHIR PENGECEKAN DEBUG ---

            // Tentukan posisi spawn (tepat di posisi objek lubang)
            Vector3 spawnPos = lubang.position;

            Debug.Log("SpawnLoop: Instantiate tikus di " + spawnPos); // <-- DEBUG BARU

            // Buat (Instantiate) prefab tikus di posisi tersebut
            // Kita atur parent-nya ke transform canvas ini agar tetap di dalam UI
            GameObject tikus = Instantiate(tikusPrefab, spawnPos, Quaternion.identity, transform);

            // Beri tahu skrip Tikus siapa manajernya
            Tikus skripTikus = tikus.GetComponent<Tikus>();
            if (skripTikus != null)
            {
                skripTikus.gameManager = this;
            }
            else
            {
                Debug.LogError("SpawnLoop: PREFAB TIKUS ANDA TIDAK MEMILIKI SCRIPT Tikus.cs!");
            }

            // Hancurkan tikus secara otomatis setelah 'durasiMuncul'
            Destroy(tikus, durasiMuncul);
        }
        Debug.Log("SpawnLoop: Coroutine SELESAI."); // <-- DEBUG BARU
    }

    // Method untuk menambah skor (dipanggil dari Tikus.cs)
    public void TambahSkor(int nilai)
    {
        if (!gameAktif) return; // Jangan tambah skor jika game sudah selesai

        score += nilai;
        tikusTepukul++; // Tambah counter tikus yang dipukul
        UpdateScoreUI();

        // Cek apakah sudah mencapai batas
        if (tikusTepukul >= batasPukulan)
        {
            StopGame(); // Hentikan game
            StartCoroutine(SelesaiOtomatis()); // Mulai coroutine untuk keluar
        }
    }

    // Coroutine untuk keluar otomatis setelah jeda
    IEnumerator SelesaiOtomatis()
    {
        Debug.Log("Batas pukulan tercapai! Game berakhir.");
        if (scoreText != null)
        {
            scoreText.text = "Selesai! Skor: " + score;
        }

        // Tunggu 2 detik agar player bisa lihat skor akhir
        // Gunakan Realtime agar tidak terpengaruh Time.timeScale = 0
        yield return new WaitForSecondsRealtime(2.0f); // <-- UBAH INI

        // Beritahu manajer utama bahwa minigame selesai
        if (manajerMinigame != null)
        {
            manajerMinigame.SelesaiMinigameWhackARat();
        }
    }

    // Update UI skor
    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            // Tampilkan skor dan progres pukulan
            scoreText.text = "Skor: " + score + " | Tikus: " + tikusTepukul + " / " + batasPukulan;
        }
    }

    // Membersihkan semua tikus saat game berakhir
    void HapusSemuaTikus()
    {
        // Cari semua GameObject dengan tag "Tikus"
        GameObject[] semuaTikus = GameObject.FindGameObjectsWithTag("Tikus");
        foreach (GameObject tikus in semuaTikus)
        {
            Destroy(tikus);
        }
    }
}