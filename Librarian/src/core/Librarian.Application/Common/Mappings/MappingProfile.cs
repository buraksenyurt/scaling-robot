using AutoMapper;
using Librarian.Application.Common.Mappings;
using System;
using System.Linq;
using System.Reflection;

namespace Travel.Application.Common.Mappings
{
    /*
     * Henüz nasıl çalıştığını keşfedemedim ancak Assembly içerisinde IMapFrom arayüzünü uyarlayan sınıfların,
     * Mapping metodunu çağırdı ve nesneden nesneye dönüşüm işlerini yürüttüğünü düşünüyorum.
     * 
     */
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var types = assembly.GetExportedTypes()
              .Where(
                        t => t.GetInterfaces()
                        .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>))
                    )
              .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod("Mapping") ?? type.GetInterface("IMapFrom`1").GetMethod("Mapping");
                methodInfo?.Invoke(instance, new object[] { this });
            }
        }
    }
}