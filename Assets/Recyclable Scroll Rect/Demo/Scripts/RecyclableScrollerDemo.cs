using System.Collections.Generic;
using UnityEngine;
using PolyAndCode.UI;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using System.Collections;
/// <summary>
/// Demo controller class for Recyclable Scroll Rect. 
/// A controller class is responsible for providing the scroll rect with datasource. Any class can be a controller class. 
/// The only requirement is to inherit from IRecyclableScrollRectDataSource and implement the interface methods
/// </summary>

//Dummy Data model for demostraion


public class RecyclableScrollerDemo : MonoBehaviour, IRecyclableScrollRectDataSource
{
    [SerializeField]
    RecyclableScrollRect _recyclableScrollRect;

    [SerializeField]
    private int _dataLength;
    public GameBilliantController _controller;
    //Dummy data List
    private List<User> _userList = new List<User>();

    //Recyclable scroll rect's data source must be assigned in Awake.
    private void Awake()
    {
        _recyclableScrollRect.DataSource = this;
    }

    //Initialising _contactList with dummy data 
    public void InitData()
    {
        if (_userList != null)
        {
            _userList.Clear();
        }
        _userList = _controller.sfs.LastJoinedRoom.UserList;
        _recyclableScrollRect.ReloadData();
    
    }

    #region DATA-SOURCE

    /// <summary>
    /// Data source method. return the list length.
    /// </summary>
    public int GetItemCount()
    {
        return _userList.Count;
    }

    /// <summary>
    /// Data source method. Called for a cell every time it is recycled.
    /// Implement this method to do the necessary cell configuration.
    /// </summary>
    public void SetCell(ICell cell, int index)
    {
        //Casting to the implemented Cell
        var item = cell as DemoCell;
        item.ConfigureCell(_userList[index], index);
    }

    #endregion
}