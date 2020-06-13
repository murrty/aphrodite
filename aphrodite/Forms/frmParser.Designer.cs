namespace aphrodite {
    partial class frmParser {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabTags = new System.Windows.Forms.TabPage();
            this.txtTagsDescription = new System.Windows.Forms.TextBox();
            this.txtTagsTags = new System.Windows.Forms.TextBox();
            this.linkTags = new System.Windows.Forms.LinkLabel();
            this.label11 = new System.Windows.Forms.Label();
            this.lbTagRating = new System.Windows.Forms.Label();
            this.lbTagScore = new System.Windows.Forms.Label();
            this.lbTagTags = new System.Windows.Forms.Label();
            this.lbTagArtist = new System.Windows.Forms.Label();
            this.lbTagUrl = new System.Windows.Forms.Label();
            this.lbTagMd5 = new System.Windows.Forms.Label();
            this.lbTagPostNumber = new System.Windows.Forms.Label();
            this.lbTagDownloadOn = new System.Windows.Forms.Label();
            this.lbTagMinimumScore = new System.Windows.Forms.Label();
            this.lbTagSearch = new System.Windows.Forms.Label();
            this.listTags = new System.Windows.Forms.ListBox();
            this.tabPools = new System.Windows.Forms.TabPage();
            this.listPools = new System.Windows.Forms.ListBox();
            this.tabImages = new System.Windows.Forms.TabPage();
            this.rtxtImages = new System.Windows.Forms.RichTextBox();
            this.txtImageTags = new System.Windows.Forms.TextBox();
            this.linkImage = new System.Windows.Forms.LinkLabel();
            this.lbImageDescription = new System.Windows.Forms.Label();
            this.lbImageRating = new System.Windows.Forms.Label();
            this.lbImageScore = new System.Windows.Forms.Label();
            this.lbImageTags = new System.Windows.Forms.Label();
            this.lbImageArtists = new System.Windows.Forms.Label();
            this.lbImageURL = new System.Windows.Forms.Label();
            this.lbImageMd5 = new System.Windows.Forms.Label();
            this.lbImagePost = new System.Windows.Forms.Label();
            this.listImages = new System.Windows.Forms.ListBox();
            this.tabMain.SuspendLayout();
            this.tabTags.SuspendLayout();
            this.tabPools.SuspendLayout();
            this.tabImages.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tabTags);
            this.tabMain.Controls.Add(this.tabPools);
            this.tabMain.Controls.Add(this.tabImages);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Location = new System.Drawing.Point(0, 0);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(367, 271);
            this.tabMain.TabIndex = 10;
            // 
            // tabTags
            // 
            this.tabTags.Controls.Add(this.txtTagsDescription);
            this.tabTags.Controls.Add(this.txtTagsTags);
            this.tabTags.Controls.Add(this.linkTags);
            this.tabTags.Controls.Add(this.label11);
            this.tabTags.Controls.Add(this.lbTagRating);
            this.tabTags.Controls.Add(this.lbTagScore);
            this.tabTags.Controls.Add(this.lbTagTags);
            this.tabTags.Controls.Add(this.lbTagArtist);
            this.tabTags.Controls.Add(this.lbTagUrl);
            this.tabTags.Controls.Add(this.lbTagMd5);
            this.tabTags.Controls.Add(this.lbTagPostNumber);
            this.tabTags.Controls.Add(this.lbTagDownloadOn);
            this.tabTags.Controls.Add(this.lbTagMinimumScore);
            this.tabTags.Controls.Add(this.lbTagSearch);
            this.tabTags.Controls.Add(this.listTags);
            this.tabTags.Location = new System.Drawing.Point(4, 22);
            this.tabTags.Name = "tabTags";
            this.tabTags.Padding = new System.Windows.Forms.Padding(3);
            this.tabTags.Size = new System.Drawing.Size(359, 245);
            this.tabTags.TabIndex = 0;
            this.tabTags.Text = "Tags";
            this.tabTags.UseVisualStyleBackColor = true;
            // 
            // txtTagsDescription
            // 
            this.txtTagsDescription.Location = new System.Drawing.Point(213, 192);
            this.txtTagsDescription.Name = "txtTagsDescription";
            this.txtTagsDescription.ReadOnly = true;
            this.txtTagsDescription.Size = new System.Drawing.Size(138, 20);
            this.txtTagsDescription.TabIndex = 4;
            this.txtTagsDescription.Text = "n/a";
            // 
            // txtTagsTags
            // 
            this.txtTagsTags.Location = new System.Drawing.Point(176, 137);
            this.txtTagsTags.Name = "txtTagsTags";
            this.txtTagsTags.ReadOnly = true;
            this.txtTagsTags.Size = new System.Drawing.Size(177, 20);
            this.txtTagsTags.TabIndex = 3;
            this.txtTagsTags.Text = "n/a";
            // 
            // linkTags
            // 
            this.linkTags.AutoSize = true;
            this.linkTags.Location = new System.Drawing.Point(166, 105);
            this.linkTags.Name = "linkTags";
            this.linkTags.Size = new System.Drawing.Size(24, 13);
            this.linkTags.TabIndex = 2;
            this.linkTags.TabStop = true;
            this.linkTags.Text = "n/a";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(146, 195);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(61, 13);
            this.label11.TabIndex = 11;
            this.label11.Text = "description:";
            // 
            // lbTagRating
            // 
            this.lbTagRating.AutoSize = true;
            this.lbTagRating.Location = new System.Drawing.Point(146, 176);
            this.lbTagRating.Name = "lbTagRating";
            this.lbTagRating.Size = new System.Drawing.Size(56, 13);
            this.lbTagRating.TabIndex = 10;
            this.lbTagRating.Text = "rating: n/a";
            // 
            // lbTagScore
            // 
            this.lbTagScore.AutoSize = true;
            this.lbTagScore.Location = new System.Drawing.Point(146, 160);
            this.lbTagScore.Name = "lbTagScore";
            this.lbTagScore.Size = new System.Drawing.Size(56, 13);
            this.lbTagScore.TabIndex = 9;
            this.lbTagScore.Text = "score: n/a";
            // 
            // lbTagTags
            // 
            this.lbTagTags.AutoSize = true;
            this.lbTagTags.Location = new System.Drawing.Point(146, 140);
            this.lbTagTags.Name = "lbTagTags";
            this.lbTagTags.Size = new System.Drawing.Size(30, 13);
            this.lbTagTags.TabIndex = 8;
            this.lbTagTags.Text = "tags:";
            // 
            // lbTagArtist
            // 
            this.lbTagArtist.AutoSize = true;
            this.lbTagArtist.Location = new System.Drawing.Point(146, 121);
            this.lbTagArtist.Name = "lbTagArtist";
            this.lbTagArtist.Size = new System.Drawing.Size(63, 13);
            this.lbTagArtist.TabIndex = 7;
            this.lbTagArtist.Text = "artist(s): n/a";
            // 
            // lbTagUrl
            // 
            this.lbTagUrl.AutoSize = true;
            this.lbTagUrl.Location = new System.Drawing.Point(146, 105);
            this.lbTagUrl.Name = "lbTagUrl";
            this.lbTagUrl.Size = new System.Drawing.Size(21, 13);
            this.lbTagUrl.TabIndex = 6;
            this.lbTagUrl.Text = "url:";
            // 
            // lbTagMd5
            // 
            this.lbTagMd5.AutoSize = true;
            this.lbTagMd5.Location = new System.Drawing.Point(146, 89);
            this.lbTagMd5.Name = "lbTagMd5";
            this.lbTagMd5.Size = new System.Drawing.Size(50, 13);
            this.lbTagMd5.TabIndex = 5;
            this.lbTagMd5.Text = "md5: n/a";
            // 
            // lbTagPostNumber
            // 
            this.lbTagPostNumber.AutoSize = true;
            this.lbTagPostNumber.Location = new System.Drawing.Point(146, 66);
            this.lbTagPostNumber.Name = "lbTagPostNumber";
            this.lbTagPostNumber.Size = new System.Drawing.Size(37, 13);
            this.lbTagPostNumber.TabIndex = 4;
            this.lbTagPostNumber.Text = "post #";
            // 
            // lbTagDownloadOn
            // 
            this.lbTagDownloadOn.AutoSize = true;
            this.lbTagDownloadOn.Location = new System.Drawing.Point(146, 45);
            this.lbTagDownloadOn.Name = "lbTagDownloadOn";
            this.lbTagDownloadOn.Size = new System.Drawing.Size(103, 13);
            this.lbTagDownloadOn.TabIndex = 3;
            this.lbTagDownloadOn.Text = "downloaded on: n/a";
            // 
            // lbTagMinimumScore
            // 
            this.lbTagMinimumScore.AutoSize = true;
            this.lbTagMinimumScore.Location = new System.Drawing.Point(146, 28);
            this.lbTagMinimumScore.Name = "lbTagMinimumScore";
            this.lbTagMinimumScore.Size = new System.Drawing.Size(99, 13);
            this.lbTagMinimumScore.TabIndex = 2;
            this.lbTagMinimumScore.Text = "minimum score: n/a";
            // 
            // lbTagSearch
            // 
            this.lbTagSearch.AutoSize = true;
            this.lbTagSearch.Location = new System.Drawing.Point(146, 11);
            this.lbTagSearch.Name = "lbTagSearch";
            this.lbTagSearch.Size = new System.Drawing.Size(97, 13);
            this.lbTagSearch.TabIndex = 1;
            this.lbTagSearch.Text = "searched tags: n/a";
            // 
            // listTags
            // 
            this.listTags.FormattingEnabled = true;
            this.listTags.Location = new System.Drawing.Point(8, 10);
            this.listTags.Name = "listTags";
            this.listTags.Size = new System.Drawing.Size(132, 225);
            this.listTags.TabIndex = 1;
            // 
            // tabPools
            // 
            this.tabPools.Controls.Add(this.listPools);
            this.tabPools.Location = new System.Drawing.Point(4, 22);
            this.tabPools.Name = "tabPools";
            this.tabPools.Padding = new System.Windows.Forms.Padding(3);
            this.tabPools.Size = new System.Drawing.Size(359, 245);
            this.tabPools.TabIndex = 1;
            this.tabPools.Text = "Pools";
            this.tabPools.UseVisualStyleBackColor = true;
            // 
            // listPools
            // 
            this.listPools.FormattingEnabled = true;
            this.listPools.Location = new System.Drawing.Point(8, 10);
            this.listPools.Name = "listPools";
            this.listPools.Size = new System.Drawing.Size(132, 225);
            this.listPools.TabIndex = 1;
            // 
            // tabImages
            // 
            this.tabImages.Controls.Add(this.rtxtImages);
            this.tabImages.Controls.Add(this.txtImageTags);
            this.tabImages.Controls.Add(this.linkImage);
            this.tabImages.Controls.Add(this.lbImageDescription);
            this.tabImages.Controls.Add(this.lbImageRating);
            this.tabImages.Controls.Add(this.lbImageScore);
            this.tabImages.Controls.Add(this.lbImageTags);
            this.tabImages.Controls.Add(this.lbImageArtists);
            this.tabImages.Controls.Add(this.lbImageURL);
            this.tabImages.Controls.Add(this.lbImageMd5);
            this.tabImages.Controls.Add(this.lbImagePost);
            this.tabImages.Controls.Add(this.listImages);
            this.tabImages.Location = new System.Drawing.Point(4, 22);
            this.tabImages.Name = "tabImages";
            this.tabImages.Padding = new System.Windows.Forms.Padding(3);
            this.tabImages.Size = new System.Drawing.Size(359, 245);
            this.tabImages.TabIndex = 2;
            this.tabImages.Text = "Images";
            this.tabImages.UseVisualStyleBackColor = true;
            // 
            // rtxtImages
            // 
            this.rtxtImages.Location = new System.Drawing.Point(146, 160);
            this.rtxtImages.Name = "rtxtImages";
            this.rtxtImages.ReadOnly = true;
            this.rtxtImages.Size = new System.Drawing.Size(204, 76);
            this.rtxtImages.TabIndex = 4;
            this.rtxtImages.Text = "n/a";
            // 
            // txtImageTags
            // 
            this.txtImageTags.Location = new System.Drawing.Point(175, 87);
            this.txtImageTags.Name = "txtImageTags";
            this.txtImageTags.ReadOnly = true;
            this.txtImageTags.Size = new System.Drawing.Size(169, 20);
            this.txtImageTags.TabIndex = 3;
            this.txtImageTags.Text = "n/a";
            // 
            // linkImage
            // 
            this.linkImage.AutoSize = true;
            this.linkImage.Location = new System.Drawing.Point(162, 54);
            this.linkImage.Name = "linkImage";
            this.linkImage.Size = new System.Drawing.Size(24, 13);
            this.linkImage.TabIndex = 2;
            this.linkImage.TabStop = true;
            this.linkImage.Text = "n/a";
            // 
            // lbImageDescription
            // 
            this.lbImageDescription.AutoSize = true;
            this.lbImageDescription.Location = new System.Drawing.Point(143, 143);
            this.lbImageDescription.Name = "lbImageDescription";
            this.lbImageDescription.Size = new System.Drawing.Size(61, 13);
            this.lbImageDescription.TabIndex = 10;
            this.lbImageDescription.Text = "description:";
            // 
            // lbImageRating
            // 
            this.lbImageRating.AutoSize = true;
            this.lbImageRating.Location = new System.Drawing.Point(143, 126);
            this.lbImageRating.Name = "lbImageRating";
            this.lbImageRating.Size = new System.Drawing.Size(56, 13);
            this.lbImageRating.TabIndex = 9;
            this.lbImageRating.Text = "rating: n/a";
            // 
            // lbImageScore
            // 
            this.lbImageScore.AutoSize = true;
            this.lbImageScore.Location = new System.Drawing.Point(143, 109);
            this.lbImageScore.Name = "lbImageScore";
            this.lbImageScore.Size = new System.Drawing.Size(56, 13);
            this.lbImageScore.TabIndex = 8;
            this.lbImageScore.Text = "score: n/a";
            // 
            // lbImageTags
            // 
            this.lbImageTags.AutoSize = true;
            this.lbImageTags.Location = new System.Drawing.Point(143, 90);
            this.lbImageTags.Name = "lbImageTags";
            this.lbImageTags.Size = new System.Drawing.Size(30, 13);
            this.lbImageTags.TabIndex = 7;
            this.lbImageTags.Text = "tags:";
            // 
            // lbImageArtists
            // 
            this.lbImageArtists.AutoSize = true;
            this.lbImageArtists.Location = new System.Drawing.Point(143, 71);
            this.lbImageArtists.Name = "lbImageArtists";
            this.lbImageArtists.Size = new System.Drawing.Size(63, 13);
            this.lbImageArtists.TabIndex = 6;
            this.lbImageArtists.Text = "artist(s): n/a";
            // 
            // lbImageURL
            // 
            this.lbImageURL.AutoSize = true;
            this.lbImageURL.Location = new System.Drawing.Point(143, 54);
            this.lbImageURL.Name = "lbImageURL";
            this.lbImageURL.Size = new System.Drawing.Size(21, 13);
            this.lbImageURL.TabIndex = 5;
            this.lbImageURL.Text = "url:";
            // 
            // lbImageMd5
            // 
            this.lbImageMd5.AutoSize = true;
            this.lbImageMd5.Location = new System.Drawing.Point(143, 37);
            this.lbImageMd5.Name = "lbImageMd5";
            this.lbImageMd5.Size = new System.Drawing.Size(50, 13);
            this.lbImageMd5.TabIndex = 4;
            this.lbImageMd5.Text = "md5: n/a";
            // 
            // lbImagePost
            // 
            this.lbImagePost.AutoSize = true;
            this.lbImagePost.Location = new System.Drawing.Point(143, 10);
            this.lbImagePost.Name = "lbImagePost";
            this.lbImagePost.Size = new System.Drawing.Size(47, 13);
            this.lbImagePost.TabIndex = 3;
            this.lbImagePost.Text = "post n/a";
            // 
            // listImages
            // 
            this.listImages.FormattingEnabled = true;
            this.listImages.Location = new System.Drawing.Point(8, 10);
            this.listImages.Name = "listImages";
            this.listImages.Size = new System.Drawing.Size(132, 225);
            this.listImages.TabIndex = 1;
            this.listImages.SelectedIndexChanged += new System.EventHandler(this.listImages_SelectedIndexChanged);
            // 
            // frmParser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(367, 271);
            this.Controls.Add(this.tabMain);
            this.MaximizeBox = false;
            this.Name = "frmParser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "nfo parser";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmParser_FormClosing);
            this.Load += new System.EventHandler(this.frmParser_Load);
            this.tabMain.ResumeLayout(false);
            this.tabTags.ResumeLayout(false);
            this.tabTags.PerformLayout();
            this.tabPools.ResumeLayout(false);
            this.tabImages.ResumeLayout(false);
            this.tabImages.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabTags;
        private System.Windows.Forms.TabPage tabPools;
        private System.Windows.Forms.TabPage tabImages;
        private System.Windows.Forms.Label lbTagDownloadOn;
        private System.Windows.Forms.Label lbTagMinimumScore;
        private System.Windows.Forms.Label lbTagSearch;
        private System.Windows.Forms.ListBox listTags;
        private System.Windows.Forms.TextBox txtTagsDescription;
        private System.Windows.Forms.TextBox txtTagsTags;
        private System.Windows.Forms.LinkLabel linkTags;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lbTagRating;
        private System.Windows.Forms.Label lbTagScore;
        private System.Windows.Forms.Label lbTagTags;
        private System.Windows.Forms.Label lbTagArtist;
        private System.Windows.Forms.Label lbTagUrl;
        private System.Windows.Forms.Label lbTagMd5;
        private System.Windows.Forms.Label lbTagPostNumber;
        private System.Windows.Forms.ListBox listPools;
        private System.Windows.Forms.ListBox listImages;
        private System.Windows.Forms.LinkLabel linkImage;
        private System.Windows.Forms.Label lbImageDescription;
        private System.Windows.Forms.Label lbImageRating;
        private System.Windows.Forms.Label lbImageScore;
        private System.Windows.Forms.Label lbImageTags;
        private System.Windows.Forms.Label lbImageArtists;
        private System.Windows.Forms.Label lbImageURL;
        private System.Windows.Forms.Label lbImageMd5;
        private System.Windows.Forms.Label lbImagePost;
        private System.Windows.Forms.TextBox txtImageTags;
        private System.Windows.Forms.RichTextBox rtxtImages;
    }
}