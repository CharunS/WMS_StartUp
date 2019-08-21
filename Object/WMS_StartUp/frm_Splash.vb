#Region "Imports"

Imports WMSAppObjectFramwork.AppObject

#End Region

Public Class frm_Splash

#Region "SystemEvents"

    Private Sub Frm_Splash_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ShowWaitCursor()
        Try

            InitForm()

        Catch ex As Exception
            HandleExceptions(ex)
        Finally
            ShowDefaultCursor()
        End Try

    End Sub

    Private Sub Frm_Splash_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown

        ShowWaitCursor()
        Try
            Dim SourcePath As String = "C:\WMSDev\bin\AppSetting.xml"

            Dim Filename As String = System.IO.Path.GetFileName(SourcePath)

            If Not System.IO.File.Exists(SourcePath) Then
                If MessageBox.Show("File [C:\WMSDev\bin\AppSetting.xml] not found, Do you want to create? Y / N" _
                        , GetMSGCAP(MSGCAP.QUE), MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                    frm_ConnectionHelper.Show()
                Else
                    Me.Close()
                End If
            End If
        Catch ex As Exception
            HandleExceptions(ex)
        Finally
            ShowDefaultCursor()
        End Try

    End Sub

    Private Sub Frm_Splash_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed

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
        Me.lblStatus.Text = String.Format("{0} Please wait..", Version)

    End Sub

    Private Sub FormSetting()

        Me.Height = (ScreenHeight * 40) / 100
        Me.Width = (ScreenWidth * 70) / 100

        Dim r As Rectangle
        r = Screen.FromPoint(Me.Location).WorkingArea
        Dim x As Integer = r.Left + ((r.Width - Me.Width) \ 2)
        Dim y As Integer = r.Top + ((r.Height - Me.Height) \ 2)
        Me.pnlFooter.Height = (Me.Height * 10) / 100
        Me.Location = New Point(x, y)

    End Sub

#End Region

End Class