using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Mod.commands;
using Mod.exceptions;
using Mod.manager;
using UnityEngine;

namespace Mod.gui
{
    public class GUIChat : Gui
    {
        private Vector3 _mScroll = new Vector3(0,0);
        public static readonly List<ChatMessage> Messages = new List<ChatMessage>();
        public static string Message = string.Empty;
        private readonly GUIStyle _inputStyle = new GUIStyle
        {
            normal = { background = Textures.GrayTexture, textColor = Color.black },
            border = new RectOffset(1, 1, 1, 1),
            padding = new RectOffset(0, 0, 0, 0),
            margin = new RectOffset(0, 0, 0, 0)
        };
        private bool _mWriting;

        public static void AddMessage(object msg, PhotonPlayer player, bool localOnly) => Messages.Insert(0, new ChatMessage(Chat.RemoveSize(msg.ToString()), player, localOnly));
        public static void AddMessage(object msg) => AddMessage(msg, null, true);
        public static void AddMessage(string msg, PhotonPlayer player) => AddMessage(msg, player, false);
        
        private static IEnumerable<Type> GetTypesWith<TAttribute>(bool inherit) where TAttribute : Attribute
        {
            return from a in AppDomain.CurrentDomain.GetAssemblies() from t in a.GetTypes() where t.IsDefined(typeof(TAttribute), inherit) select t;
        }

        public void OnGUI()
        {
            if (!PhotonNetwork.inRoom) return;
            Rect rect = GUIHelper.AlignRect(Screen.width / 3.5f, Screen.height / 4.2f, GUIHelper.Alignment.BOTTOMLEFT);
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter)
            {
                if (GUI.GetNameOfFocusedControl() == "ChatInput")
                {
                    if (Message.StartsWith("/") || Message.StartsWith("\\"))
                    {
                        Match match = Regex.Match(Message, @"[\\\/](\w+)(?:\s(.*))?.*");
                        //Command cmd = Core.CommandManager.commands.FirstOrDefault(x => x.Command.equalsIgnoreCase(match.Groups[1].Value));
                        //Command cmd = Core.CommandManager.Commands.FirstOrDefault(x => x.Commands.FirstOrDefault(command => command.equalsIgnoreCase(match.Groups[1].Value)) != null);
                        Command cmd = Core.CommandManager.FirstOrDefault(cmds => cmds.Commands.FirstOrDefault(x => x.EqualsIgnoreCase(match.Groups[1].Value)) != null);
                        

                        if (cmd == null)
                        {
                            Core.SendMessage("Command not found.");
                            Message = string.Empty;
                            GUI.FocusControl(string.Empty);
                            _mWriting = !_mWriting;
                            return;
                        }

                        var args = match.Groups[2].Value.Split(' ');
                        if (args[0].Equals(string.Empty)) args = new string[0];

                        try
                        {
                            Core.CommandManager.ExecuteCommand(cmd, PhotonNetwork.player, args);
                        }
                        catch (Exception e)
                        {
                            Core.SendMessage(Core.Lang.Get("message.commandexecutionerror.text", match.Groups[1].Value));
                            Core.Log(Core.Lang.Get("message.exeptionthrown.text", e.GetType().Name), ErrorType.Warning);
                        }

                    }
                    else if (Message != string.Empty)
                    {
                        if (Message.StartsWith("~") || Message.StartsWith("!") || Message.StartsWith("`") || Message.StartsWith("-"))
                            Core.SendPrivateChat(string.Format(RefStrings.ChatFormat, RefStrings.ChatName, RefStrings.ChatColor, GUIModMenu.EnableFade ? (GUIModMenu.UseFade2 ? Chat.Fade2(Message.Substring(1)) : Chat.Fade(Message.Substring(1))) : Message.Substring(1)));
                        else
                            Core.SendPublicMessage(string.Format(RefStrings.ChatFormat, RefStrings.ChatName, RefStrings.ChatColor, GUIModMenu.EnableFade ? (GUIModMenu.UseFade2 ? Chat.Fade2(Message) : Chat.Fade(Message)) : Message));
                    }

                    Message = string.Empty;
                    GUI.FocusControl(string.Empty);
                }
                _mScroll = new Vector3(0, 0);
                _mWriting = !_mWriting;
            }
            if (_mWriting)
            {
                GUI.DrawTexture(GUIHelper.screenRect, InterfaceManager.CreateTexture(0, 0, 0, 100));

                GUI.SetNextControlName(string.Empty);
                rect = rect.MoveY(-10 - _mScroll.y);
                foreach (ChatMessage chatMessage in Messages)
                {
                    rect = rect.MoveY(-15);
                    if (rect.y > Screen.height - 25) continue;
                    GUI.Label(rect, $"{(chatMessage.IsLocalOnly ? "" : $"[{chatMessage.GetSender.ID}] ")}{chatMessage.Message}", new GUIStyle {alignment = TextAnchor.LowerLeft, normal = {textColor = Color.white}});
                }

                GUI.DrawTexture(new Rect(2, Screen.height - 23, rect.width + 2, 17), Textures.WhiteTexture);
                GUI.SetNextControlName("ChatInput");
                Message = GUI.TextField(new Rect(3, Screen.height - 22, rect.width, 15), Message, _inputStyle);
                GUI.FocusControl("ChatInput");
                return;
            }

            GUI.SetNextControlName(string.Empty);
            rect = rect.MoveY(-5);
            foreach (ChatMessage chatMessage in Messages)
            {
                if (Time.time - 10f > chatMessage.GetTime)
                    chatMessage.visible = false;
                if (chatMessage.IsVisible)
                    GUI.Label(rect, $"{(chatMessage.IsLocalOnly ? "" : $"[{chatMessage.GetSender.ID}] ")}{chatMessage.Message}", new GUIStyle { alignment = TextAnchor.LowerLeft, normal = { textColor = Color.white } });
                rect = rect.MoveY(-15);
            }
            //while (Messages.Count > 30)
            //    Messages.RemoveAt(30);

        }

        public void Update()
        {
            if (!_mWriting) return;
            Vector3 vector = new Vector3(0, 15, 0);
            if (Input.mouseScrollDelta.y > 0)
                _mScroll += vector;
            else if (Input.mouseScrollDelta.y < 0)
                _mScroll -= vector;
        }
    }
}
