using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Peak.Can.Basic;

namespace AAPcanExample
{
	class Program
	{
		static void Main(string[] args)
		{
			StringBuilder sb = new StringBuilder(5000);
			var status = Peak.Can.Basic.PCANBasic.Initialize(81, TPCANBaudrate.PCAN_BAUD_250K);
			Console.WriteLine(status.ToString());
			status = Peak.Can.Basic.PCANBasic.GetValue(81, TPCANParameter.PCAN_HARDWARE_NAME, sb, (uint)sb.Capacity);
			Console.WriteLine(status.ToString());
			Console.WriteLine(sb.ToString());
			status = Peak.Can.Basic.PCANBasic.Uninitialize(81);
			Console.WriteLine(status.ToString());
			Console.ReadKey();
		}
	}
}
