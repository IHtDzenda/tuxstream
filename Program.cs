using System;
using TuxStream.Provider;
using TuxStream.Plugin;
using TuxStream.Core.UI;
using TuxStream.Core;
namespace TuxStream
{
    internal class Program
    {
        static void Main()
        {
            Install install = new Install();
            MainUI mainUI = new MainUI();
            mainUI.HomePage();
     
        }
    }
}