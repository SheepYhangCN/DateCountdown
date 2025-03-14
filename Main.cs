using Godot;
using System;

public partial class Main : Control
{
    [Export] internal string target_name = "Undefined";
    internal DateTime target_date = new DateTime(DateTime.MaxValue.Ticks);
	public override void _Ready()
	{
		if (OS.GetLocale() == "zh_TW" || OS.GetLocale() == "zh_HK" || OS.GetLocale() == "zh_MO")
		{
			TranslationServer.SetLocale("zh_TW");
		}
		else if (OS.GetLocaleLanguage() == "zh")
		{
			TranslationServer.SetLocale("zh_CN");
		}
		else
		{
			TranslationServer.SetLocale("en");
		}
		if (FileAccess.FileExists("user://Settings.ini"))
		{
			var cfg = new ConfigFile();
			cfg.Load("user://Settings.ini");
			TranslationServer.SetLocale(cfg.GetValue("Settings","Language","en").AsString());
			target_name = cfg.GetValue("Settings","TargetName","Undefined").AsString();
			target_date = new DateTime(cfg.GetValue("Settings","TargetDate",DateTime.MaxValue.Ticks).AsInt64());
		}
		if (!DisplayServer.WindowGetFlag(DisplayServer.WindowFlags.Borderless))
		{
			GetNode<Button>("SettingsButton").Visible = true;
			GetNode<Button>("CreditsButton").Visible = true;
			GetNode<CheckButton>("CheckButton").Visible = true;
		}
	}
    public override void _Process(double delta)
    {
		var day_left = (target_date - DateTime.Today).TotalDays;
		var container = GetNode<VBoxContainer>("PanelContainer/CenterContainer/VBoxContainer");
		if (DisplayServer.WindowGetFlag(DisplayServer.WindowFlags.Borderless))
		{
			DisplayServer.WindowSetMousePassthrough([container.Position,container.Position+(new Vector2(container.Size.X,0)),container.Position+container.Size,container.Position+(new Vector2(0,container.Size.Y))]);
		}
        GetNode<Label>("PanelContainer/CenterContainer/VBoxContainer/Label").Text = TranslationServer.Translate("locUntilDays").ToString().Replace("{TARGET}",target_name).Replace("{DAY}", day_left.ToString()).Replace("{dayS}", (day_left > 1 ? "s" : ""));
        if (Input.IsActionJustPressed("edit_mode"))
        {
			DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless,!DisplayServer.WindowGetFlag(DisplayServer.WindowFlags.Borderless));
			GetNode<Button>("SettingsButton").Visible = !DisplayServer.WindowGetFlag(DisplayServer.WindowFlags.Borderless);
			GetNode<Button>("CreditsButton").Visible = !DisplayServer.WindowGetFlag(DisplayServer.WindowFlags.Borderless);
			GetNode<CheckButton>("CheckButton").Visible = !DisplayServer.WindowGetFlag(DisplayServer.WindowFlags.Borderless);
			DisplayServer.WindowSetMousePassthrough([]);
        }
		if ((!DisplayServer.WindowGetFlag(DisplayServer.WindowFlags.Borderless)) || new Rect2(container.Position,container.Size).HasPoint(GetGlobalMousePosition()))
		{
			GetNode<Label>("PanelContainer/CenterContainer/VBoxContainer/Label2").Modulate = new Color(1,1,1,1);
		}
		else
		{
			GetNode<Label>("PanelContainer/CenterContainer/VBoxContainer/Label2").Modulate = new Color(1,1,1,0);
		}
    }

	public void _on_settings_button_pressed()
	{
        GetNode<Settings>("Settings").Visible = true;
		GetNode<Settings>("Settings").Init();
	}

	public void _on_credits_button_pressed()
	{
        GetNode<Window>("Credits").Visible = true;
	}

	public void _on_credits_close_requested()
	{
		GetNode<Window>("Credits").Visible = false;
	}

	public void _on_github_pressed()
	{
		OS.ShellOpen("https://github.com/SheepYhangCN/DateCountdown");
	}

	public void _on_check_button_toggled(bool toggled)
	{
		GetWindow().AlwaysOnTop = toggled;
	}
}
