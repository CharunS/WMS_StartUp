#Region "Improts"

Imports WMSAppObjectFramwork.AppObject
Imports System.IO
Imports System.Text
Imports System.Security.Cryptography

#End Region


Public Class frm_ConnectionHelper

#Region "Global Variable"

    Private enc As System.Text.UTF8Encoding
    Private encryptor As ICryptoTransform
    Private decryptor As ICryptoTransform

#End Region

#Region "System Events"

    Private Sub Frm_ConnectionHelper_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ShowWaitCursor()
        Try

            InitForm()

        Catch ex As Exception
            HandleExceptions(ex)
        Finally
            ShowDefaultCursor()
        End Try

    End Sub

    Private Sub Frm_ConnectionHelper_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing

        ShowWaitCursor()
        Try

            If MessageBox.Show("Do you want to exit program? Y / N", GetMSGCAP(MSGCAP.QUE) _
                               , MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then
                e.Cancel = True
            End If

        Catch ex As Exception
            HandleExceptions(ex)
        Finally
            ShowDefaultCursor()
        End Try

    End Sub

    Private Sub Frm_ConnectionHelper_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed

        ShowWaitCursor()
        Try

            Application.Exit()

        Catch ex As Exception
            HandleExceptions(ex)
        Finally
            ShowDefaultCursor()
        End Try

    End Sub

#End Region

#Region "Method"

    Private Sub InitForm()

        FormSetting()
        Me.Text = Version
        Dim KEY_128 As Byte() = {42, 1, 52, 67, 231, 13, 94, 101, 123, 6, 0, 12, 32, 91, 4, 111, 31, 70, 21, 141, 123, 142, 234, 82, 95, 129, 187, 162, 12, 55, 98, 23}
        Dim IV_128 As Byte() = {234, 12, 52, 44, 214, 222, 200, 109, 2, 98, 45, 76, 88, 53, 23, 78}
        Dim symmetricKey As RijndaelManaged = New RijndaelManaged()
        symmetricKey.Mode = CipherMode.CBC

        Me.enc = New System.Text.UTF8Encoding
        Me.encryptor = symmetricKey.CreateEncryptor(KEY_128, IV_128)
        Me.decryptor = symmetricKey.CreateDecryptor(KEY_128, IV_128)

    End Sub

    Private Sub FormSetting()

        Me.Height = (ScreenHeight * 25) / 100
        Me.Width = (ScreenWidth * 60) / 100

        Dim r As Rectangle
        r = Screen.FromPoint(Me.Location).WorkingArea
        Dim x As Integer = r.Left + ((r.Width - Me.Width) \ 2)
        Dim y As Integer = r.Top + ((r.Height - Me.Height) \ 2)
        Me.Location = New Point(x, y)

    End Sub

    Private Sub HandleButton(sender As Object, e As EventArgs) Handles btnTestConn.Click, btnCreate.Click

        ShowWaitCursor()
        Try

            If Not ValidateData() Then
                Exit Sub
            End If

            Dim ctrl As Control = CType(sender, Control)

            Select Case ctrl.Name

                Case btnTestConn.Name
                    TestConnection()

                Case btnCreate.Name
                    CreateAppsetting()

            End Select

        Catch ex As Exception
            HandleExceptions(ex)
        Finally
            ShowDefaultCursor()
        End Try

    End Sub

    Private Sub TestConnection()


    End Sub

    Private Sub CreateAppsetting()

        Dim fs As FileStream = File.Create("C:\WMSDev\bin\AppSetting.xml")
        Dim xmlStr As String
        'xmlStr = String.Format("<?xml version={0}1.0{0} encoding={0}utf-8{0}?>{1}" +
        '                        "<AppSetting>{1}" +
        '                            "<Server>{2}</Server>{1}" +
        '                            "<DataBase>{3}</DataBase>{1}" +
        '                            "<User>{4}</User>{1}" +
        '                            "<Password>{5}</Password>{1}" +
        '                        "</AppSetting>" _
        '                        , """", vbLf, txtServerName.Text, txtDataBase.Text _
        '                        , txtUserName.Text, Encrypt(txtPassword.Text))

        xmlStr = String.Format("<?xml version={0}1.0{0} encoding={0}utf-8{0}?>{1}" +
                                "<AppSetting>{1}" +
                                    "<Server>{2}</Server>{1}" +
                                    "<DataBase>{3}</DataBase>{1}" +
                                    "<User>{4}</User>{1}" +
                                    "<Password>{5}</Password>{1}" +
                                "</AppSetting>" _
                                , """", vbLf, txtServerName.Text, txtDataBase.Text _
                                , txtUserName.Text, txtPassword.Text)

        Dim info As Byte() = New UTF8Encoding(True).GetBytes(Encrypt(xmlStr))
        fs.Write(info, 0, info.Length)
        fs.Close()

    End Sub

    Private Function ValidateData() As Boolean
        Dim msgDes As String = "Please valid you data."

        Select Case String.Empty
            Case txtServerName.Text
                MessageBox.Show(msgDes, GetMSGCAP(MSGCAP.WRN), MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtServerName.Select()
                Return False
            Case txtDataBase.Text
                MessageBox.Show(msgDes, GetMSGCAP(MSGCAP.WRN), MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtDataBase.Select()
                Return False
            Case txtUserName.Text
                MessageBox.Show(msgDes, GetMSGCAP(MSGCAP.WRN), MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtUserName.Select()
                Return False
            Case txtPassword.Text
                MessageBox.Show(msgDes, GetMSGCAP(MSGCAP.WRN), MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtPassword.Select()
                Return False
        End Select

        Return True

    End Function

    Private Function Encrypt(ByVal plaintxt As String) As String

        Dim memoryStream As MemoryStream = New MemoryStream()
        Dim cryptoStream As CryptoStream = New CryptoStream(memoryStream, Me.encryptor, CryptoStreamMode.Write)
        cryptoStream.Write(Me.enc.GetBytes(plaintxt), 0, plaintxt.Length)
        cryptoStream.FlushFinalBlock()
        memoryStream.Close()
        cryptoStream.Close()

        Return Convert.ToBase64String(MemoryStream.ToArray())

    End Function

    Private Function Decrypt(ByVal enctxt As String) As String

        'Dim cypherTextBytes As Byte() = Convert.FromBase64String(Me.TextBox1.Text)
        'Dim memoryStream As MemoryStream = New MemoryStream(cypherTextBytes)
        'Dim cryptoStream As CryptoStream = New CryptoStream(memoryStream, Me.decryptor, CryptoStreamMode.Read)
        'Dim plainTextBytes(cypherTextBytes.Length) As Byte
        'Dim decryptedByteCount As Integer = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length)
        'memoryStream.Close()
        'cryptoStream.Close()
        'Me.TextBox1.Text = Me.enc.GetString(plainTextBytes, 0, decryptedByteCount)

        Return Nothing

    End Function


#End Region

End Class