// Decompiled with JetBrains decompiler
// Type: StorecfgGenerator.SetupConfigJson
// Assembly: StorecfgGenerator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0723BBEC-1C2F-455C-81B7-FC369DB2048E
// Assembly location: C:\AgileMomentum\AgileMark\source\StoreCfgGenerator\StorecfgGenerator.exe

using System.Collections.Generic;

namespace StorecfgGenerator
{
  public class SetupConfigJson
  {
    public string CustomerID { get; set; }

    public string License { get; set; }

    public string PasswordForDev { get; set; }

    public List<string> ServerSettings { get; set; }
  }
}
