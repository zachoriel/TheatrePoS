using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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


    void Update()
    {
        orderCost.text = "Order Total: $" + orderTotal.ToString("F2");
        totalCost.text = "Total Money Owed: $" + grandTotal.ToString("F2");

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void Additem(Item item)
    {
        if (orderItems < 20)
        {
            orderItems++;
            totalItems++;
            itemsOrdered.Add(orderItems, item.price);

            itemSlots[orderItems - 1].text = item.name + "  -  $" + item.price.ToString("F2");
            orderTotal += item.price;
        }

        if (totalItems >= 50)
        {
            GameObject newText = Instantiate(shiftSummaryTextPrefab, contentPrefab);
            shiftSummarySlots.Add(newText.GetComponent<TextMeshProUGUI>());
        }

        shiftSummarySlots[totalItems - 1].text = item.name + "  -  $" + item.price.ToString("F2");
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
}
