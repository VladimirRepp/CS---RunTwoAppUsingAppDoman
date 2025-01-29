using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

// В свойствах проекта измене адрес сборки ехе файла для удобства
namespace TextWindow
{
    public partial class TextForm : Form
    {
        private Module _drawerModule { get; set; }
        private object _drawer;

        public TextForm()
        {
            InitializeComponent();
            _drawerModule = null;
            _drawer = null;
        }

        public TextForm(Module drawer, object targetWindow)
        {
            InitializeComponent();
            _drawerModule = drawer;
            _drawer = targetWindow;
        }

        private void TextForm_Load(object sender, EventArgs e)
        {

        }

        private void textBox_Input_TextChanged(object sender, EventArgs e)
        {
            if (_drawerModule != null)
            {
                _drawerModule.GetType("DrawWindow.DrawForm").
                    GetMethod("SetText").
                    Invoke(_drawer, new object[] { textBox_Input.Text });
            }
        }

        private void TextForm_LocationChanged(object sender, EventArgs e)
        {
            if (_drawerModule != null)
            {
                _drawerModule.GetType("DrawWindow.DrawForm").
                    GetMethod("MoveWindow").
                    Invoke(_drawer,
                    new object[] { new Point(this.Location.X, this.Location.Y + this.Height), this.Width }
                    );
            }
        }
    }
}
