using System;
using SliderControl;
using System.Windows.Forms;

namespace testSlider
{
    public partial class Form1 : Form
    {
        Slider s;

        public Form1()
        {
            InitializeComponent();

            s = new Slider(this);
            
            s.SpanResized += S_Resized;
            s.SpanResizing += S_Resizing;
            s.SpanMoved += S_CursorMoved;
            s.SpanMoving += S_CursorMoving;
        }

        private void S_Resizing(object sender, SpanResizedEventArgs e)
        {
            label1.Text = e.NewSize.ToString();
        }

        private void S_Resized(object sender, SpanResizedEventArgs e)
        {
            label2.Text = e.NewSize.ToString();
        }

        private void S_CursorMoving(object sender, SpanMovedEventArgs e)
        {
            label3.Text = e.NewValue.ToString();
        }

        private void S_CursorMoved(object sender, SpanMovedEventArgs e)
        {
            label4.Text = e.NewValue.ToString();
        }
    }
}