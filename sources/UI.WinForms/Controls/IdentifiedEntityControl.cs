﻿using Queue.Services.DTO;
using System;
using System.Windows.Forms;

namespace Queue.UI.WinForms
{
    public partial class IdentifiedEntityControl : UserControl
    {
        public bool UseResetButton
        {
            get
            {
                return resetButton.Visible;
            }
            set
            {
                resetButton.Visible = value;
            }
        }

        public event EventHandler<EventArgs> SelectedChanged;

        private bool frozen = true;

        public IdentifiedEntityControl()
        {
            InitializeComponent();
        }

        public void Initialize<T>(IdentifiedEntity[] entities) where T : IdentifiedEntity
        {
            frozen = true;

            comboBox.Items.Clear();
            comboBox.Items.AddRange(entities);
            Enabled = entities.Length > 0;

            frozen = false;
        }

        public T Selected<T>() where T : IdentifiedEntity
        {
            return comboBox.SelectedItem != null ? (comboBox.SelectedItem as IdentifiedEntity).Cast<T>() : null;
        }

        public void Select<T>(T value) where T : IdentifiedEntity
        {
            frozen = true;

            comboBox.SelectedItem = value;

            frozen = false;
        }

        public void Clear()
        {
            comboBox.Items.Clear();
            Enabled = false;
        }

        private void resetButton_VisibleChanged(object sender, System.EventArgs e)
        {
            //comboBox.Width += resetButton.Visible ? -resetButton.Width : resetButton.Width;
        }

        private void resetButton_Click(object sender, System.EventArgs e)
        {
            comboBox.SelectedItem = null;
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedChanged != null && !frozen)
            {
                SelectedChanged(this, new EventArgs());
            }
        }
    }
}