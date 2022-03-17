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

            UpdateParams();
        }

        private void UpdateParams()
        {
            num_value.Value = s.CurrentValue;
            num_span.Value = s.CurrentSpan;
            num_smallChange.Value = s.SmallChange;
            num_largeChange.Value = s.LargeChange;
        }


        private void S_SpanResizing(object sender, SpanResizedEventArgs e)
        {
            label1.Text = "SpanResizing : " + e.NewSize.ToString();
        }

        private void S_SpanResized(object sender, SpanResizedEventArgs e)
        {
            label2.Text = "SpanResized : " + e.NewSize.ToString();
            UpdateParams();
        }

        private void S_SpanMoving(object sender, SpanMovedEventArgs e)
        {
            label3.Text = "SpanMoving : " + e.NewValue.ToString();
        }

        private void S_SpanMoved(object sender, SpanMovedEventArgs e)
        {
            label4.Text = "SpanMoved : " + e.NewValue.ToString();
            UpdateParams();
        }

        private void b_apply_Click(object sender, EventArgs e)
        {
            //deactivate events temporarly
            s.SpanResizing -= S_SpanResizing;
            s.SpanResized -= S_SpanResized;
            s.SpanMoving -= S_SpanMoving;
            s.SpanMoved -= S_SpanMoved;

            s.SetValue((int)num_value.Value);
            s.SetSpan((int)num_span.Value);
            s.SmallChange = (int)num_smallChange.Value;
            s.LargeChange = (int)num_largeChange.Value;

            //set back events
            s.SpanResizing += S_SpanResizing;
            s.SpanResized += S_SpanResized;
            s.SpanMoving += S_SpanMoving;
            s.SpanMoved += S_SpanMoved;

        }
    }
}