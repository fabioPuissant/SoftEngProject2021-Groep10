using System.Diagnostics.CodeAnalysis;
using System;

namespace VmsApi.Data.Utils
{
  
    [ExcludeFromCodeCoverage]
    public class AppRole
    {
        public string Name { get; set; }
        public string NormalizedName { get; set; }
    }
}