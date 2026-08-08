using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private Button continueBtn;
    [SerializeField] private TextMeshProUGUI characterName, characterDialog;
    [SerializeField] private RectTransform background;
    [SerializeField] private int nextScene = -1;
    [SerializeField] private bool joshuaDialogFirst = true;

    private Queue<string> sentences;

    public DialogTrigger joshuaDialog, magooDialog;

    private int index = 0;
    private bool finished = false;

    private void Start()
    {
        sentences = new Queue<string>();
        continueBtn.onClick.AddListener(() => { DisplaySentence(); });

        StartDialoug();
        DisplaySentence();

        LeanTween.scale(background.gameObject, new Vector3(40f, 40f, 40f), 1f).setEase(LeanTweenType.easeInOutQuad);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            DisplaySentence();
        }
    }

    private void StartDialoug()
    {
        sentences.Clear();

        for(int i = 0; i < joshuaDialog.dialog.sentences.Count; i++)
        {
            if (joshuaDialogFirst)
            {
                sentences.Enqueue(joshuaDialog.dialog.sentences[i]);
                sentences.Enqueue(magooDialog.dialog.sentences[i]);
            }
            else
            {
                sentences.Enqueue(magooDialog.dialog.sentences[i]);
                sentences.Enqueue(joshuaDialog.dialog.sentences[i]);
            }
        }
    }

    private void DisplaySentence()
    {
        if (finished) return;

        if(sentences.Count == 0)
        {
            finished = true;

            LeanTween.scale(background.gameObject, Vector3.one, 1f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => 
            {
                if(nextScene == -1)
                {
                    Debug.Log("Set next scene");
                }

                SceneManager.LoadScene(1); //Go to the main game scene
            });

            return;
        }

        string sentence = sentences.Dequeue();
        
        if(index % 2 == 0)
        {
            if (joshuaDialogFirst)
            {
                characterName.text = joshuaDialog.dialog.name;
            }
            else
            {
                characterName.text = magooDialog.dialog.name;
            }
        }
        else
        {
            if (joshuaDialogFirst)
            {
                characterName.text = magooDialog.dialog.name;
            }
            else
            {
                characterName.text = joshuaDialog.dialog.name;
            }
        }

        index++;

        StopAllCoroutines();
        StartCoroutine(AnimateDialog(sentence));
    }

    private IEnumerator AnimateDialog(string sentence)
    {
        characterDialog.text = "";
        yield return null;

        foreach(char letter in sentence.ToCharArray())
        {
            characterDialog.text += letter;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
