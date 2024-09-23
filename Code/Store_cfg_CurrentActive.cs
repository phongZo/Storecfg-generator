// Decompiled with JetBrains decompiler
// Type: StorecfgGenerator.StoreCfg
// Assembly: StorecfgGenerator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0723BBEC-1C2F-455C-81B7-FC369DB2048E
// Assembly location: C:\AgileMomentum\AgileMark\source\StoreCfgGenerator\StorecfgGenerator.exe

namespace StorecfgGenerator
{
  public class StoreCfg
  {
    public StoreCfgJson CurrentStoreCfg = new StoreCfgJson();

    public static StoreCfg Instance { get; set; }

    public StoreCfg() => StoreCfg.Instance = this;

    public StoreCfgJson.GridItem[] ConvertToGridItems(bool[,] inputArray)
    {
      int length1 = inputArray.GetLength(0);
      int length2 = inputArray.GetLength(1);
      StoreCfgJson.GridItem[] gridItems = new StoreCfgJson.GridItem[length1 * length2];
      int index1 = 0;
      for (int index2 = 0; index2 < length1; ++index2)
      {
        for (int index3 = 0; index3 < length2; ++index3)
        {
          gridItems[index1] = new StoreCfgJson.GridItem()
          {
            Value = inputArray[index2, index3],
            RowIndex = index2,
            ColumnIndex = index3
          };
          ++index1;
        }
      }
      return gridItems;
    }
  }
}
