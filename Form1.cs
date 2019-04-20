using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime;
using System.Runtime.InteropServices;

namespace UacControlPanel
{
    public partial class Form1 : Form
    {
        [DllImport("advapi32.dll")]
        public static extern bool InitiateSystemShutdown(string Machinename, string Message, long Timeout, int ForceAppsClosed, int RebootAfterShutdown);

        private Engine en;
        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                en = new Engine();
            }
            catch (Exception ex)
            {
                MessageBox.Show(null, ex.Message + "\nIl programma verrà chiuso.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Environment.Exit(0);
                
            }
            comboBox1.SelectedIndex = (int) en.getUac();
            comboBox2.SelectedIndex = (int)en.getElevamento();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool riavvio=false;
            if (comboBox1.SelectedIndex == 0)
                riavvio=en.disabilitaUac();
            else
                riavvio=en.abilitaUac((Engine.STATO_ELEVAMENTO) Enum.ToObject(typeof(Engine.STATO_ELEVAMENTO), comboBox2.SelectedIndex));
            if (riavvio)
            {
                label3.Text = "E' necessario riavviare il computer per rendere effettive le modifiche.";
                button1.Visible = false;
                button3.Visible = true;
                label3.Visible = true;
            }
            else
            {
                label3.Text = "Operazione completata. Premere \"Esci\" per uscire.";
                label3.Visible = true;
                button1.Visible = false;
                button2.Visible = true;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Enabled = (comboBox1.SelectedIndex != 0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            InitiateSystemShutdown("", "", 0, 1, 1);
        }
    }
}
