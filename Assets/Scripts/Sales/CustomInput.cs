using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using TMPro;

public class CustomInput : MonoBehaviour
{
    [Header("The Input Field")]
    public TMP_InputField textItem;
    [Space]
    [Header("The Custom Button's 'Item' Script")]
    public Item item;

    // This only runs when the game object has been activated in SaleManager (see: AddCustomItem()).
    private void Update()
    {
        // If the user presses enter...
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // If a custom input has been added...
            if (textItem.text != null && textItem.text != "")
            {
                // Set the custom item's price (see function comment for explanation).
                item.price = ItemPrice();

                // Add the custom item to the shopping cart.
                SaleManager.instance.AddItem(item);

                // Close the prompt for custom input.
                CloseCustomItemEditor();
            }
        }

        // If the user presses backspace...
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            // Close the prompt for custom input without entering a custom price.
            CloseCustomItemEditor();
        }
    }

    // Convert the inputted text to a float and output that value.
    float ItemPrice()
    {
        float price = float.Parse(textItem.text, CultureInfo.InvariantCulture.NumberFormat);
        return price;
    }

    // Close the input window.
    void CloseCustomItemEditor()
    {
        // Reset the input field's text.
        textItem.text = null;
        // De-activate this game object, effectively stopping the Update() loop.
        this.gameObject.SetActive(false);
    }
}
