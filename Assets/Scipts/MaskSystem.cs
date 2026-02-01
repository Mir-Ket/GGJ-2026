using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MaskSystem : MonoBehaviour
{
    [Header("Tüm Seçenekler (5 Adet)")]
    public GameObject[] allMasks;
    public GameObject[] allWeapons;

    [Header("UI Referanslarý")]
    public GameObject selectionUI;
    public Button[] selectionButtons; // Sahnedeki 3 adet buton

    private int activeIndex = -1;
    private float timer = 0f;
    private bool isGamePaused = true;

    // O an ekrandaki 3 butonun hangi maske indekslerine denk geldiðini tutar
    private int[] currentDisplayedIndices = new int[3];

    void Start()
    {
        DeactivateAll();
        ShowSelectionMenu();
    }

    void Update()
    {
        if (!isGamePaused)
        {
            timer += Time.deltaTime;
            if (timer >= 30f)
            {
                ShowSelectionMenu();
            }
        }
    }

    public void ShowSelectionMenu()
    {
        isGamePaused = true;
        Time.timeScale = 0f;
        selectionUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SetupRandomButtons();
    }

    void SetupRandomButtons()
    {
        // 0'dan 4'e kadar olan indeksleri bir listeye ekle
        List<int> possibleIndices = new List<int>();
        for (int i = 0; i < allMasks.Length; i++) possibleIndices.Add(i);

        // Listeyi karýþtýr ve ilk 3 tanesini al
        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, possibleIndices.Count);
            int selectedMaskIndex = possibleIndices[randomIndex];

            currentDisplayedIndices[i] = selectedMaskIndex;
            possibleIndices.RemoveAt(randomIndex); // Ayný maske iki kez çýkmasýn

            // Butonun üzerindeki metni güncelle (Eðer butonun içinde Text varsa)
            selectionButtons[i].GetComponentInChildren<Text>().text = "Maske " + (selectedMaskIndex + 1);
        }
    }

    // Butonlara Unity Inspector'dan bunu tanýmla: 
    // Button 0 -> SelectFromButton(0), Button 1 -> SelectFromButton(1)...
    public void SelectFromButton(int buttonIndex)
    {
        int actualMaskIndex = currentDisplayedIndices[buttonIndex];
        ApplySelection(actualMaskIndex);
    }

    void ApplySelection(int index)
    {
        DeactivateAll();

        activeIndex = index;
        allMasks[activeIndex].SetActive(true);
        allWeapons[activeIndex].SetActive(true);

        ResumeGame();
    }

    void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1f;
        selectionUI.SetActive(false);
        timer = 0f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void DeactivateAll()
    {
        foreach (GameObject m in allMasks) m.SetActive(false);
        foreach (GameObject w in allWeapons) w.SetActive(false);
    }
}