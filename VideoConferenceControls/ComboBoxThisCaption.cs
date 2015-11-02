﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VideoConferenceControls
{
    public partial class ComboBoxThisCaption : UserControl
    {
        public ComboBoxThisCaption()
        {
            InitializeComponent();
        }

        public string Caption
        {
            get { return label1.Text; }
            set { label1.Text = value; }
        }

        public ComboBox ComboBox
        {
            get { return comboBox1; }
            set { comboBox1 = value; }
        }
    }
}