using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectFF_Devops_Sec_2023
{
    public partial class Form1 : Form
    {
        private Color currentColor;
        private readonly Random random = new Random();
        public Form1()
        {
            InitializeComponent();

            currentColor = Color.Black;
            Task.Run(() => ContinuousColorChange());
        }
        private async Task ContinuousColorChange()
        {
            while (true)
            {
                // Generate a new random color for the label's forecolor
                Color newColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));

                // Transition smoothly from the current color to the new color
                int steps = 10; // Number of steps for the transition

                for (int i = 0; i <= steps; i++)
                {
                    int r = (currentColor.R * (steps - i) + newColor.R * i) / steps;
                    int g = (currentColor.G * (steps - i) + newColor.G * i) / steps;
                    int b = (currentColor.B * (steps - i) + newColor.B * i) / steps;

                    // Update the label's forecolor
                    ChangeColor(Color.FromArgb(r, g, b));

                    // Pause briefly to create a smooth transition effect
                    await Task.Delay(50);
                }

                // Update the current color for the next iteration
                currentColor = newColor;
            }
        }
        private void LoginBtn_Click(object sender, EventArgs e)
        {
            LoginPanel.Visible = false;
        }
        private void ChangeColor(Color color)
        {
            if (label1.InvokeRequired)
            {
                label1.Invoke((MethodInvoker)(() => label1.ForeColor = color));
            }
            else
            {
                label1.ForeColor = color;
            }
        }
    }
}
