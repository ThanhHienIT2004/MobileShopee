﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MobileShopee
{
    public partial class Intro: Form
    {
        public Intro()
        {
            InitializeComponent();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var form = new Form();
            form = new AuthScreen();
            form.FormClosed += (s, args) => this.Close();
            form.Show();
            this.Hide();
        }
    }
}
