using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupScript : MonoBehaviour {
    public GameObject popupBox;
    public Animator animator;
    public TMP_Text popupText;

    public void Popup(string text) {
        popupBox.SetActive(true);
        popupText.text = text;
        animator.SetTrigger("pop");
    }
}
