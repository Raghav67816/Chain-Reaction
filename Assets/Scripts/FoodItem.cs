using TMPro;
using UnityEngine;

public class FoodItem : MonoBehaviour
{
    public Food foodData;
    public GameObject popEffect;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            GameObject effect = Instantiate(popEffect, gameObject.transform.position, Quaternion.identity);
            TMP_Text value = effect.GetComponentInChildren<TMP_Text>();
            value.text = $"{foodData.massValue} g";
            Destroy(effect, 1f);
        }
    }
}
