using Microsoft.VisualStudio.TestTools.UnitTesting;
using Presentation.Panel.Components;

namespace Tests.Logic._App
{
    [TestClass]
    public class Startup
    {
        [AssemblyInitialize]
        public static void Init(TestContext testContext)
        {
            ModuleInjector.Init();
        }
    }

    [TestClass]
    public class Cleanup
    {
        [AssemblyCleanup]
        public static void Init() {}
    }
}