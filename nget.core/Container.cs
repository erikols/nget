using StructureMap;

namespace nget.core
{
    public static class Container
    {
        public static void Init()
        {
            ObjectFactory.Initialize(registry =>
                                     registry.Scan(x =>
                                         {
                                             x.TheCallingAssembly();
                                             x.WithDefaultConventions();
                                         })
                );
        }

        public static T GetService<T>()
        {
            return ObjectFactory.GetInstance<T>();
        }
    }
}