using UnityEngine;
using UnityEngine.UI;
using PolyAndCode.UI;
using TMPro;
using Sfs2X.Entities;

//Cell class for demo. A cell in Recyclable Scroll Rect must have a cell class inheriting from ICell.
//The class is required to configure the cell(updating UI elements etc) according to the data during recycling of cells.
//The configuration of a cell is done through the DataSource SetCellData method.
//Check RecyclableScrollerDemo class
public class DemoCell : MonoBehaviour, ICell
{
    //UI
    public TextMeshProUGUI nameLabel;
    public TextMeshProUGUI idLabel;

    //Model
    private int _cellIndex;

    private void Start()
    {
        //Can also be done in the inspector
        GetComponent<Button>().onClick.AddListener(ButtonListener);
    }

    //This is called from the SetCell method in DataSource
    public void ConfigureCell(User contactInfo,int cellIndex)
    {
        _cellIndex = cellIndex;
        nameLabel.text = contactInfo.Name;
        idLabel.text = contactInfo.PlayerId.ToString();
    }

    
    private void ButtonListener()
    {
   
    }
}
