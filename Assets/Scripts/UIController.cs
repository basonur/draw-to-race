using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIController : MonoBehaviour
{
    public List<Menu> menus;
    public Image image;
    public void ShowMenu(string name)
    {
        for (int i = 0; i < menus.Count; i++)
        {
            menus[i].gameObject.SetActive(false);
        }
        menus.Find(x => x.name == name).gameObject.SetActive(true);
    }

    public void UpdateProgress(float rate)
    {
        image.fillAmount = rate;
    }
}
[System.Serializable]
public class Menu
{
    public string name;
    public GameObject gameObject;
}
