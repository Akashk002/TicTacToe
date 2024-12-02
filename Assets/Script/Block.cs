using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    int blockIndex;
    Button button;
    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        button.onClick.AddListener(SetSign);
    }


    // Start is called before the first frame update
    void Start()
    {
        blockIndex = transform.GetSiblingIndex();
    }

    public void SetSign()
    {
        image.sprite = GameManager.Instance.GetSprite();
        GameManager.Instance.SetSign(blockIndex);
        ResetImageAlpha();
        button.interactable = false;
    }

    void ResetImageAlpha()
    {
        var col = image.color;
        col.a = 1;
        image.color = col;
    }
}
