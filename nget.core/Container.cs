using Amazon.S3;
using StructureMap;
using nget.core.S3;

namespace nget.core
{
    public static class Container
    {
        public static void Init()
        {
            ObjectFactory.Initialize(registry =>
                {
                    registry.Scan(x =>
                        {
                            x.TheCallingAssembly();
                            x.WithDefaultConventions();
                        });
                    registry.For<AmazonS3>().Use(ctx => ctx.GetInstance<S3ClientFactory>().CreateClient());
                });
        }

        public static T GetService<T>()
        {
            return ObjectFactory.GetInstance<T>();
        }
    }
}