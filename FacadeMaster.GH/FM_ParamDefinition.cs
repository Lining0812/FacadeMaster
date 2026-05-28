using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacadeMaster.GH
{
    public abstract class FM_ZuiComponent : GH_Component
    {
        protected enum ParamRelevance
        {
            Binding,
            Primary,
            Secondary,
            Tertiary,
            Quarternary,
            Quinary,
            Senary,
            Septenary,
            Occasional,
            None
        };

        protected class ParamDefinition
        {
            public readonly IGH_Param Param;
            public readonly ParamRelevance Relevance;
            public readonly object Tag;

            public ParamDefinition(IGH_Param param)
            {
                Param = param;
                Relevance = ParamRelevance.Binding;
                Tag = null;
            }

            public ParamDefinition(IGH_Param param, ParamRelevance relevance)
            {
                Param = param;
                Relevance = relevance;
                Tag = null;
            }

            public ParamDefinition(IGH_Param param, ParamRelevance relevance, object tag)
            {
                Param = param;
                Relevance = relevance;
                Tag = tag;
            }

            public static ParamDefinition Create<T>(string name, string nickname, string description = "", GH_ParamAccess access = GH_ParamAccess.item, bool optional = false, ParamRelevance relevance = ParamRelevance.Binding) where T : class, IGH_Param, new()
            {
                T val = new T();
                val.Name = name;
                val.NickName = nickname;
                val.Description = description;
                val.Access = access;
                val.Optional = optional;
                return new ParamDefinition(val, relevance);
            }

        }


    }
}
