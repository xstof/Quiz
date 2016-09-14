using Autofac;
using Quiz.Model;

namespace Quiz.API
{
    public static class AutofacConfig
    {
        public static void Register(ContainerBuilder builder)
        {
            builder.RegisterType<InMemoryQuizRepository>().As<IQuizRepository>().SingleInstance();
        }
    }
}