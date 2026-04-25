using BepInEx;
using Elin.Plugin.Generated;
using Elin.Plugin.Main.PluginHelpers;
using HarmonyLib;
using System;
using System.Threading;
using UnityEngine.Events;

namespace Elin.Plugin.Main
{
    [BepInPlugin(Package.Id, Mod.Name, Mod.Version)]
    public partial class Plugin : BaseUnityPlugin
    {
        #region property

        /// <summary>
        /// <see cref="Harmony.PatchAll()"/>を呼び出すか。
        /// </summary>
        /// <remarks>
        /// <para>デフォルトの<see langword="true"/>で問題ない。</para>
        /// </remarks>
        private bool CallPatchAll { get; set; } = true;

        #endregion

        #region function

        /// <summary>
        /// 起動。
        /// </summary>
        /// <remarks>本メソッドではインフラ面の構築も行っているため、プラグインとしての起動処理は <see cref="AwakePlugin(Harmony)"/> で実施する。</remarks>
        public void Awake()
        {
            ModHelper.Initialize(this, Logger, SynchronizationContext.Current);

            var harmony = new Harmony(Package.Id);

            AwakePlugin(harmony);

            if (CallPatchAll)
            {
                harmony.PatchAll();
            }
            PatchTemplate(harmony);
        }

        public void OnDestroy()
        {
            OnDestroyPlugin();
            ModHelper.Destroy();
        }

        private partial void PatchTemplate(Harmony harmony);

#if DEBUG

        private static bool PatchTemplatePublishPrefix(Func<string> funcText, UnityAction action)
        {
            ModHelper.LogDev($"idLang: {funcText}, hideAfter: {action}");
            if (funcText() == "mod_publish")
            {
                return false;
            }

            return true;
        }

        private void PatchTemplatePublish(Harmony harmony)
        {
            var addButtonOriginMethod = AccessTools.Method(typeof(UIContextMenu), nameof(UIContextMenu.AddButton), new[] { typeof(Func<string>), typeof(UnityAction) });
            var addButtonPrefixMethod = AccessTools.Method(typeof(Plugin), nameof(PatchTemplatePublishPrefix));
            ModHelper.LogDev($"addButtonOriginMethod: {addButtonOriginMethod}, addButtonPrefixMethod: {addButtonPrefixMethod}");
            harmony.Patch(
                addButtonOriginMethod,
                prefix: new HarmonyMethod(addButtonPrefixMethod)
            );
        }

        private partial void PatchTemplate(Harmony harmony)
        {
            PatchTemplatePublish(harmony);
        }
#endif

        #endregion
    }
}
