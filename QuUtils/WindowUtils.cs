using System.Security.Permissions;
using System.Threading;
using System.Windows.Threading;

namespace QuUtils
{
    /// <summary>
    /// 
    /// </summary>
    public class WindowUtils
    {
        private const int UiRefreshTime = 10;
        private const DispatcherPriority Priority = DispatcherPriority.Send;

        /// <summary>
        /// 
        /// </summary>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static void DoEvents()
        {
            var frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(Priority,
                new DispatcherOperationCallback(delegate
                {
                    frame.Continue = false;
                    return null;
                }), null);
            Dispatcher.PushFrame(frame);
            Thread.Sleep(UiRefreshTime);
        }
    }
}
