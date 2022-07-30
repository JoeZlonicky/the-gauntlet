using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseCursor : MonoBehaviour
{
    private Image _renderer;

    void Start()
    {
        _renderer = GetComponent<Image>();
    }
    // Start is called before the first frame update
    void Update()
    {
        if (Time.timeScale == 0) {
            Cursor.visible = true;
            _renderer.enabled = false;
        }
        else {
            Cursor.visible = false;
            transform.position = Input.mousePosition;
            _renderer.enabled = true;
        }
    }
}
