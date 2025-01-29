using System;
using System.Drawing;
using System.Windows.Forms;

// В свойствах проекта измене адрес сборки ехе файла для удобства
namespace DrawWindow
{
    public partial class DrawForm : Form
    {
        private string _sourceText = "No text was added!";
        private Font _drawingFont;

        public DrawForm()
        {
            InitializeComponent();

            _drawingFont = new Font("Arial", 45);
            panel.Paint += Panel_Paint;
            this.Paint += DrawForm_Paint;
        }

        private void DrawForm_Paint(object sender, PaintEventArgs e)
        {
            Panel_Paint(panel, new PaintEventArgs(panel.CreateGraphics(), panel.ClientRectangle));
        }

        private void Panel_Paint(object sender, PaintEventArgs e)
        {
            if (_sourceText.Length > 0)
            {
                // Cоздаём буферное изображение, основываясь на
                // размерах клиентской части элемента управления Panel
                Image img = new Bitmap(panel.ClientRectangle.Width, panel.ClientRectangle.Height);

                // Получаем графический контекст созданого нами изображения
                Graphics imgDC = Graphics.FromImage(img);
                // Очищаем изображение используя цвет фона окна
                imgDC.Clear(BackColor);

                // Прорисовываем на элементе управления Panel
                // текст используя выбранный шрифт
                imgDC.DrawString(_sourceText, _drawingFont, Brushes.Brown, ClientRectangle,
                new StringFormat(StringFormatFlags.NoFontFallback));

                // Прорисовываем изображение на элементе управления Panel
                e.Graphics.DrawImage(img, 0, 0);
            }
        }

        private void DrawForm_Load(object sender, EventArgs e)
        {

        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog dlg = new FontDialog();
            dlg.Font = _drawingFont;

            if (dlg.ShowDialog() == DialogResult.OK)
                _drawingFont = dlg.Font;

            Panel_Paint(panel, new PaintEventArgs(panel.CreateGraphics(), panel.ClientRectangle));
        }

        public void SetText(string text)
        {
            _sourceText = text;
            Panel_Paint(panel, new PaintEventArgs(panel.CreateGraphics(), panel.ClientRectangle));
        }
        
        public void MoveWindow(Point newLocation, int width)
        {
            this.Location = newLocation;
            this.Width = width;
        }
    }
}
