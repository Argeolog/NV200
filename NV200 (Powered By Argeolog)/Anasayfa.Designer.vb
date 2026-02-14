<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Anasayfa
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Seri_No_Buton = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Verilecek_Para_Text = New System.Windows.Forms.NumericUpDown()
        Me.Para_Ver_Buton = New System.Windows.Forms.Button()
        Me.Para_Birimi_Text = New System.Windows.Forms.TextBox()
        Me.Validator_Ac_Buton = New System.Windows.Forms.Button()
        Me.Validator_Kapat_Buton = New System.Windows.Forms.Button()
        Me.Kanal_6_Check = New System.Windows.Forms.CheckBox()
        Me.Kanal_5_Check = New System.Windows.Forms.CheckBox()
        Me.Kanal_4_Check = New System.Windows.Forms.CheckBox()
        Me.Kanal_3_Check = New System.Windows.Forms.CheckBox()
        Me.Kanal_2_Check = New System.Windows.Forms.CheckBox()
        Me.Kanal_1_Check = New System.Windows.Forms.CheckBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Kanal_Ayarla_Buton = New System.Windows.Forms.Button()
        Me.Para_Alma_Ac_Buton = New System.Windows.Forms.Button()
        Me.Para_Alma_Kapat_Buton = New System.Windows.Forms.Button()
        Me.Akilli_Bosalt_Buton = New System.Windows.Forms.Button()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Limit_Para_Birimi_Text = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Limit_Text = New System.Windows.Forms.NumericUpDown()
        Me.Banknot_Text = New System.Windows.Forms.NumericUpDown()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Limit_Ayarla_Buton = New System.Windows.Forms.Button()
        Me.Kasayi_Komple_Bosalt_Buton = New System.Windows.Forms.Button()
        Me.Yeniden_Baslat_Buton = New System.Windows.Forms.Button()
        Me.Kanal_Durum_GrupBox = New System.Windows.Forms.GroupBox()
        Me.Kanal_Bilgilerini_Al_Buton = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        CType(Me.Verilecek_Para_Text, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        CType(Me.Limit_Text, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Banknot_Text, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(12, 12)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(286, 543)
        Me.TextBox1.TabIndex = 0
        '
        'Seri_No_Buton
        '
        Me.Seri_No_Buton.Location = New System.Drawing.Point(304, 86)
        Me.Seri_No_Buton.Name = "Seri_No_Buton"
        Me.Seri_No_Buton.Size = New System.Drawing.Size(108, 54)
        Me.Seri_No_Buton.TabIndex = 1
        Me.Seri_No_Buton.Text = "Seri No Oku"
        Me.Seri_No_Buton.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Verilecek_Para_Text)
        Me.GroupBox1.Controls.Add(Me.Para_Ver_Buton)
        Me.GroupBox1.Controls.Add(Me.Para_Birimi_Text)
        Me.GroupBox1.Location = New System.Drawing.Point(466, 266)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(207, 100)
        Me.GroupBox1.TabIndex = 96
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Para Üstü Verme İşlemleri"
        '
        'Verilecek_Para_Text
        '
        Me.Verilecek_Para_Text.Location = New System.Drawing.Point(10, 22)
        Me.Verilecek_Para_Text.Name = "Verilecek_Para_Text"
        Me.Verilecek_Para_Text.Size = New System.Drawing.Size(82, 20)
        Me.Verilecek_Para_Text.TabIndex = 97
        Me.Verilecek_Para_Text.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.Verilecek_Para_Text.Value = New Decimal(New Integer() {50, 0, 0, 0})
        '
        'Para_Ver_Buton
        '
        Me.Para_Ver_Buton.Location = New System.Drawing.Point(10, 47)
        Me.Para_Ver_Buton.Name = "Para_Ver_Buton"
        Me.Para_Ver_Buton.Size = New System.Drawing.Size(179, 41)
        Me.Para_Ver_Buton.TabIndex = 92
        Me.Para_Ver_Buton.Text = "Para Ver"
        Me.Para_Ver_Buton.UseVisualStyleBackColor = True
        '
        'Para_Birimi_Text
        '
        Me.Para_Birimi_Text.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.Para_Birimi_Text.Location = New System.Drawing.Point(98, 21)
        Me.Para_Birimi_Text.Name = "Para_Birimi_Text"
        Me.Para_Birimi_Text.Size = New System.Drawing.Size(91, 20)
        Me.Para_Birimi_Text.TabIndex = 93
        Me.Para_Birimi_Text.Text = "TRY"
        Me.Para_Birimi_Text.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Validator_Ac_Buton
        '
        Me.Validator_Ac_Buton.Location = New System.Drawing.Point(304, 146)
        Me.Validator_Ac_Buton.Name = "Validator_Ac_Buton"
        Me.Validator_Ac_Buton.Size = New System.Drawing.Size(108, 54)
        Me.Validator_Ac_Buton.TabIndex = 97
        Me.Validator_Ac_Buton.Text = "Validatör Aç"
        Me.Validator_Ac_Buton.UseVisualStyleBackColor = True
        '
        'Validator_Kapat_Buton
        '
        Me.Validator_Kapat_Buton.Location = New System.Drawing.Point(425, 146)
        Me.Validator_Kapat_Buton.Name = "Validator_Kapat_Buton"
        Me.Validator_Kapat_Buton.Size = New System.Drawing.Size(108, 54)
        Me.Validator_Kapat_Buton.TabIndex = 98
        Me.Validator_Kapat_Buton.Text = "Validatör Kapat"
        Me.Validator_Kapat_Buton.UseVisualStyleBackColor = True
        '
        'Kanal_6_Check
        '
        Me.Kanal_6_Check.AutoSize = True
        Me.Kanal_6_Check.Checked = True
        Me.Kanal_6_Check.CheckState = System.Windows.Forms.CheckState.Checked
        Me.Kanal_6_Check.Location = New System.Drawing.Point(142, 42)
        Me.Kanal_6_Check.Name = "Kanal_6_Check"
        Me.Kanal_6_Check.Size = New System.Drawing.Size(62, 17)
        Me.Kanal_6_Check.TabIndex = 104
        Me.Kanal_6_Check.Text = "Kanal 6"
        Me.Kanal_6_Check.UseVisualStyleBackColor = True
        '
        'Kanal_5_Check
        '
        Me.Kanal_5_Check.AutoSize = True
        Me.Kanal_5_Check.Checked = True
        Me.Kanal_5_Check.CheckState = System.Windows.Forms.CheckState.Checked
        Me.Kanal_5_Check.Location = New System.Drawing.Point(142, 19)
        Me.Kanal_5_Check.Name = "Kanal_5_Check"
        Me.Kanal_5_Check.Size = New System.Drawing.Size(62, 17)
        Me.Kanal_5_Check.TabIndex = 103
        Me.Kanal_5_Check.Text = "Kanal 5"
        Me.Kanal_5_Check.UseVisualStyleBackColor = True
        '
        'Kanal_4_Check
        '
        Me.Kanal_4_Check.AutoSize = True
        Me.Kanal_4_Check.Checked = True
        Me.Kanal_4_Check.CheckState = System.Windows.Forms.CheckState.Checked
        Me.Kanal_4_Check.Location = New System.Drawing.Point(74, 42)
        Me.Kanal_4_Check.Name = "Kanal_4_Check"
        Me.Kanal_4_Check.Size = New System.Drawing.Size(62, 17)
        Me.Kanal_4_Check.TabIndex = 102
        Me.Kanal_4_Check.Text = "Kanal 4"
        Me.Kanal_4_Check.UseVisualStyleBackColor = True
        '
        'Kanal_3_Check
        '
        Me.Kanal_3_Check.AutoSize = True
        Me.Kanal_3_Check.Checked = True
        Me.Kanal_3_Check.CheckState = System.Windows.Forms.CheckState.Checked
        Me.Kanal_3_Check.Location = New System.Drawing.Point(74, 19)
        Me.Kanal_3_Check.Name = "Kanal_3_Check"
        Me.Kanal_3_Check.Size = New System.Drawing.Size(62, 17)
        Me.Kanal_3_Check.TabIndex = 101
        Me.Kanal_3_Check.Text = "Kanal 3"
        Me.Kanal_3_Check.UseVisualStyleBackColor = True
        '
        'Kanal_2_Check
        '
        Me.Kanal_2_Check.AutoSize = True
        Me.Kanal_2_Check.Checked = True
        Me.Kanal_2_Check.CheckState = System.Windows.Forms.CheckState.Checked
        Me.Kanal_2_Check.Location = New System.Drawing.Point(6, 42)
        Me.Kanal_2_Check.Name = "Kanal_2_Check"
        Me.Kanal_2_Check.Size = New System.Drawing.Size(62, 17)
        Me.Kanal_2_Check.TabIndex = 100
        Me.Kanal_2_Check.Text = "Kanal 2"
        Me.Kanal_2_Check.UseVisualStyleBackColor = True
        '
        'Kanal_1_Check
        '
        Me.Kanal_1_Check.AutoSize = True
        Me.Kanal_1_Check.Checked = True
        Me.Kanal_1_Check.CheckState = System.Windows.Forms.CheckState.Checked
        Me.Kanal_1_Check.Location = New System.Drawing.Point(6, 19)
        Me.Kanal_1_Check.Name = "Kanal_1_Check"
        Me.Kanal_1_Check.Size = New System.Drawing.Size(62, 17)
        Me.Kanal_1_Check.TabIndex = 99
        Me.Kanal_1_Check.Text = "Kanal 1"
        Me.Kanal_1_Check.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Kanal_1_Check)
        Me.GroupBox2.Controls.Add(Me.Kanal_2_Check)
        Me.GroupBox2.Controls.Add(Me.Kanal_6_Check)
        Me.GroupBox2.Controls.Add(Me.Kanal_3_Check)
        Me.GroupBox2.Controls.Add(Me.Kanal_5_Check)
        Me.GroupBox2.Controls.Add(Me.Kanal_4_Check)
        Me.GroupBox2.Location = New System.Drawing.Point(304, 12)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(222, 68)
        Me.GroupBox2.TabIndex = 106
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Aktif Kanallar"
        '
        'Kanal_Ayarla_Buton
        '
        Me.Kanal_Ayarla_Buton.Location = New System.Drawing.Point(425, 86)
        Me.Kanal_Ayarla_Buton.Name = "Kanal_Ayarla_Buton"
        Me.Kanal_Ayarla_Buton.Size = New System.Drawing.Size(108, 54)
        Me.Kanal_Ayarla_Buton.TabIndex = 107
        Me.Kanal_Ayarla_Buton.Text = "Kanalları Ayarla"
        Me.Kanal_Ayarla_Buton.UseVisualStyleBackColor = True
        '
        'Para_Alma_Ac_Buton
        '
        Me.Para_Alma_Ac_Buton.Location = New System.Drawing.Point(304, 206)
        Me.Para_Alma_Ac_Buton.Name = "Para_Alma_Ac_Buton"
        Me.Para_Alma_Ac_Buton.Size = New System.Drawing.Size(108, 54)
        Me.Para_Alma_Ac_Buton.TabIndex = 108
        Me.Para_Alma_Ac_Buton.Text = "Para Alma Aç"
        Me.Para_Alma_Ac_Buton.UseVisualStyleBackColor = True
        '
        'Para_Alma_Kapat_Buton
        '
        Me.Para_Alma_Kapat_Buton.Location = New System.Drawing.Point(425, 206)
        Me.Para_Alma_Kapat_Buton.Name = "Para_Alma_Kapat_Buton"
        Me.Para_Alma_Kapat_Buton.Size = New System.Drawing.Size(108, 54)
        Me.Para_Alma_Kapat_Buton.TabIndex = 109
        Me.Para_Alma_Kapat_Buton.Text = "Para Alma Kapat"
        Me.Para_Alma_Kapat_Buton.UseVisualStyleBackColor = True
        '
        'Akilli_Bosalt_Buton
        '
        Me.Akilli_Bosalt_Buton.Location = New System.Drawing.Point(547, 86)
        Me.Akilli_Bosalt_Buton.Name = "Akilli_Bosalt_Buton"
        Me.Akilli_Bosalt_Buton.Size = New System.Drawing.Size(108, 54)
        Me.Akilli_Bosalt_Buton.TabIndex = 110
        Me.Akilli_Bosalt_Buton.Text = "Kasette Para Bırakarak Boşalt"
        Me.Akilli_Bosalt_Buton.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.Label1)
        Me.GroupBox3.Controls.Add(Me.Limit_Para_Birimi_Text)
        Me.GroupBox3.Controls.Add(Me.Label6)
        Me.GroupBox3.Controls.Add(Me.Limit_Text)
        Me.GroupBox3.Controls.Add(Me.Banknot_Text)
        Me.GroupBox3.Controls.Add(Me.Label5)
        Me.GroupBox3.Controls.Add(Me.Label4)
        Me.GroupBox3.Controls.Add(Me.Limit_Ayarla_Buton)
        Me.GroupBox3.Location = New System.Drawing.Point(466, 372)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(207, 183)
        Me.GroupBox3.TabIndex = 111
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Kaset Limit Belirle"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 81)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 13)
        Me.Label1.TabIndex = 113
        Me.Label1.Text = "Para Birimi"
        '
        'Limit_Para_Birimi_Text
        '
        Me.Limit_Para_Birimi_Text.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.Limit_Para_Birimi_Text.Location = New System.Drawing.Point(74, 78)
        Me.Limit_Para_Birimi_Text.Name = "Limit_Para_Birimi_Text"
        Me.Limit_Para_Birimi_Text.Size = New System.Drawing.Size(115, 20)
        Me.Limit_Para_Birimi_Text.TabIndex = 98
        Me.Limit_Para_Birimi_Text.Text = "TRY"
        Me.Limit_Para_Birimi_Text.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(10, 148)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(191, 13)
        Me.Label6.TabIndex = 97
        Me.Label6.Text = "Hangi Kaset Kaç Adet Banknot Alacak"
        '
        'Limit_Text
        '
        Me.Limit_Text.Location = New System.Drawing.Point(74, 51)
        Me.Limit_Text.Name = "Limit_Text"
        Me.Limit_Text.Size = New System.Drawing.Size(115, 20)
        Me.Limit_Text.TabIndex = 97
        Me.Limit_Text.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'Banknot_Text
        '
        Me.Banknot_Text.Location = New System.Drawing.Point(74, 25)
        Me.Banknot_Text.Name = "Banknot_Text"
        Me.Banknot_Text.Size = New System.Drawing.Size(115, 20)
        Me.Banknot_Text.TabIndex = 96
        Me.Banknot_Text.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(10, 53)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(28, 13)
        Me.Label5.TabIndex = 95
        Me.Label5.Text = "Limit"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(10, 27)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(47, 13)
        Me.Label4.TabIndex = 94
        Me.Label4.Text = "Banknot"
        '
        'Limit_Ayarla_Buton
        '
        Me.Limit_Ayarla_Buton.Location = New System.Drawing.Point(15, 104)
        Me.Limit_Ayarla_Buton.Name = "Limit_Ayarla_Buton"
        Me.Limit_Ayarla_Buton.Size = New System.Drawing.Size(176, 41)
        Me.Limit_Ayarla_Buton.TabIndex = 92
        Me.Limit_Ayarla_Buton.Text = "Ayarla"
        Me.Limit_Ayarla_Buton.UseVisualStyleBackColor = True
        '
        'Kasayi_Komple_Bosalt_Buton
        '
        Me.Kasayi_Komple_Bosalt_Buton.Location = New System.Drawing.Point(547, 146)
        Me.Kasayi_Komple_Bosalt_Buton.Name = "Kasayi_Komple_Bosalt_Buton"
        Me.Kasayi_Komple_Bosalt_Buton.Size = New System.Drawing.Size(108, 54)
        Me.Kasayi_Komple_Bosalt_Buton.TabIndex = 112
        Me.Kasayi_Komple_Bosalt_Buton.Text = "Kasetteki Paraları Kasaya İndir"
        Me.Kasayi_Komple_Bosalt_Buton.UseVisualStyleBackColor = True
        '
        'Yeniden_Baslat_Buton
        '
        Me.Yeniden_Baslat_Buton.Location = New System.Drawing.Point(547, 206)
        Me.Yeniden_Baslat_Buton.Name = "Yeniden_Baslat_Buton"
        Me.Yeniden_Baslat_Buton.Size = New System.Drawing.Size(108, 54)
        Me.Yeniden_Baslat_Buton.TabIndex = 113
        Me.Yeniden_Baslat_Buton.Text = "Cihazı Yeniden Başlat"
        Me.Yeniden_Baslat_Buton.UseVisualStyleBackColor = True
        '
        'Kanal_Durum_GrupBox
        '
        Me.Kanal_Durum_GrupBox.Location = New System.Drawing.Point(304, 266)
        Me.Kanal_Durum_GrupBox.Name = "Kanal_Durum_GrupBox"
        Me.Kanal_Durum_GrupBox.Size = New System.Drawing.Size(156, 227)
        Me.Kanal_Durum_GrupBox.TabIndex = 114
        Me.Kanal_Durum_GrupBox.TabStop = False
        Me.Kanal_Durum_GrupBox.Text = "Kanal Durumları (True:Kasa)"
        '
        'Kanal_Bilgilerini_Al_Buton
        '
        Me.Kanal_Bilgilerini_Al_Buton.Location = New System.Drawing.Point(304, 499)
        Me.Kanal_Bilgilerini_Al_Buton.Name = "Kanal_Bilgilerini_Al_Buton"
        Me.Kanal_Bilgilerini_Al_Buton.Size = New System.Drawing.Size(156, 54)
        Me.Kanal_Bilgilerini_Al_Buton.TabIndex = 115
        Me.Kanal_Bilgilerini_Al_Buton.Text = "Kanal Bilgilerini Al"
        Me.Kanal_Bilgilerini_Al_Buton.UseVisualStyleBackColor = True
        '
        'Anasayfa
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(936, 565)
        Me.Controls.Add(Me.Kanal_Bilgilerini_Al_Buton)
        Me.Controls.Add(Me.Kanal_Durum_GrupBox)
        Me.Controls.Add(Me.Yeniden_Baslat_Buton)
        Me.Controls.Add(Me.Kasayi_Komple_Bosalt_Buton)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.Akilli_Bosalt_Buton)
        Me.Controls.Add(Me.Para_Alma_Kapat_Buton)
        Me.Controls.Add(Me.Para_Alma_Ac_Buton)
        Me.Controls.Add(Me.Kanal_Ayarla_Buton)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.Validator_Kapat_Buton)
        Me.Controls.Add(Me.Validator_Ac_Buton)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Seri_No_Buton)
        Me.Controls.Add(Me.TextBox1)
        Me.Name = "Anasayfa"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "NV200 SDK (Argeolog)"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.Verilecek_Para_Text, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        CType(Me.Limit_Text, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Banknot_Text, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Seri_No_Buton As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Verilecek_Para_Text As NumericUpDown
    Private WithEvents Para_Ver_Buton As Button
    Friend WithEvents Para_Birimi_Text As TextBox
    Friend WithEvents Validator_Ac_Buton As Button
    Friend WithEvents Validator_Kapat_Buton As Button
    Friend WithEvents Kanal_6_Check As CheckBox
    Friend WithEvents Kanal_5_Check As CheckBox
    Friend WithEvents Kanal_4_Check As CheckBox
    Friend WithEvents Kanal_3_Check As CheckBox
    Friend WithEvents Kanal_2_Check As CheckBox
    Friend WithEvents Kanal_1_Check As CheckBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents Kanal_Ayarla_Buton As Button
    Friend WithEvents Para_Alma_Ac_Buton As Button
    Friend WithEvents Para_Alma_Kapat_Buton As Button
    Private WithEvents Akilli_Bosalt_Buton As Button
    Friend WithEvents GroupBox3 As GroupBox
    Private WithEvents Label6 As Label
    Friend WithEvents Limit_Text As NumericUpDown
    Friend WithEvents Banknot_Text As NumericUpDown
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Private WithEvents Limit_Ayarla_Buton As Button
    Private WithEvents Kasayi_Komple_Bosalt_Buton As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Limit_Para_Birimi_Text As TextBox
    Private WithEvents Yeniden_Baslat_Buton As Button
    Friend WithEvents Kanal_Durum_GrupBox As GroupBox
    Friend WithEvents Kanal_Bilgilerini_Al_Buton As Button
End Class
