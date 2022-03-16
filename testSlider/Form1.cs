using System;
using System.Windows.Forms;
using SliderControl;

namespace testSlider
{
    public partial class Form1 : Form
    {
        Slider s;

        public Form1()
        {
            InitializeComponent();

            s = new Slider(this);

            s.SpanResizing += S_SpanResizing;
            s.SpanResized += S_SpanResized;
            s.SpanMoving += S_SpanMoving;
            s.SpanMoved += S_SpanMoved;
            
        }

        private void S_SpanResizing(object sender, SpanResizedEventArgs e)
        {
            label1.Text = "SpanResizing : " + e.NewSize.ToString();
        }

        private void S_SpanResized(object sender, SpanResizedEventArgs e)
        {
            label2.Text = "SpanResized : " + e.NewSize.ToString();
        }

        private void S_SpanMoving(object sender, SpanMovedEventArgs e)
        {
            label3.Text = "SpanMoving : " + e.NewValue.ToString();
        }

        private void S_SpanMoved(object sender, SpanMovedEventArgs e)
        {
            label4.Text = "SpanMoved : " + e.NewValue.ToString();
        }
    }
}