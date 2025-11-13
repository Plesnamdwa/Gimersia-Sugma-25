using UnityEngine;

// Diletakkan pada prefab "Tikus"
// Pastikan prefab ini memiliki Collider2D dan Tag "Tikus"
public class Tikus : MonoBehaviour
{
    public int skorDiberikan = 10; // Nilai poin saat tikus ini dipukul

    // Referensi ini akan diisi secara otomatis oleh GameWhackARat saat spawn
    public GameWhackARat gameManager;

    // Dipanggil ketika player mengklik collider ini
    // (Membutuhkan Physics 2D Raycaster di Main Camera)
    void OnMouseDown()
    {
        // Beri skor ke manajer
        if (gameManager != null)
        {
            gameManager.TambahSkor(skorDiberikan);
        }
        else
        {
            Debug.LogWarning("OnMouseDown: GameManager adalah NULL. Tidak bisa menambah skor.");
        }

        // Hancurkan diri sendiri (tikusnya "masuk" kembali)
        // Kita nonaktifkan collider agar tidak bisa diklik dua kali
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject);
    }
}