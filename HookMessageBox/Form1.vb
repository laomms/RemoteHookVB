Imports System.IO

Public Class Form1
    Dim channelName As String = Nothing
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Dim targetPID = Process.GetProcessById(Integer.Parse(TextBox1.Text)).Id
            EasyHook.RemoteHooking.IpcCreateServer(Of ServerInterface)(channelName, System.Runtime.Remoting.WellKnownObjectMode.Singleton)

            Dim injectionLibrary As String = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "ClassLibrary1.dll")
            Try
                If targetPID > 0 Then
                    Debug.Print("Attempting to inject into process {0}", targetPID)
                    EasyHook.RemoteHooking.Inject(targetPID, injectionLibrary, injectionLibrary, channelName)
                End If
            Catch ex As Exception
                Debug.Print("There was an error while injecting into target:")
                Debug.Print(e.ToString())
            End Try
        Catch ex As Exception

        End Try

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
