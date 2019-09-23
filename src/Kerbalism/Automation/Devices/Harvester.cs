﻿using System;
using System.Collections.Generic;
using UnityEngine;
using KSP.Localization;

namespace KERBALISM
{


	public sealed class HarvesterDevice : Device
	{
		public HarvesterDevice(Harvester harvester)
		{
			this.harvester = harvester;
			this.animator = harvester.part.FindModuleImplementing<ModuleAnimationGroup>();
		}

		public override string Name()
		{
			return Lib.BuildString(harvester.resource, " harvester").ToLower();
		}

		public override uint Part()
		{
			return harvester.part.flightID;
		}

		public override string Status()
		{
			return animator != null && !harvester.deployed
			  ? "not deployed"
			  : !harvester.running
			  ? Lib.Color(Localizer.Format("#KERBALISM_Generic_STOPPED"), Lib.KColor.Yellow)
			  : harvester.issue.Length == 0
			  ? Lib.Color(Localizer.Format("#KERBALISM_Generic_RUNNING"), Lib.KColor.Green)
			  : Lib.Color(harvester.issue, Lib.KColor.Red);
		}

		public override void Ctrl(bool value)
		{
			if (harvester.deployed)
			{
				harvester.running = value;
			}
		}

		public override void Toggle()
		{
			Ctrl(!harvester.running);
		}

		private Harvester harvester;
		private readonly ModuleAnimationGroup animator;
	}


	public sealed class ProtoHarvesterDevice : Device
	{
		public ProtoHarvesterDevice(ProtoPartModuleSnapshot harvester, Harvester prefab, uint part_id)
		{
			this.harvester = harvester;
			this.animator = FlightGlobals.FindProtoPartByID(part_id).FindModule("ModuleAnimationGroup");
			this.prefab = prefab;
			this.part_id = part_id;
		}

		public override string Name()
		{
			return Lib.BuildString(prefab.resource, " harvester").ToLower();
		}

		public override uint Part()
		{
			return part_id;
		}

		public override string Status()
		{
			bool deployed = Lib.Proto.GetBool(harvester, "deployed");
			bool running = Lib.Proto.GetBool(harvester, "running");
			string issue = Lib.Proto.GetString(harvester, "issue");

			return animator != null && !deployed
			  ? "not deployed"
			  : !running
			  ? Lib.Color(Localizer.Format("#KERBALISM_Generic_STOPPED"), Lib.KColor.Yellow)
			  : issue.Length == 0
			  ? Lib.Color(Localizer.Format("#KERBALISM_Generic_RUNNING"), Lib.KColor.Green)
			  : Lib.Color(issue, Lib.KColor.Red);
		}

		public override void Ctrl(bool value)
		{
			if (Lib.Proto.GetBool(harvester, "deployed"))
			{
				Lib.Proto.Set(harvester, "running", value);
			}
		}

		public override void Toggle()
		{
			Ctrl(!Lib.Proto.GetBool(harvester, "running"));
		}

		private readonly ProtoPartModuleSnapshot harvester;
		private readonly ProtoPartModuleSnapshot animator;
		private Harvester prefab;
		private readonly uint part_id;
	}


} // KERBALISM
