using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private Button continueBtn;
    [SerializeField] private TextMeshProUGUI characterName, characterDialog;
    [SerializeField] private RectTransform background;
    [SerializeField] private int nextScene = -1;
    [SerializeField] private bool joshuaDialogFirst = true;
    [SerializeField] private List<AudioClip> dialogSFXs, clickSFXs;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private int frequencyLevel = 2;

    private Queue<string> sentences;

    public DialogTrigger joshuaDialog, magooDialog;

    private int index = 0;
    private bool finished = false;

    private void Start()
    {
        sentences = new Queue<string>();
        continueBtn.onClick.AddListener(() => 
        {
            audioSource.Stop();
            int index = Random.Range(0, clickSFXs.Count);
            audioSource.PlayOneShot(clickSFXs[index], 0.35f);
            DisplaySentence();
        });

        StartDialoug();
        DisplaySentence();

        LeanTween.scale(background.gameObject, new Vector3(40f, 40f, 40f), 1f).setEase(LeanTweenType.easeInOutQuad);
        LeanTween.moveLocalY(continueBtn.gameObject, -15f, 0.5f).setEase(LeanTweenType.easeInOutQuad).setLoopPingPong();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            audioSource.Stop();
            int index = Random.Range(0, clickSFXs.Count);
            audioSource.PlayOneShot(clickSFXs[index], 0.35f);
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

                SceneManager.LoadScene(nextScene);
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

    //Dialog randomization from: https://youtu.be/P3FcXHEai_E?si=tlvUqZkODx9OnpeT
    private void PlayDialogSound(int currentDisplayCharacterCount, char currentCharacter)
    {
        //audioSource.Stop();
        AudioClip soundClip = null;

        if(currentDisplayCharacterCount % 10 == 0)
        {
            //Debug.Log(currentDisplayCharacterCount);
            //audioSource.Stop();
            //audioSource.pitch = Random.Range(0.5f, 2f);
            //int index = Random.Range(0, dialogSFXs.Count);
            //audioSource.PlayOneShot(dialogSFXs[index], 0.5f);

            int hashCode = currentCharacter.GetHashCode();
            int predictableindex = hashCode % dialogSFXs.Count;
            soundClip = dialogSFXs[predictableindex];

            int minPitch = (int)(1.5f * 100);
            int maxPitch = (int)(2f * 100);
            int pitchRangeInt = maxPitch - minPitch;

            if(pitchRangeInt != 0)
            {
                int predictablePitchInt = (hashCode % pitchRangeInt) + minPitch;
                float predictablePitch = predictablePitchInt / 100f;
                audioSource.pitch = predictablePitch;
            }
            else
            {
                audioSource.pitch = minPitch;
            }

            audioSource.PlayOneShot(dialogSFXs[predictableindex], 0.15f);
        }
    }

    private IEnumerator AnimateDialog(string sentence)
    {
        characterDialog.text = "";
        yield return null;

        int characterCount = 0;

        foreach(char letter in sentence.ToCharArray())
        {
            PlayDialogSound(characterCount, letter);
            characterCount++;
            characterDialog.text += letter;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
