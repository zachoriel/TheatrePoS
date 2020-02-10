using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CustomInput : MonoBehaviour
{
    public TMP_InputField textItem;
    public Item item;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            item.price = float.Parse(textItem.text, CultureInfo.InvariantCulture.NumberFormat);

            if (SaleManager.instance.orderItems < 20)
            {
                SaleManager.instance.orderItems++;
                SaleManager.instance.totalItems++;
                SaleManager.instance.itemsOrdered.Add(SaleManager.instance.orderItems, item.price);

                SaleManager.instance.itemSlots[SaleManager.instance.orderItems - 1].text = SaleManager.instance.orderItems.ToString() + ". " + item.name + "  -  $" + item.price.ToString("F2");
                SaleManager.instance.orderTotal += item.price;
            }

            if (SaleManager.instance.totalItems >= 50)
            {
                GameObject newText = Instantiate(SaleManager.instance.shiftSummaryTextPrefab, SaleManager.instance.contentPrefab);
                SaleManager.instance.shiftSummarySlots.Add(newText.GetComponent<TextMeshProUGUI>());
            }

            SaleManager.instance.shiftSummarySlots[SaleManager.instance.totalItems - 1].text = item.name + "  -  $" + item.price.ToString("F2");

            textItem.text = null;
            this.gameObject.SetActive(false);
        }
    }
}
