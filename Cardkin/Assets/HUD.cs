using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    public Player player;

    public Image healthbarBackground;
    public Color healthbarColour;
    public Color healthbarDeltaColour;


    private Vector2 last = Vector2.zero;
    private Image healthbar;
    private Image healthbarDelta;

    private void Start()
    {
        healthbarDelta = Instantiate(healthbarBackground.gameObject, healthbarBackground.gameObject.transform.parent).GetComponent<Image>();
        healthbarDelta.color = healthbarDeltaColour;

        healthbar = Instantiate(healthbarBackground.gameObject, healthbarBackground.gameObject.transform.parent).GetComponent<Image>();
        healthbar.color = healthbarColour;


    }
    void Update()
    {

        Vector2 max = healthbarBackground.rectTransform.sizeDelta;

        Vector2 desired = max;
        desired.x = desired.x * player.health / player.maxHealth;

        healthbarDelta.rectTransform.sizeDelta = Vector2.SmoothDamp(healthbarDelta.rectTransform.sizeDelta, desired, ref last, 0.5f);
        healthbar.rectTransform.sizeDelta = desired;
    }
}
