using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GaudyCalculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Enables Traditional mode (Default)
        private void rb1_CheckedChanged(object sender, EventArgs e)
        {
            if (rb1.Checked)
            {
                pb1.Visible = false;
                rb1.BackColor = Control.DefaultBackColor;
                rb2.BackColor = Control.DefaultBackColor;
                label1.BackColor = Control.DefaultBackColor;
                label2.BackColor = Control.DefaultBackColor;
                label3.BackColor = Control.DefaultBackColor;
            }
        }

        // Enables Gaudy Mode 
        private void rb2_CheckedChanged(object sender, EventArgs e)
        {
            if (rb2.Checked)
            {
                pb1.Visible = true;
                rb1.BackColor = Color.Cyan;
                rb2.BackColor = Color.Cyan;
                label1.BackColor = Color.DeepPink;
                label2.BackColor = Color.Goldenrod;
                label3.BackColor = Color.Green;
            }
        }

        private void bAdd_Click(object sender, EventArgs e)
        {
            bool b1, b2;
            InputParse(out b1, out b2);
            // Calculation between two boxes is performed else error is thrown
            if (b1 && b2) tb3.Text = Math.Round((Convert.ToDouble(tb1.Text) + Convert.ToDouble(tb2.Text)),4).ToString();
            else tb3.Text = "NaN";
        }

        private void bSub_Click(object sender, EventArgs e)
        {
            bool b1, b2;
            InputParse(out b1, out b2);
            // Calculation between two boxes is performed else error is thrown
            if (b1 && b2) tb3.Text = Math.Round((Convert.ToDouble(tb1.Text) - Convert.ToDouble(tb2.Text)),4).ToString();
            else tb3.Text = "NaN";
        }

        private void bMult_Click(object sender, EventArgs e)
        {
            bool b1, b2;
            InputParse(out b1, out b2);
            // Calculation between two boxes is performed else error is thrown
            if (b1 && b2) tb3.Text = Math.Round((Convert.ToDouble(tb1.Text) * Convert.ToDouble(tb2.Text)),4).ToString();
            else tb3.Text = "NaN";
        }

        private void bDiv_Click(object sender, EventArgs e)
        {
            bool b1, b2;
            InputParse(out b1, out b2);
            // Calculation between two boxes is performed else error is thrown
            // If denominator is equal to zero, then the resulting text will inform there is an error
            if (b1 && b2)
                tb3.Text = (Convert.ToDouble(tb2.Text).Equals(0)) ? "Undefined" :
                    Math.Round((Convert.ToDouble(tb1.Text) / Convert.ToDouble(tb2.Text)),4).ToString();
            else tb3.Text = "NaN";
        }

        // When User enters in value box, the previous answer will be cleared
        private void tb1_Enter(object sender, EventArgs e) { tb3.ResetText(); }
        private void tb2_Enter(object sender, EventArgs e) { tb3.ResetText(); }

        public void InputParse(out bool b1, out bool b2)
        {
            double input1, input2;
            // Sets default color to input textboxes
            tb1.BackColor = Control.DefaultBackColor;
            tb2.BackColor = Control.DefaultBackColor;
            // Parsing Routine
            b1 = Double.TryParse(tb1.Text, out input1);
            b2 = Double.TryParse(tb2.Text, out input2);
            // Non-Integer value is witten
            if (!b1) tb1.BackColor = Color.Red;
            if (!b2) tb2.BackColor = Color.Red;
        }
    }
}
