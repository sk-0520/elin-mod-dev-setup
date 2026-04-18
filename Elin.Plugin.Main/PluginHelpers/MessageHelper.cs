using System;
using System.IO;
using System.Threading;

namespace Elin.Plugin.Main.PluginHelpers
{
    public class MessageColorScope : IDisposable
    {
        public MessageColorScope(Color previousColor)
        {
            PreviousColor = previousColor;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #region property

        private Color PreviousColor { get; }

        #endregion

        #region IDisposable

        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Msg.SetColor(PreviousColor);
                }

                this._disposedValue = true;
            }
        }

        #endregion
    }

    public class MessageHelper
    {
        public MessageHelper(SynchronizationContext context)
        {
            Context = context;
        }

        #region property

        private SynchronizationContext Context { get; }

        /// <summary>
        /// メッセージ出力可能なシーンか。
        /// </summary>
        public static bool CanOutputMessage => Scene.scene.mode switch
        {
            Scene.Mode.Zone => true,
            Scene.Mode.StartGame => true,
            _ => false,
        };

        #endregion

        #region function

        public void DoMessage(Action action)
        {
            if (!CanOutputMessage)
            {
                return;
            }

            // TODO: とりあえず全部 Post に投げ込んでいるが、同期スレッドで実行している場合の分岐とかあった方がいい, 一旦今はこれでいい
            Context.Post(static a =>
            {
                ((Action)a)();
            }, action);
        }

        /// <summary>
        /// 現在の色を復元可能にする。
        /// </summary>
        /// <returns>戻し処理。</returns>
        public IDisposable PreserveColor()
        {
            var result = new MessageColorScope(Msg.currentColor);
            return result;
        }

        /// <summary>
        /// 色を変更しつつ、現在の色を復元可能にする。
        /// </summary>
        /// <param name="color">変更したい色。</param>
        /// <returns><inheritdoc cref="PreserveColor"/></returns>
        public IDisposable UseColor(Color color)
        {
            var result = PreserveColor();
            Msg.SetColor(color);
            return result;
        }

        public void OutputLineWithoutContext(string message)
        {
            Msg.SayRaw(message);
            Msg.NewLine();
        }

        public void OutputLine(string message)
        {
            DoMessage(() =>
            {
                OutputLineWithoutContext(message);
            });
        }

        public string ToTimestamp(DateTime dateTime) => dateTime.ToString("O");

        public string ToLogHeader(string timestamp, string callerMemberName, string callerFilePath, int callerLineNumber)
        {
            return $"[{timestamp}] {callerMemberName} <{Path.GetFileName(callerFilePath)}:{callerLineNumber}>";
        }

        public string ToLogData<T>(T data)
        {
            return data switch
            {
                null => "<null>",
                string str => str,
                _ => data.ToString() ?? $"<{typeof(T).FullName}:null>"
            };
        }

        #endregion
    }
}
