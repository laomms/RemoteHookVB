Imports System
Imports System.Linq
Imports System.Windows.Forms
Imports System.IO
Imports System.Threading
Imports System.Reflection
Imports ClassLibrary1
Imports EasyHook
Imports System.Diagnostics
Imports System.Runtime.InteropServices

Namespace WinFormEasyHook
	Partial Public Class Form1
		Inherits Form

		<DllImport("kernel32.dll", SetLastError := True, CallingConvention := CallingConvention.Winapi)>
		Friend Shared Function IsWow64Process(<[In]> ByVal process As IntPtr, <Out()> ByRef wow64Process As Boolean) As <MarshalAs(UnmanagedType.Bool)> Boolean
		End Function

		Public Sub New()
			InitializeComponent()
		End Sub

		Private Function RegGACAssembly() As Boolean
			Dim dllName = "EasyHook.dll"
			Dim dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dllName)
			If Not RuntimeEnvironment.FromGlobalAccessCache(System.Reflection.Assembly.LoadFrom(dllPath)) Then
				Call (New System.EnterpriseServices.Internal.Publish()).GacInstall(dllPath)
				Thread.Sleep(100)
			End If

			dllName = "ClassLibrary1.dll"
			dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dllName)
			Call (New System.EnterpriseServices.Internal.Publish()).GacRemove(dllPath)
			If Not RuntimeEnvironment.FromGlobalAccessCache(System.Reflection.Assembly.LoadFrom(dllPath)) Then
				Call (New System.EnterpriseServices.Internal.Publish()).GacInstall(dllPath)
				Thread.Sleep(100)
			End If

			Return True
		End Function

		Private Shared Function InstallHookInternal(ByVal processId As Integer) As Boolean
			Try
				Dim parameter = New HookParameter With {
					.Msg = "已经成功注入目标进程",
					.HostProcessId = RemoteHooking.GetCurrentProcessId()
				}

				RemoteHooking.Inject(processId, InjectionOptions.Default, GetType(HookParameter).Assembly.Location, GetType(HookParameter).Assembly.Location, String.Empty, parameter)
			Catch ex As Exception
				Debug.Print(ex.ToString())
				Return False
			End Try

			Return True
		End Function

		Private Shared Function IsWin64Emulator(ByVal processId As Integer) As Boolean
			Dim process = System.Diagnostics.Process.GetProcessById(processId)
			If process Is Nothing Then
				Return False
			End If

			If (Environment.OSVersion.Version.Major > 5) OrElse ((Environment.OSVersion.Version.Major = 5) AndAlso (Environment.OSVersion.Version.Minor >= 1)) Then
				Dim retVal As Boolean = Nothing

				Return Not (IsWow64Process(process.Handle, retVal) AndAlso retVal)
			End If

			Return False ' not on 64-bit Windows Emulator
		End Function

		Private Sub button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button1.Click
			Dim p = Process.GetProcessById(Integer.Parse(textBox1.Text))
			If p Is Nothing Then
				MessageBox.Show("指定的进程不存在!")
				Return
			End If

			If IsWin64Emulator(p.Id) <> IsWin64Emulator(Process.GetCurrentProcess().Id) Then
				Dim currentPlat = If(IsWin64Emulator(Process.GetCurrentProcess().Id), 64, 32)
				Dim targetPlat = If(IsWin64Emulator(p.Id), 64, 32)
				MessageBox.Show(String.Format("当前程序是{0}位程序，目标进程是{1}位程序，请调整编译选项重新编译后重试！", currentPlat, targetPlat))
				Return
			End If

			RegGACAssembly()
			InstallHookInternal(p.Id)
		End Sub

		Private Sub btnGet_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGet.Click
'INSTANT VB NOTE: The variable name was renamed since Visual Basic does not handle local variables named the same as class members well:
			Dim name_Conflict As String = txtProcessName.Text
			Dim process() As Process = System.Diagnostics.Process.GetProcessesByName(name_Conflict)
			If process.Length > 0 Then
				textBox1.Text = process.FirstOrDefault().Id.ToString()
			Else
				MessageBox.Show("该进程不存在！")
			End If
		End Sub
	End Class
End Namespace
