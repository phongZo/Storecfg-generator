// Decompiled with JetBrains decompiler
// Type: StorecfgGenerator.AutoMapperConfig
// Assembly: StorecfgGenerator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0723BBEC-1C2F-455C-81B7-FC369DB2048E
// Assembly location: C:\AgileMomentum\AgileMark\source\StoreCfgGenerator\StorecfgGenerator.exe

using AutoMapper;
using System;
using System.Linq.Expressions;

namespace StorecfgGenerator
{
  public class AutoMapperConfig
  {
    public static void Configure()
    {
      Mapper.Initialize((Action<IMapperConfigurationExpression>) (cfg => cfg.CreateMap<MarkerJson, MarkerJson>().ForMember<char[,]>((Expression<Func<MarkerJson, char[,]>>) (dest => dest.CipherTextGridTemplate), (Action<IMemberConfigurationExpression<MarkerJson, MarkerJson, char[,]>>) (opt => opt.Ignore()))));
    }
  }
}
