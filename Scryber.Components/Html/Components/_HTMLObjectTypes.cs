﻿using System;
namespace Scryber.Html.Components
{
	public static class HTMLObjectTypes
	{
		public static readonly ObjectType Abbreviation = (ObjectType)"hAbr";
		public static readonly ObjectType Address = (ObjectType)"hAdd";
		public static readonly ObjectType Anchor = ObjectTypes.Link;
		public static readonly ObjectType Article = (ObjectType)"hArt";
		public static readonly ObjectType BlockQuote = (ObjectType)"hBlQ";
		public static readonly ObjectType Body = (ObjectType)"hBdy";
		public static readonly ObjectType Bold = ObjectTypes.BoldSpan;
		public static readonly ObjectType Big = (ObjectType)"hBig";
		public static readonly ObjectType Small = (ObjectType)"hSml";
		public static readonly ObjectType SuperScript = (ObjectType)"hSup";
		public static readonly ObjectType SubScript = (ObjectType)"hSub";
		public static readonly ObjectType Strong = (ObjectType)"hStg";
		public static readonly ObjectType Cite = (ObjectType)"hCte";
        public static readonly ObjectType Output = (ObjectType)"hOut";
        public static readonly ObjectType Slot = (ObjectType)"hSlt";
        public static readonly ObjectType Var = (ObjectType)"hVar";
        public static readonly ObjectType Code = (ObjectType)"hCde";
		public static readonly ObjectType Sample = (ObjectType)"hSam";
        public static readonly ObjectType Keyboard = (ObjectType)"hKbd";
        public static readonly ObjectType Definition = (ObjectType)"hDef";
		public static readonly ObjectType Div = ObjectTypes.Div;
		public static readonly ObjectType Document = (ObjectType)"Html";
		public static readonly ObjectType Embed = (ObjectType)"hEmd";
		public static readonly ObjectType FieldSet = (ObjectType)"hFst";
		public static readonly ObjectType Figure = (ObjectType)"hFig";
		public static readonly ObjectType FigureCaption = (ObjectType)"hFCp";
		public static readonly ObjectType FontSpan = (ObjectType)"hFnt";
		public static readonly ObjectType Fragment = (ObjectType)"h_Ft";
		public static readonly ObjectType Head = ObjectTypes.NoOp;
		public static readonly ObjectType Header = (ObjectType)"hHdr";
		public static readonly ObjectType Footer = (ObjectType)"hFtr";
		public static readonly ObjectType Head1 = ObjectTypes.H1;
        public static readonly ObjectType Head2 = ObjectTypes.H2;
        public static readonly ObjectType Head3 = ObjectTypes.H3;
        public static readonly ObjectType Head4 = ObjectTypes.H4;
        public static readonly ObjectType Head5 = ObjectTypes.H5;
        public static readonly ObjectType Head6 = ObjectTypes.H6;
		public static readonly ObjectType HRule = (ObjectType)"hHrl";
		public static readonly ObjectType DataIf = ObjectTypes.NoOp;
		public static readonly ObjectType IFrame = (ObjectType)"hIfr";
		public static readonly ObjectType Image = ObjectTypes.Image;
		public static readonly ObjectType Picture = (ObjectType)"hPic";
		public static readonly ObjectType PictureSource = (ObjectType)"hPsc";
		public static readonly ObjectType FormInput = (ObjectType)"hInp";
		public static readonly ObjectType Italic = ObjectTypes.ItalicSpan;
		public static readonly ObjectType Emphasis = (ObjectType)"hEmp";
		public static readonly ObjectType Label = ObjectTypes.Label;
		public static readonly ObjectType Legend = (ObjectType)"hLgd";
		public static readonly ObjectType LineBreak = ObjectTypes.LineBreak;
		public static readonly ObjectType Link = (ObjectType)"hLnk";
		public static readonly ObjectType ListDefinition = ObjectTypes.DefinitionList;
		public static readonly ObjectType ListDefnItem = ObjectTypes.DefinitionListItem;
		public static readonly ObjectType ListDefnTerm = ObjectTypes.DefinitionListTerm;
		public static readonly ObjectType ListItem = ObjectTypes.ListItem;
		public static readonly ObjectType ListOrdered = ObjectTypes.OrderedList;
		public static readonly ObjectType ListUnOrdered = ObjectTypes.UnorderedList;
		public static readonly ObjectType ListMenu = (ObjectType)"hMnu";
		public static readonly ObjectType Main = (ObjectType)"hMan";
		public static readonly ObjectType Marked = (ObjectType)"hMrk";
		public static readonly ObjectType Meta = (ObjectType)"hMta";
		public static readonly ObjectType Nav = (ObjectType)"hNav";
		public static readonly ObjectType Number = ObjectTypes.NumberComp;
		public static readonly ObjectType PageNumber = (ObjectType)"hPgn";
		public static readonly ObjectType Paragraph = ObjectTypes.Paragraph;
		public static readonly ObjectType Preformatted = ObjectTypes.Preformatted;
		public static readonly ObjectType Section = ObjectTypes.Section;
		public static readonly ObjectType Span = ObjectTypes.Span;
		public static readonly ObjectType Strike = (ObjectType)"hSrk";
		public static readonly ObjectType Delete = (ObjectType)"hDel";
		public static readonly ObjectType Quoted = (ObjectType)"hQsp";
		public static readonly ObjectType StyleTag = (ObjectType)"hSty";
		public static readonly ObjectType TableCell = ObjectTypes.TableCell;
		public static readonly ObjectType TableHeaderCell = ObjectTypes.TableHeaderCell;
		public static readonly ObjectType TableFooterCell = ObjectTypes.TableFooterCell;
		public static readonly ObjectType TableRow = ObjectTypes.TableRow;
		public static readonly ObjectType Table = ObjectTypes.Table;
		public static readonly ObjectType TableHead = (ObjectType)"hThd";
		public static readonly ObjectType TableBody = (ObjectType)"hTby";
		public static readonly ObjectType TableFoot = (ObjectType)"hTft";
		public static readonly ObjectType Template = ObjectTypes.NoOp;
		public static readonly ObjectType Time = ObjectTypes.DateComp;
		public static readonly ObjectType Underline = ObjectTypes.UnderlineSpan;
		public static readonly ObjectType Insert = (ObjectType)"hIns";
		public static readonly ObjectType Progress = (ObjectType)"hPro";
		public static readonly ObjectType Meter = (ObjectType)"hMtr";

		public static readonly ObjectType Details = (ObjectType)"hDet";
		public static readonly ObjectType DetailsSummary = (ObjectType)"hDSm";

		public static readonly ObjectType Object = (ObjectType)"hObj";
    }
}

