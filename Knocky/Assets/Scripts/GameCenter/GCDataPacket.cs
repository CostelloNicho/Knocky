using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct GCDataPacket
{
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
	public string playerOneID;
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
	public string playerTwoID;
	
	public uint playerOneScore;
	public uint playerTwoScore;
	public float VelX;
	public float VelY;
	public float VelZ;
	public float PosX;
	public float PosY;
	public float PosZ;
	
}