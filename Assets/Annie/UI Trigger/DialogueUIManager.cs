using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUIManager : MonoBehaviour
{
    public static DialogueUIManager Instance;

    [Header("UI Elements")]
    public GameObject dialoguePanel;
    public Image portraitImage;
    public TextMeshProUGUI dialogueText;

    [Header("Settings")]
    public float typeSpeed = 0.02f;

    private bool isTyping = false;
    private string currentText = "";

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    public void ShowDialogue(string line, Sprite portrait)
    {
        if (dialoguePanel == null || dialogueText == null) return;

        dialoguePanel.SetActive(true);

        if (portrait != null && portraitImage != null)
            portraitImage.sprite = portrait;

        StopAllCoroutines();
        StartCoroutine(TypeLine(line));
    }

    public void HideDialogue()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    private IEnumerator TypeLine(string line)
    {
        isTyping = true;
        currentText = line;
        dialogueText.text = "";

        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }

        isTyping = false;
    }
}
