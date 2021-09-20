using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingPanel : MonoBehaviour
{
    private Text[] textFields;
    
    // Start is called before the first frame update
    void Start()
    {
        textFields = GetComponentsInChildren<Text>();
    }

    public void SetString(string val)
    {
        foreach (var t in textFields)
        {
            t.text = val;
            t.GetComponent<ContentSizeFitter>().SetLayoutHorizontal();
            t.rectTransform.anchoredPosition =
                new Vector2(t.rectTransform.rect.width, t.transform.localPosition.y);
        }
    }

    public float scrollSpeed = -1f;
    public float jumpValue = 10f;
    void Update()
    {
        foreach (var t in textFields)
        {
            t.rectTransform.anchoredPosition += Vector2.right * Time.deltaTime * scrollSpeed;
            if (t.rectTransform.anchoredPosition.x < jumpValue)
            {
                t.rectTransform.anchoredPosition =
                    new Vector2(t.rectTransform.rect.width, t.transform.localPosition.y);
            }
        }
    }
}
