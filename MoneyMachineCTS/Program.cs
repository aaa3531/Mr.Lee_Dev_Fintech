using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoneyMachineCTS
{
    static class Program
    {
        //Recommended code for runtime skin initialization.  
        [STAThread]
        static void Main()
        {
            Assembly asm = typeof(DevExpress.UserSkins.BlueSkin).Assembly;
            DevExpress.Skins.SkinManager.Default.RegisterAssembly(asm);
            // Splash screens and wait forms created with the help of the SplashScreenManager component run in a separate thread.  
            // Information on custom skins registered in the main thread is not available in the splash screen thread  
            // until you call the SplashScreenManager.RegisterUserSkins method.  
            // To provide information on custom skins to the splash screen thread, uncomment the following line. 
            //SplashScreenManager.RegisterUserSkins(asm);


            //TODO : 현재는 ebest에 바로 로그인하나 바꿔야함.

            LoginForm loginForm = new LoginForm();

            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                AgreementForm agreementForm = new AgreementForm();

                if (agreementForm.ShowDialog() == DialogResult.OK)
                {
                    Application.Run(new MainForm());
                }
            }
        }

        // This code adds the "SkinRegistration" component to the Visual Studio toolbox  
        // Drop this component onto the main application form to be able to change skins at design time  
        public class SkinRegistration : Component
        {
            public SkinRegistration()
            {
                DevExpress.Skins.SkinManager.Default.RegisterAssembly(typeof(DevExpress.UserSkins.BlueSkin).Assembly);
            }
        }
    }
}
