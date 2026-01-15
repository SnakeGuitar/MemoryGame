using Client.Core;
using Client.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Client.Helpers
{
    public static class WpfExtensions
    {
        public static async Task SafeExecute(this Button button, Func<Task> action, Window owner)
        {
            if (button == null)
            {
                return;
            }

            button.IsEnabled = false;
            var originalCursor = Mouse.OverrideCursor;
            Mouse.OverrideCursor = Cursors.Wait;

            try
            {
                await action();
            }
            catch (Exception ex)
            {
                ExceptionManager.Handle(ex, owner);
            }
            finally
            {
                if (Application.Current.MainWindow == owner)
                {
                    button.IsEnabled = true;
                }
                Mouse.OverrideCursor = originalCursor;
            }
        }
    }
}