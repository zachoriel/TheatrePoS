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

    [HideInInspector] public int orderItems, totalItems = 0;
    public Dictionary<int, float> itemsOrdered = new Dictionary<int, float>();
    [HideInInspector] public float orderTotal, grandTotal;

    public Text orderCost;
    public Text totalCost;
    public List<Text> itemSlots = new List<Text>();
    public List<TextMeshProUGUI> shiftSummarySlots = new List<TextMeshProUGUI>();

    public GameObject shiftSummaryTextPrefab;
    public Transform contentPrefab;

    public Text username;


    void Start()
    {
        username.text = "Logged In User: " + UserTracker.instance.currentUser;
    }

    void Update()
    {
        orderCost.text = "Order Total: $" + orderTotal.ToString("F2");
        totalCost.text = "Total Money Owed: $" + grandTotal.ToString("F2");

        // Return to the login screen
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("LoginScene");
        }
    }

    public void Additem(Item item)
    {
        if (orderItems < 20)
        {
            orderItems++;
            totalItems++;
            itemsOrdered.Add(orderItems, item.price);

            itemSlots[orderItems - 1].text = orderItems.ToString() + ". " + item.name + "  -  $" + item.price.ToString("F2");
            orderTotal += item.price;
        }

        if (totalItems >= 50)
        {
            GameObject newText = Instantiate(shiftSummaryTextPrefab, contentPrefab);
            shiftSummarySlots.Add(newText.GetComponent<TextMeshProUGUI>());
        }

        shiftSummarySlots[totalItems - 1].text = totalItems.ToString() + ". " + item.name + "  -  $" + item.price.ToString("F2");
    }

    public void AddCustomItem(Item item)
    {
        inputField.SetActive(true);
    }

    public void SubmitOrder()
    {
        itemsOrdered.Clear();

        grandTotal += orderTotal;       
        orderTotal = 0.0f;

        for (int i = 0; i < itemSlots.Capacity; i++)
        {
            itemSlots[i].text = "";
        }
        orderItems = 0;
    }

    public void RemoveItem()
    {
        if (orderItems > 0)
        {
            if (orderItems == 20)
            {
                itemSlots[itemSlots.Count - 1].text = "";
            }
            else
            {
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

            orderTotal -= LastItemPrice();
            itemsOrdered.Remove(orderItems);
            orderItems--;

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

            totalItems--;
        }
        else
        {
            Debug.LogWarning("There are no items in your cart to remove!");
        }
    }

    float LastItemPrice()
    {
        float value;
        itemsOrdered.TryGetValue(orderItems, out value);
        return value;
    }

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
    int GetMonth()
    {
        int localMonth = DateTime.Now.Month;
        return localMonth;
    }
    int GetDay()
    {
        int localDay = DateTime.Now.Day;
        return localDay;
    }
    int GetYear()
    {
        int localYear = DateTime.Now.Year;
        return localYear;
    }
    int GetHour()
    {
        int localTime = DateTime.Now.Hour;
        return localTime;
    }
    int GetMinute()
    {
        int localMinute = DateTime.Now.Minute;
        return localMinute;
    }
    int GetSecond()
    {
        int localSecond = DateTime.Now.Second;
        return localSecond;
    }
    #endregion

    public void SaveShiftSummary()
    {
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string summaryFolder = desktopPath + "\\Shift Summaries";

        if (!Directory.Exists(summaryFolder))
        {
            Directory.CreateDirectory(summaryFolder);
        }

        string month = GetMonth().ToString();
        string day = GetDay().ToString();
        string year = GetYear().ToString();
        string hour = GetHour().ToString();
        string minute = GetMinute().ToString();
        string second = GetSecond().ToString();

        string dateFolder = summaryFolder + "\\" + month + "-" + day + "-" + year;

        if (!Directory.Exists(dateFolder))
        {
            Directory.CreateDirectory(dateFolder);
        }

        string fileLocation = dateFolder + "\\Shift Summary - " + hour + "-" + minute + "-" + second + ".txt";

        File.CreateText(fileLocation).Dispose();
        TextWriter writer = new StreamWriter(fileLocation, true);

        for (int i = 0; i < shiftSummarySlots.Count; i++)
        {
            if (shiftSummarySlots[i].text != "")
            {
                writer.WriteLine(shiftSummarySlots[i].text + "\n");
            }
        }

        writer.WriteLine("Total profits this shift: $" + grandTotal.ToString());

        writer.Close();
    }
}
