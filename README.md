
 I have a ring device that holds 16 melodies and produces a 'Mean ;-)' Meander sound.

 Device is composed with a few microchips, the most important of them are two:

 КР1012ГП3 -  Generator of the upper tones of the tempered scale.
 
 КМ573РФ2  -  N-MOS ROM (2Kx8) 16Kbit = 2048bytes of music data. (Only 1024kb is used.)



 I decided to unsolder N-MOS ROM for better needs, like reparing of old computers,
 
 and also to dump a musical data from N-MOS ROM to file (ROM.BIN).

 In place of N-MOS ROM an temporary buttons were soldered to create some kind of musical instrument.

 (See images IMG_20220501_090701.jpg and IMG_20220501_091021.jpg)



 Next I want to write a program for a dumped musical data listening.
 
 And to use a Triangle (And colored Triangle) sound form instead of Meander.

 Clean Triangle:
 
 / \ / \  
 
 Colored Triangle:
 
 / | \ | /  
 
  
  
 КР1012ГП3 can produce 32 tones, an multimeter was used to take a frequency readings.
 


 Each byte of ROM.BIN data contains a 8 digits that can be interpretated as:

 D0 -	frequency select
 
 D1 -	frequency select
 
 D2 -	frequency select
 
 D3 -	frequency select
 
 D4 -	frequency select
 
 D5 -	1-Interrupt with Pause	/	0-No Actions	
 
 D6 -	1-No Actions		/	0-Low Octave
 
 D7 - 1-No Actions		/	0-Stop

 Place a ROM.BIN next to compiled RING.EXE for correct execution.
 
 
