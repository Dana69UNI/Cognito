using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextDialogueHandler : MonoBehaviour
{
    public TextMeshProUGUI textComp;
    public string[] dialogos;
    public float textSpeed;

    public bool InDialogue;
    public bool wasSelected;
    public int index;
    private int contEnd;
    private Coroutine typingCoroutine;

    void Start()
    {
        textComp.text = string.Empty;
    }

    public void CallDialogue(int Index, int Cont)
    {
        Debug.Log("CallDialogue");

        
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        index = Index;
        contEnd = Index + Cont;
        textComp.text = string.Empty;

       
        typingCoroutine = StartCoroutine(TypeLine(index));
    }

    public void NextLine()
    {
        
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        if (index >= contEnd || index >= dialogos.Length - 1)
        {
            textComp.text = string.Empty;
            typingCoroutine = null;
            return;
        }
        else
        {
            index++;
            textComp.text = string.Empty;

           
            typingCoroutine = StartCoroutine(TypeLine(index));
        }
    }

    IEnumerator TypeLine(int index)
    {
        
        if (index < dialogos.Length)
        {
            foreach (char c in dialogos[index].ToCharArray())
            {
                textComp.text += c;
                yield return new WaitForSeconds(textSpeed);
            }
        }

       
        typingCoroutine = null;
    }
}