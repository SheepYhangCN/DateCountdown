using Godot;
using System;

public partial class Settings : Window
{
	static readonly Godot.Collections.Array<string> langs = new(){"en","zh_CN","zh_TW"};
	public override void _Ready()
	{
		Init();
	}
	public void _on_back_pressed()
	{
		var lang = langs[GetNode<OptionButton>("PanelContainer/CenterContainer/VBoxContainer/HSplitContainer/OptionButton").Selected];
		var cfg = new ConfigFile();
		GetNode<Main>("../").target_date = new DateTime(Mathf.RoundToInt(GetNode<SpinBox>("PanelContainer/CenterContainer/VBoxContainer/HBoxContainer2/Year").Value),Mathf.RoundToInt(GetNode<SpinBox>("PanelContainer/CenterContainer/VBoxContainer/HBoxContainer2/Month").Value),Mathf.RoundToInt(GetNode<SpinBox>("PanelContainer/CenterContainer/VBoxContainer/HBoxContainer2/Day").Value));
		GetNode<Main>("../").target_name = GetNode<LineEdit>("PanelContainer/CenterContainer/VBoxContainer/HSplitContainer2/LineEdit").Text;
		cfg.SetValue("Settings","Language",lang);
		cfg.SetValue("Settings","TargetName",GetNode<Main>("../").target_name);
		cfg.SetValue("Settings","TargetDate",GetNode<Main>("../").target_date.Ticks);
		cfg.Save("user://Settings.ini");
		TranslationServer.SetLocale(lang);
		GetTree().ReloadCurrentScene();
	}
	public void _on_option_button_window_item_selected(int selected)
	{
		if (selected == 0)
		{
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
		}
		else if (selected == 1)
		{
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
		}
	}

	public void _on_github_pressed()
	{
		OS.ShellOpen("https://github.com/SheepYhangCN/DateCountdown");
	}

	public void _on_close_requested()
	{
		Visible = false;
	}

	internal void Init()
	{
		GetNode<OptionButton>("PanelContainer/CenterContainer/VBoxContainer/HSplitContainer/OptionButton").Selected = langs.IndexOf(TranslationServer.GetLocale());
		var year = GetNode<SpinBox>("PanelContainer/CenterContainer/VBoxContainer/HBoxContainer2/Year");
		var month = GetNode<SpinBox>("PanelContainer/CenterContainer/VBoxContainer/HBoxContainer2/Month");
		var day = GetNode<SpinBox>("PanelContainer/CenterContainer/VBoxContainer/HBoxContainer2/Day");
		var datetime_max = new DateTime(DateTime.MaxValue.Ticks);
		var datetime_min = new DateTime(DateTime.MinValue.Ticks);
		year.MinValue = datetime_min.Year;
		year.MaxValue = datetime_max.Year;
		month.MinValue = datetime_min.Month;
		month.MaxValue = datetime_max.Month;
		day.MinValue = datetime_min.Day;
		day.MaxValue = datetime_max.Day;
		var target_date = GetNode<Main>("../").target_date;
		year.Value = target_date.Year;
		month.Value = target_date.Month;
		day.Value = target_date.Day;
		GetNode<LineEdit>("PanelContainer/CenterContainer/VBoxContainer/HSplitContainer2/LineEdit").Text = GetNode<Main>("../").target_name;
	}
}
