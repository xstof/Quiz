using Autofac;
using Quiz.Model;

namespace Quiz.API
{
    public static class AutofacConfig
    {
        public static void Register(ContainerBuilder builder)
        {
            builder.RegisterType<InMemoryQuizRepository>().Named<IQuizRepository>("innerrepo").SingleInstance();
            builder.Register(c => new PrepopulatedQuizRepositoryDecorator(c.ResolveNamed<IQuizRepository>("innerrepo")))
                   .As<IQuizRepository>().SingleInstance();

        }
    }
}