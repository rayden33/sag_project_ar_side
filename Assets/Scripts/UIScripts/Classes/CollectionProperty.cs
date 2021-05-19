using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionProperty
{
    private string _id;
    private string _collectionId;
    private string _density;
    private string _base;
    private string _pileHeight;
    private string _pileYarn;
    private string _yarnComposition;
    private string _weight;
    private string _edging;
    private string _design;
    private string _maintenance;

    public string Id
    {
        get { return _id; }
        set { _id = value; }
    }
    public string CollectionId
    {
        get { return _collectionId; }
        set { _collectionId = value; }
    }
    public string Density
    {
        get { return _density; }
        set { _density = value; }
    }
    public string Base
    {
        get { return _base; }
        set { _base = value; }
    }
    public string PileHeight
    {
        get { return _pileHeight; }
        set { _pileHeight = value; }
    }
    public string PileYarn
    {
        get { return _pileYarn; }
        set { _pileYarn = value; }
    }
    public string YarnComposition
    {
        get { return _yarnComposition; }
        set { _yarnComposition = value; }
    }
    public string Weight
    {
        get { return _weight; }
        set { _weight = value; }
    }
    public string Edging
    {
        get { return _edging; }
        set { _edging = value; }
    }
    public string Design
    {
        get { return _design; }
        set { _design = value; }
    }
    public string Maintenance
    {
        get { return _maintenance; }
        set { _maintenance = value; }
    }
}
