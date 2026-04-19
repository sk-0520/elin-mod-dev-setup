using Elin.Plugin.Generator.Test.Use.PluginInfoGeneratorUseTestSandbox;
using System;

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
        public void SimpleInstanceResetTest()
        {
            var configInstance = new SimpleConfig();
            Assert.Throws<NotSupportedException>(() => configInstance.Reset());
        }

        [Fact]
        public void SimpleBindTest()
        {
            var configInstance = new SimpleConfig();
            var configBound = SimpleConfig.Bind(new BepInEx.Configuration.ConfigFile("NUL", false), configInstance);
            // バインドを通すとプロキシになってる
            Assert.IsNotType<SimpleConfig>(configBound);
            // プロキシなので使用者側は何も考える必要はない, というか使用者は型を知っておく必要はない(どうしても高速化したい場合はそうでもないがこのテストでは関係ない)
            Assert.IsAssignableFrom<SimpleConfig>(configBound);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 2)]
        [InlineData(2, 3)]
        public void SimpleBindResetTest(int initValue, int changeValue)
        {
            Assert.NotEqual(initValue, changeValue); // ここで死ぬ場合テストが間違ってる

            var configInstance = new SimpleConfig()
            {
                Value = initValue,
            };
            var configBound = SimpleConfig.Bind(new BepInEx.Configuration.ConfigFile("NUL", false), configInstance);
            // 初期化で通した値になっている(本当は ConfigFile 経由で前回値が読まれるから正確ではないがこのテストではファイルがないのでOK)
            Assert.Equal(initValue, configBound.Value);

            // 値変更は直感的に使用可能
            configBound.Value = changeValue;
            Assert.Equal(changeValue, configBound.Value);

            // リセットしたら初期値に戻る
            configBound.Reset();
            Assert.NotEqual(changeValue, configBound.Value);
            Assert.Equal(initValue, configBound.Value);
        }

        #endregion
    }
}
