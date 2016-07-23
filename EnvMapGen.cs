using System;

namespace EnvMapGen
{
	class MainClass
	{
		public static int mapSize = 8; // Map size, x by x

		public static Random rnd = new Random();
		public static km[,] map = new km[mapSize, mapSize];
		public static int[] cursor = new int[2] {0, 0};
		public static void Main (string[] args)
		{
			bool drawEnv = true;

			CreateEnv (); // creates biomes, population and other key variables
			CreateAltitude (); // create mountains
			CheckForUndefined (); // error checking in biomes for devs

			while (drawEnv) {
				Console.Clear();
				Console.WriteLine (mapSize+" km2 map\n");
				Console.WriteLine(DrawEnv ()); // calls DrawEnv and writes out returned draw buffer
				MoveCursor (); // checks keyboard for movement
			}
		}

		static void CreateEnv(){
			// weather and env generation

			for (int i = 0; i < mapSize; i++) {
				for (int ii = 0; ii < mapSize; ii++) {

					map [i, ii] = new km (); // Initializing array object
					bool isUndef = true;
					while (isUndef) { // checks if biome atributed is undefined and re-atributed the current km
						int temp = rnd.Next (-10, 50);
						int humidity = rnd.Next (0, 100);
						bool isWater = false;
						isUndef = false;

						if (rnd.Next (0, 4) == 2) {
							isWater = true;
						}

						map [i, ii].temp = temp;
						map [i, ii].humidity = humidity;
						if (isWater) { // checks if water because it's has diffrent properties than the other biomes
							map [i, ii].biome = "Water";
							map [i, ii].humidity = rnd.Next (90,100);
							map [i, ii].Height = rnd.Next(-10,10);
							if (temp < 0) {
								map [i, ii].biome = "Ice";
								map [i, ii].humidity = rnd.Next (80,100);
							}
						} else if (temp > 10 && temp < 48 && humidity > 10 && humidity < 70) {
							map [i, ii].biome = "Grassland";
						} else if (temp > -10 && temp < 2 && humidity > 85) {
							map [i, ii].biome = "Snow";
						} else if (rnd.Next(0,5) == 2) {
							map [i, ii].biome = "Wasteland";
						} else if (temp > 20 && temp < 40 && humidity > 70 && humidity < 95) {
							map [i, ii].biome = "Jungle";
						} else if (temp > 40 && humidity > 8 && humidity < 50) {
							map [i, ii].biome = "Desert";
						} else {
							map [i, ii].biome = "Undefined";
							isUndef = true;
						}

						if(rnd.Next(0,4) == 2 ){ // 1 in 4 to populate area
							map[i, ii].population = rnd.Next(0,100);
							if(map[i, ii].population >= 10 && map[i, ii].population <= 50){
								map[i, ii].popType = "Village";
							} else if(map[i, ii].population > 50){
								map[i, ii].popType = "Large Village";
							} else if(map[i, ii].population >= 1 && map[i, ii].population < 10){
								map[i ,ii].popType = "Populated";
							} else { map[i ,ii].popType = ""; }

							if(rnd.Next(0,4) == 2){ // 1 in 4 again to lower the chances of more populated areas
								map[i, ii].population = rnd.Next(0, 500);
								if(map[i, ii].population > 150){ map[i, ii].popType = "Town"; }
							}
						}
						
						// Sets prefixes to the name of the place (temperature)
						if(temp >= -10 && temp <= 5){
							map[i, ii].prefix2 = "Freezing ";
						} else if(temp > 5 && temp <= 20){
                                                        map[i, ii].prefix2 = "Cold ";
                                                } else if(temp > 30 && temp <= 40){
                                                        map[i, ii].prefix2 = "Warm ";
                                                } else if(temp > 40){
                                                        map[i, ii].prefix2 = "Burning ";
                                                }

						// Sets prefixes to the of the (humidity)
						if(humidity < 30){
                                                        map[i, ii].prefix3 = "Dry ";
                                                } else if(humidity > 75){
                                                        map[i, ii].prefix3 = "Humid ";
                                                }

						// Re-checks if the biome is water and makes sure it's name and other variables makes sense
						if(map [i, ii].biome == "Water" || map [i, ii].biome == "Ice"){
							map[i, ii].prefix = "";
							map[i, ii].prefix2 = "";
							map[i, ii].prefix3 = "";
							if(map [i, ii].biome == "Water"){
								map[i, ii].popType = "";
								map[i, ii].population = 0;
							}
						}
					}
				}
			}
		}

