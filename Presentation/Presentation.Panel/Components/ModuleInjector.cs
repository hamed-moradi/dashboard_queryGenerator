using System.Web.Mvc;
using Asset.Infrastructure._App;
using SimpleInjector;
using SimpleInjector.Integration.Web.Mvc;

namespace Presentation.Panel.Components
{
    public class ModuleInjector
    {
        public static readonly Container Container = new Container();
        public static void Init()
        {
            Asset.Infrastructure._App.ModuleInjector.Init(Container);
            
            Container.Register(() => new MapperConfig().Init().CreateMapper(Container.GetInstance));
            Container.Register(() => new ServiceLocatorAdapter(Container));
            Container.Register<ILog4Net, Log4Net>(Lifestyle.Singleton);

            Domain.Application._App.ModuleInjector.Init(Container);

            Container.Verify();
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(Container));
        }
    }
}