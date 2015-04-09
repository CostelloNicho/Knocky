using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;

public static class NetworkHelper {
	
	/// <summary>
	/// Serializes the game pkt.
	/// </summary>
	/// <returns>
	/// The game pkt in a Byte array for network transmission.
	/// </returns>
	/// <param name='msg'>
	/// Message.
	/// </param>
	/// <typeparam name='T'>
	/// The 1st type parameter.
	/// </typeparam>
	public static Byte[] SerializeGamePkt<T> (T msg) where T : struct
	{
		int objsize = Marshal.SizeOf(typeof(T));
		Byte[] ret = new Byte[objsize];
		IntPtr buff = Marshal.AllocHGlobal(objsize);
		Marshal.StructureToPtr(msg, buff, true);
		Marshal.Copy(buff, ret, 0, objsize);
		Marshal.FreeHGlobal(buff);
		return ret;
	}
	
	/// <summary>
	/// Deserializes the game pkt.
	/// </summary>
	/// <returns>
	/// The game pkt in struct format.
	/// </returns>
	/// <param name='data'>
	/// Data which is a byte array from the network.
	/// </param>
	/// <typeparam name='T'>
	/// The 1st type parameter.
	/// </typeparam>
	public static T DeserializeGamePkt<T>(Byte[] data) where T : struct
	{
		int objsize = Marshal.SizeOf(typeof(T));
		IntPtr buff = Marshal.AllocHGlobal(objsize);
		Marshal.Copy(data, 0, buff, objsize);
		T retStruct = (T)Marshal.PtrToStructure(buff, typeof(T));
		Marshal.FreeHGlobal(buff);
		return retStruct;
	}
}
