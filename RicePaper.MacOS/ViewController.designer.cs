// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace RicePaper.MacOS
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSButton ButtonRefDefinition { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefEnglishSentence { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefFurigana { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefImagePicker { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefJapaneseSentence { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefKanji { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefPosCB { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefPosCM { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefPosCT { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefPosLB { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefPosLM { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefPosLT { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefPosRB { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefPosRM { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefPosRT { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefRadioInOrder { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefRadioRandom { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefRomaji { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefWordListPicker { get; set; }

		[Outlet]
		AppKit.NSPopUpButton DropdownRefImageList { get; set; }

		[Outlet]
		AppKit.NSPopUpButton DropdownRefWordList { get; set; }

		[Outlet]
		AppKit.NSTextField FieldRefImageInterval { get; set; }

		[Outlet]
		AppKit.NSTextField FieldRefWordInterval { get; set; }

		[Outlet]
		AppKit.NSTextField LabelImagePath { get; set; }

		[Outlet]
		AppKit.NSTextField LabelWordListPath { get; set; }

		[Outlet]
		AppKit.NSStepper StepperRefImageInterval { get; set; }

		[Outlet]
		AppKit.NSStepper StepperRefWordInterval { get; set; }

		[Action ("ButtonImageDialog:")]
		partial void ButtonImageDialog (AppKit.NSButton sender);

		[Action ("ButtonNextWord:")]
		partial void ButtonNextWord (Foundation.NSObject sender);

		[Action ("ButtonPositionCB:")]
		partial void ButtonPositionCB (Foundation.NSObject sender);

		[Action ("ButtonPositionCM:")]
		partial void ButtonPositionCM (Foundation.NSObject sender);

		[Action ("ButtonPositionCT:")]
		partial void ButtonPositionCT (Foundation.NSObject sender);

		[Action ("ButtonPositionLB:")]
		partial void ButtonPositionLB (Foundation.NSObject sender);

		[Action ("ButtonPositionLM:")]
		partial void ButtonPositionLM (Foundation.NSObject sender);

		[Action ("ButtonPositionLT:")]
		partial void ButtonPositionLT (Foundation.NSObject sender);

		[Action ("ButtonPositionRB:")]
		partial void ButtonPositionRB (Foundation.NSObject sender);

		[Action ("ButtonPositionRM:")]
		partial void ButtonPositionRM (Foundation.NSObject sender);

		[Action ("ButtonPositionRT:")]
		partial void ButtonPositionRT (Foundation.NSObject sender);

		[Action ("ButtonWordListDialog:")]
		partial void ButtonWordListDialog (Foundation.NSObject sender);

		[Action ("CheckboxDefinition:")]
		partial void CheckboxDefinition (Foundation.NSObject sender);

		[Action ("CheckboxEnglishSentence:")]
		partial void CheckboxEnglishSentence (Foundation.NSObject sender);

		[Action ("CheckboxFurigana:")]
		partial void CheckboxFurigana (Foundation.NSObject sender);

		[Action ("CheckboxJapaneseSentence:")]
		partial void CheckboxJapaneseSentence (Foundation.NSObject sender);

		[Action ("CheckboxKanji:")]
		partial void CheckboxKanji (Foundation.NSObject sender);

		[Action ("CheckboxRomaji:")]
		partial void CheckboxRomaji (Foundation.NSObject sender);

		[Action ("DropdownDictionary:")]
		partial void DropdownDictionary (Foundation.NSObject sender);

		[Action ("DropdownImageIntervalUnit:")]
		partial void DropdownImageIntervalUnit (Foundation.NSObject sender);

		[Action ("DropdownImageList:")]
		partial void DropdownImageList (Foundation.NSObject sender);

		[Action ("DropdownWordIntervalUnit:")]
		partial void DropdownWordIntervalUnit (Foundation.NSObject sender);

		[Action ("DropdownWordList:")]
		partial void DropdownWordList (Foundation.NSObject sender);

		[Action ("FieldImageInterval:")]
		partial void FieldImageInterval (Foundation.NSObject sender);

		[Action ("FieldWordInterval:")]
		partial void FieldWordInterval (Foundation.NSObject sender);

		[Action ("RadioIterateInOrder:")]
		partial void RadioIterateInOrder (Foundation.NSObject sender);

		[Action ("RadioIterateRandom:")]
		partial void RadioIterateRandom (Foundation.NSObject sender);

		[Action ("StepperImageInterval:")]
		partial void StepperImageInterval (Foundation.NSObject sender);

		[Action ("StepperWordInterval:")]
		partial void StepperWordInterval (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ButtonRefKanji != null) {
				ButtonRefKanji.Dispose ();
				ButtonRefKanji = null;
			}

			if (ButtonRefFurigana != null) {
				ButtonRefFurigana.Dispose ();
				ButtonRefFurigana = null;
			}

			if (ButtonRefRomaji != null) {
				ButtonRefRomaji.Dispose ();
				ButtonRefRomaji = null;
			}

			if (ButtonRefDefinition != null) {
				ButtonRefDefinition.Dispose ();
				ButtonRefDefinition = null;
			}

			if (ButtonRefEnglishSentence != null) {
				ButtonRefEnglishSentence.Dispose ();
				ButtonRefEnglishSentence = null;
			}

			if (ButtonRefJapaneseSentence != null) {
				ButtonRefJapaneseSentence.Dispose ();
				ButtonRefJapaneseSentence = null;
			}

			if (ButtonRefImagePicker != null) {
				ButtonRefImagePicker.Dispose ();
				ButtonRefImagePicker = null;
			}

			if (ButtonRefPosCB != null) {
				ButtonRefPosCB.Dispose ();
				ButtonRefPosCB = null;
			}

			if (ButtonRefPosCM != null) {
				ButtonRefPosCM.Dispose ();
				ButtonRefPosCM = null;
			}

			if (ButtonRefPosCT != null) {
				ButtonRefPosCT.Dispose ();
				ButtonRefPosCT = null;
			}

			if (ButtonRefPosLB != null) {
				ButtonRefPosLB.Dispose ();
				ButtonRefPosLB = null;
			}

			if (ButtonRefPosLM != null) {
				ButtonRefPosLM.Dispose ();
				ButtonRefPosLM = null;
			}

			if (ButtonRefPosLT != null) {
				ButtonRefPosLT.Dispose ();
				ButtonRefPosLT = null;
			}

			if (ButtonRefPosRB != null) {
				ButtonRefPosRB.Dispose ();
				ButtonRefPosRB = null;
			}

			if (ButtonRefPosRM != null) {
				ButtonRefPosRM.Dispose ();
				ButtonRefPosRM = null;
			}

			if (ButtonRefPosRT != null) {
				ButtonRefPosRT.Dispose ();
				ButtonRefPosRT = null;
			}

			if (ButtonRefRadioInOrder != null) {
				ButtonRefRadioInOrder.Dispose ();
				ButtonRefRadioInOrder = null;
			}

			if (ButtonRefRadioRandom != null) {
				ButtonRefRadioRandom.Dispose ();
				ButtonRefRadioRandom = null;
			}

			if (ButtonRefWordListPicker != null) {
				ButtonRefWordListPicker.Dispose ();
				ButtonRefWordListPicker = null;
			}

			if (DropdownRefImageList != null) {
				DropdownRefImageList.Dispose ();
				DropdownRefImageList = null;
			}

			if (DropdownRefWordList != null) {
				DropdownRefWordList.Dispose ();
				DropdownRefWordList = null;
			}

			if (FieldRefImageInterval != null) {
				FieldRefImageInterval.Dispose ();
				FieldRefImageInterval = null;
			}

			if (FieldRefWordInterval != null) {
				FieldRefWordInterval.Dispose ();
				FieldRefWordInterval = null;
			}

			if (LabelImagePath != null) {
				LabelImagePath.Dispose ();
				LabelImagePath = null;
			}

			if (LabelWordListPath != null) {
				LabelWordListPath.Dispose ();
				LabelWordListPath = null;
			}

			if (StepperRefImageInterval != null) {
				StepperRefImageInterval.Dispose ();
				StepperRefImageInterval = null;
			}

			if (StepperRefWordInterval != null) {
				StepperRefWordInterval.Dispose ();
				StepperRefWordInterval = null;
			}
		}
	}
}
