using System;
using System.Collections.Generic;

[Serializable]
public class LevelConfig
{
    public float gameSpeed = 0.5f;
    public List<Item> box;
}

[Serializable]
public class Item
{
    public List<ItemIndex> item;
}

[Serializable]
public class ItemIndex
{
    public int index;
}