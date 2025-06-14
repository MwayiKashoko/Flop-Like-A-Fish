using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {
    private Image thisImage;
    private Vector3 _startPosition;

    void Start() {
        thisImage = this.GetComponent<Image>();
        _startPosition = this.transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        thisImage.raycastTarget = false;
    }
    
    public void OnDrag (PointerEventData eventData) {
        transform.localPosition = new Vector3(eventData.position.x-1065f, eventData.position.y-645f, 0);
        transform.SetAsLastSibling();
    }

    public void OnEndDrag(PointerEventData eventData) {
        transform.position = _startPosition;
        thisImage.raycastTarget = true;
    }

    public Vector3 startPosition {
        get {
            return _startPosition;
        }

        set {
            _startPosition = value;
        }
    }
}
