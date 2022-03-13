using System.Windows.Forms;

namespace SliderControl
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Slider s = new Slider(this);
            s.Resized += S_Resized;
            s.CursorMoved += S_CursorMoved;
        }

        private void S_CursorMoved(object sender, CursorMovedEventArgs e)
        {
            label1.Text = e.NewValue.ToString();
            //e.NewValue
        }

        private void S_Resized(object sender, ResizedEventArgs e)
        {
            label2.Text = e.NewSize.ToString();
            //e.NewSize
        }

    }
}
