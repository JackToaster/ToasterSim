using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class UDP_Recieve : MonoBehaviour, Reciever_Interface{
	static bool ping = true;

	UdpClient client;
	Thread receiveThread;

	orientation lastRecieved;
	static bool ready = false;

	public int port;

	//called when the program starts
	void Start () {

		//set up the recieve thread
		receiveThread = new Thread(
			new ThreadStart(ReceiveData));
		receiveThread.IsBackground = true;
		receiveThread.Start();
	}

	void OnApplicationQuit(){
		ping = false;
	}
	//run forever in a separate thread.
	private void ReceiveData(){
		client = new UdpClient(port);
		IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
		client.Client.ReceiveTimeout = 1000;
		while (ping) {
			//ping = false;
			try{
				
				byte[] data = client.Receive(ref anyIP);
				string text = Encoding.UTF8.GetString(data);
				print("Recieved UDP Packet: " + text);
				ready = true;
				lastRecieved = readPacket(data);
			}catch (Exception e){
				print (e.ToString());
			}
		}
	}

	private orientation readPacket(byte[] packet){
		if (packet.Length == 13) {
			byte ckSum = 0;
			for (int i = 0; i < 12; i++) {
				ckSum += packet [i];
			}
			print ("Checksum: " + ckSum);
			if (ckSum == packet [12]) {
				float angle = bytesToFloat (packet [0], packet [1], packet [2], packet [3]);
				float x = bytesToFloat (packet [4], packet [5], packet [6], packet [7]);
				float y = bytesToFloat (packet [8], packet [9], packet [10], packet [11]);
				print ("Angle: " + angle + ", X: " + x + ", Y: " + y);
				return new orientation (angle, x, y);
			} else {
				print ("Mismatched checksum.");
			}
		} else {
			print ("packet length (" + packet.Length + ") does not match expected length of 13 bytes.");
		}
		return lastRecieved;
	}
	
	private float bytesToFloat(params byte[] bytes){
		if (bytes.Length == 4) {
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse(bytes); // Convert big endian to little endian
			}
			return BitConverter.ToSingle (bytes, 0);
		} else
			return 0f;
	}

	public orientation getOrientation(){
		ready = false;
		return lastRecieved;
	}

	public bool isReady(){
		return ready;
	}
}

public struct orientation{
	public float angle;
	public float xPos;
	public float yPos;
	public orientation(float angle, float x, float y){
		this.angle = angle;
		this.xPos = x;
		this.yPos = y;
	}
}