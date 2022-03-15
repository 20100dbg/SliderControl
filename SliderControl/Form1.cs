using System.Windows.Forms;

namespace SliderControl
{
    public partial class Form1 : Form
    {
        Slider s;

        public Form1()
        {
            InitializeComponent();

            s = new Slider(this);

            s.Resized += S_Resized;
            s.Resizing += S_Resizing;
            s.CursorMoved += S_CursorMoved;
            s.CursorMoving += S_CursorMoving;
        }

        private void S_Resizing(object sender, ResizedEventArgs e)
        {
            label1.Text = e.NewSize.ToString();
        }

        private void S_Resized(object sender, ResizedEventArgs e)
        {
            label2.Text = e.NewSize.ToString();
        }

        private void S_CursorMoving(object sender, CursorMovedEventArgs e)
        {
            label3.Text = e.NewValue.ToString();
        }

        private void S_CursorMoved(object sender, CursorMovedEventArgs e)
        {
            label4.Text = e.NewValue.ToString();
        }
    }
}
