using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swpublished;

using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace MpcExport
{
    public class MpcExport : ISwAddin
    {

        #region Private Members
        private TaskpaneView mTaskpaneView;
        private TaskpaneHostUI mTaskpaneHost;
        #endregion

        #region Public Members
        public const string SWTASKPANE_PROGID = "MpcExport.SolidWorks.Addin";
        #endregion


        int SessionCookie;
        SldWorks swApp;
        #region SolidWorks connection

        public bool ConnectToSW(object ThisSW, int Cookie)
        {
            swApp = ThisSW as SldWorks;
            swApp.SetAddinCallbackInfo(0, this, Cookie);
            SessionCookie = Cookie;
            swApp.FileNewNotify2 += SwApp_FileNewNotify2;
            LoadUI();
            return true;
        }

        private void LoadUI()
        {
            mTaskpaneView = swApp.CreateTaskpaneView2("", "Mpc Export Add-in");
            mTaskpaneHost = (TaskpaneHostUI)mTaskpaneView.AddControl(SWTASKPANE_PROGID, string.Empty);
            mTaskpaneHost.SetSwApp(swApp);
        }

        private int SwApp_FileNewNotify2(object NewDoc, int DocType, string TemplateName)
        {
            swApp.SendMsgToUser("New file created!");
            return 0;
        }

        public bool DisconnectFromSW()
        {
            UnloadUI();
            GC.Collect();
            swApp = null;
            return true;
        }

        private void UnloadUI()
        {
            mTaskpaneHost = null;
            mTaskpaneView.DeleteView();
            Marshal.ReleaseComObject(mTaskpaneView);
            mTaskpaneView = null;
        }
        #endregion

        #region com register-unregister functions

        [ComRegisterFunction]
        private static void RegisterAssembly(Type t)
        {
            string Path = String.Format(@"SOFTWARE\SolidWorks\AddIns\{0:b}", t);
            RegistryKey Key = Registry.LocalMachine.CreateSubKey(Path);

            //startup int
            Key.SetValue(null, 1);
            Key.SetValue("Title", "MpcExport");
            Key.SetValue("Description", "This is a description");
        }

        [ComUnregisterFunction]
        private static void UnregisterAssembly(Type t)
        {
            string Path = String.Format(@"SOFTWARE\SolidWorks\AddIns\{0:b}", t);
            Registry.LocalMachine.DeleteSubKey(Path);
        }

        #endregion
    }
}
