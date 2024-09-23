// Decompiled with JetBrains decompiler
// Type: StorecfgGenerator.MotdJson
// Assembly: StorecfgGenerator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0723BBEC-1C2F-455C-81B7-FC369DB2048E
// Assembly location: C:\AgileMomentum\AgileMark\source\StoreCfgGenerator\StorecfgGenerator.exe

namespace StorecfgGenerator
{
  public class MotdJson
  {
    public string Hash { get; set; } = "0";

    public string ImageUrl { get; set; } = "";

    public string ImageHash { get; set; } = "";

    public string Link { get; set; } = "";

    public int[,] Position { get; set; } = new int[3, 3];

    public string DisplayCondition { get; set; } = "AgentNextContact";

    public string UpdateTime { get; set; } = "";

    public int DisplayCount { get; set; } = 0;

    public int Counting { get; set; } = 0;

    public string EndTime { get; set; } = "";
  }
}
