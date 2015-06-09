

using System;
using System.Collections.Generic;
using System.Linq;
using RainIt.Interfaces;
using RainIt.Interfaces.Repository;

namespace RainIt.Business
{
    public class LookUpManager : ILookUpManager
    {
        public IRainItContext RainItContext { get; set; }
        public Dictionary<int, string> Roles
        {
            get
            {
                var list = RainItContext.RoleSet
                        .OrderBy(r => r.RoleId)
                        .ToDictionary(r => r.RoleId, r => r.Name);
                return list;
            }
            
        }
    }
}
