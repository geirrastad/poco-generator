// Decompiled with JetBrains decompiler
// Type: POCOGen.Properties.Resources
// Assembly: POCOGen, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7E14F844-B8BF-4B4C-8F4A-E6B2FD050651
// Assembly location: C:\Users\geir\Desktop\POCO\POCOGen.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace POCOGen.Properties
{
  [CompilerGenerated]
  [DebuggerNonUserCode]
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals((object) POCOGen.Properties.Resources.resourceMan, (object) null))
          POCOGen.Properties.Resources.resourceMan = new ResourceManager("POCOGen.Properties.Resources", typeof (POCOGen.Properties.Resources).Assembly);
        return POCOGen.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get
      {
        return POCOGen.Properties.Resources.resourceCulture;
      }
      set
      {
        POCOGen.Properties.Resources.resourceCulture = value;
      }
    }
  }
}
