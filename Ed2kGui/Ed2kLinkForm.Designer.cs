namespace Ed2kGui
{
    partial class Ed2kLinkForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Results = new System.Windows.Forms.TextBox();
            this._worker = new System.ComponentModel.BackgroundWorker();
            this._progress = new System.Windows.Forms.ProgressBar();
            this._resultAppender = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // Results
            // 
            this.Results.Location = new System.Drawing.Point(12, 151);
            this.Results.Multiline = true;
            this.Results.Name = "Results";
            this.Results.ReadOnly = true;
            this.Results.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.Results.Size = new System.Drawing.Size(776, 268);
            this.Results.TabIndex = 0;
            this.Results.WordWrap = false;
            // 
            // _worker
            // 
            this._worker.WorkerReportsProgress = true;
            // 
            // _progress
            // 
            this._progress.Location = new System.Drawing.Point(12, 122);
            this._progress.Name = "_progress";
            this._progress.Size = new System.Drawing.Size(776, 23);
            this._progress.TabIndex = 1;
            // 
            // Ed2kLinkForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this._progress);
            this.Controls.Add(this.Results);
            this.Name = "Ed2kLinkForm";
            this.Text = "ed2k File Link Generator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox Results;
        private System.ComponentModel.BackgroundWorker _worker;
        private ProgressBar _progress;
        private System.ComponentModel.BackgroundWorker _resultAppender;
    }
}