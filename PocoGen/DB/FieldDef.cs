// Decompiled with JetBrains decompiler
// Type: POCOGen.DB.FieldDef
// Assembly: POCOGen, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7E14F844-B8BF-4B4C-8F4A-E6B2FD050651
// Assembly location: C:\Users\geir\Desktop\POCO\POCOGen.exe

namespace POCOGen.DB
{
  public class FieldDef
  {
    private string _name;
    private string _dataType;
    private string _description;
    private bool _isPrimaryKey;
    private bool _canBeNull;
    private int _fieldLength;

    public int FieldLength
    {
      get
      {
        return this._fieldLength;
      }
      set
      {
        this._fieldLength = value;
      }
    }

    public string Description
    {
      get
      {
        return this._description;
      }
      set
      {
        this._description = value;
      }
    }

    public bool CanBeNull
    {
      get
      {
        return this._canBeNull;
      }
      set
      {
        this._canBeNull = value;
      }
    }

    public bool IsPrimaryKey
    {
      get
      {
        return this._isPrimaryKey;
      }
      set
      {
        this._isPrimaryKey = value;
      }
    }

    public string DataType
    {
      get
      {
        return this._dataType;
      }
      set
      {
        this._dataType = value;
      }
    }

    public string Name
    {
      get
      {
        return this._name;
      }
      set
      {
        this._name = value;
      }
    }
  }
}
