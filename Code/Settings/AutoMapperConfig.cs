// Decompiled with JetBrains decompiler
// Type: StorecfgGenerator.AutoMapperConfig
// Assembly: StorecfgGenerator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0723BBEC-1C2F-455C-81B7-FC369DB2048E
// Assembly location: C:\AgileMomentum\AgileMark\source\StoreCfgGenerator\StorecfgGenerator.exe

using AutoMapper;

namespace StorecfgGenerator
{
    public class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<MarkerJson, MarkerJson>()
                    .ForMember(dest => dest.CipherTextGridTemplate, opt => opt.Ignore())
                    .ForMember(dest => dest.CipherShapeGridTemplate, opt => opt.Ignore());
            });
        }
    }
}
