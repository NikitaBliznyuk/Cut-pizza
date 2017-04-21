using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public GameObject pizza;
    public Text percentText;
    public Button restartButton;

    private PizzaCutter pizzaHandler;

    private void Start()
    {
        CreatePizza();
    }

    private void Update()
    {
        if (pizzaHandler != null)
            percentText.text = (int)pizzaHandler.Percents + "%";
        else
        {
            percentText.text = "0%";
            restartButton.gameObject.SetActive(true);
        }
    }

    public void CreatePizza()
    {
        restartButton.gameObject.SetActive(false);
        pizzaHandler = Instantiate(pizza).GetComponent<PizzaCutter>();
    }
}
