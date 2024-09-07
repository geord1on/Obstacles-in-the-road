using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    public GameObject[] tilePrefabs; // Πίνακας με τα prefab πλακίδια
    public Transform playerTransform; // Αναφορά στον χαρακτήρα
    public float zSpawn = 0; // Θέση όπου θα δημιουργηθεί το επόμενο πλακίδιο
    public float tileLength = 30; // Μήκος κάθε πλακιδίου
    public int numberOfTiles = 7; // Ο αριθμός των πλακιδίων που διατηρούνται στην οθόνη ταυτόχρονα

    private List<GameObject> activeTiles = new List<GameObject>(); // Λίστα με τα ενεργά πλακίδια

    void Start()
    {
        // Έλεγχος για την ύπαρξη των prefabs
        if (tilePrefabs == null || tilePrefabs.Length == 0)
        {
            Debug.LogError("tilePrefabs δεν έχουν εκχωρηθεί ή είναι άδεια.");
            return;
        }

        // Δημιουργία αρχικών πλακιδίων
        for (int i = 0; i < numberOfTiles; i++)
        {
            if (i == 0)
                SpawnTile(0); // Το πρώτο πλακίδιο (εισαγωγικό)
            else
                SpawnTile(Random.Range(0, tilePrefabs.Length)); // Τυχαίο πλακίδιο
        }

        // Τοποθέτηση του παίκτη στην αρχή του πρώτου πλακιδίου
        playerTransform.position = new Vector3(0, 1, 0);
    }

    void Update()
    {
        // Δημιουργία νέου πλακιδίου όταν ο παίκτης προχωρήσει αρκετά
        if (playerTransform.position.z - 35 > zSpawn - (numberOfTiles * tileLength))
        {
            SpawnTile(Random.Range(0, tilePrefabs.Length));
            DeleteTile();
        }
    }

    // Μέθοδος για τη δημιουργία νέου πλακιδίου
    public void SpawnTile(int tileIndex)
    {
        if (tilePrefabs == null || tileIndex < 0 || tileIndex >= tilePrefabs.Length)
        {
            Debug.LogError("Λάθος δείκτης ή κενά tilePrefabs.");
            return;
        }

        GameObject go = Instantiate(tilePrefabs[tileIndex], transform.forward * zSpawn, transform.rotation);
        activeTiles.Add(go); // Προσθήκη του νέου πλακιδίου στη λίστα
        zSpawn += tileLength; // Ενημέρωση της θέσης για το επόμενο πλακίδιο
    }

    // Μέθοδος για τη διαγραφή του παλαιότερου πλακιδίου
    private void DeleteTile()
    {
        if (activeTiles.Count == 0) return;

        Destroy(activeTiles[0]); // Διαγραφή του πρώτου πλακιδίου στη λίστα
        activeTiles.RemoveAt(0); // Αφαίρεση του πρώτου στοιχείου από τη λίστα
    }
}
