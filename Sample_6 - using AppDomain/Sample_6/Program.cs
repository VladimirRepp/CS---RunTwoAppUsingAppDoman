using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Threading;

namespace Sample_6
{
    internal static class Program
    {
        private static AppDomain _drawDomain;        
        private static AppDomain _textDomain;    
        
        private static Assembly _drawAsm;     
        private static Assembly _textAsm;  
        
        private static Form _drawForm;       
        private static Form _textForm;      

        private static string _drawAppName = "DrawWindow";
        private static string _textAppName = "TextWindow";

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        [LoaderOptimization(LoaderOptimization.MultiDomain)] // для того, чтобы созданные нами домены имели доступ к исполняемому коду друг друга.
        static void Main()
        {
            Application.EnableVisualStyles();

            _drawDomain = AppDomain.CreateDomain(_drawAppName + " MyDomain");
            _textDomain = AppDomain.CreateDomain(_textAppName + " MyDomain");

            _drawAsm = _drawDomain.Load(AssemblyName.GetAssemblyName($"{_drawAppName}.exe"));
            _textAsm = _drawDomain.Load(AssemblyName.GetAssemblyName($"{_textAppName}.exe"));
          
            _drawForm = Activator.CreateInstance(_drawAsm.GetType($"{_drawAppName}.DrawForm")) as Form;
            _textForm = Activator.CreateInstance(_textAsm.GetType($"{_textAppName}.TextForm"),
             new object[]{ _drawAsm.GetModule($"{_drawAppName}.exe"), _drawForm}) as Form;

            new Thread(new ThreadStart(RunTextWindow)).Start();
            new Thread(new ThreadStart(RunDrawWindow)).Start();

            _drawDomain.DomainUnload += new EventHandler(DrawDomain_DomainUnload);
        }

        private static void DrawDomain_DomainUnload(object sender, EventArgs e)
        {
            MessageBox.Show($"Домен с именем - {(sender as AppDomain).FriendlyName} был успешно выгружен!");
        }

        static void RunDrawWindow()
        {
            _drawForm.ShowDialog();         // запускаем окно модально в текущем потоке
            AppDomain.Unload(_drawDomain);  // отгружаем домен приложения
        }

        static void RunTextWindow()
        {
            _textForm.ShowDialog(); // запускаем окно модально в текущем потоке
            
            // Завершаем работу приложения, следствием чего
            // станет закрытие потока 
            Application.Exit();
        }
    }
}
