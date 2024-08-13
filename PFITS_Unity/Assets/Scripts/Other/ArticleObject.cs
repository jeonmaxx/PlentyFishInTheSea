using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArticleObject : MonoBehaviour
{
    public DayManager dayManager;
    public TextMeshProUGUI title;

    private void Start()
    {
        dayManager = FindObjectOfType<DayManager>();
    }

    public void OnButton()
    {
        dayManager.ChoseArticle();
        gameObject.transform.parent.parent.gameObject.SetActive(false);
    }
}
