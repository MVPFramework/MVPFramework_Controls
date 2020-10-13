namespace MVPFramework_Controls
{
    partial class TestControlsWindow
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.textButton1 = new MVPFramework.Control.TextButton();
            this.SuspendLayout();
            // 
            // textButton1
            // 
            this.textButton1.CustomFontName = null;
            this.textButton1.CustomFontSize = 0F;
            this.textButton1.DisableImage = global::MVPControls.Properties.Resources.blueBtn_dis;
            this.textButton1.DownImage = global::MVPControls.Properties.Resources.blueBtn_dwn;
            this.textButton1.Enable = true;
            this.textButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.textButton1.Location = new System.Drawing.Point(253, 148);
            this.textButton1.Name = "textButton1";
            this.textButton1.NormalImage = global::MVPControls.Properties.Resources.blueBtn_nml;
            this.textButton1.OverrideImage = global::MVPControls.Properties.Resources.blueBtn_ovr;
            this.textButton1.Size = new System.Drawing.Size(178, 51);
            this.textButton1.Status = MVPFramework.Control.ButtonStatus.Normal;
            this.textButton1.TabIndex = 0;
            this.textButton1.Text = "textButton1";
            this.textButton1.UseVisualStyleBackColor = true;
            // 
            // TestControlsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.textButton1);
            this.Name = "TestControlsWindow";
            this.Text = "TestControlsWindow";
            this.ResumeLayout(false);

        }

        #endregion

        private MVPFramework.Control.TextButton textButton1;
    }
}

