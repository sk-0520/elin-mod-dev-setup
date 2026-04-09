using Elin.Plugin.Main.Samples;
using HarmonyLib;
using System;
using System.Diagnostics;
using System.Reflection;

namespace Elin.Plugin.Main
{
    partial class Plugin
    {
        #region function

        /// <summary>
        /// 起動時のプラグイン独自処理。
        /// </summary>
        /// <param name="harmony"></param>
        private void AwakePlugin(Harmony harmony)
        {
            // サンプル用パッチ処理のため削除してください
            PatchSample(harmony);
            //NOP
        }

        /// <summary>
        /// 終了時のプラグイン独自処理。
        /// </summary>
        private void OnDestroyPlugin()
        {
            //NOP
        }

        #endregion

        #region sample

        /// <summary>
        /// サンプル用パッチ処理です。
        /// </summary>
        /// <remarks>不要なので削除してください。</remarks>
        /// <param name="harmony"></param>
        [Conditional("DEBUG")]
        void PatchSample(Harmony harmony)
        {
            // ネストクラスのフル名（例: Namespace.SourceElement+Row）
            var nestedType = AccessTools.TypeByName($"{typeof(SourceElement).FullName}+{nameof(SourceElement.Row)}");

            // オリジナルメソッド（シグネチャを正確に指定）
            var original = AccessTools.Method(nestedType, nameof(SourceElement.Row.GetText), new Type[] { typeof(string), typeof(bool) });

            // Prefix の MethodInfo を取得（このクラス内に static メソッドを用意）
            var prefix = typeof(SourceElementRowPatch).GetMethod(nameof(SourceElementRowPatch.GetTextPrefix),
                BindingFlags.Static | BindingFlags.Public);

            harmony.Patch(original, prefix: new HarmonyMethod(prefix));
        }

        #endregion
    }
}
