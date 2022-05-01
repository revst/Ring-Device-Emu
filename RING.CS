using System;
using System.IO;
class RING
{
	static void Main()
    {
	
		MemoryStream MS = new MemoryStream(); // Prepare a place for processed PCM 44100kHz / 8-bit data.
		
		byte level = 127; //Set 8-bit PCM level to silence for next use.
		
		byte [] bstore = File.ReadAllBytes("ROM.BIN"); // Read ROM file that was dumped from N-MOS.
		
		foreach (byte b in bstore) //Start to process 1024 bytes of ROM.
		{
		
		int octave = 2 - ((b & (1<<6))/ (1<<6)); //Pick out Octave bit from the sixth digit of the source byte.
												//int octave = 2(Low Octave) or 1(No Actions)
		
		int freq = ((b & (1<<4))+(b & (1<<3))+(b & (1<<2))+(b & (1<<1))+(b & (1<<0))); //Get tone frequency responsible bits
																						//from the source byte.

		
		switch (freq)						//Convert coded tone frequency value from ROM file to real frequency in Hz.
		{									//Every 32 values of frequency were experimentally taken with multimeter. 
		case 0: freq = 686;
		break;
		case 16: freq = 697;
		break;
		case 8: freq = 709;
		break;
		case 24: freq = 720;
		break;
		case 4: freq = 733;
		break;
		case 20: freq = 745;
		break;
		case 12: freq = 758;
		break;
		case 28: freq = 771;
		break;
		case 2: freq = 785;
		break;
		case 18: freq = 800;
		break;
		case 10: freq = 814;
		break;
		case 26: freq = 830;
		break;
		case 6: freq = 846;
		break;
		case 22: freq = 863;
		break;
		case 14: freq = 880;
		break;
		case 30: freq = 896;
		break;
		case 1: freq = 917;
		break;
		case 17: freq = 937;
		break;
		case 9: freq = 957;
		break;
		case 25: freq = 978;
		break;
		case 5: freq = 1000;
		break;
		case 21: freq = 1024;
		break;
		case 13: freq = 1048;
		break;
		case 29: freq = 1074;
		break;
		case 3: freq = 1101;
		break;
		case 19: freq = 1129;
		break;
		case 11: freq = 1160;
		break;
		case 27: freq = 1191;
		break;
		case 7: freq = 1225;
		break;
		case 23: freq = 1260;
		break;
		case 15: freq = 1297;
		break;
		case 31: freq = 1336;
		break;
		}
		
		freq/=octave; //Set frequency by 2 times lower if low octave was selected.
		int interrupt = ((b & (1<<5))/ (1<<5)); //Pick out Interrupt bit from the fifth digit of the source byte.
												// (1-Interrupt with Pause, 0-No Actions)
		int stop =  ((b & (1<<7))/ (1<<7)); //Pick out Stop bit from the seventh digit of the source byte.
												// (0-Stop, 1-No Actions)
		byte silence = 127; // Prepare a silence value of PCM 8-bit wave data. 
		
			if (stop == 0) // Write 1 sec of silence in wave data.
			{
				for (int z=44100; z>0; z--)
				{
				MS.WriteByte(silence);
				}
			}
		
			else if (interrupt == 1) // Write 0.125 sec of pause in wave data.
			{
			
				for (int z=5500; z>0; z--)
				{
				MS.WriteByte(silence);
				}
			}
		
		
			else
			{

			double smp44100time = 2.2675736961451247165532879818594e-5; // Time of the one sample at 44100 quantization frequency.
			double freqtime = 1d/freq; // Time of the one complete triangle period of the incoming playback frequency.
			int smpcnt = (int)((freqtime/smp44100time)/4); // Estimate amount of samples needed for the ascending or 
														// descending edge of the triangle sound frequency.
			byte smpincr = (byte)((128/smpcnt)-1); //Estimate increment of the 8-bit PCM level for the ascending or descending
												// edge of the triangle sound frequency.
		
			int smpnotelength = 5500; // Set a time for one note playback to 0.125 sec.
	

			while (smpnotelength >0)
			
				// PCM data will be filled with triangle edges: /\
				//                                                \/
			
			{
			
//				Fill first edge: /

				for (int i=smpcnt; i!=0; i--)
				{
				MS.WriteByte(level);
				level+=smpincr;
				}
	
	
//	MS.WriteByte(127); // Unremark this line to produce an colored triangle sound.

			
//				Fill second and third edges: \
//							      \

				for (int i=(smpcnt*2); i!=0; i--) 
				{
				MS.WriteByte(level);
				level-=smpincr;
				}

				
//	MS.WriteByte(127); // Unremark this line to produce an colored triangle sound.



//				Fill forth edge:
//						  /

				for (int i=smpcnt; i!=0; i--)
				{
				MS.WriteByte(level);
				level+=smpincr;
				}		
			smpnotelength = smpnotelength - (smpcnt*4);
			}
			}
		}
		
		byte [] wavedata = MS.ToArray(); //Dump 8bit PCMwave data to Array.
		MS.Close();		
		
						//8-bit PCM wave header:
		UInt32 Riff = 0x46464952;
		UInt32 ChunkSize = (UInt32)(wavedata.Length)+36;
		UInt32 Wave = 0x45564157;
		UInt32 fmt = 0x20746D66;
		UInt32 Subchunk1Size = 16;
		UInt16 FormatTag = 1;
		UInt16 Channels = 1;
		UInt32 SamplesPerSec = 44100;
		UInt32 AvgBytesPerSec = 44100;
		UInt16 BlockAlign = 1;
		UInt16 BitsPerSample = 8;
		UInt32 data = 0x61746164;
		UInt32 Subchunk2Size = (UInt32)(wavedata.Length);

		FileStream FS = new FileStream("PCM 8bit 44100Hz.wav", FileMode.Create); // Create output file & fill it.
		BinaryWriter BW = new BinaryWriter(FS);
			
			BW.Write(Riff);
			BW.Write(ChunkSize);
			BW.Write(Wave);
			BW.Write(fmt);
			BW.Write(Subchunk1Size);
			BW.Write(FormatTag);
			BW.Write(Channels);
			BW.Write(SamplesPerSec);
			BW.Write(AvgBytesPerSec);
			BW.Write(BlockAlign);
			BW.Write(BitsPerSample);
			BW.Write(data);
			BW.Write(Subchunk2Size);
			BW.Write(wavedata);


		BW.Close();
		FS.Close();

 //      Console.ReadLine();
    }
	
}
