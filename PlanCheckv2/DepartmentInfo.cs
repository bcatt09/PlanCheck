using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PlanCheck
{
	public static class DepartmentInfo
	{
		/// <summary>
		/// Gets all machine IDs from a department
		/// </summary>
		/// <param name="dep"></param>
		/// <returns></returns>
		public static List<string> GetMachineIDs(Department dep)
		{
			return Departments[dep].Machines;
		}

		/// <summary>
		/// Gets all CT IDs from a department
		/// </summary>
		/// <param name="dep"></param>
		/// <returns></returns>
		public static List<string> GetCTIDs(Department dep)
		{
			return Departments[dep].CTs;
		}

		/// <summary>
		/// Gets all rad onc user names from a department
		/// </summary>
		/// <param name="dep"></param>
		/// <returns></returns>
		public static List<string> GetRadOncUserNames(Department dep)
		{
			return Departments[dep].RadOncUserNames;
		}

		/// <summary>
		/// Gets the department that the machine is in.
		/// Returns Department.None if the machine ID cannot be found
		/// </summary>
		/// <param name="machineID"></param>
		/// <returns></returns>
		public static Department GetDepartment(string machineID)
		{
			try
			{
				return Departments.Where(x => x.Value.Machines.Contains(machineID)).Select(x => x.Key).First();
			}
			catch
			{
				MessageBox.Show($"Could not find a corresponding department for machine: {machineID}\nPlease ensure that it has been added to the department list", "Unknown Machine", MessageBoxButton.OK, MessageBoxImage.Error);

				return Department.None;
			}
		}

		/// <summary>
		/// Access to all department specific IDs
		/// </summary>
		private static Dictionary<Department, DepartmentInfoStruct> Departments = new Dictionary<Department, DepartmentInfoStruct>()
		{
			{ Department.BAY,
				new DepartmentInfoStruct {
					Machines = new List<string> { MachineNames.BAY_TB },
					CTs = new List<string> { CTNames.BAY },
					RadOncUserNames = RadOncUserNames.BAY
				}
			},
			{ Department.CEN,
				new DepartmentInfoStruct {
					Machines = new List<string> { MachineNames.CEN_EX },
					CTs = new List<string> { CTNames.CEN },
					RadOncUserNames = RadOncUserNames.CEN
				}
			},
			{ Department.CLA,
				new DepartmentInfoStruct {
					Machines = new List<string> { MachineNames.CLA_TB },
					CTs = new List<string> { CTNames.CLA },
					RadOncUserNames = RadOncUserNames.CLA
				}
			},
			{ Department.DET,
				new DepartmentInfoStruct {
					Machines = new List<string> { MachineNames.DET_IX, MachineNames.DET_TB },
					CTs = new List<string> { CTNames.DET },
					RadOncUserNames = RadOncUserNames.DET
				}
			},
			{ Department.FAR,
				new DepartmentInfoStruct {
					Machines = new List<string> { MachineNames.FAR_IX },
					CTs = new List<string> { CTNames.FAR },
					RadOncUserNames = RadOncUserNames.FAR
				}
			},
			{ Department.FLT,
				new DepartmentInfoStruct {
					Machines = new List<string> { MachineNames.FLT_FrontTB, MachineNames.FLT_BackTB },
					CTs = new List<string> { CTNames.FLT },
					RadOncUserNames = RadOncUserNames.FLT
				}
			},
			{ Department.LAN,
				new DepartmentInfoStruct {
					Machines = new List<string> { MachineNames.LAN_IX },
					CTs = new List<string> { CTNames.LAN },
					RadOncUserNames = RadOncUserNames.LAN
				}
			},
			{ Department.LAP,
				new DepartmentInfoStruct {
					Machines = new List<string> { MachineNames.LAP_IX },
					CTs = new List<string> { CTNames.LAP },
					RadOncUserNames = RadOncUserNames.LAP
				}
			},
			{ Department.MAC,
				new DepartmentInfoStruct {
					Machines = new List<string> { MachineNames.MAC_IX, MachineNames.MAC_TB },
					CTs = new List<string> { CTNames.MAC },
					RadOncUserNames = RadOncUserNames.MAC
				}
			},
			{ Department.MPH,
				new DepartmentInfoStruct {
					Machines = new List<string> { MachineNames.MPH_TB },
					CTs = new List<string> { CTNames.MPH },
					RadOncUserNames = RadOncUserNames.MPH
				}
			},
			{ Department.NOR,
				new DepartmentInfoStruct {
					Machines = new List<string> { MachineNames.NOR_TB, MachineNames.NOR_IX },
					CTs = new List<string> { CTNames.NOR },
					RadOncUserNames = RadOncUserNames.NOR
				}
			},
			{ Department.OWO,
				new DepartmentInfoStruct {
					Machines = new List<string> { MachineNames.OWO_IX },
					CTs = new List<string> { CTNames.OWO },
					RadOncUserNames = RadOncUserNames.OWO
				}
			},
			{ Department.PRO,
				new DepartmentInfoStruct {
					Machines = new List<string> { MachineNames.PRO_G1, MachineNames.PRO_G2 },
					CTs = new List<string> { CTNames.PRO },
					RadOncUserNames = RadOncUserNames.PRO
				}
			},
		};

		/// <summary>
		/// Dictionary of machine names in Aria
		/// </summary>
		public static class MachineNames
		{
			public static readonly string BAY_TB = "BAY_TB3384";
			public static readonly string CEN_EX = "CMCH-21EX";
			public static readonly string CLA_EX = "21EX";
			public static readonly string CLA_TB = "CLK_TB5190";
			public static readonly string DET_IX = "IX_GROC";
			public static readonly string DET_TB = "GROC_TB1601";
			public static readonly string FAR_IX = "IX_Farmington";
			public static readonly string FLT_FrontTB = "TrueBeamSN2873";
			public static readonly string FLT_BackTB = "TrueBeam1030";
			public static readonly string PRO_G1 = "Proton GR1";
			public static readonly string PRO_G2 = "Proton GR2";
			public static readonly string LAN_IX = "ING21IX1";
			public static readonly string LAP_IX = "21IX-SN3743";
			public static readonly string MAC_TB = "MAC_TB3568";
			public static readonly string MAC_IX = "TRILOGY3789";
			public static readonly string MPH_TB = "TB2681";
			public static readonly string NOR_EX = "2100ex";
			public static readonly string NOR_IX = "TRILOGY";
			public static readonly string NOR_TB = "NOR_TB4780";
			public static readonly string OWO_IX = "21IX-SN3856";
		}

		public static List<string> LinearAccelerators = new List<string>
		{
			MachineNames.BAY_TB,
			MachineNames.CEN_EX,
			MachineNames.CLA_EX,
			MachineNames.CLA_TB,
			MachineNames.DET_IX,
			MachineNames.DET_TB,
			MachineNames.FAR_IX,
			MachineNames.FLT_BackTB,
			MachineNames.FLT_FrontTB,
			MachineNames.LAN_IX,
			MachineNames.LAP_IX,
			MachineNames.MAC_IX,
			MachineNames.MAC_TB,
			MachineNames.MPH_TB,
			MachineNames.NOR_EX,
			MachineNames.NOR_IX,
			MachineNames.NOR_TB,
			MachineNames.OWO_IX
		};

		public static List<string> TrueBeams = new List<string>
		{
			MachineNames.BAY_TB,
			MachineNames.CLA_TB,
			MachineNames.DET_TB,
			MachineNames.FLT_BackTB,
			MachineNames.FLT_FrontTB,
			MachineNames.MAC_TB,
			MachineNames.MPH_TB,
			MachineNames.NOR_TB
		};

		public static List<string> Clinacs = new List<string>
		{
			MachineNames.CEN_EX,
			MachineNames.CLA_EX,
			MachineNames.DET_IX,
			MachineNames.FAR_IX,
			MachineNames.LAN_IX,
			MachineNames.LAP_IX,
			MachineNames.MAC_IX,
			MachineNames.NOR_EX,
			MachineNames.NOR_IX,
			MachineNames.OWO_IX
		};

		public static List<string> ProtonGantries = new List<string>
		{
			MachineNames.PRO_G1,
			MachineNames.PRO_G2
		};

		/// <summary>
		/// Dictionary of CT names in Aria
		/// </summary>
		private static class CTNames
		{
			public static readonly string BAY = "CT99";
			public static readonly string CEN = "cmchctcon";
			public static readonly string CLA = "LightSpeed RT16";
			public static readonly string DET = "DET_ROC CT Sim";
			public static readonly string FAR = "Farmington CT";
			public static readonly string FLT = "FLT-BigBore20032";
			public static readonly string LAN = "BBCT";
			public static readonly string LAP = "FLT-BigBore20032";
			public static readonly string MAC = "LightSpeed RT16";
			public static readonly string MPH = "MPH CT Sim";
			public static readonly string NOR = "Oncology";
			public static readonly string OWO = "FLT-BigBore20032";
			public static readonly string PRO = "FLT-BigBore20032";
		}

		/// <summary>
		/// Dictionary of allowable rad onc user names for plan approval
		/// </summary>
		private static class RadOncUserNames
		{
			public static readonly List<string> BAY = new List<string> { "" };
			public static readonly List<string> CEN = new List<string> { "evzx38" };
			public static readonly List<string> CLA = new List<string> { "afrazier", "sfranklin", "mjohnson5", "zqfh28" };
			public static readonly List<string> DET = new List<string> { "" };
			public static readonly List<string> FAR = new List<string> { "" };
			public static readonly List<string> FLT = new List<string> { "heshamg", "zcpe57", "kirand", "ogayar", "trqs64" };
			public static readonly List<string> LAN = new List<string> { "abhatt1" };
			public static readonly List<string> LAP = new List<string> { "heshamg", "zcpe57", "kirand", "ogayar", "trqs64" };
			public static readonly List<string> MAC = new List<string> { "afrazier", "sfranklin", "mjohnson5", "zqfh28" };
			public static readonly List<string> MPH = new List<string> { "afrazier", "sfranklin", "mjohnson5", "zqfh28" };
			public static readonly List<string> NOR = new List<string> { "ikaufman", "rhmg27" };
			public static readonly List<string> OWO = new List<string> { "heshamg", "zcpe57", "kirand", "ogayar", "trqs64" };
			public static readonly List<string> PRO = new List<string> { "heshamg", "zcpe57", "kirand", "ogayar", "trqs64" };
		}

		/// <summary>
		/// Dictionary of department names in Aria
		/// </summary>
		private static class DepartmentNames
		{
			public static readonly string BAY = "Bay";
			public static readonly string CEN = "Central";
			public static readonly string CLA = "Clarkston";
			public static readonly string DET = "Detroit";
			public static readonly string FAR = "Farmington";
			public static readonly string FLT = "Flint";
			public static readonly string LAN = "Lansing";
			public static readonly string LAP = "Lapeer";
			public static readonly string MAC = "Macomb";
			public static readonly string MPH = "Port Huron";
			public static readonly string NOR = "Northern";
			public static readonly string OWO = "Owosso";
			public static readonly string PRO = "Proton";
		}

		/// <summary>
		/// Department specific info (machine IDs, CT IDs, Rad onc user names)
		/// </summary>
		private struct DepartmentInfoStruct
		{
			public List<string> Machines;
			public List<string> CTs;
			public List<string> RadOncUserNames;
		}
	}

	/// <summary>
	/// KCI Departments
	/// </summary>
	public enum Department
	{
		BAY,
		CEN,
		CLA,
		DET,
		FAR,
		FLT,
		LAN,
		LAP,
		MAC,
		MPH,
		NOR,
		OWO,
		PRO,
		None
	}
}