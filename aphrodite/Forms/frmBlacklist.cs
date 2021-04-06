﻿using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace aphrodite {
    public partial class frmBlacklist : Form {

        public frmBlacklist() {
            InitializeComponent();
        }

        private void frmBlacklist_Load(object sender, EventArgs e) {
            if (Program.UseIni) {
                if (File.Exists(Environment.CurrentDirectory + "\\graylist.cfg")) {
                    rtbBlacklist.Text = File.ReadAllText(Environment.CurrentDirectory + "\\graylist.cfg").Replace(' ', '\n');
                }

                if (File.Exists(Environment.CurrentDirectory + "\\blacklist.cfg")) {
                    rtbZTB.Text = File.ReadAllText(Environment.CurrentDirectory + "\\blacklist.cfg").Replace(' ', '\n');
                }

                lbMutual.Location = new Point(12, 25);
                lbIni.Visible = true;
            }
            else {
                General.Default.Reload();
                rtbBlacklist.Text = General.Default.blacklist.Replace(' ', '\n');
                rtbZTB.Text = General.Default.zeroToleranceBlacklist.Replace(' ', '\n');
            }
        }

        private void rtbBlacklist_TextChanged(object sender, EventArgs e) {
            var txtSender = (RichTextBox)sender;
            var curPos = txtSender.SelectionStart;
            txtSender.Text = Regex.Replace(txtSender.Text, " ", "\r\n");
            txtSender.SelectionStart = curPos;
        }
        private void rtbZTB_TextChanged(object sender, EventArgs e) {
            var txtSender = (RichTextBox)sender;
            var curPos = txtSender.SelectionStart;
            txtSender.Text = Regex.Replace(txtSender.Text, " ", "\r\n");
            txtSender.SelectionStart = curPos;
        }

        private void btnSort_Click(object sender, EventArgs e) {
            rtbBlacklist.Text = rtbBlacklist.Text.Replace(" ", "\n");
            ListBox blacklist = new ListBox();
            string[] bl;
            bl = rtbBlacklist.Text.Split('\n');
            for (int y = 0; y < bl.Length; y++) {
                blacklist.Items.Add(bl[y]);
            }
            blacklist.Sorted = true;
            rtbBlacklist.Clear();
            for (int y = 0; y < blacklist.Items.Count; y++) {
                rtbBlacklist.Text += blacklist.Items[y] + "\n";
            }
            rtbBlacklist.Text = rtbBlacklist.Text.TrimEnd('\n');
        }
        private void btnSortZTB_Click(object sender, EventArgs e) {
            rtbZTB.Text = rtbZTB.Text.Replace(" ", "\n");
            ListBox blacklist = new ListBox();
            string[] bl;
            bl = rtbZTB.Text.Split('\n');
            for (int y = 0; y < bl.Length; y++) {
                blacklist.Items.Add(bl[y]);
            }
            blacklist.Sorted = true;
            rtbZTB.Clear();
            for (int y = 0; y < blacklist.Items.Count; y++) {
                rtbZTB.Text += blacklist.Items[y] + "\n";
            }
            rtbZTB.Text = rtbZTB.Text.TrimEnd('\n');
        }

        private void btnSave_Click(object sender, EventArgs e) {
            if (Program.UseIni) {
                if (rtbBlacklist.TextLength > 0)
                    File.WriteAllText(Environment.CurrentDirectory + "\\graylist.cfg", rtbBlacklist.Text.Replace(" ", "_").Replace("\r\n", " "));
                else
                    File.Delete(Environment.CurrentDirectory + "\\graylist.cfg");

                if (rtbZTB.TextLength > 0)
                    File.WriteAllText(Environment.CurrentDirectory + "\\blacklist.cfg", rtbZTB.Text.Replace(" ", "_").Replace("\r\n", " "));
                else
                    File.Delete(Environment.CurrentDirectory + "\\blacklist.cfg");
            }
            else {
                General.Default.blacklist = rtbBlacklist.Text.Replace(" ", "_").Replace("\r\n", " ");
                General.Default.zeroToleranceBlacklist = rtbZTB.Text.Replace(" ", "_").Replace("\r\n", " ");
                General.Default.Save();
            }
            this.DialogResult = DialogResult.OK;
        }
        private void btnCancel_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
        }

        private void llGraylist_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            MessageBox.Show("Tags in this category will be downloaded into a separated folder, unless specified otherwise. If the user disallows blacklisted files from being saved, they will not be downloaded at all.", "aphrodite", MessageBoxButtons.OK);
        }

        private void llBlacklist_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            MessageBox.Show("Tags in this category will never be downloaded no matter what. They will still be parsed for counting, but never added to any download list.", "aphrodite", MessageBoxButtons.OK);
        }
    }
}