		static void CheckForUndefined(){ // *decapricated* function that checks for undefined biomes and alerts user/dev
			for (int i = 0; i < mapSize; i++) {
				for (int ii = 0; ii < mapSize; ii++) {
					if (map [i, ii].biome == "Undefined") {
						Console.WriteLine ("Undefined at " + i + ", " + ii + "  -> Temp: " + map [i, ii].temp + " C / Humidity: " + map [i, ii].humidity + " %");
					}
				}
			}
		}

		static void CreateAltitude(){

			// altitude generation (mountains and stuff)
			for (int i = 0; i < mapSize; i++) {
				for (int ii = 0; ii < mapSize; ii++) {
					map [i, ii].Height = rnd.Next (-300,3000);
				}
			}

			for (int i = 0; i < mapSize; i++) {
				for (int ii = 0; ii < mapSize; ii=ii+2) {
					if (ii < mapSize && ii > 1) {
						map [i, ii].Height = (map [i, ii + 1].Height + map [i, ii - 1].Height) / 2;

						// Sets prefixes related to altitude to the area 
						if(rnd.Next(0,2) == 1 && map[i, ii].Height > 1500){map[i, ii].prefix = "Mountainous ";}
						if(rnd.Next(0,2) == 1 && map[i, ii].Height > 200){map[i, ii].prefix = "Flat ";}
					}
				}
			}
		}

		static void MoveCursor(){ // function that moves and updates cursor/select
			ConsoleKeyInfo input = Console.ReadKey();

			if (input.KeyChar == 'w') {
				if (cursor [0] - 1 >= 0) {
					cursor [0] -= 1;
				}
			} else if (input.KeyChar == 's') {
				if (cursor [0] + 1 < mapSize) {
					cursor [0] += 1;
				}
			} else if (input.KeyChar == 'a') {
				if (cursor [1] - 1 >= 0) {
					cursor [1] -= 1;
				}
			} else if (input.KeyChar == 'd') {
				if (cursor [1] + 1 < mapSize) {
					cursor [1] += 1;
				}
			}
		}


		static string DrawEnv(){
			string drawBuffer = " ";

			// draw code
			string b;
			char fc;
			for (int i = 0; i < mapSize; i++) {
				for (int ii = 0; ii < mapSize; ii++) {
					b = map [i, ii].biome; 
					fc = b [0];
					if (cursor [0] == i && cursor [1] == ii) { // sets cursor on right target area
						if(drawBuffer.Length > 0){ // makes sure it doesn't delete "a nothing"
							drawBuffer = drawBuffer.Remove(drawBuffer.Length - 1); // deletes last char to makes sure everything aligns
						}
						drawBuffer += "["+fc+"]"; // sets cursor box around selected area
					} else {
						drawBuffer += fc+" "; // normal area draw
					}
				}
				drawBuffer += "\n "; // new line for each i array
			}

			km cmap = map[cursor[0], cursor[1]]; // sets cmap to current km obj
			
			// adds properties to current area
			drawBuffer += "\n\n";
			drawBuffer += cmap.prefix+cmap.prefix2+cmap.prefix3+cmap.biome+"\n";
			drawBuffer += "Temperature: "+map[cursor[0], cursor[1]].temp+" C\n";
			drawBuffer += "Humidity: "+map[cursor[0], cursor[1]].humidity+"%\n";
			if(cmap.popType != null){
				if(cmap.popType.Length > 1){
					drawBuffer += "Population: "+cmap.population+"  `"+cmap.popType+"`"+"\n";
				} else {
					drawBuffer += "Population: "+cmap.population+"\n";
				}
			} else {
				drawBuffer += "Population: "+cmap.population+"\n";
			}

			drawBuffer += "Altitude: "+map[cursor[0], cursor[1]].Height+" Meters\n";

			return drawBuffer; // returns drawbuffer for rendering

		}
	}

	class km{ // km object for areas in map
		public int temp, humidity, population, Height = 0;
		public string biome, popType, prefix, prefix2 , prefix3= "";
	}
}
