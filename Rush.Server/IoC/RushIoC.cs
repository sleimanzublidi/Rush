namespace Rush
{
    using System;
    using Rush.Data;
    using TinyIoC;

    public static class RushIoC
    {
        static RushIoC()
        {
            TinyIoCContainer.Current.Register<IStoreRepository, StoreMongoDbRepository>();
        }

        public static void Ininitalize()
        {}

        public static object Resolve(Type type)
        {
            return TinyIoCContainer.Current.Resolve(type);
        }
    }
}
