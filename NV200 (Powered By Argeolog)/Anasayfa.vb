Imports System.Threading


Public Class Anasayfa

    Dim NV As New NV200_SDK
    Private Sub Anasayfa_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        NV.InvokeSync = SynchronizationContext.Current
        NV.LogTextBox = TextBox1
        NV.Baglanti_Kur()
        Kanal_Bilgilerini_Cek()


    End Sub

    Private Sub Seri_No_Buton_Click(sender As Object, e As EventArgs) Handles Seri_No_Buton.Click
        MsgBox(NV.Seri_No_Oku)

    End Sub

    Private Sub Validator_Ac_Buton_Click(sender As Object, e As EventArgs) Handles Validator_Ac_Buton.Click

        NV.Validator_Ac()
        NV.Para_Alma_Ac()
    End Sub

    Sub Kanallari_Ayarla()
        Dim Bytes(6) As Byte
        Bytes(0) = CByte(Kanal_1_Check.Checked)
        Bytes(1) = CByte(Kanal_2_Check.Checked)
        Bytes(2) = CByte(Kanal_3_Check.Checked)
        Bytes(3) = CByte(Kanal_4_Check.Checked)
        Bytes(4) = CByte(Kanal_5_Check.Checked)
        Bytes(5) = CByte(Kanal_6_Check.Checked)

        NV.Kanallari_Ayarla(Bytes)
    End Sub


    Private Sub Kanal_Ayarla_Buton_Click(sender As Object, e As EventArgs) Handles Kanal_Ayarla_Buton.Click
        Kanallari_Ayarla()
    End Sub

    Private Sub Validator_Kapat_Buton_Click(sender As Object, e As EventArgs) Handles Validator_Kapat_Buton.Click
        NV.Validator_Kapat()
        NV.Para_Alma_Kapat()
    End Sub

    Private Sub Para_Alma_Ac_Buton_Click(sender As Object, e As EventArgs) Handles Para_Alma_Ac_Buton.Click
        NV.Para_Alma_Ac()
    End Sub

    Private Sub Para_Alma_Kapat_Buton_Click(sender As Object, e As EventArgs) Handles Para_Alma_Kapat_Buton.Click
        NV.Para_Alma_Kapat()
    End Sub

    Private Sub Para_Ver_Buton_Click(sender As Object, e As EventArgs) Handles Para_Ver_Buton.Click

        NV.Para_Ver(CInt(Verilecek_Para_Text.Value), Para_Birimi_Text.Text)
    End Sub


    Private Sub Limit_Ayarla_Buton_Click(sender As Object, e As EventArgs) Handles Limit_Ayarla_Buton.Click
        NV.Limit_Belirle(CInt(Banknot_Text.Value), CByte(Limit_Text.Value), Limit_Para_Birimi_Text.Text)
    End Sub

    Private Sub Akilli_Bosalt_Buton_Click(sender As Object, e As EventArgs) Handles Akilli_Bosalt_Buton.Click
        NV.Kasa_Bosalt_Akilli()
    End Sub

    Private Sub Kasayi_Komple_Bosalt_Buton_Click(sender As Object, e As EventArgs) Handles Kasayi_Komple_Bosalt_Buton.Click
        NV.Kasa_Bosalt_Tamami()
    End Sub

    Private Async Sub Yeniden_Baslat_Buton_Click(sender As Object, e As EventArgs) Handles Yeniden_Baslat_Buton.Click

        Await NV.Yeniden_Baslat()


        NV.Baglanti_Kur() 'Yeniden Başladığında Cihaz Bağlantısı Kopar.
        NV.Cihazi_Yapilandir()
        Kanallari_Ayarla() 'Cihaz Resetlenmiş ise Kanalların Tekrar Ayarlanması Gerekir.

    End Sub

    Private Sub Kanal_1_Check_Click(sender As Object, e As EventArgs) Handles Kanal_1_Check.Click
        Kanallari_Ayarla()
    End Sub

    Private Sub Kanal_2_Check_Click(sender As Object, e As EventArgs) Handles Kanal_2_Check.Click
        Kanallari_Ayarla()
    End Sub

    Private Sub Kanal_3_Check_Click(sender As Object, e As EventArgs) Handles Kanal_3_Check.Click
        Kanallari_Ayarla()
    End Sub

    Private Sub Kanal_4_Check_Click(sender As Object, e As EventArgs) Handles Kanal_4_Check.Click
        Kanallari_Ayarla()
    End Sub

    Private Sub Kanal_5_Check_Click(sender As Object, e As EventArgs) Handles Kanal_5_Check.Click
        Kanallari_Ayarla()
    End Sub

    Private Sub Kanal_6_Check_Click(sender As Object, e As EventArgs) Handles Kanal_6_Check.Click
        Kanallari_Ayarla()
    End Sub

    Private Sub Kanal_Bilgilerini_Al_Buton_Click(sender As Object, e As EventArgs) Handles Kanal_Bilgilerini_Al_Buton.Click



    End Sub

    Sub Kanal_Bilgilerini_Cek()

        Kanal_Durum_GrupBox.Controls.Clear()

        For i As Integer = 0 To NV.KanalListesi.Count - 1
            Dim Banknot As Integer = (NV.KanalListesi(i).Banknot \ NV.BanknotCarpanDeger)
            Dim ParaBirimi As String = New String(NV.KanalListesi(i).ParaBirimi).Trim(Chr(0))

            Dim KanalCheck As New CheckBox With {
                .Text = "CH:" & NV.KanalListesi(i).KanalNo & " - " & Banknot & " [" & (NV.KanalListesi(i).BanknotAdet) & "] " & ParaBirimi,
                .Checked = NV.KanalListesi(i).KasayaIndir,
                .Location = New Point(10, 25 + i * 25),
                .Tag = ParaBirimi,
                .TabIndex = Banknot,
                .AutoSize = True
            }

            AddHandler KanalCheck.Click, AddressOf Kasaya_Indir
            Kanal_Durum_GrupBox.Controls.Add(KanalCheck)
        Next
    End Sub

    Sub Kasaya_Indir(Sender As Object, e As EventArgs)

        Dim Check As CheckBox = DirectCast(Sender, CheckBox)
        If Check IsNot Nothing AndAlso Check.Tag IsNot Nothing Then
            NV.Banknot_Yonlendir(Check.TabIndex, Check.Tag.ToString, Check.Checked)
        End If
    End Sub

    Private Sub Kanal_Durum_GrupBox_Enter(sender As Object, e As EventArgs) Handles Kanal_Durum_GrupBox.Enter

    End Sub
End Class
