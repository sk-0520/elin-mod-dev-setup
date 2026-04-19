using Elin.Plugin.Generator.Test.Use.PluginInfoGeneratorUseTestSandbox;

namespace Elin.Plugin.Generator.Test.Use
{

    public class PluginInfoGeneratorUseTest
    {
        #region function

        [Fact]
        public void SimpleInstanceTest()
        {
            var configInstance = new SimpleConfig();
            Assert.IsType<SimpleConfig>(configInstance); // 当たり前だよなぁ！
        }

        [Fact]
        public void SimpleBindTest()
        {
            var configInstance = new SimpleConfig();
            var config = SimpleConfig.Bind(new BepInEx.Configuration.ConfigFile("NUL", false), configInstance);
            Assert.IsNotType<SimpleConfig>(config);
            Assert.IsType<SimpleConfigProxy>(config);

        }

        #endregion
    }
}
