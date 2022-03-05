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
        }

        private void S_Resized(object sender, ResizedEventArgs e)
        {
            //e.NewSize
        }

        public void UpdateLabel(string txt)
        {
            label1.Text = txt;
        }

    }
}
