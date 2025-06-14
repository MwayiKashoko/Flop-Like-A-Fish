using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour, IDropHandler{
    public string slotName = "null";
    private int highestRank = 0;
    public bool isFoundation = false;

    private Regex regexRank;

    void Start() {
        regexRank = new Regex(@"\d+");
    }

    public void OnDrop(PointerEventData eventData) {
        //if (eventData.pointerDrag.transform.name == slotName) {
        Drag draggable = eventData.pointerDrag.GetComponent<Drag>();

        eventData.pointerDrag.tag = "Moved";

        if (isFoundation && eventData.pointerDrag.transform.name.Contains(slotName) && Convert.ToInt32(regexRank.Match((eventData.pointerDrag.transform.name)).Groups[0].Value)-1 == highestRank) {
            this.GetComponent<Image>().sprite = eventData.pointerDrag.GetComponent<Image>().sprite;
            eventData.pointerDrag.GetComponent<Image>().sprite = null;
            highestRank++;
        } else if (!isFoundation && ((transform.name.Contains("Red") && eventData.pointerDrag.transform.name.Contains("Black")) || (transform.name.Contains("Black") && eventData.pointerDrag.transform.name.Contains("Red"))) && Convert.ToInt32(regexRank.Match((eventData.pointerDrag.transform.name)).Groups[0].Value)+1 == Convert.ToInt32(regexRank.Match((transform.name)).Groups[0].Value)) {
            if (!eventData.pointerDrag.transform.name.Contains("Waste")) {
                draggable.startPosition = transform.position;
            } else {
                string cardName = eventData.pointerDrag.transform.name;
                eventData.pointerDrag.transform.name = "WasteCard";
                GameObject card = GameObject.Find(cardName);

                card.transform.position = transform.position;
                eventData.pointerDrag.GetComponent<Image>().sprite = null;
            }
        } else {
            eventData.pointerDrag.tag = "Untagged";
        }
    }
}
