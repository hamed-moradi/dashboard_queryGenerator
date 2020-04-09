using SimpleInjector;

namespace Asset.Infrastructure._App
{
    public class ModuleInjector
    {
        public static void Init(Container container)
        {
            // چندریختی به شکل خیلی جالبی نقض شده :)
            container.Register<ISmsService, SmsService>();
            container.Register<ICandooSmsService, CandooSmsService>();
        }
    }
}