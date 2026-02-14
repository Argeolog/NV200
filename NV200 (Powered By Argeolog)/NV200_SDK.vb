

Imports System.IO
Imports System.IO.Ports
Imports System.Runtime.InteropServices
Imports System.Security.Cryptography
Imports System.Text
Imports System.Threading
Imports Microsoft.VisualBasic.Logging

Public Class NV200_SDK
    Public InvokeSync As New SynchronizationContext
    Private WithEvents CihazSorguTimer As New System.Timers.Timer(250)
    Private TimerCalisiyor As Boolean

    Public LogTextBox As TextBox
    Private SerialComPort As SerialPort

    ReadOnly SSPCommand As New SSP_COMMAND
    ReadOnly ssp As New SSP_TX_RX_PACKET
    ReadOnly RngCsp As New RNGCryptoServiceProvider
    ReadOnly SspInfo As New SSP_COMMAND_INFO

    ReadOnly SegBit As Byte = 128
    ReadOnly PaketSayisi As UInteger = 0
    Public KanalListesi As New List(Of KanalData)
    Public BanknotCarpanDeger As Integer = 1

    Dim m_ProtocolVersion As Integer = 0

    ' --- SSP SINIFLARI (ORİJİNAL KODDAN) ---
    Public Class SSP_FULL_KEY
        Public FixedKey As ULong
        Public VariableKey As ULong
    End Class

    Public Class SSP_COMMAND
        Public Key As New SSP_FULL_KEY()
        Public Timeout As UInteger = 750
        Public SSPAddress As Byte = 0
        Public EncryptionStatus As Boolean = False
        Public CommandDataLength As Byte
        Public CommandData(254) As Byte
        Public ResponseDataLength As Byte
        Public ResponseData(254) As Byte
        Public encPktCount As UInteger
        Public sspSeq As Byte
    End Class

    Public Class SSP_PACKET
        Public PacketLength As Byte
        Public PacketData(254) As Byte
    End Class

    Public Class SSP_COMMAND_INFO
        Public Transmit As New SSP_PACKET()
        Public Receive As New SSP_PACKET()
        Public PreEncryptedTransmit As New SSP_PACKET()
        Public PreEncryptedRecieve As New SSP_PACKET()
    End Class

    Class SSP_TX_RX_PACKET
        Public txPtr As Byte
        Public txData(254) As Byte
        Public rxPtr As Byte
        Public rxData(254) As Byte
        Public txBufferLength As Byte
        Public rxBufferLength As Byte
        Public SSPAddress As Byte
        Public NewResponse As Boolean
        Public CheckStuff As Byte
    End Class

    Public Enum PORT_STATUS
        PORT_CLOSED
        PORT_OPEN
        PORT_ERROR
        Veri_Gonderme_Basarili
        SSP_PACKET_ERROR
        SSP_CMD_TIMEOUT
        SSP_PACKET_ERROR_CRC_FAIL
        SSP_PACKET_ERROR_ENC_COUNT
    End Enum

    Public Enum CCommands As Byte

        SSP_CMD_RESET = &H1
        SSP_CMD_SET_CHANNEL_INHIBITS = &H2
        SSP_CMD_DISPLAY_ON = &H3
        SSP_CMD_DISPLAY_OFF = &H4
        SSP_CMD_SETUP_REQUEST = &H5
        SSP_CMD_HOST_PROTOCOL_VERSION = &H6
        SSP_CMD_POLL = &H7
        SSP_CMD_REJECT_BANKNOTE = &H8
        SSP_CMD_DISABLE = &H9
        SSP_CMD_ENABLE = &HA
        SSP_CMD_GET_SERIAL_NUMBER = &HC
        SSP_CMD_UNIT_DATA = &HD
        SSP_CMD_CHANNEL_VALUE_REQUEST = &HE
        SSP_CMD_CHANNEL_SECURITY_DATA = &HF
        SSP_CMD_CHANNEL_RE_TEACH_DATA = &H10
        SSP_CMD_SYNC = &H11
        SSP_CMD_LAST_REJECT_CODE = &H17
        SSP_CMD_HOLD = &H18
        SSP_CMD_GET_FIRMWARE_VERSION = &H20
        SSP_CMD_GET_DATASET_VERSION = &H21
        SSP_CMD_GET_ALL_LEVELS = &H22
        SSP_CMD_GET_BAR_CODE_READER_CONFIGURATION = &H23
        SSP_CMD_SET_BAR_CODE_CONFIGURATION = &H24
        SSP_CMD_GET_BAR_CODE_INHIBIT_STATUS = &H25
        SSP_CMD_SET_BAR_CODE_INHIBIT_STATUS = &H26
        SSP_CMD_GET_BAR_CODE_DATA = &H27
        SSP_CMD_SET_REFILL_MODE = &H30
        SSP_CMD_PAYOUT_AMOUNT = &H33
        SSP_CMD_SET_DENOMINATION_LEVEL = &H34
        SSP_CMD_GET_DENOMINATION_LEVEL = &H35
        SSP_CMD_COMMUNICATION_PASS_THROUGH = &H37
        SSP_CMD_HALT_PAYOUT = &H38
        SSP_CMD_SET_DENOMINATION_ROUTE = &H3B
        SSP_CMD_GET_DENOMINATION_ROUTE = &H3C
        SSP_CMD_FLOAT_AMOUNT = &H3D
        SSP_CMD_GET_MINIMUM_PAYOUT = &H3E
        SSP_CMD_EMPTY_ALL = &H3F
        SSP_CMD_SET_COIN_MECH_INHIBITS = &H40
        SSP_CMD_GET_NOTE_POSITIONS = &H41
        SSP_CMD_PAYOUT_NOTE = &H42
        SSP_CMD_STACK_NOTE = &H43
        SSP_CMD_FLOAT_BY_DENOMINATION = &H44
        SSP_CMD_SET_VALUE_REPORTING_TYPE = &H45
        SSP_CMD_PAYOUT_BY_DENOMINATION = &H46
        SSP_CMD_SET_COIN_MECH_GLOBAL_INHIBIT = &H49
        SSP_CMD_SET_GENERATOR = &H4A
        SSP_CMD_SET_MODULUS = &H4B
        SSP_CMD_REQUEST_KEY_EXCHANGE = &H4C
        SSP_CMD_SET_BAUD_RATE = &H4D
        SSP_CMD_SET_Limit_Belirle = &H4E
        SSP_CMD_GET_BUILD_REVISION = &H4F
        SSP_CMD_SET_HOPPER_OPTIONS = &H50
        SSP_CMD_GET_HOPPER_OPTIONS = &H51
        SSP_CMD_SMART_EMPTY = &H52
        SSP_CMD_CASHBOX_PAYOUT_OPERATION_DATA = &H53
        SSP_CMD_CONFIGURE_BEZEL = &H54
        SSP_CMD_POLL_WITH_ACK = &H56
        SSP_CMD_EVENT_ACK = &H57
        SSP_CMD_GET_COUNTERS = &H58
        SSP_CMD_RESET_COUNTERS = &H59
        SSP_CMD_COIN_MECH_OPTIONS = &H5A
        SSP_CMD_DISABLE_PAYOUT_DEVICE = &H5B
        SSP_CMD_ENABLE_PAYOUT_DEVICE = &H5C
        SSP_CMD_SET_FIXED_ENCRYPTION_KEY = &H60
        SSP_CMD_RESET_FIXED_ENCRYPTION_KEY = &H61
        SSP_CMD_REQUEST_TEBS_BARCODE = &H65
        SSP_CMD_REQUEST_TEBS_LOG = &H66
        SSP_CMD_TEBS_UNLOCK_ENABLE = &H67
        SSP_CMD_TEBS_UNLOCK_DISABLE = &H68

        SSP_POLL_TEBS_CASHBOX_OUT_OF_SERVICE = &H90
        SSP_POLL_TEBS_CASHBOX_TAMPER = &H91
        SSP_POLL_TEBS_CASHBOX_IN_SERVICE = &H92
        SSP_POLL_TEBS_CASHBOX_UNLOCK_ENABLED = &H93
        SSP_POLL_JAM_RECOVERY = &HB0
        SSP_POLL_ERROR_DURING_PAYOUT = &HB1
        SSP_POLL_SMART_EMPTYING = &HB3
        SSP_POLL_SMART_EMPTIED = &HB4
        SSP_POLL_CHANNEL_DISABLE = &HB5
        SSP_POLL_INITIALISING = &HB6
        SSP_POLL_COIN_MECH_ERROR = &HB7
        SSP_POLL_EMPTYING = &HC2
        SSP_POLL_EMPTIED = &HC3
        SSP_POLL_COIN_MECH_JAMMED = &HC4
        SSP_POLL_COIN_MECH_RETURN_PRESSED = &HC5
        SSP_POLL_PAYOUT_OUT_OF_SERVICE = &HC6
        SSP_POLL_NOTE_FLOAT_REMOVED = &HC7
        SSP_POLL_NOTE_FLOAT_ATTACHED = &HC8
        SSP_POLL_NOTE_TRANSFERED_TO_STACKER = &HC9
        SSP_POLL_NOTE_PAID_INTO_STACKER_AT_POWER_UP = &HCA
        SSP_POLL_NOTE_PAID_INTO_STORE_AT_POWER_UP = &HCB
        SSP_POLL_NOTE_STACKING = &HCC
        SSP_POLL_NOTE_DISPENSED_AT_POWER_UP = &HCD
        SSP_POLL_NOTE_HELD_IN_BEZEL = &HCE
        SSP_POLL_BAR_CODE_TICKET_ACKNOWLEDGE = &HD1
        SSP_POLL_DISPENSED = &HD2
        SSP_POLL_JAMMED = &HD5
        SSP_POLL_HALTED = &HD6
        SSP_POLL_FLOATING = &HD7
        SSP_POLL_FLOATED = &HD8
        SSP_POLL_TIME_OUT = &HD9
        SSP_POLL_DISPENSING = &HDA
        SSP_POLL_NOTE_STORED_IN_PAYOUT = &HDB
        SSP_POLL_INCOMPLETE_PAYOUT = &HDC
        SSP_POLL_INCOMPLETE_FLOAT = &HDD
        SSP_POLL_CASHBOX_PAID = &HDE
        SSP_POLL_COIN_CREDIT = &HDF
        SSP_POLL_NOTE_PATH_OPEN = &HE0
        SSP_POLL_NOTE_CLEARED_FROM_FRONT = &HE1
        SSP_POLL_NOTE_CLEARED_TO_CASHBOX = &HE2
        SSP_POLL_CASHBOX_REMOVED = &HE3
        SSP_POLL_CASHBOX_REPLACED = &HE4
        SSP_POLL_BAR_CODE_TICKET_VALIDATED = &HE5
        SSP_POLL_FRAUD_ATTEMPT = &HE6
        SSP_POLL_STACKER_FULL = &HE7
        SSP_POLL_DISABLED = &HE8
        SSP_POLL_UNSAFE_NOTE_JAM = &HE9
        SSP_POLL_SAFE_NOTE_JAM = &HEA
        SSP_POLL_NOTE_STACKED = &HEB
        SSP_POLL_NOTE_REJECTED = &HEC
        SSP_POLL_NOTE_REJECTING = &HED
        SSP_POLL_CREDIT_NOTE = &HEE
        SSP_POLL_READ_NOTE = &HEF
        SSP_POLL_SLAVE_RESET = &HF1

        Veri_Alma_Basarili = &HF0
        SSP_RESPONSE_COMMAND_NOT_KNOWN = &HF2
        SSP_RESPONSE_WRONG_NO_PARAMETERS = &HF3
        SSP_RESPONSE_PARAMETER_OUT_OF_RANGE = &HF4
        SSP_RESPONSE_COMMAND_CANNOT_BE_PROCESSED = &HF5
        SSP_RESPONSE_SOFTWARE_ERROR = &HF6
        SSP_RESPONSE_FAIL = &HF8
        SSP_RESPONSE_KEY_NOT_SET = &HFA
        ParaOrtadaBekliyor = &HFE
        IslemBasarisiz = &HFF

        SSP_CMD_SSP_ENCRYPTION_RESET_TO_DEFAULT = &H4F


    End Enum

    Enum Nakit_Para_Status
        Basarisiz = 0
        Basarili = 1
    End Enum


    Public Class SSP_KEYS
        Public Generator As ULong
        Public Modulus As ULong
        Public HostInter As ULong
        Public HostRandom As ULong
        Public SlaveInterKey As ULong
        Public SlaveRandom As ULong
        Public KeyHost As ULong
        Public KeySlave As ULong
    End Class

    Class KanalData
        Property Banknot As Integer
        Property KanalNo As Byte
        Property ParaBirimi As Char()
        Property BanknotAdet As Integer
        Property KasayaIndir As Boolean
        Property BankNotCarpan As Integer
        Public Sub New()
            ParaBirimi = New Char(2) {}
        End Sub
    End Class

    Sub Poll_Sonuc_Degerlendir()

        Dim ResponseLength As Integer = SSPCommand.ResponseDataLength
        Dim ResponseData(ResponseLength - 1) As Byte
        Array.Copy(SSPCommand.ResponseData, ResponseData, ResponseLength)
        Dim i As Integer

        ' Poll yanıtını parse et
        Dim data As New KanalData()
        i = 1
        While i < SSPCommand.ResponseDataLength
            Select Case ResponseData(i)
                    ' Bu yanıt, birimin sıfırlandığını ve reset'ten bu yana ilk kez 
                    ' poll çağrıldığını gösterir.
                Case CCommands.SSP_POLL_SLAVE_RESET
                    Log_Yazdir("Birim sıfırlandı.")
                            'UpdateData()

                    ' Bu yanıt birimin devre dışı olduğunu gösterir.
                Case CCommands.SSP_POLL_DISABLED
                    Log_Yazdir("Birim devre dışı.")

                    ' Bir not şu anda validatör sensörleri tarafından okunuyor.
                Case CCommands.SSP_POLL_READ_NOTE

                    If SSPCommand.ResponseData(i + 1) > 0 Then
                        Log_Yazdir("Banknot Ortada Bekliyor.")

                        ' GetDataByChannel(response(i + 1), data)
                        'Log.AppendText("Escrow'da not, miktar: " & FormatToCurrency(data.Value) & vbCrLf)
                        'm_HoldCount = m_HoldNumber
                    Else
                        'Log.AppendText("Not okunuyor" & vbCrLf)
                        Log_Yazdir("Banknot Okunuyor.")
                    End If
                    i += 1

                Case CCommands.SSP_POLL_CREDIT_NOTE  ' Bir kredi olayı tespit edildi, bu validatörün bir notu yasal para olarak kabul ettiği zamandır.

                            'GetDataByChannel(response(i + 1), data)
                            'Log.AppendText("Kredi " & FormatToCurrency(data.Value) & vbCrLf)
                            'UpdateData()
                            'i += 1

                    ' Bir not validatörden reddediliyor.
                Case CCommands.SSP_POLL_NOTE_REJECTING
                            'Log.AppendText("Not reddediliyor" & vbCrLf)

                    ' Bir not validatörden reddedildi, not bezel'de olacak.
                Case CCommands.SSP_POLL_NOTE_REJECTED
                            'Log.AppendText("Not reddedildi" & vbCrLf)
                            'QueryRejection(Log)

                    ' Bir not cashbox'a taşınıyor.
                Case CCommands.SSP_POLL_NOTE_STACKING
                            'Log.AppendText("Not istifleniyor" & vbCrLf)

                    ' Ödeme cihazı belirtilen miktarda notu 'floating' yapıyor.
                Case CCommands.SSP_POLL_FLOATING

                            'Log.AppendText("Notlar floating yapılıyor" & vbCrLf)
                            ' Şimdi indeks, bu yanıt tarafından sağlanan verilerin üzerinden atlamak için ilerletilmeli
                            'i += CByte((response(i + 1) * 7) + 1)

                    ' Bir not cashbox'a ulaştı.
                Case CCommands.SSP_POLL_NOTE_STACKED
                    'Log.AppendText("Not istiflendi" & vbCrLf)
                    Log_Yazdir("Banknot İstifendi.")
                    ' Float işlemi tamamlandı.
                Case CCommands.SSP_POLL_FLOATED
                            'Log.AppendText("Floating tamamlandı" & vbCrLf)
                            'GetCashboxPayoutOpData(Log)
                            'UpdateData()
                            'EnableValidator()
                            'i += CByte((response(i + 1) * 7) + 1)

                    ' Bir not ödeme için cashbox'a gitmek yerine payout cihazında saklandı.
                Case CCommands.SSP_POLL_NOTE_STORED_IN_PAYOUT
                    Log_Yazdir("Banknot Saklandı.")
                            'Log.AppendText("Not saklandı" & vbCrLf)
                            'UpdateData()

                    ' Güvenli sıkışma tespit edildi.
                Case CCommands.SSP_POLL_SAFE_NOTE_JAM
                            'Log.AppendText("Güvenli sıkışma" & vbCrLf)

                    ' Güvensiz sıkışma tespit edildi.
                Case CCommands.SSP_POLL_UNSAFE_NOTE_JAM
                            'Log.AppendText("Güvensiz sıkışma" & vbCrLf)

                    ' Ödeme birimi tarafından bir hata tespit edildi.
                Case CCommands.SSP_POLL_ERROR_DURING_PAYOUT
                    'Log.AppendText("Ödeme cihazında hata tespit edildi" & vbCrLf)
                    i += CByte((ResponseData(i + 1) * 7) + 2)

                    ' Sahtecilik girişimi tespit edildi.
                Case CCommands.SSP_POLL_FRAUD_ATTEMPT
                    'Log.AppendText("Sahtecilik girişimi" & vbCrLf)
                    i += CByte((ResponseData(i + 1) * 7) + 1)

                    ' Stacker (cashbox) dolu.
                Case CCommands.SSP_POLL_STACKER_FULL
                            'Log.AppendText("Stacker dolu" & vbCrLf)

                    ' Başlangıçta validatörün içinde bir not tespit edildi ve birimin önünden reddedildi.
                Case CCommands.SSP_POLL_NOTE_CLEARED_FROM_FRONT
                    'Log.AppendText("Validatörün önünden not temizlendi" & vbCrLf)
                    i += 1

                    ' Başlangıçta validatörün içinde bir not tespit edildi ve cashbox'a temizlendi.
                Case CCommands.SSP_POLL_NOTE_CLEARED_TO_CASHBOX
                    'Log.AppendText("Not cashbox'a temizlendi" & vbCrLf)
                    i += 1

                    ' Başlangıçta validatörde bir not tespit edildi ve payout cihazına taşındı.
                Case CCommands.SSP_POLL_NOTE_PAID_INTO_STORE_AT_POWER_UP
                    'Log.AppendText("Başlangıçta not payout cihazına yatırıldı" & vbCrLf)
                    i += 7

                    ' Başlangıçta validatörde bir not tespit edildi ve cashbox'a taşındı.
                Case CCommands.SSP_POLL_NOTE_PAID_INTO_STACKER_AT_POWER_UP
                    'Log.AppendText("Başlangıçta not cashbox'a yatırıldı" & vbCrLf)
                    i += 7

                    ' Cashbox birimden çıkarıldı.
                Case CCommands.SSP_POLL_CASHBOX_REMOVED
                            'Log.AppendText("Cashbox çıkarıldı" & vbCrLf)

                    ' Cashbox yerine kondu.
                Case CCommands.SSP_POLL_CASHBOX_REPLACED
                           'Log.AppendText("Cashbox yerine kondu" & vbCrLf)

                    ' Validatör bir not ödemesi yapma sürecinde.
                Case CCommands.SSP_POLL_DISPENSING
                    'Log.AppendText("Not(lar) dağıtılıyor" & vbCrLf)
                    i += CByte((ResponseData(i + 1) * 7) + 1)

                    ' Not dağıtıldı ve kullanıcı tarafından bezel'den alındı.
                Case CCommands.SSP_POLL_DISPENSED
                    'Log.AppendText("Not(lar) dağıtıldı" & vbCrLf)
                    'UpdateData()
                    'EnableValidator()
                    i += CByte((ResponseData(i + 1) * 7) + 1)

                    ' Ödeme cihazı sakladığı tüm notları cashbox'a boşaltma sürecinde.
                Case CCommands.SSP_POLL_EMPTYING
                            'Log.AppendText("Boşaltılıyor" & vbCrLf)

                    ' Ödeme cihazı boşaltmayı tamamladı.
                Case CCommands.SSP_POLL_EMPTIED
                            'Log.AppendText("Boşaltıldı" & vbCrLf)
                            'UpdateData()
                            'EnableValidator()

                    ' Ödeme cihazı sakladığı tüm notları cashbox'a SMART boşaltma sürecinde.
                Case CCommands.SSP_POLL_SMART_EMPTYING
                    'Log.AppendText("SMART Boşaltma yapılıyor..." & vbCrLf)
                    i += CByte((ResponseData(i + 1) * 7) + 1)

                    ' Ödeme cihazı SMART boşaltmayı tamamladı.
                Case CCommands.SSP_POLL_SMART_EMPTIED
                    'Log.AppendText("SMART Boşaltıldı, bilgi alınıyor..." & vbCrLf)
                    'UpdateData()
                    'GetCashboxPayoutOpData(Log)
                    'EnableValidator()
                    i += CByte((ResponseData(i + 1) * 7) + 1)

                    ' Ödeme cihazı sıkışma ile karşılaştı.
                Case CCommands.SSP_POLL_JAMMED
                    'Log.AppendText("Birim sıkıştı..." & vbCrLf)
                    i += CByte((ResponseData(i + 1) * 7) + 1)

                    ' Ödeme bir host komutu tarafından durdurulduğunda bildirilir.
                Case CCommands.SSP_POLL_HALTED
                    'Log.AppendText("Durduruldu..." & vbCrLf)
                    i += CByte((ResponseData(i + 1) * 7) + 1)

                    ' Ödeme işlemi sırasında güç kesildiğinde bildirilir.
                Case CCommands.SSP_POLL_INCOMPLETE_PAYOUT
                    'Log.AppendText("Tamamlanmamış ödeme" & vbCrLf)
                    i += CByte((ResponseData(i + 1) * 11) + 1)

                    ' Float işlemi sırasında güç kesildiğinde bildirilir.
                Case CCommands.SSP_POLL_INCOMPLETE_FLOAT
                    'Log.AppendText("Tamamlanmamış float" & vbCrLf)
                    i += CByte((ResponseData(i + 1) * 11) + 1)

                    ' Bir not payout biriminden stacker'a transfer edildi.
                Case CCommands.SSP_POLL_NOTE_TRANSFERED_TO_STACKER
                    'Log.AppendText("Not stacker'a transfer edildi" & vbCrLf)
                    i += 7

                    ' Bir not kullanıcı tarafından alınmayı beklerken bezel'de duruyor.
                Case CCommands.SSP_POLL_NOTE_HELD_IN_BEZEL
                    'Log.AppendText("Bezel'de not bekleniyor..." & vbCrLf)
                    i += 7
                    Log_Yazdir("Banknot Bezelde Bekliyor.")
                    ' Ödeme servis dışı kaldı.
                Case CCommands.SSP_POLL_PAYOUT_OUT_OF_SERVICE
                            'Log.AppendText("Ödeme servis dışı..." & vbCrLf)

                    ' Birim ödenecek bir not ararken zaman aşımına uğradı.
                Case CCommands.SSP_POLL_TIME_OUT
                    'Log.AppendText("Not aranırken zaman aşımı" & vbCrLf)
                    i += CByte((ResponseData(i + 1) * 7) + 1)

                Case Else
                    ' Log.AppendText("Desteklenmeyen poll yanıtı alındı: " & CInt(SSPCommand.ResponseData(i)) & vbCrLf)
            End Select
            i += 1
        End While

    End Sub


    ' ----------------------------------------------------------------
    ' ANA FONKSİYON
    ' ----------------------------------------------------------------
    Private Sub CihazSorguTimer_Tick(sender As Object, e As EventArgs) Handles CihazSorguTimer.Elapsed
        CihazSorguTimer.Enabled = False
        TimerCalisiyor = True


        If SerialComPort IsNot Nothing AndAlso SerialComPort.IsOpen Then

            SSPCommand.CommandData(0) = CCommands.SSP_CMD_POLL
            SSPCommand.CommandDataLength = 1
            If SSPSendCommand(SSPCommand) = PORT_STATUS.Veri_Gonderme_Basarili Then
                Poll_Sonuc_Degerlendir()
            End If

        End If

        TimerCalisiyor = False
        CihazSorguTimer.Enabled = True
    End Sub

    Function Senkronizasyon_Gonder() As PORT_STATUS

        Log_Yazdir("Senkronizasyon Verisi Gönderiliyor.")
        SSPCommand.CommandData(0) = CCommands.SSP_CMD_SYNC
        SSPCommand.CommandDataLength = 1
        Return SSPSendCommand(SSPCommand)

    End Function

    Sub Timer_Durdur()
        Dim Zaman As DateTime = Now.AddMilliseconds(CihazSorguTimer.Interval + 25)
        CihazSorguTimer.Stop()
        Do While (TimerCalisiyor)
            Application.DoEvents()
            If Zaman <= Now Then
                TimerCalisiyor = False
                Exit Do
            End If
        Loop

        System.Threading.Thread.Sleep(50)
    End Sub

    Sub Timer_Baslat()
        CihazSorguTimer.Start()
    End Sub

    Function Seri_No_Oku() As String

        Timer_Durdur()
        Dim SeriNo As String = ""

        If SerialComPort IsNot Nothing AndAlso SerialComPort.IsOpen = True Then

            If Senkronizasyon_Gonder() = PORT_STATUS.Veri_Gonderme_Basarili Then

                Log_Yazdir("Seri No Okuma Gönderiliyor.")
                SSPCommand.CommandData(0) = CCommands.SSP_CMD_GET_SERIAL_NUMBER
                SSPCommand.CommandDataLength = 1

                If SSPSendCommand(SSPCommand) = PORT_STATUS.Veri_Gonderme_Basarili Then
                    Log_Yazdir("SeriNo Okuma Gönderildi.")

                    Log_Yazdir("SeriNo Verisi Bekleniyor.")
                    If SSPCommand.ResponseData(0) = CCommands.Veri_Alma_Basarili Then
                        Array.Reverse(SSPCommand.ResponseData, 1, 4)
                        SeriNo = BitConverter.ToUInt32(SSPCommand.ResponseData, 1).ToString
                        Log_Yazdir("SeriNo Verisi Geldi. SeriNo:" & SeriNo.ToString)
                    Else
                        Log_Yazdir("SeriNo Okumada Hata: Hata Kodu: SeriNo:" & SSPCommand.ResponseData(0).ToString("X2"))
                    End If
                End If

            End If
        Else
            Log_Yazdir("Bağlantı Portu Kapalı.")
        End If

        Timer_Baslat()
        Return SeriNo.ToString
    End Function

    Function Validator_Ac() As Boolean
        Timer_Durdur()

        SSPCommand.CommandData(0) = CCommands.SSP_CMD_ENABLE
        SSPCommand.CommandDataLength = 1
        Log_Yazdir("Gönderilen Komut:SSP_CMD_ENABLE")
        If SSPSendCommand(SSPCommand) = PORT_STATUS.Veri_Gonderme_Basarili AndAlso Data_isleme_Basarili() Then
            Log_Yazdir("Validatör Açıldı.")
            Timer_Baslat()
            Return True
        End If


        Log_Yazdir("Validatör Açılamadı.")
        Timer_Baslat()
        Return False
    End Function

    Function Validator_Kapat() As Boolean
        Timer_Durdur()

        SSPCommand.CommandData(0) = CCommands.SSP_CMD_DISABLE
        SSPCommand.CommandDataLength = 1
        Log_Yazdir("Gönderilen Komut:SSP_CMD_DISABLE")

        If SSPSendCommand(SSPCommand) = PORT_STATUS.Veri_Gonderme_Basarili AndAlso Data_isleme_Basarili() Then
            Log_Yazdir("Validatör Kapatıldı.")
        End If

        Log_Yazdir("Para Alma Kapatılamadı.")
        Timer_Baslat()
        Return False
    End Function

    Function Para_Alma_Ac() As Boolean
        Timer_Durdur()

        SSPCommand.CommandData(0) = CCommands.SSP_CMD_ENABLE_PAYOUT_DEVICE
        SSPCommand.CommandDataLength = 1
        Log_Yazdir("Gönderilen Komut:SSP_CMD_ENABLE_PAYOUT_DEVICE")
        If SSPSendCommand(SSPCommand) = PORT_STATUS.Veri_Gonderme_Basarili AndAlso Data_isleme_Basarili() Then
            Log_Yazdir("Para Alma Açıldı.")
            Timer_Baslat()
            Return True
        End If

        Log_Yazdir("Para Alma Açılamadı.")
        Timer_Baslat()
        Return False
    End Function

    Function Para_Alma_Kapat() As Boolean

        Timer_Durdur()

        SSPCommand.CommandData(0) = CCommands.SSP_CMD_DISABLE_PAYOUT_DEVICE
        SSPCommand.CommandDataLength = 1
        Log_Yazdir("Gönderilen Komut:SSP_CMD_DISABLE_PAYOUT_DEVICE")
        If SSPSendCommand(SSPCommand) = PORT_STATUS.Veri_Gonderme_Basarili AndAlso Data_isleme_Basarili() Then
            Timer_Baslat()
            Log_Yazdir("Para Alma Kapatıldı.")
            Return True

        End If

        Log_Yazdir("Para Alma Kapatılamadı.")
        Timer_Baslat()
        Return False
    End Function

    Sub Cihaz_Pool()

        SSPCommand.CommandData(0) = CCommands.SSP_CMD_POLL
        SSPCommand.CommandDataLength = 1
        Log_Yazdir("Gönderilen Komut:SSP_CMD_SYNC")
        If SSPSendCommand(SSPCommand) = PORT_STATUS.Veri_Gonderme_Basarili Then
            Log_Yazdir("Senkronizasyon Başarılı.")
        End If
    End Sub


    Function Para_Ver(Miktar As Integer, ParaBirimi As String) As Boolean

        Timer_Durdur()
        Dim Birim() As Char = ParaBirimi.ToCharArray
        Miktar *= BanknotCarpanDeger

        Dim MiktarBytes As Byte() = BitConverter.GetBytes(Miktar)

        SSPCommand.CommandData(0) = CCommands.SSP_CMD_PAYOUT_AMOUNT
        SSPCommand.CommandData(1) = MiktarBytes(0)
        SSPCommand.CommandData(2) = MiktarBytes(1)
        SSPCommand.CommandData(3) = MiktarBytes(2)
        SSPCommand.CommandData(4) = MiktarBytes(3)
        SSPCommand.CommandData(5) = CByte(AscW(Birim(0)))
        SSPCommand.CommandData(6) = CByte(AscW(Birim(1)))
        SSPCommand.CommandData(7) = CByte(AscW(Birim(2)))
        SSPCommand.CommandData(8) = &H58 ' gerçek ödeme (&H19 test için)
        SSPCommand.CommandDataLength = 9

        Log_Yazdir("Gönderilen Komut:SSP_CMD_PAYOUT_AMOUNT")

        If SSPSendCommand(SSPCommand) = PORT_STATUS.Veri_Gonderme_Basarili AndAlso Data_isleme_Basarili() Then
            Timer_Baslat()
            Log_Yazdir("Ödeme Yapılıyor...")
            Return True
        End If

        Timer_Baslat()
        Log_Yazdir("Ödeme Yapma İşlemi Başarısız.")
        Return True
    End Function


    Function Baglanti_Kur() As Boolean



        Try
            Baglantiyi_Kapat()

            Dim PortsList() As String = SerialPort.GetPortNames
            For i As Integer = 0 To PortsList.Count - 1
                Try
                    Log_Yazdir("Otomatik Port Aranıyor.Deneme:" & i + 1 & " Port:" & PortsList(i))
                    SerialComPort = New SerialPort
                    AddHandler SerialComPort.DataReceived, AddressOf SerialComPort_DataReceived
                    SerialComPort.PortName = PortsList(i)
                    SerialComPort.BaudRate = 9600
                    SerialComPort.Parity = Parity.None
                    SerialComPort.StopBits = StopBits.Two
                    SerialComPort.DataBits = 8
                    SerialComPort.Handshake = Handshake.None
                    SerialComPort.WriteTimeout = 1500
                    SerialComPort.ReadTimeout = 1500
                    SerialComPort.Open()
                    Log_Yazdir("Port Açıldı. Bağlantı Portu:" & SerialComPort.PortName)
                    Log_Yazdir("Bağlantı Deneniyor.")

                    If Senkronizasyon_Gonder() = PORT_STATUS.Veri_Gonderme_Basarili Then
                        Log_Yazdir("Bağlantı Başarılı.")

                        If Protokolu_Sifresi_Olustur() = False Then
                            Log_Yazdir("Anahtar Eşleşme Hatası!")
                        Else
                            System.Threading.Thread.Sleep(500)
                            Cihazi_Yapilandir()
                            CihazSorguTimer.Enabled = True
                        End If
                        Exit For
                    Else
                        Log_Yazdir("Porta Bağlandı.Cihaza Bağlantı Başarısız.")
                        Baglantiyi_Kapat()
                    End If

                Catch ex As Exception

                    Log_Yazdir("Bağlantı Açılamadı.")
                    Continue For
                End Try

            Next



            Return True
        Catch ex As Exception
            Log_Yazdir(ex.Message)
        End Try

        Return False
    End Function

    Private Function SetProtocolVersion(Versiyon As Byte) As Boolean

        Timer_Durdur()

        SSPCommand.CommandData(0) = CCommands.SSP_CMD_HOST_PROTOCOL_VERSION
        SSPCommand.CommandData(1) = 8
        SSPCommand.CommandDataLength = 2
        Log_Yazdir("Gönderilen Komut:SSP_CMD_HOST_PROTOCOL_VERSION")

        If SSPSendCommand(SSPCommand) = PORT_STATUS.Veri_Gonderme_Basarili AndAlso Data_isleme_Basarili() Then
            Timer_Baslat()
            Log_Yazdir("Versiyon Ayarlanıyor.")
            Return True
        End If

        Timer_Baslat()
        Log_Yazdir("Versiyon Ayarlama Başarısız.")
        Return False

    End Function




    Public Function Cihazi_Yapilandir() As Nakit_Para_Status

        Timer_Durdur()

        SetProtocolVersion(8)



        SSPCommand.CommandData(0) = CCommands.SSP_CMD_SETUP_REQUEST
        SSPCommand.CommandDataLength = 1
        Log_Yazdir("Gönderilen Komut:SSP_CMD_SETUP_REQUEST")

        Dim Basarili As Boolean = SSPSendCommand(SSPCommand) = PORT_STATUS.Veri_Gonderme_Basarili AndAlso Data_isleme_Basarili()
        If Basarili = True Then
            Dim sbDisplay As New StringBuilder(1000)
            Dim index As Integer = 1
            sbDisplay.Append("Birim Tipi: ")  ' Birim tipi
            Dim m_UnitType As Char = ChrW(SSPCommand.ResponseData(index))
            Select Case m_UnitType
                Case ChrW(&H0) : sbDisplay.Append("Validatör")
                Case ChrW(&H3) : sbDisplay.Append("SMART Hopper")
                Case ChrW(&H6) : sbDisplay.Append("SMART Payout")
                Case ChrW(&H7) : sbDisplay.Append("NV11")
                Case Else : sbDisplay.Append("Bilinmeyen Tip")
            End Select


            ' Firmware
            index = 2
            sbDisplay.AppendLine()
            sbDisplay.Append("Firmware: ")
            While index <= 5
                sbDisplay.Append(ChrW(SSPCommand.ResponseData(index)))
                index += 1
                If index = 4 Then
                    sbDisplay.Append(".")
                End If
            End While
            sbDisplay.AppendLine()


            ' Ülke kodu (eski kod, atla)
            index += 3

            ' Değer çarpanı (eski kod, atla)
            index += 3

            ' Kanal sayısı
            sbDisplay.AppendLine()
            sbDisplay.Append("Kanal Sayısı: ")
            Dim KanalSayisi As Integer = SSPCommand.ResponseData(index)
            index += 1
            sbDisplay.Append(KanalSayisi)
            sbDisplay.AppendLine()

            ' Kanal değerleri (eski kod, atla)
            index += KanalSayisi

            ' Kanal güvenliği (eski kod, atla)
            index += KanalSayisi

            ' Gerçek değer çarpanı (big endian)
            sbDisplay.Append("Gerçek Değer Çarpanı: ")
            BanknotCarpanDeger = SSPCommand.ResponseData(index + 2)
            BanknotCarpanDeger += SSPCommand.ResponseData(index + 1) << 8
            BanknotCarpanDeger += SSPCommand.ResponseData(index) << 16

            sbDisplay.Append(BanknotCarpanDeger)
            sbDisplay.AppendLine()
            index += 3

            ' Protokol versiyonu
            sbDisplay.Append("Protokol Versiyonu: ")
            m_ProtocolVersion = SSPCommand.ResponseData(index)
            index += 1
            sbDisplay.Append(m_ProtocolVersion)
            sbDisplay.AppendLine()

            ' Kanal verilerini listeye ekle ve göster
            ' Listeyi temizle
            KanalListesi.Clear()

            ' Tüm kanalları döngüye al
            For i As Integer = 0 To KanalSayisi - 1
                ' Kanal değeri
                Dim KanalDurumu As New KanalData With {
                    .KanalNo = CByte(i + 1),
                    .Banknot = BitConverter.ToInt32(SSPCommand.ResponseData, index + (KanalSayisi * 3) + (i * 4)) * BanknotCarpanDeger
                }
                ' Kanal para birimi
                KanalDurumu.ParaBirimi(0) = ChrW(SSPCommand.ResponseData(index + (i * 3)))
                KanalDurumu.ParaBirimi(1) = ChrW(SSPCommand.ResponseData((index + 1) + (i * 3)))
                KanalDurumu.ParaBirimi(2) = ChrW(SSPCommand.ResponseData((index + 2) + (i * 3)))
                ' Kanal seviyesi
                KanalDurumu.BanknotAdet = CheckNoteLevel(KanalDurumu.Banknot, KanalDurumu.ParaBirimi)
                KanalDurumu.KasayaIndir = Kanal_Yonlendirme_Getir(KanalDurumu.Banknot, KanalDurumu.ParaBirimi)
                ' Veriyi listeye ekle
                KanalListesi.Add(KanalDurumu)
                ' Veriyi göster
                sbDisplay.Append("Kanal ")
                sbDisplay.Append(KanalDurumu.KanalNo)
                sbDisplay.Append(": ")
                sbDisplay.Append(KanalDurumu.Banknot \ BanknotCarpanDeger)
                sbDisplay.Append(" ")
                sbDisplay.Append(KanalDurumu.ParaBirimi)
                sbDisplay.Append("Kasaya İndir:" & KanalDurumu.KasayaIndir)
                sbDisplay.AppendLine()
            Next

            KanalListesi.Sort(Function(d1, d2) d1.Banknot.CompareTo(d2.Banknot))
            Log_Yazdir(sbDisplay.ToString)

            Timer_Baslat()
            Log_Yazdir("Cihaz Yapılandıma Başarılı.")


        Else


            Timer_Baslat()
            Log_Yazdir("SSP_CMD_SETUP_REQUEST Başarısız.")
            Return Nakit_Para_Status.Basarisiz
        End If

        Timer_Baslat()
        Log_Yazdir("Cihaz Yapılandıma Başarısız.")
        Return Nakit_Para_Status.Basarisiz

    End Function





    Public Function CheckNoteLevel(Banknot As Integer, ParaBirimi As Char()) As Integer


        Dim MiktarBytes As Byte() = BitConverter.GetBytes(Banknot)
        SSPCommand.CommandData(0) = CCommands.SSP_CMD_GET_DENOMINATION_LEVEL
        SSPCommand.CommandData(1) = MiktarBytes(0)
        SSPCommand.CommandData(2) = MiktarBytes(1)
        SSPCommand.CommandData(3) = MiktarBytes(2)
        SSPCommand.CommandData(4) = MiktarBytes(3)

        SSPCommand.CommandData(5) = CByte(AscW(ParaBirimi(0)) And &HFF)
        SSPCommand.CommandData(6) = CByte(AscW(ParaBirimi(1)) And &HFF)
        SSPCommand.CommandData(7) = CByte(AscW(ParaBirimi(2)) And &HFF)
        SSPCommand.CommandDataLength = 8

        If SSPSendCommand(SSPCommand) = PORT_STATUS.Veri_Gonderme_Basarili AndAlso Data_isleme_Basarili() Then
            Return CInt(SSPCommand.ResponseData(1))
        End If

        Return 0
    End Function


    Public Function Kanal_Yonlendirme_Getir(Banknot As Integer, ParaBirimi As Char()) As Boolean

        Dim MiktarBytes As Byte() = BitConverter.GetBytes(Banknot) ' Notun şu anda geri dönüşümde olup olmadığını belirle
        SSPCommand.CommandData(0) = CCommands.SSP_CMD_GET_DENOMINATION_ROUTE
        SSPCommand.CommandData(1) = MiktarBytes(0)
        SSPCommand.CommandData(2) = MiktarBytes(1)
        SSPCommand.CommandData(3) = MiktarBytes(2)
        SSPCommand.CommandData(4) = MiktarBytes(3)
        SSPCommand.CommandData(5) = CByte(AscW(ParaBirimi(0)))  ' Para birimini ekle
        SSPCommand.CommandData(6) = CByte(AscW(ParaBirimi(1)))
        SSPCommand.CommandData(7) = CByte(AscW(ParaBirimi(2)))
        SSPCommand.CommandDataLength = 8

        If SSPSendCommand(SSPCommand) = PORT_STATUS.Veri_Gonderme_Basarili AndAlso Data_isleme_Basarili() Then
            If SSPCommand.ResponseData(1) = &H0 Then
                Return False ' False ise değil
            ElseIf SSPCommand.ResponseData(1) = &H1 Then
                Return True
            End If
        End If


        Return False
    End Function


    Public Function Banknot_Yonlendir(Banknot As Integer, ParaBirimi As String, KasayaIndir As Boolean) As Nakit_Para_Status


        Log_Yazdir("Banknot Yönlendirme Yapılıyor.")
        Dim ParaBirimChr() As Char = ParaBirimi.ToCharArray
        If ParaBirimChr.Count <> 3 Then
            Log_Yazdir("Banknot Yönlendirme Para Birimi Geçersiz.")
            Return Nakit_Para_Status.Basarisiz
        End If

        Timer_Durdur()
        Banknot *= BanknotCarpanDeger

        Dim MiktarBytes As Byte() = BitConverter.GetBytes(Banknot) ' Notun şu anda geri dönüşümde olup olmadığını belirle

        SSPCommand.CommandData(0) = CCommands.SSP_CMD_SET_DENOMINATION_ROUTE
        SSPCommand.CommandData(1) = CByte(Math.Abs(CInt(KasayaIndir))) '0 ise, Arka Para verme Haznesi 1 ise Kasaya İndirir.
        SSPCommand.CommandData(2) = MiktarBytes(0)
        SSPCommand.CommandData(3) = MiktarBytes(1)
        SSPCommand.CommandData(4) = MiktarBytes(2)
        SSPCommand.CommandData(5) = MiktarBytes(3)
        SSPCommand.CommandData(6) = CByte(AscW(ParaBirimChr(0)))  ' Para birimini ekle
        SSPCommand.CommandData(7) = CByte(AscW(ParaBirimChr(1)))
        SSPCommand.CommandData(8) = CByte(AscW(ParaBirimChr(2)))
        SSPCommand.CommandDataLength = 9

        If SSPSendCommand(SSPCommand) = PORT_STATUS.Veri_Gonderme_Basarili AndAlso Data_isleme_Basarili() Then

            Dim KasaStr As String = "Kasaya İnecek"
            If KasayaIndir = False Then
                KasaStr = "Kasete Gönderilecek."
            End If
            Log_Yazdir("Banknot Yönlendirme Başarılı." & KasaStr)
            Timer_Baslat()
            Return Nakit_Para_Status.Basarili
        End If

        Timer_Baslat()
        Log_Yazdir("Banknot Yönlendirme Başarısız.")
        Return Nakit_Para_Status.Basarisiz

    End Function



    Public Function Baglantiyi_Kapat() As Boolean
        Try


            If SerialComPort IsNot Nothing Then

                Thread.Sleep(5)
                Application.DoEvents()
                Thread.Sleep(100)
                Application.DoEvents()
                CihazSorguTimer.Enabled = False
                SerialComPort.Close()
                SerialComPort = Nothing
            End If
        Catch
        End Try
        Return True
    End Function

    ' ----------------------------------------------------------------
    ' SSP SEND COMMAND (ORİJİNAL)
    ' ----------------------------------------------------------------
    Private Function SSPSendCommand(ByVal cmd As SSP_COMMAND) As PORT_STATUS

        Array.Clear(ssp.rxData, 0, ssp.rxData.Length)
        If cmd.CommandData(0) = &H11 Then
            cmd.sspSeq = 128
        End If

        With SspInfo.PreEncryptedTransmit  ' Ön şifrelenmemiş paket oluştur
            .PacketLength = CByte(cmd.CommandDataLength + 5)
            .PacketData(0) = &H7F
            .PacketData(1) = CByte(cmd.SSPAddress Or cmd.sspSeq)
            .PacketData(2) = cmd.CommandDataLength
            Buffer.BlockCopy(cmd.CommandData, 0, .PacketData, 3, cmd.CommandDataLength)
            Dim crc As UShort = Crc_Hesapla(CUShort(cmd.CommandDataLength + 2), 1US, .PacketData, UShort.MaxValue, 32773US)
            .PacketData(3 + cmd.CommandDataLength) = CByte(crc And &HFFUS)
            .PacketData(4 + cmd.CommandDataLength) = CByte((crc >> 8) And &HFFUS)
        End With


        If cmd.EncryptionStatus Then ' Şifreleme etkinse

            Try
                Dim BytesP(254) As Byte
                Dim totalLength As Integer = cmd.CommandDataLength + 7
                Dim padding As Integer = (16 - (totalLength Mod 16)) Mod 16
                Dim finalLen As Integer = totalLength + padding
                BytesP(0) = cmd.CommandDataLength

                For i As Integer = 0 To 3  ' Paket sayacı (4 byte)
                    BytesP(1 + i) = CByte((cmd.encPktCount >> (8 * i)) And &HFFUI)
                Next

                ' Komut verisini kopyala
                Buffer.BlockCopy(cmd.CommandData, 0, BytesP, 5, cmd.CommandDataLength)

                ' Rastgele padding ekle
                For i As Integer = 0 To padding - 1
                    BytesP(5 + cmd.CommandDataLength + i) = CByte(GenerateRandomNumber() Mod 256UL)
                Next

                Dim crcValue As UShort = Crc_Hesapla(CUShort(finalLen - 2), 0US, BytesP, UShort.MaxValue, 32773US)  ' CRC hesapla ve ekle
                BytesP(finalLen - 2) = CByte(crcValue And &HFFUS)
                BytesP(finalLen - 1) = CByte((crcValue >> 8) And &HFFUS)


                Dim lengthForAes As Byte = CByte(finalLen)  ' AES şifreleme
                Aes_Encrypt(cmd.Key, BytesP, lengthForAes, 0)


                cmd.CommandDataLength = CByte(finalLen + 1)  ' Çıkış paketini hazırla
                cmd.CommandData(0) = &H7E
                Buffer.BlockCopy(BytesP, 0, cmd.CommandData, 1, finalLen)
                cmd.encPktCount = If(cmd.encPktCount = UInteger.MaxValue, 0UI, cmd.encPktCount + 1UI)    ' Paket sayacını artır
            Catch

                Log_Yazdir("Sifreleme Durumu:" & cmd.EncryptionStatus & " Paket Hesaplama Hatası.")
                Return PORT_STATUS.SSP_PACKET_ERROR
            End Try
        End If



        ' SSP TX paketi oluştur
        ssp.CheckStuff = 0
        ssp.SSPAddress = cmd.SSPAddress
        ssp.rxPtr = 0
        ssp.txPtr = 0
        ssp.txBufferLength = CByte(cmd.CommandDataLength + 5)
        ssp.txData(0) = &H7F
        ssp.txData(1) = CByte(cmd.SSPAddress Or cmd.sspSeq)
        ssp.txData(2) = cmd.CommandDataLength
        Buffer.BlockCopy(cmd.CommandData, 0, ssp.txData, 3, cmd.CommandDataLength)

        Dim crc2 As UShort = Crc_Hesapla(CUShort(ssp.txBufferLength - 3), 1US, ssp.txData, UShort.MaxValue, 32773US)
        ssp.txData(3 + cmd.CommandDataLength) = CByte(crc2 And &HFFUS)
        ssp.txData(4 + cmd.CommandDataLength) = CByte((crc2 >> 8) And &HFFUS)

        ' Kopyala
        Buffer.BlockCopy(ssp.txData, 0, SspInfo.Transmit.PacketData, 0, ssp.txBufferLength)
        SspInfo.Transmit.PacketLength = ssp.txBufferLength

        ' Stuffing işlemi
        Dim temp(254) As Byte
        Dim outIndex As Integer = 0
        temp(outIndex) = ssp.txData(0)
        outIndex += 1

        For i As Integer = 1 To ssp.txBufferLength - 1
            temp(outIndex) = ssp.txData(i)
            outIndex += 1
            If ssp.txData(i) = &H7F Then
                temp(outIndex) = &H7F
                outIndex += 1
            End If
        Next

        Buffer.BlockCopy(temp, 0, ssp.txData, 0, outIndex)
        ssp.txBufferLength = CByte(outIndex)



        ssp.NewResponse = False
        ssp.rxBufferLength = 0
        ssp.rxPtr = 0


        Try

            SerialComPort.Write(ssp.txData, 0, ssp.txBufferLength)
        Catch
            Log_Yazdir("Seriport Hatası Bağlantı Kapatıldı.")
            Baglantiyi_Kapat()
            Return PORT_STATUS.PORT_ERROR
        End Try


        Dim sw As New Stopwatch()
        sw.Start()

        While Not ssp.NewResponse
            If sw.ElapsedMilliseconds > cmd.Timeout Then
                sw.Stop()
                Log_Yazdir("Veri Dinlerken, Zaman Aşımı Oluştu. Beklenilen Süre:" & sw.ElapsedMilliseconds & " ms")
                Return PORT_STATUS.SSP_CMD_TIMEOUT
                Exit While
            End If
        End While


        SspInfo.Receive.PacketLength = CByte(ssp.rxData(2) + 5)
        For i As Integer = 0 To SspInfo.Receive.PacketLength - 1
            SspInfo.Receive.PacketData(i) = ssp.rxData(i)
        Next

        ' Şifre çözme
        If ssp.rxData(3) = 126 Then
            Dim length As Byte = CByte(ssp.rxData(2) - 1)
            Aes_Decrypt(cmd.Key, ssp.rxData, length, 4)

            Dim num As UShort = Crc_Hesapla(CUShort(length - 2), 4, ssp.rxData, UShort.MaxValue, 32773)
            If CByte(num And &HFF) <> ssp.rxData(ssp.rxData(2) + 1) OrElse
               CByte((num >> 8) And &HFF) <> ssp.rxData(ssp.rxData(2) + 2) Then
                Return PORT_STATUS.SSP_PACKET_ERROR_CRC_FAIL
            End If

            Dim num2 As UInteger = 0
            For i As Integer = 0 To 3
                num2 += CUInt(ssp.rxData(5 + i)) << (i * 8)
            Next

            If num2 <> cmd.encPktCount Then
                Return PORT_STATUS.SSP_PACKET_ERROR_ENC_COUNT
            End If

            ssp.rxBufferLength = CByte(ssp.rxData(4) + 5)
            Dim p(254) As Byte
            p(0) = ssp.rxData(0)
            p(1) = ssp.rxData(1)
            p(2) = ssp.rxData(4)
            For i As Integer = 0 To ssp.rxData(4) - 1
                p(3 + i) = ssp.rxData(9 + i)
            Next

            num = Crc_Hesapla(CUShort(ssp.rxBufferLength - 3), 1, p, UShort.MaxValue, 32773)
            p(3 + ssp.rxData(4)) = CByte(num And &HFF)
            p(4 + ssp.rxData(4)) = CByte((num >> 8) And &HFF)

            For i As Integer = 0 To ssp.rxBufferLength - 1
                ssp.rxData(i) = p(i)
            Next
        End If

        cmd.ResponseDataLength = ssp.rxData(2)
        For i As Integer = 0 To cmd.ResponseDataLength - 1
            cmd.ResponseData(i) = ssp.rxData(i + 3)
        Next

        If cmd.sspSeq = 128 Then
            cmd.sspSeq = 0
        Else
            cmd.sspSeq = 128
        End If
        Log_Yazdir("Veri Gönderme Başarılı.")
        Return PORT_STATUS.Veri_Gonderme_Basarili
    End Function







    Private Sub SerialComPort_DataReceived(ByVal sender As Object, ByVal e As SerialDataReceivedEventArgs)
        Try
            While SerialComPort.BytesToRead > 0
                SSPDataIn(CByte(SerialComPort.ReadByte()))
            End While
        Catch
        End Try
    End Sub

    Private Function Crc_Hesapla(l As UShort, offset As UShort, ByRef p() As Byte, seed As UShort, cd As UShort) As UShort
        Dim crc As UShort = seed

        For i As UShort = 0US To CUShort(l - 1US)
            crc = CUShort(crc Xor CUShort(CUShort(p(offset + i)) << 8))
            For j As UShort = 0US To 7US
                If (crc And &H8000US) = 0US Then
                    crc = CUShort((CUInt(crc) << 1) And &HFFFFUS)
                Else
                    crc = CUShort(((CUInt(crc) << 1) Xor cd) And &HFFFFUS)
                End If
            Next
        Next

        Return crc
    End Function

    Private Sub SSPDataIn(RxChar As Byte)
        ' Başlangıç baytı kontrolü
        If RxChar = &H7F AndAlso ssp.rxPtr = 0 Then
            ssp.rxData(ssp.rxPtr) = RxChar
            ssp.rxPtr = CByte(ssp.rxPtr + 1)
            Return
        End If

        If ssp.CheckStuff = 1 Then
            If RxChar <> &H7F Then
                ssp.rxData(0) = &H7F
                ssp.rxData(1) = RxChar
                ssp.rxPtr = 2
            Else
                ssp.rxData(ssp.rxPtr) = RxChar
                ssp.rxPtr = CByte(ssp.rxPtr + 1)
            End If
            ssp.CheckStuff = 0
        ElseIf RxChar = &H7F Then
            ssp.CheckStuff = 1
        Else
            ssp.rxData(ssp.rxPtr) = RxChar
            ssp.rxPtr = CByte(ssp.rxPtr + 1)

            If ssp.rxPtr = 3 Then
                ssp.rxBufferLength = CByte(ssp.rxData(2) + 5)
            End If
        End If

        ' Paket tamamlanmadıysa çık
        If ssp.rxPtr <> ssp.rxBufferLength Then Return

        ' CRC kontrolü
        If (ssp.rxData(1) And &H7F) = ssp.SSPAddress Then
            Dim crc As UShort = Crc_Hesapla(CUShort(ssp.rxBufferLength - 3), 1US, ssp.rxData, UShort.MaxValue, 32773US)
            Dim crcLow As Byte = CByte(crc And &HFFUS)
            Dim crcHigh As Byte = CByte((crc >> 8) And &HFFUS)

            If crcLow = ssp.rxData(ssp.rxBufferLength - 2) AndAlso crcHigh = ssp.rxData(ssp.rxBufferLength - 1) Then
                ssp.NewResponse = True
            End If
        End If

        ssp.rxPtr = 0
        ssp.CheckStuff = 0
    End Sub





    Private Sub Aes_Encrypt(ByRef sspKey As SSP_FULL_KEY, ByRef data() As Byte, ByRef length As Byte, ByVal offset As Byte)
        Using memoryStream As New MemoryStream()
            Using aesAlg As Aes = Aes.Create()
                Dim keyArray(15) As Byte

                For b As Integer = 0 To 7
                    keyArray(b) = CByte((sspKey.FixedKey >> (8 * b)) And &HFFUL)
                    keyArray(b + 8) = CByte((sspKey.VariableKey >> (8 * b)) And &HFFUL)
                Next

                aesAlg.BlockSize = 128
                aesAlg.KeySize = 128
                aesAlg.Key = keyArray
                aesAlg.Mode = CipherMode.ECB
                aesAlg.Padding = PaddingMode.None

                Using cryptoStream As New CryptoStream(memoryStream, aesAlg.CreateEncryptor(), CryptoStreamMode.Write)
                    cryptoStream.Write(data, offset, length)
                    cryptoStream.FlushFinalBlock()
                End Using

                Buffer.BlockCopy(memoryStream.ToArray(), 0, data, offset, length)
            End Using
        End Using
    End Sub


    Private Sub Aes_Decrypt(ByRef sspKey As SSP_FULL_KEY, ByRef data() As Byte, ByRef length As Byte, ByVal offset As Byte)
        Using memoryStream As New MemoryStream()
            Using aesAlg As Aes = Aes.Create()
                Dim keyArray(15) As Byte

                For b As Integer = 0 To 7
                    keyArray(b) = CByte((sspKey.FixedKey >> (8 * b)) And &HFFUL)
                    keyArray(b + 8) = CByte((sspKey.VariableKey >> (8 * b)) And &HFFUL)
                Next

                aesAlg.BlockSize = 128
                aesAlg.KeySize = 128
                aesAlg.Key = keyArray
                aesAlg.Mode = CipherMode.ECB
                aesAlg.Padding = PaddingMode.None

                Using cryptoStream As New CryptoStream(memoryStream, aesAlg.CreateDecryptor(), CryptoStreamMode.Write)
                    cryptoStream.Write(data, offset, length)
                    cryptoStream.FlushFinalBlock()
                End Using

                Dim decryptedData As Byte() = memoryStream.ToArray()
                Buffer.BlockCopy(decryptedData, 0, data, offset, length)
            End Using
        End Using
    End Sub


    ' ----------------------------------------------------------------
    ' ANAHTAR MÜZAKERESİ (ORİJİNAL)
    ' ----------------------------------------------------------------
    Private Function Protokolu_Sifresi_Olustur() As Boolean
        If SerialComPort IsNot Nothing AndAlso SerialComPort.IsOpen Then


            SSPCommand.EncryptionStatus = False

            Dim keys As New SSP_KEYS
            InitiateSSPHostKeys(keys, SSPCommand)

            SSPCommand.CommandData(0) = CCommands.SSP_CMD_SET_GENERATOR
            SSPCommand.CommandDataLength = 9

            BitConverter.GetBytes(keys.Generator).CopyTo(SSPCommand.CommandData, 1)
            Log_Yazdir("Gönderilen Komut:SSP_CMD_SET_GENERATOR")
            If SSPSendCommand(SSPCommand) <> PORT_STATUS.Veri_Gonderme_Basarili Then Return False

            SSPCommand.CommandData(0) = CCommands.SSP_CMD_SET_MODULUS
            SSPCommand.CommandDataLength = 9

            BitConverter.GetBytes(keys.Modulus).CopyTo(SSPCommand.CommandData, 1)
            Log_Yazdir("Gönderilen Komut:SSP_CMD_SET_MODULUS")
            If SSPSendCommand(SSPCommand) <> PORT_STATUS.Veri_Gonderme_Basarili Then Return False

            SSPCommand.CommandData(0) = CCommands.SSP_CMD_REQUEST_KEY_EXCHANGE
            SSPCommand.CommandDataLength = 9
            BitConverter.GetBytes(keys.HostInter).CopyTo(SSPCommand.CommandData, 1)
            Log_Yazdir("Gönderilen Komut:SSP_CMD_REQUEST_KEY_EXCHANGE")
            If SSPSendCommand(SSPCommand) <> PORT_STATUS.Veri_Gonderme_Basarili Then Return False

            keys.SlaveInterKey = BitConverter.ToUInt64(SSPCommand.ResponseData, 1)
            CreateSSPHostEncryptionKey(keys)

            SSPCommand.Key.FixedKey = &H123456701234567UL
            SSPCommand.Key.VariableKey = keys.KeyHost
            SSPCommand.EncryptionStatus = True

            Log_Yazdir("Anahtar Eşleşme Başarılı!")
            Return True
        Else
            Log_Yazdir("Bağlantı Portu Kapalı.")
        End If

        SSPCommand.EncryptionStatus = False
        Return False
    End Function

    Function Kanallari_Ayarla(KanalList() As Byte) As Boolean


        ' Inhibitleri ayarla
        Timer_Durdur()

        Dim KanalSetByte As Byte = 0
        For i As Integer = 0 To KanalList.Length - 1
            KanalSetByte = CByte(KanalSetByte Or (KanalList(i) << i))
        Next

        SSPCommand.CommandData(0) = CCommands.SSP_CMD_SET_CHANNEL_INHIBITS
        SSPCommand.CommandData(1) = KanalSetByte
        SSPCommand.CommandData(2) = &HFF
        SSPCommand.CommandDataLength = 3
        Log_Yazdir("Kanallar Ayarlanıyor")

        If SSPSendCommand(SSPCommand) = PORT_STATUS.Veri_Gonderme_Basarili AndAlso Data_isleme_Basarili() Then
            Timer_Baslat()
            Log_Yazdir("Kanal Ayarlama Başarılı.")
            Return True
        End If

        Timer_Baslat()
        Log_Yazdir("Kanal Ayarlama Başarısız.")
        Return False

    End Function

    Function Kasa_Bosalt_Akilli() As Nakit_Para_Status

        Timer_Durdur()

        SSPCommand.CommandData(0) = CCommands.SSP_CMD_SMART_EMPTY
        SSPCommand.CommandDataLength = 1

        If SSPSendCommand(SSPCommand) = PORT_STATUS.Veri_Gonderme_Basarili AndAlso Data_isleme_Basarili() Then
            Timer_Baslat()
            Log_Yazdir("Akıllı Kaset Boşaltma İşlemi Başladı.")
            Return Nakit_Para_Status.Basarili
        End If


        Timer_Baslat()
        Log_Yazdir("Akıllı Kaset Boşaltma İşlemi Başarısız.")
        Return Nakit_Para_Status.Basarisiz

    End Function

    Function Kasa_Bosalt_Tamami() As Nakit_Para_Status


        Timer_Durdur()

        SSPCommand.CommandData(0) = CCommands.SSP_CMD_EMPTY_ALL
        SSPCommand.CommandDataLength = 1

        If SSPSendCommand(SSPCommand) = PORT_STATUS.Veri_Gonderme_Basarili AndAlso Data_isleme_Basarili() Then
            Timer_Baslat()
            Log_Yazdir("Tüm Kaset Kasaya Aktarma İşlemi Başladı.")
            Return Nakit_Para_Status.Basarili
        End If


        Timer_Baslat()
        Log_Yazdir("Tüm Kaset Kasaya Aktarma İşlemi Başarısız.")
        Return Nakit_Para_Status.Basarisiz
    End Function

    Async Function Yeniden_Baslat() As Task(Of Nakit_Para_Status)


        Timer_Durdur()

        SSPCommand.CommandData(0) = CCommands.SSP_CMD_RESET
        SSPCommand.CommandDataLength = 1

        If SSPSendCommand(SSPCommand) = PORT_STATUS.Veri_Gonderme_Basarili AndAlso Data_isleme_Basarili() Then
            Timer_Baslat()
            Log_Yazdir("Cihaz Yeniden Başlatma Başarılı.")

            System.Threading.Thread.Sleep(100)

            Baglantiyi_Kapat()
            Await Task.Delay(10000)
            Return Nakit_Para_Status.Basarili
        End If


        Timer_Baslat()
        Log_Yazdir("Cihaz Yeniden Başlatma Başarısız.")
        Return Nakit_Para_Status.Basarisiz
    End Function




    Function Limit_Belirle(Banknot As Integer, Adet As Byte, ParaBirimi As String) As Nakit_Para_Status

        Timer_Durdur()
        Dim Birim() As Char = ParaBirimi.ToCharArray
        If Birim.Count <> 3 Then
            Return Nakit_Para_Status.Basarisiz
        End If
        Banknot *= BanknotCarpanDeger

        SSPCommand.CommandData(0) = CCommands.SSP_CMD_SET_Limit_Belirle
        SSPCommand.CommandData(1) = 1
        SSPCommand.CommandData(2) = CByte(Adet And &HFF)          ' Low byte
        SSPCommand.CommandData(3) = CByte((Adet >> 8) And &HFF)   ' High byte
        SSPCommand.CommandData(4) = CByte(Banknot And &HFF)   ' Banknot değeri = 1000 (örnek: 10 TL)
        SSPCommand.CommandData(5) = CByte((Banknot >> 8) And &HFF)
        SSPCommand.CommandData(6) = CByte((Banknot >> 16) And &HFF)
        SSPCommand.CommandData(7) = CByte((Banknot >> 24) And &HFF)
        SSPCommand.CommandData(8) = CByte(AscW(ParaBirimi(0)))  ' Ülke kodu = "TRY"
        SSPCommand.CommandData(9) = CByte(AscW(ParaBirimi(1)))
        SSPCommand.CommandData(10) = CByte(AscW(ParaBirimi(2)))
        SSPCommand.CommandDataLength = 11 ' toplam kaç byte doluysa


        If SSPSendCommand(SSPCommand) = PORT_STATUS.Veri_Gonderme_Basarili AndAlso Data_isleme_Basarili() Then
            Timer_Baslat()
            Log_Yazdir("Kanal Limit Ayarlama Başarılı.")
            Return Nakit_Para_Status.Basarili
        End If



        Timer_Baslat()
        Log_Yazdir("Kanal Limit Ayarlama Başarısız.")
        Return Nakit_Para_Status.Basarisiz





    End Function




    Private Function Data_isleme_Basarili() As Boolean

        If SSPCommand.ResponseData(0) = CCommands.Veri_Alma_Basarili Then
            Return True
        Else

            Select Case SSPCommand.ResponseData(0)
                Case CCommands.SSP_RESPONSE_COMMAND_CANNOT_BE_PROCESSED

                    If SSPCommand.ResponseData(1) = &H3 Then
                        Log_Yazdir("Komut Yanıtı:Cihaz Meşgul.")
                    Else
                        Log_Yazdir("Komut Yanıtı:İşlenemeyen Komut:" & BitConverter.ToString(SSPCommand.ResponseData, 1, 1))
                    End If

                Case CCommands.SSP_RESPONSE_FAIL
                    Log_Yazdir("Komut Yanıtı:Komut Başarısız.")
                Case CCommands.SSP_RESPONSE_KEY_NOT_SET
                    Log_Yazdir("Komut Yanıtı:Anahtar Ayarlanmamış,Yeniden Görüş.")
                Case CCommands.SSP_RESPONSE_PARAMETER_OUT_OF_RANGE
                    Log_Yazdir("Komut Yanıtı:Parametre Aralık Dışı")
                Case CCommands.SSP_RESPONSE_SOFTWARE_ERROR
                    Log_Yazdir("Komut Yanıtı:Yazılımm Hatası")
                Case CCommands.SSP_RESPONSE_COMMAND_NOT_KNOWN
                    Log_Yazdir("Komut Yanıtı:Bilinmiyor")
                Case CCommands.SSP_RESPONSE_WRONG_NO_PARAMETERS
                    Log_Yazdir("Komut Yanıtı:Yanlış Parametre Sayısı")
            End Select
        End If




        Return False
    End Function



    Public Function InitiateSSPHostKeys(ByVal keys As SSP_KEYS, ByVal cmd As SSP_COMMAND) As Boolean
        Dim num As ULong
        keys.Generator = GeneratePrime()
        keys.Modulus = GeneratePrime()
        If keys.Generator < keys.Modulus Then
            num = keys.Generator
            keys.Generator = keys.Modulus
            keys.Modulus = num
        End If

        If Not CreateHostInterKey(keys) Then Return False
        cmd.encPktCount = 0
        Return True
    End Function

    Public Function CreateHostInterKey(ByVal keys As SSP_KEYS) As Boolean
        If keys.Generator = 0 OrElse keys.Modulus = 0 Then Return False
        keys.HostRandom = GenerateRandomNumber() Mod 2147483648UL
        keys.HostInter = XpowYmodN(keys.Generator, keys.HostRandom, keys.Modulus)
        Return True
    End Function

    Public Function CreateSSPHostEncryptionKey(ByVal keys As SSP_KEYS) As Boolean
        keys.KeyHost = XpowYmodN(keys.SlaveInterKey, keys.HostRandom, keys.Modulus)
        Return True
    End Function

    ' ----------------------------------------------------------------
    ' MATEMATİK FONKSİYONLARI (ORİJİNAL)
    ' ----------------------------------------------------------------
    Public Function GenerateRandomNumber() As ULong
        Dim tmp(7) As Byte
        RngCsp.GetBytes(tmp)
        Return BitConverter.ToUInt64(tmp, 0)
    End Function

    Public Function GeneratePrime() As ULong
        Dim tmp As ULong = GenerateRandomNumber() Mod &H7FFFFFFFUL

        If (tmp And 1UL) = 0UL Then
            tmp += 1UL
        End If

        While MillerRabin(tmp, 5US) = Primality.COMPOSITE
            tmp += 2UL
        End While

        Return tmp
    End Function


    Private Enum Primality
        COMPOSITE
        PSEUDOPRIME
    End Enum
    Private Function MillerRabin(n As ULong, trials As UShort) As Primality
        For i As UShort = 0US To CUShort(trials - 1US)
            Dim nm3 As ULong = n - 3UL
            Dim a As ULong = (GenerateRandomNumber() Mod nm3) + 2UL
            If SingleMillerRabin(n, a) = Primality.COMPOSITE Then
                Return Primality.COMPOSITE
            End If
        Next
        Return Primality.PSEUDOPRIME
    End Function

    Private Function SingleMillerRabin(n As ULong, a As ULong) As Primality
        Dim s As UShort = 0US
        Dim d As ULong = n - 1UL

        While (d And 1UL) = 0UL
            s = CUShort(s + 1US)
            d >>= 1
        End While

        If s = 0US Then Return Primality.COMPOSITE

        Dim x As ULong = XpowYmodN(a, d, n)
        If x = 1UL OrElse x = (n - 1UL) Then Return Primality.PSEUDOPRIME

        For r As UShort = 1US To CUShort(s - 1US)
            x = (x * x) Mod n
            If x = 1UL Then Return Primality.COMPOSITE
            If x = (n - 1UL) Then Return Primality.PSEUDOPRIME
        Next

        Return Primality.COMPOSITE
    End Function

    Public Function XpowYmodN(x As ULong, y As ULong, N As ULong) As ULong
        Dim rptsq As ULong = x
        Dim result As ULong = 1UL

        While y <> 0UL
            If (y And 1UL) <> 0UL Then
                result = (result * rptsq) Mod N
            End If
            rptsq = (rptsq * rptsq) Mod N
            y >>= 1
        End While

        Return result
    End Function



    Sub Log_Yazdir(Hata As String)


        If LogTextBox IsNot Nothing Then
            If LogTextBox.Text.Length > 2000 Then
                InvokeSync.Post(Sub() LogTextBox.ResetText(), Nothing)
            End If
            InvokeSync.Post(Sub() LogTextBox.AppendText(Hata & vbCrLf), Nothing)
        End If



    End Sub

End Class