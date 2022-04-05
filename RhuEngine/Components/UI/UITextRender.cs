﻿using RhuEngine.WorldObjects;
using RhuEngine.WorldObjects.ECS;

using RNumerics;
using RhuEngine.Linker;
using System.Linq;
using System.Collections.Generic;
using System;

namespace RhuEngine.Components
{
	public class UITextRender
	{

		public SafeList<TextChar> Chars = new();

		public class TextChar
		{
			public string id;
			public char c;
			public Matrix p;
			public Colorf color;
			public RFont rFont;
			public FontStyle fontStyle;
			public Vector2f textCut;
			
			public TextChar(string id, char c, Matrix p, Colorf color, RFont rFont, FontStyle fontStyle, Vector2f textCut) {
				this.id = id;
				this.c = c;
				this.p = p;
				this.color = color;
				this.rFont = rFont;
				this.fontStyle = fontStyle;
				this.textCut = textCut;
			}

			public void Render(Matrix root) {
				RText.Add(id, c, p * root, color, rFont, fontStyle, textCut);
			}
		}

		public void Render(Matrix root) {
			Chars.SafeOperation((list) => {
				foreach (var item in list) {
					item.Render(root);
				}
			});
		}

		public void LoadText(string Id ,string Text, RFont Font, Colorf StartingColor, FontStyle StartingStyle = FontStyle.Regular,float StatingSize = 10f) {
			if(Font is null) {
				return;
			}
			Chars.SafeOperation((list) => list.Clear());
			var size = 0f;
			var style = new Stack<FontStyle>();
			style.Push(StartingStyle);
			var fontSize = new Stack<float>();
			fontSize.Push(StatingSize);
			var color = new Stack<Colorf>();
			color.Push(StartingColor);
			var index = 0;
			void RenderText(string text) {
				foreach (var item in text) {
					var textsize = RText.Size(Font, item,style.Peek());
					Chars.SafeAdd(new TextChar(Id + item + index.ToString(), item, Matrix.TRS(new Vector3f(size, 0, 0), Quaternionf.Yawed180, fontSize.Peek() / 100), color.Peek(), Font, style.Peek(), textsize));
					size += (textsize.x + 0.01f) * (fontSize.Peek() / 100);
					index++;
				}
			}
			void Loop(string segment) {
				var first = segment.IndexOf('<');
				if(first <= -1) {
					RenderText(segment);
					return;
				}
				var check = segment.IndexOf('<',first + 1);
				var last = segment.IndexOf('>',first);
				if (last <= -1) {
					RenderText(segment);
					return;
				}
				if(last > check) {
					RenderText(segment.Substring(first,check - first));
					return;
				}
				var command = segment.Substring(first, last - first).ToLower();
				var smartCommand = new string(command.ToCharArray().Where(c => !char.IsWhiteSpace(c)).ToArray());
				var haseq = smartCommand.Contains('=');
				if (smartCommand.StartsWith("<color")) {
					var data = smartCommand.Substring(6 + (haseq ? 1 : 0));
					color.Push(Colorf.Parse(data));
				}
				else if (smartCommand.StartsWith("<style")) {
					var data = smartCommand.Substring(6 + (haseq ? 1 : 0));
					if(Enum.TryParse(data,true,out FontStyle fontStyle)) {
						style.Push(fontStyle);
					}
				}
				else if (smartCommand.StartsWith("<size")) {
					var data = smartCommand.Substring(5 + (haseq ? 1 : 0));
					try {
						fontSize.Push(float.Parse(data));
					}
					catch { }
				}
				else if (smartCommand.StartsWith("</color") || smartCommand.StartsWith("<\\color")) {
					try {
						if (color.Count != 1) {
							color.Pop();
						}
					}
					catch { }
				}
				else if (smartCommand.StartsWith("</size") || smartCommand.StartsWith("<\\size")) {
					try {
						if (fontSize.Count != 1) {
							fontSize.Pop();
						}
					}
					catch { }
				}
				else if (smartCommand.StartsWith("</style") || smartCommand.StartsWith("<\\style")) {
					try {
						if (style.Count != 1) {
							style.Pop();
						}
					}
					catch { }
				}
				else {
					RenderText(command + segment.Substring(last,1));
				}
				var end = segment.IndexOf('<',last);
				if (end <= -1) {
					RenderText(segment.Substring(last+1));
					return;
				}
				RenderText(segment.Substring(last + 1, end - last - 1));
				Loop(segment.Substring(end));
			}
			Loop(Text);
		}
	}
}
