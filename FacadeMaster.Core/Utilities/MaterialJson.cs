using FacadeMaster.Core.Models.Material;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace FacadeMaster.Core.Utilities
{
    /// <summary>
    /// 物料多态序列化配置：限制 $type 仅允许 FM_Material 子类，降低 TypeNameHandling 反序列化风险
    /// </summary>
    public static class MaterialJson
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
            TypeNameHandling = TypeNameHandling.Auto,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
            // 序列化private字段？
            //ContractResolver = new DefaultContractResolver(),
            SerializationBinder = new MaterialTypeBinder()
        };

        private sealed class MaterialTypeBinder : ISerializationBinder
        {
            public void BindToName(Type serializedType, out string assemblyName, out string typeName)
            {
                assemblyName = serializedType.Assembly.FullName;
                typeName = serializedType.FullName;
            }

            public Type BindToType(string assemblyName, string typeName)
            {
                var type = typeof(FM_Material).Assembly.GetType(typeName);
                if (type == null || !type.IsSubclassOf(typeof(FM_Material)))
                    throw new JsonSerializationException($"不允许反序列化的物料类型: {typeName}");
                return type;
            }
        }
    }
}
