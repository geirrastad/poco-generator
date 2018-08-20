// Decompiled with JetBrains decompiler
// Type: POCOGen.DB.TableDef
// Assembly: POCOGen, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7E14F844-B8BF-4B4C-8F4A-E6B2FD050651
// Assembly location: C:\Users\geir\Desktop\POCO\POCOGen.exe

namespace POCOGen.DB
{
  public class TableDef
  {
    private string _name;
    private FieldDef[] _fields;
    private string _originalName;

    public string OriginalName
    {
      get
      {
        return this._originalName;
      }
      set
      {
        this._originalName = value;
      }
    }

    public FieldDef[] Fields
    {
      get
      {
        return this._fields;
      }
      set
      {
        this._fields = value;
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
