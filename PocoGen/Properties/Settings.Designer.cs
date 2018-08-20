// Decompiled with JetBrains decompiler
// Type: POCOGen.Properties.Settings
// Assembly: POCOGen, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7E14F844-B8BF-4B4C-8F4A-E6B2FD050651
// Assembly location: C:\Users\geir\Desktop\POCO\POCOGen.exe

using System.CodeDom.Compiler;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace POCOGen.Properties
{
  [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
  [CompilerGenerated]
  internal sealed class Settings : ApplicationSettingsBase
  {
    private static Settings defaultInstance = (Settings) SettingsBase.Synchronized((SettingsBase) new Settings());

    public static Settings Default
    {
      get
      {
        Settings defaultInstance = Settings.defaultInstance;
        return defaultInstance;
      }
    }
  }
}
