using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SaleManager : MonoBehaviour
{
    public static SaleManager instance;
    #region Singleton
    void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    [SerializeField] GameObject inputField;

    // Number of items in each order and total number of items sold throughout the shift.
    [HideInInspector] public int orderItems, totalItems = 0;
    // Amount of $ owed in each order and total $ earned throughout the shift.
    [HideInInspector] public float orderTotal, grandTotal;

    // Key: the # of the item in the order, Value: the price of that item.
    public Dictionary<int, float> itemsOrdered = new Dictionary<int, float>();

    [Header("Profits UI Elements")]
    public Text orderCost;
    public Text totalCost;
    [Space]
    [Header("Item Lists UI Elements")]
    public List<Text> itemSlots = new List<Text>();
    [Space]
    public List<TextMeshProUGUI> shiftSummarySlots = new List<TextMeshProUGUI>();

    [Header("Shift Summary UI")]
    public GameObject shiftSummaryTextPrefab; // For adding more shift summary item slots.
    public Transform summarySlotParent; // For parenting new shift summary item slots.

    [Header("Employee Info")]
    public Text username;

    [Header("Shopping Settings")]
    [SerializeField] int maxItemsPerOrder = 20;
    [SerializeField] int numShiftSummarySlots = 50;


    void Start()
    {
        username.text = "Logged In User: " + UserTracker.instance.currentUser;
    }

    void Update()
    {
        // Return to the login screen.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("LoginScene");
        }
    }

    // This function is set to each item button's OnClick, with that button's own Item script passed in through the inspector as the 'item' reference.
    public void AddItem(Item item)
    {
        // If the shopping cart does not exceed the maximum allowed items per order...
        if (orderItems < maxItemsPerOrder)
        {
            // Increment the order items and total items sold.
            orderItems++;
            totalItems++;

            // Add the selected item to the dictionary.
            itemsOrdered.Add(orderItems, item.price);

            // Update the shopping cart item info text with purchased items.
            itemSlots[orderItems - 1].text = orderItems.ToString() + ". " + item.name + "  -  $" + item.price.ToString("F2");

            // Add the selected item's price to the order total.
            orderTotal += item.price;
        }

        // This is my dumb solution to being too lazy to plug in more than 50 text objects in the inspector. 
        // If the # of shift summary items exceeds 50, more slots are created. 
        if (totalItems >= numShiftSummarySlots)
        {
            GameObject newText = Instantiate(shiftSummaryTextPrefab, summarySlotParent);
            shiftSummarySlots.Add(newText.GetComponent<TextMeshProUGUI>());
        }

        // Update the shift summary item info text with purchased items.
        shiftSummarySlots[totalItems - 1].text = totalItems.ToString() + ". " + item.name + "  -  $" + item.price.ToString("F2");

        // Update profit texts.
        orderCost.text = "Order Total: $" + orderTotal.ToString("F2");
        totalCost.text = "Total Money Owed: $" + grandTotal.ToString("F2");
    }

    // This function is set to the 'CUSTOM' button's OnClick, passing in that button's own Item script as the 'item' reference.
    public void AddCustomItem(Item item)
    {
        // Activates the input field for adding a custom price item, also activating the 'CustomInput' script on the inputField object.
        inputField.SetActive(true);
    }

    // This method is set to the 'Submit Order' button's OnClick.
    public void SubmitOrder()
    {
        // Clear the shopping cart dictionary.
        itemsOrdered.Clear();

        // Add the shopping cart total to the grand total in the shift summary.
        grandTotal += orderTotal; 
        // Reset the shopping cart total.
        orderTotal = 0.0f;

        // Reset the text of each shopping cart slot.
        for (int i = 0; i < itemSlots.Capacity; i++)
        {
            itemSlots[i].text = "";
        }
        // Reset the number of items in the shopping cart.
        orderItems = 0;

        // Update profit texts.
        orderCost.text = "Order Total: $" + orderTotal.ToString("F2");
        totalCost.text = "Total Money Owed: $" + grandTotal.ToString("F2");
    }

    // This method is set to the 'Remove Item' button's OnClick.
    public void RemoveItem()
    {
        // If we have at least 1 item in our shopping cart...
        if (orderItems > 0)
        {
            // If our shopping cart is at maximum capacity...
            if (orderItems == maxItemsPerOrder)
            {
                // Reset the text of the bottom item slot.
                itemSlots[itemSlots.Count - 1].text = "";
            }
            else
            {
                // Loop through the item slots and reset the text that was most recently filled.
                for (int i = 0; i < itemSlots.Capacity; i++)
                {
                    if (itemSlots[i].text != "")
                    {
                        continue;
                    }
                    else
                    {
                        itemSlots[i - 1].text = "";
                        break;
                    }
                }
            }

            // Remove the price of the most recently added item from the order total.
            orderTotal -= LastItemPrice();
            // Remove the most recently added item from the order dictionary.
            itemsOrdered.Remove(orderItems);
            // Decrement the number of items in the shopping cart.
            orderItems--;

            // Loop through the shift summary item slots and reset the text that was most recently filled.
            for (int i = 0; i < shiftSummarySlots.Capacity; i++)
            {
                if (shiftSummarySlots[i].text != "")
                {
                    continue;
                }
                else
                {
                    shiftSummarySlots[i - 1].text = "";
                    break;
                }
            }

            // Decrement the total number of items sold.
            totalItems--;
        }
        else
        {
            Debug.LogWarning("There are no items in your cart to remove!");
        }

        // Update profit texts.
        orderCost.text = "Order Total: $" + orderTotal.ToString("F2");
        totalCost.text = "Total Money Owed: $" + grandTotal.ToString("F2");
    }

    // Function which returns the price of the most recently added item in the shopping cart.
    float LastItemPrice()
    {
        float value;
        itemsOrdered.TryGetValue(orderItems, out value);
        return value;
    }

    // This is very dumb please ignore this nothing to see here
    public void ToggleShiftSummary(RectTransform rect)
    {
        if (rect.eulerAngles != Vector3.zero)
        {
            rect.eulerAngles = Vector3.zero;
        }
        else
        {
            rect.eulerAngles = new Vector3(90f, 0f, 0f);
        }
    }

    #region Get Date & Time Functions
    // Gets today's date and outputs it as a string with a file-friendly format (DateTime.Now.Date uses colons, which are not file-friendly).
    string GetDate()
    {
        string month = DateTime.Now.Month.ToString();
        string day = DateTime.Now.Day.ToString();
        string year = DateTime.Now.Year.ToString();

        return month + "-" + day + "-" + year;
    }
    // Gets the current time and outputs it as a string with a file-friendly format (see GetDate for explanation).
    string GetTime()
    {
        string hour = DateTime.Now.Hour.ToString();
        string minute = DateTime.Now.Minute.ToString();
        string second = DateTime.Now.Second.ToString();

        return hour + "-" + minute + "-" + second;
    }
    #endregion

    // Saves the contents of the shift summary to a text file on the desktop.
    public void SaveShiftSummary()
    {
        // Gets the local date & time for use later in the function.
        string date = GetDate();
        string time = GetTime();

        // Create a path to the Shift Summary folder on the desktop.
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string summaryFolder = desktopPath + "\\Shift Summaries";

        // If the Shift Summary folder doesn't exist, create one.
        if (!Directory.Exists(summaryFolder))
        {
            Directory.CreateDirectory(summaryFolder);
        }

        // Create a path to the date folder within the Shift Summary folder.
        string dateFolder = summaryFolder + "\\" + date;

        // If the folder for today's date doesn't exist, create one.
        if (!Directory.Exists(dateFolder))
        {
            Directory.CreateDirectory(dateFolder);
        }

        // Create a path to the shift summary file location within the date folder.
        string fileLocation = dateFolder + "\\Shift Summary - " + time + ".txt";

        // Create a read/write-able text file at the file path
        File.CreateText(fileLocation).Dispose();
        TextWriter writer = new StreamWriter(fileLocation, true);

        // Loop through each item in the shift summary and add it's content to the saved file.
        for (int i = 0; i < shiftSummarySlots.Count; i++)
        {
            if (shiftSummarySlots[i].text != "")
            {
                writer.WriteLine(shiftSummarySlots[i].text + "\n");
            }
        }

        // At the bottom of the shift summary file, write the total profits and logged in user during that shift.
        writer.WriteLine("Total profits this shift: $" + grandTotal.ToString() + "\n");
        writer.WriteLine(username.text);

        // Close the file.
        writer.Close();
    }
}
