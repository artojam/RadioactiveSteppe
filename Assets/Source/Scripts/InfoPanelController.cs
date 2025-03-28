using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanelController : MonoBehaviour
{
    [SerializeField]
    private Button UseButton;
    [SerializeField]
    private Button RemoveButton;
    [SerializeField]
    private Button DropButton;
    [SerializeField] 
    private Button DivideButton;
    

    [Space(25)]

    [SerializeField]
    private GameObject SellPanel;
    [SerializeField]
    private Slider CountSellSlider;
    [SerializeField]
    private TMP_InputField CountSellInput;
    [SerializeField]
    private TMP_Text SellText;

    [Space(25)]

    [SerializeField]
    private GameObject TradePanel;
    [SerializeField]
    private Slider CountBuySlider;
    [SerializeField]
    private TMP_InputField CountBuyInput;
    [SerializeField]
    private TMP_Text CostText;

    [Space(25)]

    [SerializeField]
    private TMP_Text TextNameInfo;
    [SerializeField]
    private TMP_Text TextInfo;

    public void Active(
        string Name, string Info, 
        bool isUse = false, bool isRemove = false, bool isDrop = false, bool isDivige = false, bool isSell = false,
        bool isTrade = false
        )
    {
        gameObject.SetActive(true);
        
        UseButton.gameObject.SetActive(isUse);
        RemoveButton.gameObject.SetActive(isRemove);
        DropButton.gameObject.SetActive(isDrop);
        DivideButton.gameObject.SetActive(isDivige);
        SellPanel.SetActive(isSell);

        TradePanel.SetActive(isTrade);

        TextNameInfo.text = Name;
        TextInfo.text = Info;
    }

    public void Close() => gameObject.SetActive(false);

    public Button GetUseButton() => UseButton;
    public Button GetRemoveButton() => RemoveButton;
    public Button GetDropButton() => DropButton;
    public Button GetDivigeButton() => DivideButton;

    public Slider GetCountBuySlider() => CountBuySlider;
    public TMP_InputField GetCountBuyInput() => CountBuyInput;
    public TMP_Text GetCostText() => CostText;
    public TMP_Text GetSellText() => SellText;

    public Slider GetCountSellSlider() => CountSellSlider;
    public TMP_InputField GetCountSellInput() => CountSellInput;
}
